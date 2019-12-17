using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AzureAuthorizationFunctionApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace AzureAuthorizationFunctionApp.Controllers
{
  /// <summary>A class to manage the CosmosDb operations</summary>
  public class CosmosDbController
  {
    private const string Selector = "/key";
    /// <summary>The cosmos database client</summary>
    private readonly CosmosClient _cosmosDbClient;
    /// <summary>The CosmosDb database reference</summary>
    private Database _database;
    /// <summary>The CosmosDb container reference</summary>
    private Container _container;
    /// <summary>The logger
    /// object</summary>
    private ILogger _logger;

    /// <summary>Initializes a new instance of the <see cref="CosmosDbController"/> class.</summary>
    /// <param name="log">The log.</param>
    public CosmosDbController(ILogger log)
    {
        _cosmosDbClient = new CosmosClient(ServiceConfigs.CosmosDbEndpointUri, ServiceConfigs.CosmosDbPrimaryKey);
        _logger = log;
    }

    /// <summary>Gets the user roles and permissions.</summary>
    /// <param name="userDetails">The user details.</param>
    /// <returns></returns>
    /// <exception cref="Exception">The user could not be found in the system database</exception>
    public async Task<DbModelUserRolesPermissions> GetUserRolesPermissions(UserDetails userDetails)
    {
      try
      {
        _database = await this._cosmosDbClient.CreateDatabaseIfNotExistsAsync(ServiceConfigs.CosmosDbDatabaseId);

        var activeCache = await GetActiveCache(userDetails.UserPrincipalName, _database);

        // return the cached user permissions object
        if (activeCache != null) return activeCache;


        // Read Users 
        _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.UsersContainerId, Selector);

        var usersQry = new QueryDefinition("SELECT TOP 1 * FROM c where c.key = @key order by c.created desc");
        usersQry.WithParameter("@key", userDetails.UserPrincipalName);

        FeedIterator<DbModelUser> queryResultUsersSetIterator = this._container.GetItemQueryIterator<DbModelUser>(usersQry);

        var usersList = new List<DbModelUser>();

        while (queryResultUsersSetIterator.HasMoreResults)
        {
          FeedResponse<DbModelUser> currentRolesResultSet = await queryResultUsersSetIterator.ReadNextAsync();
          usersList.AddRange(currentRolesResultSet);
        }

        // Read user Roles by Key
        if (!usersList.Any())
        {
          throw new Exception("The user could not be found in the system database");
        }

        var qryUser = usersList.First();

        var newCacheItem = new DbModelUserRolesPermissions { Id = Guid.NewGuid().ToString(), Key = qryUser.Key, Roles = qryUser.Roles, Created = DateTime.Now, Permissions = new List<BaseModel>() };


        if (qryUser.Roles.Count <= 0)
        {
          return newCacheItem;
        }

        var filterQry = string.Empty;
        foreach (var role in qryUser.Roles) filterQry += (string.IsNullOrEmpty(filterQry) ? $"'{role.Key}'" : $", '{role.Key}'");

        var rolesQry = new QueryDefinition($"SELECT * FROM c where c.key in ({filterQry})");

        // Select only roles where role.Key equal to any of the user roles
        _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.RolesContainerId, Selector);

        FeedIterator<DbModelRole> queryResultRolesSetIterator = this._container.GetItemQueryIterator<DbModelRole>(rolesQry);

        var rolesList = new List<DbModelRole>();

        while (queryResultRolesSetIterator.HasMoreResults)
        {
            FeedResponse<DbModelRole> currentRolesResultSet = await queryResultRolesSetIterator.ReadNextAsync();
            rolesList.AddRange(currentRolesResultSet);
        }

        if (!rolesList.Any())
        {
            return newCacheItem;
        }


        foreach (var role in qryUser.Roles)
        {
            var roleDefinition = rolesList.FirstOrDefault(r => r.Key == role.Key);
            if (roleDefinition == null) continue;

            foreach (var permission in roleDefinition.Permissions)
            {
                newCacheItem.Permissions.Add(new BaseModel { Key = permission.Key });
            }
        }

        newCacheItem.Permissions = newCacheItem.Permissions.Distinct().ToList();

        // Store in Cache Table 
        _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.UserRolesPermissionsContainerId, Selector);

        ItemResponse<DbModelUserRolesPermissions> newCacheItemResponse = await _container.CreateItemAsync(newCacheItem);

        return newCacheItemResponse.StatusCode == HttpStatusCode.Created ? newCacheItem : null;
      }
      catch (Exception exception)
      {
          _logger.LogError(exception.ToString());
          return null;
      }
    }

    /// <summary>Check if there is a cached value of UserRolesPermissions or not, and returns it if exist or a insert a new value</summary>
    /// <param name="userPrincipalName">Name of the user principal.</param>
    /// <param name="cosmosDb">The cosmos database.</param>
    /// <returns></returns>
    private async Task<DbModelUserRolesPermissions> GetActiveCache(string userPrincipalName, Database cosmosDb)
    {
      try
      {
        _container = await cosmosDb.CreateContainerIfNotExistsAsync(ServiceConfigs.UserRolesPermissionsContainerId, Selector);

        var userRolesPermissionsQry = new QueryDefinition("SELECT * FROM c WHERE c.key = @key");

        userRolesPermissionsQry.WithParameter("@key", userPrincipalName);

        var queryResultSetIterator = this._container.GetItemQueryIterator<DbModelUserRolesPermissions>(userRolesPermissionsQry);

        var userRolesPermissions = new List<DbModelUserRolesPermissions>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<DbModelUserRolesPermissions> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            userRolesPermissions.AddRange(currentResultSet);
        }

        // The check for the expiry time in case TTL is applied
        return userRolesPermissions.Any() ? userRolesPermissions.First() : null;
      }
      catch (Exception exception)
      {
        _logger.LogError(exception.ToString());
        return null;
      }

    }
  }
}

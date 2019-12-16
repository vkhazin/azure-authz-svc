using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AzureAuthorizationFunctionApp.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace AzureAuthorizationFunctionApp.Controllers
{
    public class CosmosDbController
    {
        private readonly CosmosClient _cosmosDbClient;
        private Database _database;
        private Container _container;

        public static readonly CosmosDbController Instance = new CosmosDbController();

        private CosmosDbController()
        {
            _cosmosDbClient = new CosmosClient(ServiceConfigs.CosmosDbEndpointUri, ServiceConfigs.CosmosDbPrimaryKey);
        }

        public UserRolesResponseModel GetUserPermissions(string accessToken)
        {
            return new UserRolesResponseModel();
        }

        public async Task<DbModelUserRolesPermissions> GetUserRolesPermissions(UserDetails userDetails)
        {
            try
            {
                _database = await this._cosmosDbClient.CreateDatabaseIfNotExistsAsync(ServiceConfigs.CosmosDbDatabaseId);

                var activeCache = await GetActiveCache(userDetails.userPrincipalName, _database);

                if (activeCache == null)
                {

                    // Read user Roles by Key


                    // Select only roles where role.Key equal to any of the user roles





                    // Read Roles 
                    _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.RolesContainerId, "/partition");

                    var rolesQry = new QueryDefinition("SELECT * FROM c");
                    FeedIterator<DbModelRole> queryResultRolesSetIterator = this._container.GetItemQueryIterator<DbModelRole>(rolesQry);

                    var rolesList = new List<DbModelRole>();

                    while (queryResultRolesSetIterator.HasMoreResults)
                    {
                        FeedResponse<DbModelRole> currentRolesResultSet = await queryResultRolesSetIterator.ReadNextAsync();
                        rolesList.AddRange(currentRolesResultSet);
                    }


                    // Read Users 
                    _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.UsersContainerId, "/partition");

                    var usersQry = new QueryDefinition("SELECT TOP 1 * FROM c where c.key = @key order by c.created desc");
                    usersQry.WithParameter("@key", userDetails.userPrincipalName);

                    FeedIterator<DbModelUser> queryResultUsersSetIterator = this._container.GetItemQueryIterator<DbModelUser>(usersQry);

                    var usersList = new List<DbModelUser>();

                    while (queryResultUsersSetIterator.HasMoreResults)
                    {
                        FeedResponse<DbModelUser> currentRolesResultSet = await queryResultUsersSetIterator.ReadNextAsync();
                        usersList.AddRange(currentRolesResultSet);
                    }


                    // Generate Result 

                    if (!usersList.Any())
                    {
                        throw new Exception("The user could not be found in the database");
                    }

                    if (!rolesList.Any())
                    {
                        throw new Exception("There are no roles found in the database");
                    }

                    var user = usersList.First();

                    var newCacheItem = new DbModelUserRolesPermissions {Id = Guid.NewGuid().ToString(), key = user.key, Roles = user.Roles, Created = DateTime.Now, Permissions = new List<BaseModel>() };



                    foreach (var role in user.Roles)
                    {
                        var roleDefinition = rolesList.FirstOrDefault(r => r.key == role.key);
                        if (roleDefinition == null) continue;

                        foreach (var permission in roleDefinition.Permissions)
                        {
                          
                            newCacheItem.Permissions.Add(new BaseModel { key = permission.key });
                        }
                    }

                    newCacheItem.Permissions = newCacheItem.Permissions.Distinct().ToList();
                    
                    // Store in Cache Table 
                    _container = await this._database.CreateContainerIfNotExistsAsync(ServiceConfigs.UserRolesPermissionsContainerId, "/partition");

                    // Check if TTL can be specified to the record

                    ItemResponse<DbModelUserRolesPermissions> newCacheItemResponse = await _container.CreateItemAsync(newCacheItem);

                    if (newCacheItemResponse.StatusCode == HttpStatusCode.Created)
                        return newCacheItem;


                    // return the original object which was not stored in the database, and write to the log possible error message
                    return null;
                }

                return activeCache;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
           
        }

        private async Task<DbModelUserRolesPermissions> GetActiveCache(string userPrincipalName, Database cosmosDb)
        {
            try
            {
                _container = await cosmosDb.CreateContainerIfNotExistsAsync(ServiceConfigs.UserRolesPermissionsContainerId, "/partition");

                var userRolesPermissionsQry = new QueryDefinition("SELECT * FROM c WHERE c.key = @key");

                userRolesPermissionsQry.WithParameter("@key", userPrincipalName);

                FeedIterator<DbModelUserRolesPermissions> queryResultSetIterator = this._container.GetItemQueryIterator<DbModelUserRolesPermissions>(userRolesPermissionsQry);

                var userRolesPermissions = new List<DbModelUserRolesPermissions>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<DbModelUserRolesPermissions> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    userRolesPermissions.AddRange(currentResultSet);
                }

                // The check for the expiry time in case TTL is applied
                return userRolesPermissions.Any(r => DateTime.Now.Subtract(r.Created).Minutes <= 60) ? userRolesPermissions.First() : null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }

    }
}

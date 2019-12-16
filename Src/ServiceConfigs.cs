using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos;

namespace AzureAuthorizationFunctionApp
{
    /// <summary>A class to store Service config values</summary>
    public static class ServiceConfigs
    {
#if DEBUG
        public static string CosmosDbEndpointUri = "<to be replaced as per the environment setup>";
        public static string CosmosDbPrimaryKey = "<to be replaced as per the environment setup>";
        public static string CosmosDbDatabaseId = "<to be replaced as per the environment setup>";
        public static string RolesContainerId = "Roles";
        public static string UsersContainerId = "Users";
        public static string UserRolesPermissionsContainerId = "UserRolesPermissions";

#else
public static string CosmosDbEndpointUri =  Environment.GetEnvironmentVariable("CosmosDbEndpointUri");
        public static string CosmosDbPrimaryKey =  Environment.GetEnvironmentVariable("CosmosDbPrimaryKey");
        public static string CosmosDbDatabaseId = Environment.GetEnvironmentVariable("CosmosDbDatabaseId");
        public static string RolesContainerId = Environment.GetEnvironmentVariable("RolesContainerId");
        public static string UsersContainerId = Environment.GetEnvironmentVariable("UsersContainerId");
        public static string UserRolesPermissionsContainerId = Environment.GetEnvironmentVariable("UserRolesPermissionsContainerId");
#endif





    }
}

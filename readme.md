# Azure Authorization Service

## Objectives

* A single-concern service responsible for application permission/role checking
* Accepts `access_token` for caller identity as an Http Header and permission(s) or role(s) as url path params
* Returns a yes/no answer for each permission/role requested
* The service does not implement any meaning for permissions or roles
* The calling GUI/Service provides an interpretation for the role/permission
* The service does not provide authorization data management functionality, a separate end-point will address these concerns

## Design

![](./media/AuthZ.jpg)

* The decision was not to rely on Azure AD user groups to eliminate dependency on IT personnel
* The service is solely responsible for high performance and availability
* The service will leverage Cosmos DB with Document API for high performance and availability
* The owner of the service may decide to implement a caching strategy in case of performance issues detected

## End-points

### Permissions

The end-point will validate access token before processing the request

#### Request

Verb: GET  
Http Headers: 
* x-access-token: the token issued by authentication end-point
* x-correlation-id: optional header for traceability

Path: `/permissions/key-1,key2`

#### Response - Success

Status Code: 200
Body:
```
{
  "userId": "user-email-address",
  "correlationId": "UUID",
  "timestamp": "date-time"
  "permissions": [
    { "key-1": false },
    { "key-2": true },
  ]
}
```

### Roles

The end-point will validate access token before processing the request

#### Request

Verb: GET  
Http Headers: 
* x-access-token: the token issued by authentication end-point
* x-correlation-id: optional header for traceability

Path: `/roles/key-1,key2`

#### Response - Success

Status Code: 200
Body:
```
{
  "userId": "user-email-address",
  "correlationId": "UUID",
  "timestamp": "date-time"
  "roles": [
    { "key-1": false },
    { "key-2": true },
  ]
}
```
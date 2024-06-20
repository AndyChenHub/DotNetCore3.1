# User Management API

This is a User Management API built with .NET Core 3.1 and Entity Framework Core 3.1. This project includes a Postman testing collection.

## Features

- User operations such as GET, CREATE, UPDATE, and DELETE.
- Retrieve all managers with their associated clients.
- Retrieve a list of clients for a specified manager username.
- Retrieve all clients including their manager.

## Getting Started
Make sure .Net Core 3.1 and Entity Framework Core 3.1 is installed.

1. Clone the repository
2. Update the SQL connection string in appsettings.json
3. In the terminal run this command - dotnet ef database update
4. Run this command - dotnet run
5. Install the postman collection
6. Run the tests

## Endpoints

| Endpoint | HTTP Method | Description | HTTP Status Codes |
|----------|-------------|-------------|-------------------|
| `/api/managers/clients` | GET | Get all managers with clients | 200, 500 |
| `/api/managers/{username}/clients` | GET | Get all clients by manager's username | 200, 404, 500 |
| `/api/users/{userId}` | GET | Get user by ID | 200, 404, 500 |
| `/api/users/query` | GET | Get users by filter | 200, 404, 500 |
| `/api/users` | POST | Create a new user | 201, 400, 500 |
| `/api/users/{userId}` | PATCH | Update user information | 204, 400, 404, 500 |
| `/api/users/{userId}` | DELETE | Delete a user | 204, 400, 404, 500 |
| `/api/clients` | GET | Get all clients with managers | 200, 500 |

### Database Controller

- **Endpoint**: `POST /api/InitialiseDb`
- **Description**: Inserts dummy data to the database
- **HTTP Status Codes**:
  - `200 OK`: Database initialized successfully.

### ManagersController

#### 1. Get All Managers with Clients

- **Endpoint**: `GET /api/managers/clients`
- **Description**: Retrieves all managers along with their clients.
- **HTTP Status Codes**:
  - `200 OK`: Returns a list of managers with their clients.
  - `500 Internal Server Error`: An error occurred while processing the request.

  #### Example Response:
```json
[
    {
        "managerId": 1,
        "managerName": "Kevin",
        "clients": [
            {
                "clientId": 2,
                "clientName": "Bob"
            }
        ]
    }
]
```

#### 2. Get Clients by Manager's Username

- **Endpoint**: `GET /api/managers/{username}/clients`
- **Description**: Retrieves all clients managed by a specific manager identified by username.
- **HTTP Status Codes**:
  - `200 OK`: Returns a list of clients managed by the specified manager.
  - `404 Not Found`: No clients found for the specified manager username.
  - `500 Internal Server Error`: An error occurred while processing the request.

  #### Example Response:

```json
[
    {
        "clientId": 5,
        "userName": "Williams_James"
    },
    {
        "clientId": 12,
        "userName": "Sophia_Wilson"
    }
]
```

### UsersController

#### 3. Get User by ID

- **Endpoint**: `GET /api/users/{userId}`
- **Description**: Retrieves a user by their ID.
- **HTTP Status Codes**:
  - `200 OK`: Returns the user with the specified ID.
  - `404 Not Found`: No user found with the specified ID.
  - `500 Internal Server Error`: An error occurred while processing the request.

  #### Example Response:
```json
{
    "userId": 1,
    "userName": "John_Smith",
    "email": "John_Smith@hotmail.com",
    "alias": "John_Smith",
    "firstName": "John",
    "lastName": "Smith",
    "userType": "Manager"
}
```
#### 4. Get Multiple Users by Filter

- **Endpoint**: `GET /api/users/query`
- **Description**: Retrieves users based on any combination of username, email, alias, first name, and last name.
- **HTTP Status Codes**:
  - `200 OK`: Returns a list of users matching the filter criteria.
  - `404 Not Found`: No users found matching the filter criteria.
  - `500 Internal Server Error`: An error occurred while processing the request.

#### Example Response:
```json
[
    {
        "userId": 1,
        "userName": "John_Smith",
        "email": "John_Smith@hotmail.com",
        "alias": "John_Smith",
        "firstName": "John",
        "lastName": "Smith",
        "userType": "Manager"
    },
    {
        "userId": 17,
        "userName": "John_Williams",
        "email": "John_Williams@hotmail.com",
        "alias": "John_Williams",
        "firstName": "John",
        "lastName": "Williams",
        "userType": "Manager"
    },
    {
        "userId": 18,
        "userName": "John_James",
        "email": "John_James@hotmail.com",
        "alias": "John_James",
        "firstName": "John",
        "lastName": "James",
        "userType": "Manager"
    }
]
```
#### 5. Create a New User

- **Endpoint**: `POST /api/users`
- **Description**: Creates a new user.
- **HTTP Status Codes**:
  - `201 Created`: The user was successfully created.
  - `400 Bad Request`: The provided user data is invalid.
  - `500 Internal Server Error`: An error occurred while processing the request.

#### Example Request: 
```json
{
"userName": "andrew_chen",
"email": "andrew.chen@hotmail.com",
"alias": "AndrewC",
"firstName": "Andrew",
"lastName": "Chen",
"userType": "Manager"
}
```

#### Example Response:
```json
{
    "userId": 21,
    "userName": "andrew_chen",
    "email": "andrew.chen@hotmail.com",
    "alias": "AndrewC",
    "firstName": "Andrew",
    "lastName": "Chen",
    "userType": "Manager"
}
```

#### 6. Update User Information

- **Endpoint**: `PATCH /api/users/{userId}`
- **Description**: Updates the information of an existing user.
- **HTTP Status Codes**:
  - `204 No Content`: The user was successfully updated.
  - `400 Bad Request`: The provided user data is invalid.
  - `404 Not Found`: No user found with the specified ID.
  - `500 Internal Server Error`: An error occurred while processing the request.
  
  #### Example Request:
```json
{
  "alias": "BATMAN",
  "userName": "Batmaniscool",
  "email": "brucewayne@hotmail.com",
  "userType": "Client",
  "firstName": "Bruce",
  "lastName": "Wayne"
}
```
#### 7. Delete a User

- **Endpoint**: `DELETE /api/users/{userId}`
- **Description**: Deletes a user. If the user is a manager, they must not have any clients before they are deleted.
- **HTTP Status Codes**:
  - `204 No Content`: The user was successfully deleted.
  - `400 Bad Request`: The user cannot be deleted due to invalid operation.
  - `404 Not Found`: No user found with the specified ID.
  - `500 Internal Server Error`: An error occurred while processing the request.

### ClientsController

#### 8. Get All Clients with Managers

- **Endpoint**: `GET /api/clients`
- **Description**: Retrieves all clients along with their managers.
- **HTTP Status Codes**:
  - `200 OK`: Returns a list of clients with their managers.
  - `500 Internal Server Error`: An error occurred while processing the request.
  #### Example Response:
```json
[
    {
        "clientId": 3,
        "userName": "Batmaniscool",
        "managerUserName": "John_Williams",
        "managerId": 17
    },
    {
        "clientId": 4,
        "userName": "Charlotte_Martin",
        "managerUserName": "John_James",
        "managerId": 18
    }
]
```

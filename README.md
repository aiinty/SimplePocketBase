# SimplePocketBase

Simple asynchronous REST client for [PocketBase](https://pocketbase.io/) written in C#.

## Current status

This project is under development.

Currently supported:

- user authentication/registration
- records CRUD operations with generic-types
- `Newtonsoft.Json` serialization

## Quick start

```csharp
using System;
using System.Threading.Tasks;
using SimplePocketBase;

class Program
{
    static async Task Main(string[] args)
    {
        using (var client = new PocketBaseClient("http://127.0.0.1:8090"))
        {
            var result = await client.Records.GetListAsync<Category>("categories");

            foreach (var category in result.Items)
            {
                Console.WriteLine(category.Name);
            }
        }
    }
}

public class Category
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

## Authentication

### Registration


```csharp
var user = await client.Auth.RegisterAsync<User>(
    "users",
    new
    {
        email = "user@example.com",
        username = "user123",
        fullName = "John Smith",
        role = "user",
        password = "Password123!",
        passwordConfirm = "Password123!"
    }
);
```

### Login

```csharp
var auth = await client.Auth.LoginAsync<User>(
    "users",
    "user@example.com",
    "password"
);

Console.WriteLine(auth.Record.Username);
```

The authentication token is stored in the client and automatically added to other requests.

## Records

### Get a paginated list

```csharp
var medicines = await client.Records.GetListAsync<Medicine>(
    "medicines",
    page: 1,
    perPage: 20,
    filter: "price > 0",
    sort: "-price"
);

foreach (var medicine in medicines.Items)
{
    Console.WriteLine(medicine.Title + " - " + medicine.Price);
}
```

### Create a record

```csharp
var medicine = await client.Records.CreateAsync<Medicine>(
    "medicines",
    new Medicine
    {
        Title = "Aspirin",
        Price = 100,
        Description = "Example medicine",
        Stock = 10,
        IsPromo = false
    }
);
```

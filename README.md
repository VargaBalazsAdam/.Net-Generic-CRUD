# Generic CRUD Repository

This repository provides a generic CRUD (Create, Read, Update, Delete) framework for database operations using Entity Framework Core. It is designed to be highly customizable and can be used as a foundation for various database handling scenarios. Below are some important points to keep in mind when using this repository.

## Usage

### 1. Request Objects

When making requests to create or update entities, ensure that you do not use default values for properties unless you explicitly intend to change them. Instead, make properties nullable in your request objects. This way, you can easily differentiate between properties you want to leave unchanged and properties you want to update.

### 2. Database Migration and Update

To migrate and update your database schema, use the following Entity Framework Core commands:

```shell
dotnet ef migrations add MigrationName
dotnet ef database update
```


Replace `MigrationName` with a meaningful name for your migration.

### 3. Example Controllers

There are example controllers provided in the `NsideController` and `1SideController` namespaces. These controllers demonstrate how to use the generic repository for various database operations. You can refer to these examples to understand how to interact with the repository and adapt it to your specific needs.

### 4. Unique Attribute

This repository includes a `UniqueAttribute` that can be applied to properties of your entity models to enforce uniqueness constraints in the database. Use this attribute on properties that should have unique values.

## Repository Overview

The core of this repository is the `GenericRepository<T>` class, which provides a set of generic methods for database operations:

- `GetAll(params Expression<Func<T, object>>[] includes)`: Retrieves a list of entities with optional inclusion of related entities specified by the `includes` parameter.
- `GetByKey(object key, params Expression<Func<T, object>>[] includes)`: Retrieves an entity by its primary key, with optional inclusion of related entities.
- `Find(Func<T, bool> predicate)`: Searches for entities that match the provided predicate.
- `Insert(T entity)`: Inserts a new entity into the database.
- `InsertWithConnectedModel<TConnectedModel>(T entity, int connectedModelId)`: Inserts a new entity and associates it with a connected model.
- `Update<U>(U request)`: Updates an entity based on the properties of a request object. Only non-null properties in the request object are updated.
- `UpdateConnectionById<TConnectedModel>(int id, int connectedModelId)`: Updates the connection between an entity and a connected model by their IDs.
- `Delete(int id)`: Deletes an entity by its primary key.

## DataContext Configuration

The `DataContext` class is responsible for configuring the database connection and entity model. It includes support for specifying unique constraints on entity properties using the `UniqueAttribute`. You can customize this class to fit your database schema and connection requirements.

## Logging

The repository includes a logging mechanism that logs exceptions to a file. The log file path can be configured in the `appsettings.json` file under the key `"Logfile:Path"`. When an exception occurs, it is logged to a file with a timestamp. This logging can be useful for diagnosing issues in a production environment.

**Note**: Make sure to configure your application's `appsettings.json` file with the appropriate log file path.

## Getting Started

To start using this generic CRUD repository, follow these steps:

1. Create your entity models and request objects.
2. Configure your `DataContext` class to match your database schema.
3. Use the example controllers in the `NsideController` and `1SideController` namespaces as a guide for implementing your own controllers.
4. Customize the repository to fit your specific database requirements.

With this repository, you have a solid foundation for handling database operations in your .NET Core application. Feel free to adapt and extend it as needed for your project.

## License

This project is licensed under the [Creative Commons Attribution 4.0 International License (CC BY 4.0)](LICENSE.md).
For more details, please see the [LICENSE.md](LICENSE.md) file.

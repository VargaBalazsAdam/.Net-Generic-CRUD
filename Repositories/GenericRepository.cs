namespace Generic_CRUD.Repositories
{
  public class GenericRepository<T> where T : class, new()
  {
    protected readonly DbContext dbContext;
    public GenericRepository(DbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public virtual List<T> GetAll(params Expression<Func<T, object>>[] includes)
    {
      var query = dbContext.Set<T>().AsQueryable();

      if (includes != null)
      {
        query = includes.Aggregate(query, (current, include) => current.Include(include));
      }

      return query.ToList();
    }


    public virtual T? GetByKey(object key, params Expression<Func<T, object>>[] includes)
    {
      var query = dbContext.Set<T>().AsQueryable();

      if (includes != null)
      {
        query = includes.Aggregate(query, (current, include) => current.Include(include));
      }

      // Get the primary key property dynamically using reflection
      var primaryKeyProperty = dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();

      if (primaryKeyProperty == null)
      {
        throw new InvalidOperationException("The entity does not have a primary key property.");
      }

      // Create an expression to build the key comparison
      var parameter = Expression.Parameter(typeof(T), "entity");
      var keyExpression = Expression.Property(parameter, primaryKeyProperty.PropertyInfo);
      var keyEquality = Expression.Equal(keyExpression, Expression.Constant(key));

      // Build the where clause dynamically
      var whereExpression = Expression.Lambda<Func<T, bool>>(keyEquality, parameter);

      return query.FirstOrDefault(whereExpression);
    }


    public virtual List<T> Find(Func<T, bool> predicate)
    {
      return dbContext.Set<T>().Where(predicate).ToList();
    }

    public virtual T Insert(T entity)
    {
      dbContext.Set<T>().Add(entity);
      dbContext.SaveChanges();
      return entity;
    }

    public virtual T InsertWithConnectedModel<TConnectedModel>(T entity, int connectedModelId) where TConnectedModel : class
    {
      // Get the connected model by its ID
      var connectedModel = dbContext.Set<TConnectedModel>().Find(connectedModelId);

      if (connectedModel == null)
      {
        throw new ArgumentException("Connected model with the specified ID not found.");
      }

      // Find the navigation property in T that corresponds to the connected model
      var navigationProperty = typeof(T).GetProperties()
          .FirstOrDefault(prop => prop.PropertyType == typeof(TConnectedModel));

      if (navigationProperty == null)
      {
        throw new ArgumentException("No navigation property found for the connected model type.");
      }

      // Set the navigation property value to associate the connected model with the entity
      navigationProperty.SetValue(entity, connectedModel);

      dbContext.Set<T>().Add(entity);
      dbContext.SaveChanges();
      return entity;
    }


    public virtual T Update<U>(U request)
    {
      var keyProperty = typeof(U).GetProperty("id") ?? throw new ArgumentException("Request object must have an 'id' property.");

#pragma warning disable CS8605 // Unboxing a possibly null value.
      var key = (int)keyProperty.GetValue(request);
#pragma warning restore CS8605 // Unboxing a possibly null value.

      var originalObject = GetByKey(key) ?? throw new Exception("Object not found.");
      var updatedObject = originalObject ?? new T();
      var requestProperties = typeof(U).GetProperties();
      var objectProperties = typeof(T).GetProperties();

      foreach (var requestProperty in requestProperties)
      {
        var objectProperty = objectProperties.FirstOrDefault(p => p.Name == requestProperty.Name);
        if (objectProperty != null && requestProperty.GetValue(request) != null)
        {
          objectProperty.SetValue(updatedObject, requestProperty.GetValue(request));
        }
      }

      dbContext.Entry(updatedObject).State = EntityState.Modified;
      dbContext.SaveChanges();
      return updatedObject;
    }

    public virtual T UpdateConnectionById<TConnectedModel>(int id, int connectedModelId) where TConnectedModel : class
    {
      var originalModel = dbContext.Set<T>().Find(id);
      var connectedModel = dbContext.Set<TConnectedModel>().Find(connectedModelId);

      if (originalModel != null && connectedModel != null)
      {
        var connectedModelProperty = typeof(T).GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(TConnectedModel));

        if (connectedModelProperty != null)
        {
          connectedModelProperty.SetValue(originalModel, connectedModel);
          dbContext.SaveChanges();
        }
        else
        {
          throw new ArgumentException("Connected model property not found.");
        }
      }
      else
      {
        throw new ArgumentException("Original or connected model not found.");
      }

      return originalModel;
    }



    public virtual void Delete(int id)
    {
      var entity = GetByKey(id);
#pragma warning disable CS8604 // Possible null reference argument.
      dbContext.Set<T>().Remove(entity);
#pragma warning restore CS8604 // Possible null reference argument.
      dbContext.SaveChanges();
    }
  }
}

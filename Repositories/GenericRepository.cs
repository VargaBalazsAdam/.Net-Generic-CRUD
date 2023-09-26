namespace Generic_CRUD.Repositories
{
  public class GenericRepository<T> where T : class, new()
  {
    protected readonly DbContext dbContext;
    public GenericRepository(DbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public virtual List<T> GetAll()
    {
      return dbContext.Set<T>().ToList();
    }

    public virtual T? GetByKey(object key)
    {
      return dbContext.Set<T>().Find(key);
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

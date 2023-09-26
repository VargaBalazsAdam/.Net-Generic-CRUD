using System.Reflection;

namespace Generic_CRUD
{
  public class DataContext : DbContext
  {
    public DbSet<ModelName1> tableName1 { get; set; }
    public DbSet<ModelNameN> tableNameN { get; set; }


    private readonly IConfiguration _configuration;
    private readonly string connStr;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DataContext(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
      _configuration = configuration;
#if DEBUG
      connStr = _configuration.GetConnectionString("LocalDB");
#else
      connStr = _configuration.GetConnectionString("ReleaseDB");
#endif

    }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public DataContext(DbContextOptions options, IConfiguration configuration)
        : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
      _configuration = configuration;
#if DEBUG
      connStr = _configuration.GetConnectionString("LocalDB");
#else
      connStr = _configuration.GetConnectionString("ReleaseDB");
#endif
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!string.IsNullOrEmpty(connStr))
        optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      ModelBuilder modelBuilder2 = modelBuilder;
      foreach (IMutableEntityType entityType in modelBuilder2.Model.GetEntityTypes())
      {
        (from p in entityType.ClrType.GetProperties()
         where p.GetCustomAttribute<UniqueAttribute>() != null
         select p).ToList().ForEach(delegate (PropertyInfo property)
         {
           modelBuilder2.Entity(entityType.ClrType).HasIndex(property.Name).IsUnique();
         });
      }

      modelBuilder.Entity<ModelName1>().HasData
      (
          new() { id = 1, email = "Example 1" },
          new() { id = 2, email = "Example 2" },
          new() { id = 3, email = "Example 3" },
          new() { id = 4, email = "Example 4" }
      );
    }
  }
}

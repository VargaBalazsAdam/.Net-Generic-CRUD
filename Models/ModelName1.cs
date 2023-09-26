namespace Generic_CRUD.Models
{
  public class ModelName1
  {
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    [Unique]
    public string email { get; set; } = string.Empty;
  }
}

namespace Generic_CRUD.Models
{
  public class ModelNameN
  {
    public int id { get; set; }
    [Required]
    [MaxLength(500, ErrorMessage = "Name can't be more than 500 character.")]
    public string? name { get; set; } = string.Empty;
    [Required]
    public ModelName1? connectedModel { get; set; }
    [Range(0, 5)]
    public int? rate { get; set; }
  }
}

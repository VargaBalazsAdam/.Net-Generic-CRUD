namespace Generic_CRUD.Models.Requests
{
    public class ModelNameNRequest
    {
      public int id { get; set; }
      [MaxLength(500, ErrorMessage = "Name can't be more than 500 character.")]
      public string? name { get; set; }
      public int? connectedModelId { get; set; }
      [Range(0, 5)]
      public int? rate { get; set; }
    }
}

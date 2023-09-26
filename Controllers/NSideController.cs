namespace Generic_CRUD.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class NSideController : Controller
  {
    private readonly NSideRepository repository;
    private readonly DataContext dataContext;
    private readonly IConfiguration _configuration;
    public NSideController(IConfiguration configuration)
    {
      _configuration = configuration;
      dataContext = new(_configuration);
      repository = new(dataContext);
    }

    [HttpPost("create")]
    public IActionResult Create(ModelNameN model)
    {
      return this.Run(() =>
      {
        return Ok(repository.InsertWithConnectedModel<ModelName1>(model, model.connectedModel.id));
      });
    }

    [HttpGet("get")]
    public IActionResult Get(int id)
    {
      return this.Run(() =>
      {
        return Ok(repository.GetByKey(id, m => m.connectedModel));
      });
    }

    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
      return this.Run(() =>
      {
        return Ok(repository.GetAll(m => m.connectedModel));
      });
    }

    [HttpPatch("update")]
    public IActionResult Update(ModelNameNRequest request)
    {
      return this.Run(() =>
      {
        if (request.connectedModelId != null)
        {
          repository.UpdateConnectionById<ModelName1>(request.id, (int)request.connectedModelId);
        }
        return Ok(repository.Update(request));
      });
    }

    [HttpDelete("delete")]
    public IActionResult Delete(int id)
    {
      return this.Run(() =>
      {
        repository.Delete(id);
        return Ok("Succesfully deleted.");
      });
    }
  }
}

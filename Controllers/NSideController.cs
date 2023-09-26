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
        return Ok(repository.Insert(model));
      });
    }

    [HttpGet("get")]
    public IActionResult Get(int id)
    {
      return this.Run(() =>
      {
        return Ok(repository.GetByKey(id));
      });
    }

    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
      return this.Run(() =>
      {
        return Ok(repository.GetAll());
      });
    }

    [HttpPatch("update")]
    public IActionResult Update(ModelNameN model)
    {
      return this.Run(() =>
      {
        return Ok(repository.Update(model));
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

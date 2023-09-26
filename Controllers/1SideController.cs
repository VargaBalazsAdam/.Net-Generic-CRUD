namespace Generic_CRUD.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class _1SideController : Controller
  {
    private readonly _1SideRepository repository;

    [HttpGet("get")]
    public IActionResult GetFeedback(int id)
    {
      return this.Run(() =>
      {
        return Ok(repository.GetByKey(id));
      });
    }

  }
}

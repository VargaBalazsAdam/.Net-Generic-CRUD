using Microsoft.AspNetCore.Mvc;

namespace Generic_CRUD.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class _1SideController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}

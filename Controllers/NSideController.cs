using Microsoft.AspNetCore.Mvc;

namespace Generic_CRUD.Controllers
{
  public class NSideController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}

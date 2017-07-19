using Microsoft.AspNetCore.Mvc;

namespace Diporto.Controllers {
  [Route("admin")]
  public class AdminController : Controller {
    [HttpGet]
    public ViewResult Index() {
      return View();
    }

    [HttpGet]
    [Route("places")]
    public ViewResult Places() {
      return View();
    }

    [HttpGet]
    [Route("users")]
    public ViewResult Users() {
      return View();
    }
  }
}

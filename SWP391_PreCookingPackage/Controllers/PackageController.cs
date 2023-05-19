using Microsoft.AspNetCore.Mvc;

namespace SWP391_PreCookingPackage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackageController : ControllerBase
    {
        [HttpGet]
        public String GetPackages() {
            return "Test package";
        }
    }
}

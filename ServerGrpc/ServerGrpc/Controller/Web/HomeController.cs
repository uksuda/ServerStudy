using Microsoft.AspNetCore.Mvc;

namespace ServerGrpc.Controller.Web
{

    [ApiController]
    [Route("/")]
    public class HomeController
    {
        public HomeController()
        {

        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet]
        public JsonResult Index()
        {
            return new JsonResult(new {
                test = "test"
            });
        }
    }    
}

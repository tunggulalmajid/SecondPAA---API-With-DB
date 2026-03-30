using Microsoft.AspNetCore.Mvc;
using SecondPAA___API_With_DB.Models;

namespace SecondPAA___API_With_DB.Controllers
{
    public class PersonController : Controller
    {
        private string __connstr;
        public PersonController(IConfiguration configuration) {
            __connstr = configuration.GetConnectionString("WebApiDatabase");
        }

        public IActionResult Index() { 
            return View();
        }

        [HttpGet("/api/person")]
        public ActionResult<Person> ListPerson()
        {
            PersonContext context = new PersonContext(this.__connstr);
            List<Person> data = context.ListPerson();
            return Ok(data);

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SecondPAA___API_With_DB.Models;

namespace SecondPAA___API_With_DB.Controllers
{
    public class LoginController : Controller
    {
        private string _connstr;
        private IConfiguration __config;
        public IActionResult Index()
        {
            return View();
        }

        public LoginController(IConfiguration configuration)
        {
            __config = configuration;
            _connstr = configuration.GetConnectionString("WebApiDatabase");

        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] Auth request)
        {
            LoginContext context = new LoginContext(_connstr);
            var dataUser = context.Authentifikasi(request.username, request.password, __config);
            if (dataUser == null || dataUser.Count == 0)
            {
                return Unauthorized(new
                {
                    status = false,
                    message = "Username atau password salah"
                });
            }
            return Ok(new
            {
                status = true,
                message = "Login Berhasil",
                data = dataUser[0]
            });
        }
    }
}

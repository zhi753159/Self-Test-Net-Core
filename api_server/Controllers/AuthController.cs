using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api_server.Data;
using api_server.HTTPRequest;
using api_server.Models;

namespace api_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ProjectContext _context;

        public AuthController(ProjectContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult AuthLogin(LoginRequest body)
        {
            if (_context.Customers == null) {
                return Problem("Database schema not found");
            }

            ValidateResult requestValidate = body.validate();
            if (!requestValidate.valid) {
                return BadRequest(requestValidate.valid_error);
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Username == body.Username);
            if (customer == null)
            {
                return BadRequest("User not found");
            }

            var saltedByte = Convert.FromBase64String(customer.Salt);
            if (saltedByte == null)
            {
                return Problem("User cryptography corrupted");
            }
            var passwordSubmitted = Customer.HashPassword(body.Password, saltedByte);
            if (String.Compare(customer.Password, passwordSubmitted) != 0)
            {
                return BadRequest("Username or Password not correct");
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("UserId", customer.Id);
            HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString(ProgramConfig.DateTimeFormat));

            return Ok("Login Success");
        }

        [HttpGet]
        public ActionResult LoginCheck()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var loginTimeStr = HttpContext.Session.GetString("LoginTime");
            if (loginTimeStr == null || id == null) {
               return Unauthorized();
            }
            DateTime loginTime = DateTime.Parse(loginTimeStr);

            return Ok(new { Id = id, LoginTime = loginTime });
        }
    }
}

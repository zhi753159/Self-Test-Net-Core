using api_server.Data;
using api_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api_server.HTTPRequest;
using Microsoft.EntityFrameworkCore;
using api_server.HTTPResponse;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace api_server.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ProjectContext _context;

        public CustomerController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInfoResponse>>> GetCustomerInfoList([FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "limit")] int limit = 50) {
            if (_context.Products == null)
            {
                return NotFound();
            }

            List<Customer> customerList = await _context.Customers.Skip((page - 1) * limit).Take(limit).ToListAsync();
            List<CustomerInfoResponse> res = new List<CustomerInfoResponse>();
            foreach (Customer customer in customerList)
            {
                CustomerInfoResponse customerInfo = new CustomerInfoResponse(customer);
                res.Add(customerInfo);
            }
            return res;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInfoResponse>> GetCustomerInfo(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerInfoResponse res = new CustomerInfoResponse(customer);
            return res;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerInfoResponse>> CreateNewCustomer(CustomerPostRequest body)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ProjectContext.Customers'  is null.");
            }

            var existCustomer = _context.Customers.FirstOrDefault(c => c.Username == body.Username);
            if (existCustomer != null) {
                return BadRequest("Username already existed");
            }

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            Customer customer = new () {
                Username = body.Username,
                Password = Customer.HashPassword(body.Password, salt),
                Address = body.Address,
                FirstName = body.FirstName,
                LastName = body.LastName,
                Email = body.Email,
                Phone = body.Phone,
                Salt = Convert.ToBase64String(salt),
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            CustomerInfoResponse res = new CustomerInfoResponse(customer);
            return CreatedAtAction("GetCustomerInfo", new { id = customer.Id }, res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id) {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}

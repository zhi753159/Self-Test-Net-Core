using api_server.Models;

namespace api_server.HTTPResponse
{
    public class CustomerInfoResponse
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public CustomerInfoResponse(Customer customer)
        {
            Id = customer.Id;
            Username = customer.Username;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Address = customer.Address;
            Phone = customer.Phone;
            Email = customer.Email;
        }
    }
}

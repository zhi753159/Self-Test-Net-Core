namespace api_server.HTTPRequest
{
    public class LoginRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public ValidateResult validate()
        {
            ValidateResult usernameValidate = RequestValidator.ValidateUsername(Username);
            if (!usernameValidate.valid) {
                return usernameValidate;
            }
            ValidateResult passwordValidate = RequestValidator.ValidatePassword(Password);
            if (!passwordValidate.valid)
            {
                return passwordValidate;
            }
            return new ValidateResult(true);
        }
    }
}

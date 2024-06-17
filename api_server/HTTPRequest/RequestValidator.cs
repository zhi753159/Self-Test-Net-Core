using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace api_server.HTTPRequest
{
    public partial class RequestValidator
    {

        [GeneratedRegex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)[A-Za-z\\d!%*#?&]{6,}$")]
        private static partial Regex PasswordRegex();

        [GeneratedRegex("^[A-Za-z][A-Za-z0-9]{3,}$")]
        private static partial Regex UsernameRegex();

        public static ValidateResult ValidateUsername(string username) {
            if (username == null) {
                return new ValidateResult(false, "Username is empty");
            }
            if (username.Length < 3)
            {
                return new ValidateResult(false, "Username length must greater than 6");
            }
            bool valid = UsernameRegex().Match(username).Success;
            if (!valid)
            {
                return new ValidateResult(false, "Username input is invalid");
            }
            return new ValidateResult(true);
        }

        public static ValidateResult ValidatePassword(string username)
        {
            if (username == null)
            {
                return new ValidateResult(false, "Password is empty");
            }
            if (username.Length < 6)
            {
                return new ValidateResult(false, "Password length must greater than 6");
            }
            bool valid = PasswordRegex().Match(username).Success;
            if (!valid)
            {
                return new ValidateResult(false, "Password input is invalid");
            }
            return new ValidateResult(true);
        }
    }
}

namespace api_server.HTTPRequest
{
    public class ValidateResult
    {
        public Boolean valid {  get; set; }
        public string? valid_error { get; set;}

        public ValidateResult(bool valid)
        {
            this.valid = valid;
        }

        public ValidateResult(bool valid, string? valid_error)
        {
            this.valid = valid;
            this.valid_error = valid_error;
        }
    }
}

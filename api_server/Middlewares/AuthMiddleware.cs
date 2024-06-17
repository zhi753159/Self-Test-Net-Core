namespace api_server.Middlewares
{
    public static class AuthMiddlewareExtensions
    {
        public static void UseAuth(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthMiddleware>();
        }
    }

    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var id = context.Session.GetInt32("UserId");
            if (id == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unathorized");
            } else
            {
                await _next(context);
            }
        }
    }
}

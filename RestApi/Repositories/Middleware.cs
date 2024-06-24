namespace RestApi.Repositories
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("X-Custom-Header", "MyHeaderValue");
            await _next(context);
        }
    }
}


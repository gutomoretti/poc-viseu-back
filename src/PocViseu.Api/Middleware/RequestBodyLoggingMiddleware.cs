namespace PocViseu.Api.Middleware
{
    public class RequestBodyLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Só vamos capturar o body se for uma requisição POST, PUT, etc.
            if (context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put)
            {
                // Ativa o buffering do stream, permitindo que ele seja lido várias vezes
                context.Request.EnableBuffering();

                // Lê o stream do body
                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();

                    // Armazena o body no HttpContext.Items para acesso posterior
                    context.Items["RequestBody"] = body;

                    // Volta a posição do stream para o início para que ele possa ser lido novamente
                    context.Request.Body.Position = 0;
                }
            }

            await _next(context);
        }
    }
}

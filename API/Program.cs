using Application.Services;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            EnvLoader.LoadEnvFile(".env");
#endif
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();
            
            var app = builder.Build();

            app.ConfigureMiddleware();
            
            app.Run();
        }
    }
}

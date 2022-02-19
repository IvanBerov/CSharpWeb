using Microsoft.EntityFrameworkCore;
using SharedTrip.Data;

namespace SharedTrip
{
    using System.Threading.Tasks;

    using MyWebServer;
    using MyWebServer.Controllers;

    using Controllers;
    using MyWebServer.Results.Views;

    public class Startup
    {
        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<ApplicationDbContext>())
                .WithConfiguration<ApplicationDbContext>(context => context
                    .Database.Migrate())
                .Start();
    }
}

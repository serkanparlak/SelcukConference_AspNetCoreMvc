using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.ConfigureServices(serviceCollection =>
                //    {
                //        serviceCollection.AddSingleton(new ResourceManager("WebUI.Resources.GlobalViews",
                //            typeof(Startup).GetTypeInfo().Assembly));
                //    })
                .UseStartup<Startup>()
                .Build();
    }
}

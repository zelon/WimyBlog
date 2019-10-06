using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TestWebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("Need root directory path");
                return;
            }
            string web_root_path = args[0];
            Console.WriteLine("Web root path: {0}", web_root_path);

            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseWebRoot(web_root_path)
                    .UseStartup<Startup>();
            }).Build().Run();            
        }
    }
}

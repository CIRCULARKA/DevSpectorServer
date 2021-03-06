using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DevSpector.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Directory.CreateDirectory("logs");
                DateTime now = DateTime.Now;

                File.AppendAllLines(
                    Path.Combine("logs", now.ToString("dd-MM-yyyy") + ".txt"),
                    new string[] {
                        $"[{DateTime.Now.ToString("HH:mm MMMM dd, yyyy")}]",
                        $"Message: {e.Message}",
                        $"Assembly: {e.Source}",
                        $"Method: {e.TargetSite}",
                        e.InnerException == null ? "No inner exception" : $"Inner exception message: {e.InnerException.Message}",
                        $"Stack trace:\n {e.StackTrace}",
                        "\n"
                    }
                );

                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
                ConfigureWebHostDefaults(
                    webBuilder => {
                        var port = Environment.GetEnvironmentVariable("DEVSPECTOR_SERVER_PORT");

                        webBuilder.UseStartup<Startup>().
                            UseUrls($"http://*:{port}");
                    }
                );
    }
}

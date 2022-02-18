using System;
using System.IO;
using Microsoft.Extensions.Configuration;
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
                File.AppendAllLines(
                    "UnhandledLogs.txt",
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
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).
                ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}

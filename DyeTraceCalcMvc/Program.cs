using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace DyeTraceCalcMvc
{

    /// <summary>
    /// Auto-build starter file for the site. Most of the work is done by Startup.cs.
    /// </summary>
    public class Program
    {



        /// <summary>
        /// Starts everything running.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }




        /// <summary>
        /// Ultimately runs Startup.cs
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }




}

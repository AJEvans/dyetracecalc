using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Io.Github.AJEvans.DyeTraceCalc.Shared;
using Microsoft.EntityFrameworkCore;

namespace DyeTraceCalcMvc
{


    /// <summary>
    /// Class that registers services to use, including the database context to use with them, 
    /// and also outlines how URL paths are taken to instantiate controllers and actions.
    /// </summary>
    public class Startup
    {


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">Configuration injection.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }




        /// <summary>
        /// System called.
        /// </summary>
        /// <value>Configuration info.</value>
        public IConfiguration Configuration { get; }




        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            string databasePath = Path.Combine("..", "parametersdb.db");
            // Context takes in a DBContextOptions object, but here we generate this using a factory object 
            // in turn made by an Sqlite factory method.
            services.AddDbContext<ParametersDB>(options => options.UseSqlite($"Data Source={databasePath}"));
        }




        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // AJE: Turned off to make it simpiler for people to run this exemplar. 
                //app.UseHsts();
            }

            // AJE: Again. This allows people to run http without having to install a https certificate server-side.
            //app.UseHttpsRedirection();

            // Don't actually use any at the moment.
            app.UseStaticFiles();

            app.UseRouting();

            // AJE: Again, so people don't have to log in.
            //app.UseAuthorization();

            // Sets up URL to controller and action parsing.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }




    }




}

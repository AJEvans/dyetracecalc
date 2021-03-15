using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DyeTraceCalcMvc.Models;
using Io.Github.AJEvans.DyeTraceCalc.Shared;
using Io.Github.AJEvans.DyeTraceCalc.Models;
using Io.Github.AJEvans.DyeTraceCalc.Calc;

namespace DyeTraceCalcMvc.Controllers
{

    /// <summary>
    /// Chief controller for site.
    /// </summary>
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private ParametersDB db;




        /// <summary>
        /// Constructor that injects the database dependency.
        /// </summary>
        /// <param name="logger">Unused here.</param>
        /// <param name="injectedContext">The database context.</param>
        public HomeController(ILogger<HomeController> logger, ParametersDB injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }




        /// <summary>
        /// The chief action for displaying the core webpage.
        /// </summary>
        /// <returns>An indication of the action's results.</returns>
        public IActionResult Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel
            {
                parameters = db.Parameters.ToList(),

            };

            return View(model);
        }




        /// <summary>
        /// The chief action for calculating the result, which is then displayed by the 
        /// index page.
        /// </summary>
        /// <param name="Increment">Increment value from web form.</param>
        /// <param name="Tolerance">Tolerance value from web form.</param>
        /// <param name="TimeOne">TimeOne value from web form.</param>
        /// <param name="TimeTwo">Increment value from web form.</param>
        /// <param name="Distance">Increment value from web form.</param>
        /// <returns>An indication of the action's results.</returns>
        public IActionResult Calc(string Increment, string Tolerance, string TimeOne, string TimeTwo, 
        string Distance)
        { 

            if ((Increment == null) || (Increment == "")) {Increment = "0.1";}
            if ((Tolerance == null) || (Tolerance == "")) {Tolerance = "0.01";}
            if ((TimeOne == null) || (TimeOne == "")) {TimeOne = "1000";}
            if ((TimeTwo == null) || (TimeTwo == "")) {TimeTwo = "1500";}
            if ((Distance == null)|| (Distance == "")) {Distance = "700";}

            // Don't want any negatives.
            Increment = Increment.Trim().Replace("-","");
            Tolerance = Tolerance.Trim().Replace("-","");
            TimeOne = TimeOne.Trim().Replace("-","");
            // No decimals for time.
            if (TimeOne.Contains(".")) {
                TimeOne = TimeOne.Remove(TimeOne.IndexOf("."));
            }
            TimeTwo = TimeTwo.Trim().Replace("-","");
            if (TimeTwo.Contains(".")) {
                TimeTwo = TimeTwo.Remove(TimeTwo.IndexOf("."));
            }
            Distance = Distance.Trim().Replace("-","");
            

            if (ModelState.IsValid)
            {
                // Model founded around Entity Framework representation of 
                // the database "parameters" table.
                HomeIndexViewModel model = new HomeIndexViewModel
                    {
                        parameters = db.Parameters.ToList()
                    };

                // Update model EF with values from the web form.   
                // Note that there's only one record/row in the database, 
                // hence use of [0].
                try
                {
                    model.parameters[0].PrimaryKey = 1;
                    model.parameters[0].Increment =  decimal.Parse(Increment);
                    model.parameters[0].Tolerance = decimal.Parse(Tolerance);
                    model.parameters[0].TimeOne = int.Parse(TimeOne);
                    model.parameters[0].TimeTwo = int.Parse(TimeTwo);
                    model.parameters[0].Distance = decimal.Parse(Distance);
                    // TimeOne should be before TimeTwo.
                    if (model.parameters[0].TimeOne >  model.parameters[0].TimeTwo) 
                    {
                        int temp = model.parameters[0].TimeTwo;
                        model.parameters[0].TimeTwo = model.parameters[0].TimeOne;
                        model.parameters[0].TimeOne = temp;
                    }
                } 
                catch 
                {
                    model.parameters[0].Time = "please be sure to enter numerical figures";
                    model.parameters[0].Dispersion = "";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                // Possibly better implemented as a service?
                var results = new Calculator().GetResults (
                                    model.parameters[0].Increment, 
                                    model.parameters[0].Tolerance,
                                    model.parameters[0].TimeOne,
                                    model.parameters[0].TimeTwo,
                                    model.parameters[0].Distance,
                                    false); 

                // We store the result in the database to make it 
                // easier to then extract into the index page.
                model.parameters[0].Time = results.Time + " minutes";
                model.parameters[0].Dispersion = results.Dispersion;

                // Update the database with new values that the  
                // user may have changed via the website form and the 
                // results.
                db.SaveChanges();

                // Redirect to index, which will show the results.
                return RedirectToAction("Index");

            } else {

                return RedirectToAction("Index");

            }

        }




    }




}

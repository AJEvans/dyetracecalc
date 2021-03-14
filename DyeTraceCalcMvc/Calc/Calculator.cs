using System;
using System.Collections.Generic;


namespace Io.Github.AJEvans.DyeTraceCalc.Calc 
{

    /// <summary>
    /// Core processing class. Calculates dye dispersion coefficients.
    /// </summary>
    /// <remarks>
    /// This does the work of converting the parameters from the database and/or 
    /// user into answers.
    /// It calculates values associated with the dispersion of a cloud of dye as it travels through a 
    /// water system. 'Time one' is the time to reach half the peak value in minutes, 
    /// 'time two' is the time from the start for it to fall back to half the peak value after the peak 
    /// has passed. The output includes the estimate of the peak time and the dispersion 
    /// coefficient. Basically it estimates the diffusivity iteratively 
    /// until it stabilizes within a tolerance. For more details, 
    /// see the associated website (/Views/Home/Index.cshtml)
    /// This is a direct port of Java code written by me in 1998 for Dr Mike Crabtree's PhD work 
    /// in Iceland (https://www.sheffield.ac.uk/economics/staff/professional/mike-crabtree). At the 
    /// time it was based on a widely used Excel version which didn't optimise for a single result. It has 
    /// to be said that I haven't looked at it since then, and one would 
    /// hope there are better ways of calculating this now. Before using it in a real situation 
    /// I'd take time to double check the calculations.
    /// </remarks>
    public class Calculator 
    {

        // Apologies, horrible use of a mix of instance and method variables in this code, 
        // and almost no comments - really needs a thorough going through - probably 
        // amongst the first Java code I wrote!
        private decimal maxTime;
        private decimal minTime;
        private List<decimal> results = new List<decimal> ();
        private List<decimal> oldResults = new List<decimal> ();



        /// <summary>
        /// Public access to the functionality of this class.
        /// </summary>
        /// <remarks>
        /// The output includes the estimate of the peak time and the dispersion 
        /// coefficient. For more details, see the associated website (/Views/Home/Index.cshtml).
        /// </remarks>
        /// <param name="increment">Time increment to use in calculations.</param>
        /// <param name="tolerance">Tolerance controlling when iterative calculations are assumed complete.</param>
        /// <param name="time1">The time to reach half the peak value in minutes.</param>
        /// <param name="time2">The time from start to reach half the peak value again in minutes after the peak has passed.</param>
        /// <param name="distance">Distance between dye input and measurements of concentration.</param>
        /// <param name="optimise">Whether to make a one-off estimate or return multiple results. Here, for ease of understanding, we set 
        /// this to 'true' removing the option for outputting multiple results for bespoke averaging etc.</param>
        /// <returns>
        /// A tuple containing:
        /// Time: An estimate of the peak concentration time.
        /// Dispersion: An estimate of the dispersion coefficient.
        /// </returns>
        public (string Time, string Dispersion) GetResults (decimal increment, 
                                  decimal tolerance,
                                  int time1,
                                  int time2,
                                  decimal distance,
                                  bool optimise) {


            minTime = time1;
            maxTime = time2;
            
            int size;
            
            if (!optimise) {
                Iterate (time1, time2, increment, tolerance, distance);
            } else {
                bool firstTime = true;
                for (;;) {
                    size = oldResults.Count;
                    oldResults = results;
                    results.Clear();
                    Iterate (time1, time2, increment, tolerance, distance);
                    if ((results.Count == 0) || ((results.Count >= size ) && (!firstTime))) {
                        results = oldResults;
                        break;
                    }
                    increment = increment / 10;
                    firstTime = false;
                }
                
                tolerance = tolerance / 10;
                firstTime = true;
                
                for (;;) {
                    size = oldResults.Count;
                    oldResults = results;
                    results.Clear();
                    Iterate (time1, time2, increment, tolerance,distance);
                    if ((results.Count == 0) || (results.Count >= size )) {
                        results = oldResults;
                        if (firstTime) {
                            tolerance *= 10;
                        }
                        break;
                    }
                    tolerance = tolerance / 10;
                    firstTime = false;
                } 

            }
            
            string a = "";
            string b = "";

            /*
            If optimisation is turn off the method will generate multiple results, although this will currently 
            break the code as it returns a tuple not a list of tuples. I've left the code here incase I want to come 
            back to this. If the non-optimise result doesn't settle, a warning that more than 300 iterations have occurred 
            can be generated. It should be noted that this might be adjusted - back when this was written, 300 iterations took 
            a long time!
            */
            //if (results.Count > 300) {
            //    a = "Greater than 300 results";
            //    b = "";
            //} else {
                for (int j = 0; j < results.Count;) {
                    a = results[j].ToString();
                    b = results[j+1].ToString();
                    j += 2;
                }
            //}
            
            results.Clear(); 
        
            // To implement the non-optimised version, return results.
            return (Time: a, Dispersion: b);

        } 
            



        /// <summary>
        /// Chief processing iteration.
        /// </summary>
        /// <param name="time1">The time to reach half the peak value in minutes.</param>
        /// <param name="time2">The time to reach half the peak value again in minutes after the peak has passed.</param>
        /// <param name="increment">Time increment to use in calculations.</param>
        /// <param name="tolerance">Tolerance controlling when iterative calculations are assumed complete.</param>
        /// <param name="distance">Distance between dye input and measurements of concentration.</param>
        private void Iterate (
                        decimal time1, 
                        decimal time2, 
                        decimal increment, 
                        decimal tolerance, 
                        decimal distance) {
        
            bool first;
            first = true;
            
            
            for (int i = 0; i < ((maxTime - minTime) / increment); i++) {
                
                decimal peakEventTime = minTime + (increment * i); 
                
                decimal left = Diffusivity (distance, peakEventTime, time1);
                decimal right = Diffusivity (distance, peakEventTime, time2);
                
                if (left >= (right - tolerance)) {
                    if (first) {
                        minTime = peakEventTime;
                        first = false;
                    }
                    if (left <= (right + tolerance)) {
                        results.Add(peakEventTime);
                        results.Add(Math.Round(left, 3, MidpointRounding.AwayFromZero));
                    } else {
                        maxTime = peakEventTime;
                        return;
                    }
                }
            }
        } 
        
        
        
        
        /// <summary>
        /// Main calculation.
        /// </summary>
        /// <param name="distance">Distance from dye input to concentration measurement.</param>
        /// <param name="peakEventTime">Estimate of time of peak concentrartion.</param>
        /// <param name="time">Current time in calculation.</param>
        /// <returns>Estimate of diffusivity coefficient.</returns>
        private decimal Diffusivity (decimal distance, decimal peakEventTime, decimal time) {

            decimal sqRoot = (decimal)Math.Sqrt((double)(peakEventTime / time));
            decimal logVar = (decimal)Math.Log((double)(2 * sqRoot));
            decimal diffuse = (    (distance * distance)*((peakEventTime - time)*(peakEventTime - time)) / 
                        ((4 * (peakEventTime * peakEventTime)) * time * logVar)  );
            return diffuse;
        }
                
                

    }




}

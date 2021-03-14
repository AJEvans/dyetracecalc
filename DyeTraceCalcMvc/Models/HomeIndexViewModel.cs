using System.Collections.Generic;
using Io.Github.AJEvans.DyeTraceCalc.Shared;

namespace Io.Github.AJEvans.DyeTraceCalc.Models
{

    /// <summary>
    /// Core model for this site. Builds a list representing records/rows in 
    /// the Parameters database (though only one row is ever present).
    /// </summary>
    /// <remarks>
    /// For details of the fields in the database rows, see 
    /// See <see cref="Io.Github.AJEvans.DyeTraceCalc.Shared.Parameter"/>
    /// </remarks>
    public class HomeIndexViewModel 
    {
        /// <summary>
        /// A list representing records/rows in 
        /// the Parameters database (though only one row is ever present).
        /// </summary>
        /// <value>Injected from database.</value>
        public IList<Parameter> parameters { get; set; }

    }




}
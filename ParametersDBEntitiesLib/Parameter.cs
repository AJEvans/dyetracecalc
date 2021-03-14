using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable


namespace Io.Github.AJEvans.DyeTraceCalc.Shared
{
    /// <summary>
    /// Entity Framework translation class between database and C#. Represents the 
    /// records in the database's "Parameters" table.
    /// </summary>
    public partial class Parameter
    {


        /// <summary>
        /// This is just added as a primary key because updates don't work otherwise.
        /// </summary>
        /// <value>0: unused otherwise.</value>
        [Key]
        public int PrimaryKey { get; set; }


        /// <summary>
        /// Increment parameter.
        /// </summary>
        /// <value>Variable</value>
        [Column(TypeName = "DECIMAL")]
        public decimal Increment { get; set; }


        /// <summary>
        /// Tolerance parameter.
        /// </summary>
        /// <value>Variable</value>
        [Column(TypeName = "DECIMAL")]
        public decimal Tolerance { get; set; }


        /// <summary>
        /// TimeOne parameter.
        /// </summary>
        /// <value>Variable</value>
        public int TimeOne { get; set; }


        /// <summary>
        /// TimeTwo parameter.
        /// </summary>
        /// <value>Variable</value>
        public int TimeTwo { get; set; }


        /// <summary>
        /// Distance parameter.
        /// </summary>
        /// <value>Variable</value>
        [Column(TypeName = "DECIMAL")]
        public decimal Distance { get; set; }


        /// <summary>
        /// Time result.
        /// </summary>
        /// <value>Calculated by the system</value>
        public string Time { get; set; }


        /// <summary>
        /// Dispersion result.
        /// </summary>
        /// <value>Calculated by the system</value>
        public string Dispersion { get; set; }




    }



}

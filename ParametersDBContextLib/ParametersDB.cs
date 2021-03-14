using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Io.Github.AJEvans.DyeTraceCalc.Shared
{
    /// <summary>
    /// Database context class used in linking Entity Framework classes to 
    /// the database.
    /// </summary>
    /// <remarks>
    /// Auto generated file.
    /// </remarks>
    public partial class ParametersDB : DbContext
    {


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ParametersDB()
        {
        }




        /// <summary>
        /// Constructor with options. These are built by a factory method.
        /// </summary>
        /// <param name="options"></param>
        public ParametersDB(DbContextOptions<ParametersDB> options)
            : base(options)
        {
        }




        /// <summary>
        /// Set of Entity Framework classes representing records in the Parameters table in 
        /// the database.
        /// </summary>
        /// <value></value>
        public virtual DbSet<Parameter> Parameters { get; set; }





        /// <summary>
        /// Chief connection to database.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=../parametersdb.db");
            }
        }




        /// <summary>
        /// Attachment to models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.Property(e => e.PrimaryKey).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }




        /// <summary>
        /// Unused here.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);




    }




}

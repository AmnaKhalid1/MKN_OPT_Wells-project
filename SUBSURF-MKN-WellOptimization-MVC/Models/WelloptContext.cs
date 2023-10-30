using SUBSURF_MKN_WellOptimization_MVC.Migrations;
using SUBSURF_MKN_WellOptimization_MVC.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
    public partial class WelloptContext : DbContext
    {
        public WelloptContext()
            : base("name=WelloptContext")
        {
        }


        public virtual DbSet<Wellopt> Wellopts { get; set; }

      
    
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

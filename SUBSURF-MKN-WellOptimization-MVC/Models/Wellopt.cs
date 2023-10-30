using SUBSURF_MKN_WellOptimization_MVC.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations.Schema;


namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
    public class Wellopt
    {

        [Display(Name = "ID")]
        [Required(ErrorMessage = "Required Field")]
        public int Id { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RecordDate { get; set; }

        [Display(Name = "UWI")]
        public string UWI { get; set; }

        [Display(Name = "Well Name")]
        [Required(ErrorMessage = "Required Field")]
        public string WellName { get; set; }

        [Display(Name = "Pump Type")]
        [Required(ErrorMessage = "Required Field")]
        public string PumpType { get; set; }

        [Display(Name = "Optimization Types")]
        [Required(ErrorMessage = "Required Field")]
        public OPTIMIZATIONTYPE OPTIMIZATIONTYPES { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        public string WellToString() {
            return WellName + "|" + PumpType + "|" + Comments+"|"+OPTIMIZATIONTYPES+"|"+RecordDate;
        }
    }
}
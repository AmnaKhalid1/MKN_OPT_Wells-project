using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SUBSURF_MKN_WellOptimization_MVC.Models

{
    public enum ActionType{
        Edit,
        create,
        Delete
    }
    public class ActionLog
    {

        [Display(Name = "ID")]
        [Required(ErrorMessage = "Required Field")]
        public int Id { get; set; }

        [Display(Name = "Log Date")]
        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy HH:mm  tt}", ApplyFormatInEditMode = true)]
        public DateTime LogDate { get; set; }


        [Display(Name = "Well Record Id")]
        [Required(ErrorMessage = "Required Field")]
        public int RecordId { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Required Field")]
        public string Username { get; set; }

        [Display(Name = " Action Type")]
        [Required(ErrorMessage = "Required Field")]
        public ActionType actionType { get; set; }

        [Display(Name = " Previous Record")]
        public string PreviousRecord { get; set; }


        [Display(Name = "UWI")]
        [Required(ErrorMessage = "Required Field")]
        public string UWI { get; set; }
    }
}
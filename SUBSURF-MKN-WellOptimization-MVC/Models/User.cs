using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
    public class User
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "Required Field")]
        public int Id { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Required Field")]
        public string Role { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUBSURF_MKN_WellOptimization_MVC.Models.APIObj
{
    public class Role
    {
        public int roleID { get; set; }
        public string roleName { get; set; }
        public int sysID { get; set; }
        public Sys sys { get; set; }
    }
}
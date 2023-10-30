using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUBSURF_MKN_WellOptimization_MVC.Models.APIObj
{
    public class Employees
    {
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public Role[] roles { get; set; }

    }
}
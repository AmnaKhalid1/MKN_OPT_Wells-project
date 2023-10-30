
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
    static internal class Records
    {
        public static List<Wellopt> wells = new List<Wellopt>();

        public static Wellopt welloptBefore = new Wellopt ();
    }
}
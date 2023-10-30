using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using F23.StringSimilarity;

namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
    public  class DBActivity
    {

        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }



        [Required(ErrorMessage = "Required Field")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }


        public static string GetSimilarWell(string wellName)
        {
            string Recommended = "";
            SqlConnection c = new SqlConnection();


            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                List<string> shortNames = new List<string> ();
                List<string> LongNames = new List<string> ();

                List<double> shortNamesSimi = new List<double>();
                List<double> LongtNamesSimi = new List<double>();

                var l = new Levenshtein();

                

                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                //
                q.CommandText = $"SELECT * from MasterWells";

                SqlDataReader read = q.ExecuteReader();

                while (read.Read())
                {
                    shortNames.Add(read["PLOTNAME"].ToString().Trim());
                    LongNames.Add(read["WELLNAME"].ToString().Trim());
                }

                read.Close();

                for (int i = 0; i < shortNames.Count; i++)
                {

                    shortNamesSimi.Add(l.Distance(wellName, shortNames[i]));
                    LongtNamesSimi.Add(l.Distance(wellName, LongNames[i]));
                }
                double short_min = shortNamesSimi.Min();
                double long_min = LongtNamesSimi.Min();

                string all_short = "";
                string all_Long = "";

                for (int i = 0; i < shortNamesSimi.Count; i++)
                {
                    if (short_min == shortNamesSimi[i])
                    {

                        all_short +=  shortNames[i]+ "  , ";
                    }
                    if (long_min == LongtNamesSimi[i])
                    {
                        all_Long +=  LongNames[i]+ "  , ";
                    }

                }


                if (short_min <= long_min)
                {
                    //Recommended = shortNames[shortNamesSimi.IndexOf(short_min)];
                    Recommended = "Well ShortName " + all_short;
                    


                }
                else
                {
                    Recommended = "Well Long Name " + all_Long;
                    //Recommended = LongNames[LongtNamesSimi.IndexOf(long_min)];
                   

                }

            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

            return Recommended;
        }

    }
}
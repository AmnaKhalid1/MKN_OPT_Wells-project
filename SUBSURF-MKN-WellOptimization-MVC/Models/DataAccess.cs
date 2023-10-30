
using Newtonsoft.Json;
using SUBSURF_MKN_WellOptimization_MVC.Models.APIObj;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls.WebParts;

namespace SUBSURF_MKN_WellOptimization_MVC.Models
{
     static class DataAccess
    {

        public static string Username = "";
        public static string Role = "Guest";

        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        public static string GetSimilarWell(string wellname)
        {
            string Recommended = "";
            SqlConnection c = new SqlConnection();


            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                //
                q.CommandText = $"SELECT * from MasterWells where ( DIFFERENCE( PLOTNAME ,'{wellname}') *25 )> 90 or  (DIFFERENCE( WELLNAME ,'{wellname}') *25) >75";

                SqlDataReader read = q.ExecuteReader();

                while (read.Read())
                {
                    Recommended +="||||"+ read["PLOTNAME"].ToString();

                }

                read.Close();
            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

            return Recommended;
        }




        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        public static string GetROW(string wellname)
        {
            string row = null;
            SqlConnection c = new SqlConnection();


            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                q.CommandText = $"Select * from MasterWells where WELLNAME = '{wellname.Trim()}' or PLOTNAME = '{wellname.Trim()}'";

                SqlDataReader read = q.ExecuteReader();

                if (read.Read())
                {
                    row = read["PROJECT"].ToString();

                }


                read.Close();
            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

            return row;




        }
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        public static string GetUWI(string wellname) {
            string Uwi = null;
            
            SqlConnection c = new SqlConnection();


            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                q.CommandText = $"Select * from MasterWells where WELLNAME = '{wellname.Trim()}' or PLOTNAME = '{wellname.Trim()}' ";

                SqlDataReader read = q.ExecuteReader();

                if (read.Read())
                {
                    string Uwi_full = read["PID"].ToString();
                    //Uwi = Uwi_full.Substring(0,Uwi_full.Length-2);
                    Uwi = Uwi_full;
                }


                read.Close();
            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

            return Uwi;

        }

        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        public static bool checkWellName(string wellname) {
            bool exist = false;
            SqlConnection c = new SqlConnection();
            

            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                q.CommandText = $"Select * from MasterWells where WELLNAME = '{wellname.Trim()}' or PLOTNAME = '{wellname.Trim()}'";

                SqlDataReader read = q.ExecuteReader();
                
                if (read.Read() ) {
                   exist = true; 
                    
                }


                read.Close();
            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

            return exist;
        }



        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        public static  string GetWellName() {
            SqlConnection c = new SqlConnection();
               
            c.ConnectionString = @"Server=NMOWSQL8;Database=MUKHAIZNA;User Id=muk_read;Password=muk_read;TrustServerCertificate=True";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                q.CommandText = "Select * from MasterWells";

                SqlDataReader read = q.ExecuteReader();
                while (read.Read())
                {
                    Message += $"{read["WELLNAME"]} \n {read["PROJECT"]}";
                }
                read.Close();
            }
            catch (Exception ee) { Message= $"Error: {ee.Message}"; }

            return Message;
        
        }

        //_____________________________________________________________________________________
        //_____________________________________________________________________________________
        //_____________________________________________________________________________________


        static string sys_name = "SUBSURF_MKN_WellOptimization_MVC";
        static string compared = "Contributor";

        static public bool IsContributor(Employees em)
        {
            bool cont = false;
            foreach (Role r in em.roles)
            {
                if (r.roleName == compared && r.sys.sysName == sys_name) { cont = true; }
            }
            return cont;

        }

        public static async Task GetRole(string username) {



            string role = "Guest No response";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    UseDefaultCredentials = true
                };


                if (username.Contains('\\')) { username = username.Split('\\')[1]; }
                 
                 Username = username;
                Employees ems = new Employees();
                using (HttpClient client = new HttpClient(handler, disposeHandler: true))
                {
                    HttpResponseMessage response = await client.GetAsync($"http://mknss-dev.oxy.com:8012/api/Employees/{username}");

                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {

                        //Storing the response details recieved from web api
                        var EmpResponse = response.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list
                        ems = JsonConvert.DeserializeObject<Employees>(EmpResponse);

                        if (IsContributor(ems)) { Role = "Contributor"; }
                        
                    }
                    else
                    {
                        role = "unsuccessful";
                    }
                }
            }
            catch (HttpRequestException e)
            {
                role = "Error :" + e.Message;
            }
            



        }




       /* public static void GetRole(string userName) {
           // if (userName == @"NAOXY\alnadabak") { Role = "Contributor"; }
            Username = userName;


            SqlConnection c = new SqlConnection();

            c.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test2;Integrated Security=True;Pooling=False";
            string Message = "";
            try
            {
                c.Open();
                SqlCommand q = new SqlCommand();
                q.Connection = c;
                q.CommandText = $"Select * from [User] where username = '{userName.Trim()}'";

                SqlDataReader read = q.ExecuteReader();
                if (read.Read())
                {
                    if (read["role"].ToString() == "Contributor") { Role = read["role"].ToString(); }
                   
                }
                read.Close();
            }
            catch (Exception ee) { Message = $"Error: {ee.Message}"; }

        }
        */

    }
}
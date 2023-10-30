using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ExcelDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System;
using System.IO;

namespace ExcelDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult v(string RecordDate, string Optimization, IFormFile file)
        {
            string all = "";
            if (file == null) { return Content("null file"); }
            using (FileStream fileStream = System.IO.File.Create(file.FileName))
            {

                file.CopyTo(fileStream);
                fileStream.Flush();
            }//should delete the file after finishing 

            //Note user should enter the data with the exact order as follows
            //WELL_Name	Pump_Type	Comments

            // Open the document as read-only.
            using (SpreadsheetDocument spreadsheetDocument =
                SpreadsheetDocument.Open(file.FileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();

// SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
               
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
               
                foreach (Row row in rows)
                {
                    foreach(Cell c in row)
                    {
                       all= all + c.CellValue.Text.ToString();
                    }
                }
                

            }
            ViewBag.all = all;

            //delete the file 
            if (System.IO.File.Exists(file.FileName))
            {
                // Delete the file
                System.IO.File.Delete(file.FileName);
            }

            return View();
        }


        [HttpPost]
        public IActionResult Index( string RecordDate, string Optimization, IFormFile file)
        {
           
            if (file == null) { return Content("null file"); }
            using (FileStream fileStream = System.IO.File.Create(file.FileName))
            {

                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            //Note user should enter the data with the exact order as follows
            //WELL_Name	Pump_Type	Comments

            // Open the document as read-only.
            using (SpreadsheetDocument spreadsheetDocument =
                SpreadsheetDocument.Open(file.FileName, false))
            {

                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();


                string text;
                string all = "";
                foreach (Row r in sheetData.Elements<Row>())
                {
                     string name = r.Elements<Cell>().ElementAt(0).CellValue.Text.ToString();
                     string typee = r.Elements<Cell>().ElementAt(1).CellValue.Text.ToString();
                     string comment = r.Elements<Cell>().ElementAt(2).CellValue.Text.ToString();
                    /*
                       foreach (Cell c in r.Elements<Cell>())
                    {

                        text = c.CellValue.Text.ToString();
                        all = all +" "+ text;
                    }

                     */


                    //construct wellopt object and add it to the database 
                    //note that ROW and UWI will be retrived from different database
                    string Row = "22";
                    string UWI = "q123";
                    ViewBag.name = "Name : "+name+" Type  :"+ typee+ " comment : "+ comment;
                }
                all = all + RecordDate + Optimization;
                ViewBag.all = all;
            }
            /*
              if (System.IO.File.Exists(file.FileName))
            {
                // Delete the file
                System.IO.File.Delete(file.FileName);
            }

             */


            return View();
        }
















        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Upload()
        {
            return View();
        }
        /*
         
         
         public IActionResult Upload(string RecordDate, string Optimization, IFormFile file, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (file != null)
            {
                string filename = $"{hostingEnvironment.WebRootPath}\\{file.FileName}";
                using (FileStream fileStream = System.IO.File.Create(filename))
                {

                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }




                // Open the document as read-only.
                using (SpreadsheetDocument spreadsheetDocument =
                    SpreadsheetDocument.Open(filename, false))
                {

                    WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    string text;
                    string all = "";
                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        foreach (Cell c in r.Elements<Cell>())
                        {
                            text = c.CellValue.Text;
                            all = all + text;
                        }
                    }
                    ViewBag.all = all;
                }
            }
            return View();
        }
         
         
         */
         
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
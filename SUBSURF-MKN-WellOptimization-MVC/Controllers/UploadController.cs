using Microsoft.AspNetCore.Http;
using SUBSURF_MKN_WellOptimization_MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using SUBSURF_MKN_WellOptimization_MVC.Models.Enums;
using DocumentFormat.OpenXml.EMMA;
using System.Web.Routing;
using System.Web.WebPages;
using System.Data.Entity.Validation;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;

namespace SUBSURF_MKN_WellOptimization_MVC.Controllers
{
    public class UploadController : Controller
    {
        private WelloptContext db = new WelloptContext();

        public UploadController() {


            ViewBag.role = DataAccess.Role;

        }
        public ActionResult UploadToDatabase() {
            string error = null;
            string success = null;
            string WellNamecheck = null;
            try
            {
                int count = 1;
                bool first = true;
                foreach (Wellopt well in Records.wells)
                {

                    count += 1;
                    if (!DataAccess.checkWellName(well.WellName) && first)
                    {
                        WellNamecheck += $"Error couldn't find well name at row {count},name {well.WellName} \n";
                        first = false;
                    }

                    else if (!DataAccess.checkWellName(well.WellName))
                    {
                        WellNamecheck += $", row {count} ,name {well.WellName} ";
                    }
                    //to add the similar name
                    if (!DataAccess.checkWellName(well.WellName))
                    {
                        string simi = DBActivity.GetSimilarWell(well.WellName);
                        if (simi != "") { WellNamecheck += " A similar may be the " + simi + "<br>"; }


                    }
                }

                if (Records.wells != null && WellNamecheck == null)
                {
                    db.Wellopts.AddRange(Records.wells);
                    db.SaveChanges();
                    success = "The records uploaded Successfully.";
                    ViewBag.success = success;

                    foreach (Wellopt w in Records.wells)
                    {
                        //add the create action to the database
                        ActionLog actionLog = new ActionLog()
                        {
                            LogDate = DateTime.Now,
                            RecordId = w.Id,
                            Username = DataAccess.Username,
                            actionType = ActionType.create,
                            UWI = w.UWI,
                            PreviousRecord = w.WellToString()
                        };

                        db.ActionLogs.Add(actionLog);
                        db.SaveChanges();

                    }


                    return View("Index");
                }
                else
                {
                    ViewBag.WellNameCheck = WellNamecheck;
                    return View("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    error += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:";

                    foreach (var ve in eve.ValidationErrors)
                    {
                        error += $"- Property: \"{ ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";


                    }
                }
                ViewBag.dbError = error;
                return View("Index");
            }
            finally {

                //empty the list after finishing
                Records.wells = null;
            }
           

        }

        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________


        [HttpGet]
        public ActionResult UploadView(List<Wellopt> records)
        {

            if (records == null || records.Count ==0) return Content("Null passed");
            ViewBag.opts = records[0].WellName;
            return View();
        }


        // GET: Upload
        [HttpGet]
        public ActionResult Index()
        {
            if (DataAccess.Role != "Contributor")
            {
                return RedirectToRoute(new
                {
                    controller = "AccessDenied",
                    action = "AccessDenied"

                });

            }

            return View();
        }
        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________


        // function to get value of cell in excel sheet
        private string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = "";
            if (cell.CellValue != null)
            { value = cell.CellValue.InnerText; }
        
            //cell.DataType != null && 
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________
        public static string GetColumnName(string cellReference)
        {
            // Match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "RecordDate,OPTIMIZATIONTYPES")] Wellopt wellopt, HttpPostedFileBase file)
        {
            

                List<Wellopt> records = new List<Wellopt>();
                //these two varibles should be retrived from a database 

               

                ViewBag.error = string.Empty;
                string filePath = string.Empty;
            if (file == null) { return View(); }

                else
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);

                    try
                    {
                        file.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        TempData["notice"] = "fail";
                        TempData["msg"] = "Couldn't upload the file, please make sure it's not open.";
                        return RedirectToAction("Upload");
                    }
                    try
                    {


                        // Open the document as read-only.
                        using (SpreadsheetDocument spreadsheetDocument =
                        SpreadsheetDocument.Open(filePath, false))
                        {
                            //Read the first Sheet from Excel file.
                            Sheet sheet = spreadsheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                            //Get the Worksheet instance.
                            Worksheet worksheet = (spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                            //Fetch all the rows present in the Worksheet.
                            IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();


                            
                            int count = 1;
                       
                            //Loop through the Worksheet rows.
                            foreach (Row row in rows)
                            {
                            /*
                             The coulmns Must be satisfied for this to work:
                            A==>WELL_Name
                            B==>Pump_Type
                            C==>Comments
                             */

                            //set defualt values
                            string comment = "";
                            string Pump_Type = "";
                            string WELL_Name = "";

                           
                            if (count != 1)//skip header
                                {

                                foreach (Cell cell in row)
                                {
                                    if (cell != null)
                                    {
                                        string columnName = GetColumnName(cell.CellReference);
                                        switch (columnName)
                                        {
                                            case "A":
                                                // code block
                                                WELL_Name = GetValue(spreadsheetDocument, cell);
                                                break;

                                            case "B":
                                                // code block
                                                Pump_Type = GetValue(spreadsheetDocument, cell);
                                                break;

                                            case "C":
                                                comment = GetValue(spreadsheetDocument, cell);
                                                break;
                                        }


                                    }
                                }
                                /*
                                 
                                try { 
                                    WELL_Name = GetValue(spreadsheetDocument, row.Descendants<Cell>().ElementAt(0));
                                    ViewBag.error = GetColumnName (row.Descendants<Cell>().ElementAt(0).CellReference);

                                } catch (Exception ex1) { WELL_Name = ""; }

                                
                                 if (row.Count() < 3)
                                    {
                                        //comment can be empty
                                        comment = "";
                                    }
                                    else
                                    {
                                        comment = GetValue(spreadsheetDocument, row.Descendants<Cell>().ElementAt(2));
                                    }
                                 */
                                string UWI = DataAccess.GetUWI(WELL_Name);


                                    //construct a well opbject from the collected info
                                    Wellopt opt = new Wellopt
                                    {
                                        WellName = WELL_Name,
                                     
                                        UWI = UWI,
                                        Comments = comment,
                                        PumpType = Pump_Type,
                                        RecordDate = wellopt.RecordDate,
                                        OPTIMIZATIONTYPES = wellopt.OPTIMIZATIONTYPES
                                    };

                                    records.Add(opt);

                                }
                              

                                count++;

                            }

                        }

                    }
                    catch (Exception ex) { ViewBag.error = "Error :" + ex.Message.ToString(); }



                }


                //delete the file 
                if (System.IO.File.Exists(filePath))
                {
                    // Delete the file
                    System.IO.File.Delete(filePath);
                }
                //View the records

                ViewBag.records = records;
                Records.wells = records;
                return View();

            }
            
            
            
        }
           
    


     




    }



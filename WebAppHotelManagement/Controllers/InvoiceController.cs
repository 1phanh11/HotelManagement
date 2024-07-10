using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppHotelManagement.Models;
using WebAppHotelManagement.ViewModel;
using ClosedXML.Excel;
using System.IO;

namespace WebAppHotelManagement.Controllers
{
    //[Authorize(Roles = ("Admin,Staff"))]
    public class InvoiceController : Controller
    {
        private HotelDBEntities objHotelDBEntities;

        public InvoiceController()
        {
            objHotelDBEntities = new HotelDBEntities();
        }
        // GET: Invoice

        public ActionResult Index(int i = -1)
        {

            var iplInvoice = new InvoiceViewModel();
            var model = iplInvoice.ListAll();
            if (i == 1)
            {
                model = iplInvoice.ListAll_1();
            }
            if (i == 0)
            {
                model = iplInvoice.ListAll_0();
            }

            return View(model);
        }


        public ActionResult ChangeActive(int id)
        {

            return View(objHotelDBEntities.Invoids.Find(id));
        }

        [HttpPost]
        public ActionResult ChangeActive(int id, Invoid invoid)
        {
            var iplInvoice = new InvoiceViewModel();
            //iplInvoice.ChangeActive(id);
            Invoid inv = objHotelDBEntities.Invoids.Single(model => model.InvoidID == id);
            inv.IsActive = false;
            inv.DateP = DateTime.Now;
            objHotelDBEntities.SaveChanges();
            Cal_Montly();
            FileResult f = ExpoctEcell();
            
            return RedirectToAction("Index");
        }


        public void Cal_Montly()
        {

            DateTime dateTimeS = new DateTime(2023, 3, 13);
            DateTime dateTimeE = DateTime.Now;
            decimal total = (decimal)(from o in objHotelDBEntities.Invoids where o.DateP >= dateTimeS && o.DateP <= dateTimeE select o.ToTalPayment).Sum();
            objHotelDBEntities.MonthlyRs.Add(new MonthlyR { MonthE = dateTimeE, MonthS = dateTimeS, Total = total });
            objHotelDBEntities.SaveChanges();
        }

        public FileResult ExpoctEcell()
        {
            DateTime dateTimeS = new DateTime(2023, 3, 13);
            DateTime dateTimeE = DateTime.Now;

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("ID Invoice"),
                                            new DataColumn("ID Booking"),
                                            new DataColumn("DateP"),
                                            new DataColumn("Book Payment"),
                                            new DataColumn("Service"),
                                            new DataColumn("ToTal")});
            var invoiceList = from o in objHotelDBEntities.Invoids where o.DateP >= dateTimeS && o.DateP <= dateTimeE select o;
            decimal i = 0;
            foreach (var iv in invoiceList)
            {
                dt.Rows.Add(iv.InvoidID,iv.BookingID,iv.DateP,iv.BookingAmount,iv.ServiceAmount,iv.ToTalPayment);
                i += (decimal)iv.ToTalPayment;
            }
            dt.Rows.Add("", "", "", "", "Total:", i);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }
}
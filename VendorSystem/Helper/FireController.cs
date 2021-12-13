using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Mvc;
using VendorSystem.Helper;
using VendorSystem.Models.Model1;
using VendorSystem.Models.Model2;
using VendorSystem.Repository;
using VendorSystem.ViewModel;

namespace VendorSystem.Helper
{
    public class FireController : Controller
    {
        string Pass = "QBS@2020"; // it will be configuration and get from database depending on every Company ID

        BayanEntities db = new BayanEntities();

        [Route("Fire/Firing")]
        public JsonResult Firing(string Password)
        {
            if (Password != Pass)
                return Json(false, JsonRequestBehavior.AllowGet);

            Thread Th = new Thread(UploadAndDownloadData);
            Th.Start();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        private void UploadAndDownloadData()
        {
            try
            {
                var Date = DateTime.Now.AddDays(-3).Date;
                db.RemoveOldJobs(Date);
                db.SaveChanges();


                var LastJob = db.Tbl_Jobs.Where(w => w.Description.StartsWith("StartPrccess")).OrderByDescending(w => w.FiringDate).FirstOrDefault();
                var LastComplete = db.Tbl_Jobs.Where(w => w.Description.StartsWith("Completed")).OrderByDescending(w => w.FiringDate).FirstOrDefault();

                if (LastJob != null && LastComplete != null && LastJob.FiringDate > LastComplete.FiringDate && LastJob.FiringDate.AddMinutes(5) > DateTime.Now)
                {
                    Thread.Sleep(30000);
                    UploadAndDownloadData();
                    return;
                }

                ErrorUnit.InsertStatus("StartPrccess", "");

                Integration Integration = new Integration(Pass);
                Integration.UploadData();
                Integration.DownloadAllData();
                ErrorUnit.InsertStatus("Completed", "");

            }
            catch (Exception ex)
            {
                ErrorUnit.InsertStatus("Error_UploadAndDownloadData", ErrorUnit.RetriveExceptionMsg(ex));
            }



        }
    }
}
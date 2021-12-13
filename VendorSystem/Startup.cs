using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
//using Hangfire;
using VendorSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using Hangfire;
using VendorSystem.Repository;
using System.Web;
//using Hangfire.Dashboard;
using System.Security.Principal;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using VendorSystem.Models.Model1;

[assembly: OwinStartup(typeof(VendorSystem.Startup))]

namespace VendorSystem
{
    public class Startup
    {
        BayanEntities context = new BayanEntities();
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HashAllUsersPasswords();


            //GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection");
            //var options = new DashboardOptions
            //{
            //    Authorization = new[] { new CustomAuthorizationFilter() }
            //};
            //app.UseHangfireDashboard("/hangfire", options);
            //RecurringJob.AddOrUpdate(() => GetAllData(), Cron.Hourly);
            //app.UseHangfireServer();



            //////////////RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), "*/MinutNumber * * * *"); for specific number of minutes 

        }

        //public async Task<Uri> GetAllData()
        //{
        //    Integration Integration = new Integration();
        //    context.Jobs.Add(new Job() { Job_Name = "GetData()", Date = DateTime.Now });
        //    context.SaveChanges();
        //    Integration.DownloadAllData();

        //    return new HttpClient().BaseAddress;
        //}


        //public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
        //{

        //    public bool Authorize(DashboardContext context)
        //    {
        //        return true;
        //    }
        //}


        private void HashAllUsersPasswords()
        {
            var db = new BayanEntities();

            string _Password = "";
            var Users = db.Users.Where(a => a.IsPasswordHash != true).ToList();
            foreach (var user in Users)
            {
                _Password = HashPassword.Hash(user.Password);
                user.Password = _Password;
                user.IsPasswordHash = true;
                db.SaveChanges();
            }

        }
    }
}

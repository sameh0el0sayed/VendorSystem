using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;

namespace VendorSystem
{
    public static class Authentication
    {

        public static bool IsVisiable(string PageName, TypeButton type, string _RoleID)
        {

            try
            {
                BayanEntities _context = new BayanEntities();


                var PageID = _context.Pages.Where(c => c.NameEng == PageName && c.IsActive == true).FirstOrDefault().ID;
                var RoleID = Convert.ToDecimal(_RoleID);
                var RoleVsPage = _context.PageVSRoles.Where(c => c.RoleId == RoleID && c.PageId == PageID).FirstOrDefault();
                
                if (RoleVsPage == null)
                    return false;
                
                if (type == TypeButton.Show)
                { 
                    return RoleVsPage.Show;
                }
                else if (type == TypeButton.Save)
                {
                    return RoleVsPage.Save;
                }
                else if (type == TypeButton.SaveAndPost)
                {
                    return RoleVsPage.SaveAndPost;
                }
                else if (type == TypeButton.Search)
                {
                    return RoleVsPage.Search;
                }
                else if (type == TypeButton.Print)
                {
                    return RoleVsPage.Print;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }

        }
    }
    public enum TypeButton
    {
        Show, Save, SaveAndPost, Print, Search
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendorSystem.Models;
using VendorSystem.Models.Model1;
using VendorSystem.ViewModel;

namespace VendorSystem.Repository
{
    public class UserUnit
    {
        BayanEntities DB;
        public UserUnit(BayanEntities _Db)
        {
            DB = _Db;
        }
        public UserUnit()
        {
            DB = new BayanEntities();
        }

        public IQueryable<User> GetAllUsers(string Vendor_CompanyID)
        {
            return DB.Users.Where(w => w.Vendor_CompanyID == Vendor_CompanyID);
        }

        public List<UserVM> GetAllUsersVM(string Vendor_CompanyID)
        {
            var Qry = (from Role in DB.Roles
                       from Usr in DB.Users
                       where Usr.RoleId == Role.ID
                       && Usr.Vendor_CompanyID == Vendor_CompanyID
                       select new UserVM
                       {
                           IsActive = Usr.IsActive,
                           Mobile = Usr.Mobile,
                           Name = Usr.Name,
                           NameEng = Usr.NameEng,
                          // Password = Usr.Password,
                           RoleName = Role.Name,
                           RoleNameEng = Role.NameEng,
                           UserName = Usr.User_Name,
                           ID = Usr.ID,
                           IsManufacture = Usr.IsManufacture,
                           DistributorCode = Usr.DistributorCode
                       });

            return Qry.ToList();
        }

        public string Save(UserVM UserVM, int UserID, string Vendor_CompanyID)
        {
            try
            {
                var UserNameCheck = DB.Users.Where(w => w.ID != UserVM.ID && w.Vendor_CompanyID == Vendor_CompanyID && w.User_Name == UserVM.UserName).FirstOrDefault();
                if (UserNameCheck != null)
                {
                    return CheckUnit.RetriveCorrectMsg("اسم المستخدم موجود من قبل", "User Name Exist Befor");
                }

                var OldUser = DB.Users.Where(w => w.ID == UserVM.ID).FirstOrDefault();

                if (OldUser == null)
                {
                    var NewUser = new User()
                    {
                        CreationDate = DateTime.Now,
                        CreatedBy = UserID,
                        Vendor_CompanyID = Vendor_CompanyID,
                        Password = HashPassword.Hash(UserVM.Password)

                };
                    
                    FillUser(UserVM, NewUser, UserID);
                    DB.Users.Add(NewUser);
                }
                else
                {
                    FillUser(UserVM, OldUser, UserID);
                }
                DB.SaveChanges();
                return "Done";
            }
            catch (Exception ex)
            {
                return ErrorUnit.RetriveExceptionMsg(ex);
            }
        }

        private void FillUser(UserVM UserVM, User User, int UserID)
        {
            User.IsActive = UserVM.IsActive;
            User.LastUpdate = DateTime.Now;
            User.Mobile = UserVM.Mobile;
            User.Name = UserVM.Name;
            User.NameEng = UserVM.NameEng;
           // User.Password =HashPassword.Hash( UserVM.Password);
            User.RoleId = UserVM.RoleID;
            User.UpdateBy = UserID;
            User.User_Name = UserVM.UserName;
            User.IsManufacture = UserVM.IsManufacture;
            User.DistributorCode = UserVM.DistributorCode;
        }

    }
}
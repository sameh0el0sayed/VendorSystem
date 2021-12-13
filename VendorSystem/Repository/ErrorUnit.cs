using System; 
using System.Data.Entity.Validation; 
using System.Linq;  
using VendorSystem.Models.Model1;

namespace VendorSystem.Repository
{
    public static class ErrorUnit
    {
        public static string RetriveExceptionMsg(Exception ex)
        {
            var Message = ex.InnerException == null ? ex.Message : (ex.InnerException.InnerException == null ? ex.InnerException.Message : (ex.InnerException.InnerException.InnerException == null ? ex.InnerException.InnerException.Message : ex.InnerException.InnerException.InnerException.Message));


            // Added By Lamia and Raed 22-12-2020

            var EntityException = ex as DbEntityValidationException;
            if (EntityException != null)
            {
                var ValidationLst = EntityException.EntityValidationErrors.ToList();
                if (ValidationLst.Count > 0)
                {
                    string ValidationMsg = "";
                    foreach (var EntityValidation in EntityException.EntityValidationErrors.ToList())
                    {
                        var EntityName = EntityValidation.Entry.Entity.GetType().Name;
                        var DBStatus = EntityValidation.Entry.State; //Add or Delete or Update
                        foreach (var ValidErr in EntityValidation.ValidationErrors)
                        {
                            ValidationMsg += "Object Name: " + EntityName + ", Curd Operation Type: " + DBStatus + ", Column Name: " + ValidErr.PropertyName + ", Message: " + ValidErr.ErrorMessage;
                        }
                    }
                    if (ValidationMsg != "")
                    {
                        Message = ValidationMsg;
                    }
                }
            }

            return Message;
        } 
        public static void InsertStatus(string Status_FunctionName, string Message)
        {

            try
            {
                BayanEntities Context = new BayanEntities();
                Context.Tbl_Jobs.Add(new Models.Model1.Tbl_Jobs { FiringDate = DateTime.Now, Description = Status_FunctionName + ": " + Message });
                Context.SaveChanges();
            }



            catch (DbEntityValidationException e)
            {
                var msg = RetriveExceptionMsg(e);
            }
        }
    }
}
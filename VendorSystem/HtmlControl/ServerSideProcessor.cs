using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Linq.Dynamic;
using System.Globalization;

namespace VendorSystem.HtmlControl
{
    public static class ServerSideProcessor
    {
        public static IQueryable<T> ToGlobalSearchInAllColumn<T>(this IQueryable<T> table, DTParameters Param)
        {
            var GlobalSearchText = Param.Search != null && Param.Search.Value != null ? Param.Search.Value : string.Empty;
            if (!string.IsNullOrEmpty(GlobalSearchText))
            {
                // return BooksData.Where(x => x.BookId.ToString() == GlobalSearchText || x.BookName.Contains(GlobalSearchText) || x.Category.Contains(GlobalSearchText));
                StringBuilder WhereQueryMaker = new StringBuilder();
                Type BookType = table.FirstOrDefault().GetType();
                DateTime CreatedOn;
                foreach (PropertyInfo prop in BookType.GetProperties())
                {
                    if (prop.PropertyType == typeof(System.String))
                        WhereQueryMaker.Append((WhereQueryMaker.Length == 0 ? "" : " OR ") + prop.Name + ".Contains(@0)");
                    else if (prop.PropertyType == typeof(System.Int32))
                        //if data type is integer then you need to parse to ToString() to use Contains() function
                        WhereQueryMaker.Append((WhereQueryMaker.Length == 0 ? "" : " OR ") + prop.Name + ".ToString().Contains(@0)");
                    else if (prop.PropertyType == typeof(System.DateTime?) && DateTime.TryParseExact(GlobalSearchText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedOn))
                        //Date object comparison required to follow DateTime(2018,08,15) as format. so need to supply yyyy, MM, dd value on it.
                        WhereQueryMaker.Append((WhereQueryMaker.Length == 0 ? "" : " OR ") + prop.Name + "== DateTime(" + CreatedOn.Year + ", " + CreatedOn.Month + ", " + CreatedOn.Day + ")");
                }
                return table.Where(WhereQueryMaker.ToString(), GlobalSearchText);
            }
            return table;
        }

        public static IQueryable<T> ToIndividualColumnSearch<T>(this IQueryable<T> table, DTParameters Param)
        {
            if (Param.Columns != null && Param.Columns.Count() > 0 && table.FirstOrDefault() != null)
            {
                Type EntityType = table.FirstOrDefault().GetType();
                var Properties = EntityType.GetProperties();
                DateTime CreatedOn;
                int Id;
                //listing necessary column where individual columns search has applied. Filtered with search text as well it data types
                Param.Columns.Where(w => w.Search != null &&
                !string.IsNullOrEmpty(w.Search.Value)).ToList().ForEach(x =>
                {
                    //x.Data is column name as string format coming from Param object.
                    //x.Search.Value specific search text applied on column
                    //Added extra check on column name coming from Param and its data type on search text.
                    if (int.TryParse(x.Search.Value, out Id) && Properties.Count(p => p.Name == x.Data && p.PropertyType == typeof(System.Int32)) > 0)
                        table = table.Where(x.Data + ".ToString().Contains(@0)", x.Search.Value);
                    else if (DateTime.TryParseExact(x.Search.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedOn) && Properties.Count(p => p.Name == x.Data && p.PropertyType == typeof(System.DateTime?)) > 0)
                        table = table.Where(x.Data + "==DateTime(" + CreatedOn.Year + ", " + CreatedOn.Month + ", " + CreatedOn.Day + ")");
                    else if (Properties.Count(p => p.Name == x.Data && p.PropertyType == typeof(System.String)) > 0)
                        table = table.Where(x.Data + ".Contains(@0)", x.Search.Value);
                });
            }
            return table;
        }

        public static IQueryable<T> ToSorting<T>(this IQueryable<T> table, DTParameters Param)
        {
            //Param.SortOrder return sorting column name
            //Param.Order[0].Dir return direction as asc/desc

            return table.OrderBy(Param.SortOrder + " " + Param.Order[0].Dir).AsQueryable();
        }

        public static IQueryable<T> ToPagination<T>(this IQueryable<T> table, DTParameters Param)
        {
            //Param.Start return start index
            //Param.Length page length
            if (Param.Length > 0)
                return table.Skip(Param.Start).Take(Param.Length);
            else return table.Skip(Param.Start);
        }
    }
}
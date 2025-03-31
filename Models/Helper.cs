using Microsoft.EntityFrameworkCore; 
using System.ComponentModel.DataAnnotations.Schema;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Models.Context;

namespace MotorMemo.Models
{
    public class Helper
    {
        public class entityLog
        {
            public string name { get; set; } = null!;
            public string value { get; set; }=null!;


        }
        public class BankTxn
        {
            public int value { get; set; }
            public string name { get; set; } = null!;
        }

        //[NotMapped]
        //public class postdatedPending
        //{
        //    public List<PayPost> Payments { get; set; } = new List<PayPost>();
        //    public List<RecPost> Receipts { get; set; } = new List<RecPost>();

        //}
        private static string? getValue(MotorMemoDbContext db, string? prop, string value)
        {
            if (value != null && prop!=null)
            {
                string? retval = prop;
                string[] acc_code = { "accCode", "accode", "dcode", "d_code" };
                string[] prod_code = { "IId", "prod_code", "bom_id", "item_id", "item_code" };
                string[] s_code = { "s_code", "scode" };
                string[] places = { "CityId", "place_id", "to_dstn" };
               

                if (acc_code.Contains(prop))
                    return db.Mst011s.Where(w => w.AccCode.ToString() == value).Select(s => s.AccName).First();

                if (prod_code.Contains(prop))
                    return db.Mst010s.Where(w => w.IId.ToString() == value).Select(s => s.IName).First();

                if (places.Contains(prop))
                    return db.Mst006s.Where(w => w.CityId.ToString() == value).Select(s => s.CityName).First();
                
            }
            return value;

        }

        public static void ChangeTracker(MotorMemoDbContext db, List<entityLog> tbls,
         string primaryKey, int firm_id, string challan_no, string? user)
        {

            var modifiedEntities = db.ChangeTracker.Entries()
           .Where(p => p.State == EntityState.Modified)
           .ToList();
            var addedEntities = db.ChangeTracker.Entries()
         .Where(p => p.State == EntityState.Added)
         .ToList();
            var deletedEntities = db.ChangeTracker.Entries()
            .Where(p => p.State == EntityState.Deleted).ToList();

            foreach (var change in modifiedEntities)
            {
                var arr = change.Entity.GetType().Name.Split('_');
                string oldString = "_" + arr[arr.Length - 1];
                var entityName = change.Entity.GetType().Name.Replace(oldString, "");
                var items = tbls.Select(s1 => s1.name).ToList();

                if (items.Contains(entityName))
                {
                    foreach (var prop in change.OriginalValues.Properties)
                    {
                        string? originalValue = change.OriginalValues[prop]?.ToString();
                        string? currentValue = change.CurrentValues[prop]?.ToString();

                        string? OldValue = getValue(db, prop.Name, originalValue);
                        string? NewValue = getValue(db, prop.Name, currentValue);

                        if (originalValue != OldValue || originalValue != currentValue)
                        {

                            Sys200 log = new Sys200()
                            {
                                FirmId = firm_id, 
                                EntityName = tbls.Where(w => w.name == entityName).Select(s => s.value).First(),
                                PmKey = primaryKey,
                                ChallanNo = challan_no,
                                PropertyName = prop.Name,
                                OldValue = OldValue,
                                NewValue = NewValue,
                                DateChanged = DateTime.Now.ToString(),
                                UserName = user,
                                Status = originalValue == "" ? "A" : (currentValue == "" ? "D" : "M")

                            };

                            db.Sys200s.Add(log);
                        }

                    }
                }
            }
            foreach (var change in deletedEntities)
            {

                var arr = change.Entity.GetType().Name.Split('_');
                string oldString = "_" + arr[arr.Length - 1];
                var entityName = change.Entity.GetType().Name.Replace(oldString, "");
                var items = tbls.Select(s1 => s1.name).ToList();

                if (items.Contains(entityName))

                {


                    foreach (var prop in change.OriginalValues.Properties)
                    {
                        string originalValue = change.OriginalValues[prop]?.ToString();
                        string? OldValue = getValue(db, prop.Name, originalValue);

                        if (originalValue != null)
                        {
                            
                            Sys200 log = new Sys200()
                            {
                                FirmId = firm_id, 
                                EntityName = tbls.Where(w => w.name == entityName).Select(s => s.value).First(),
                                PmKey = primaryKey,
                                ChallanNo = challan_no,
                                PropertyName = prop.Name,
                                OldValue = OldValue,
                                DateChanged = DateTime.Now.ToString(),
                                UserName = user,
                                Status =  "D" 

                            };

                            db.Sys200s.Add(log);
                        }

                    }
                }
            }
            foreach (var change in addedEntities)
            {

                var arr = change.Entity.GetType().Name.Split('_');
                string oldString = "_" + arr[arr.Length - 1];
                var entityName = change.Entity.GetType().Name.Replace(oldString, "");
                var items = tbls.Select(s1 => s1.name).ToList();

                if (items.Contains(entityName))
                {
                    foreach (var prop in change.CurrentValues.Properties)
                    {
                        string NewValue = change.CurrentValues[prop]?.ToString();
                        if (NewValue != null)
                        {
                            Sys200 log = new Sys200()
                            {
                                FirmId = firm_id, 
                                EntityName = tbls.Where(w => w.name == entityName).Select(s => s.value).First(),
                                PmKey = primaryKey,
                                ChallanNo = challan_no,
                                PropertyName = prop.Name,
                                OldValue = NewValue,
                                DateChanged = DateTime.Now.ToString(),
                                UserName = user,
                                Status = "D"

                            };

                            db.Sys200s.Add(log);
                        }

                    }
                }
            }
        }

      
    }
}

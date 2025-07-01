using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MotorMemo.Models.Context;
using System.Data;

using System.Threading.Tasks;
using System.Security.Claims;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static MotorMemo.Models.Helper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; 
using Microsoft.AspNetCore.Authorization; 

using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public ReceiptController(MotorMemoDbContext context, MainDbContext mainDb, IOptions<UserSettings> op)
        {
            db = context;
            MainDb = mainDb;
            Settings = op;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int FirmId, string DivId)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Acc003>();

                var query = db.Acc003s.Where(w => w.FirmId == FirmId && w.DivId == DivId)
                    .Include(c => c.AccCodeNavigation)
                    .Include(c => c.Acc00301).ThenInclude(c => c.AccCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderByDescending(o => o.VchDate)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FirmId, 
                        i.DivId,
                        i.VchId,
                        i.VchNo,
                        i.VchDate,
                        i.ChallanNo,
                        i.AccCode,
                        i.AccCodeNavigation,
                        i.AccCodeNavigation.AccName,
                        toCredit = i.Acc00301.AccCodeNavigation.AccName,
                        Amount = i.Acc00301.Amount,
                        
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Acc003>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<IActionResult> receipts(int FirmId, string DivId, string username)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc003s.AsNoTracking()
                    .Include(c => c.AccCodeNavigation).AsNoTracking()
                    .Include(c => c.Acc00301).ThenInclude(c => c.AccCodeNavigation).AsNoTracking()

                       .Where(w => w.FirmId == FirmId && w.DivId == DivId)

                        .Select(s => new
                        {
                            s.FirmId, 
                            s.DivId,
                            s.VchId,
                            s.VchDate,
                            s.ChallanNo,
                            s.AccCode,
                            s.AccCodeNavigation.AccName, 
                            s.Amount
                        }).ToListAsync();
             
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }
        [HttpGet]
        public async Task<IActionResult> receipt(long id)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc003s.AsNoTracking()
                   .Where(w => w.VchId == id)
                   .Include(i => i.Acc00300).AsNoTracking()
                    .Include(i => i.Motormemo).AsNoTracking()
                    .Include(a => a.AccCodeNavigation).AsNoTracking()
                   .Include(i => i.Acc00301).ThenInclude(i => i.AccCodeNavigation).AsNoTracking() 
                     
                   .SingleOrDefaultAsync();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "422",
                        message = "Record Not Found"
                    };
                }

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }
        
 
        [HttpPost]
        public async Task<IActionResult> insert(Acc003 receipt)
        {
           
            var t = db.Database.BeginTransaction();

            rtn.status_cd = 1;
            Acc999 acc999;

            try
            {
                if (receipt.VchNo == 0)
                {
                    var vch = db.Acc003s
                         .Where(w => w.FirmId == receipt.FirmId && w.DivId == receipt.DivId )
                         .Max(c => (int?)c.VchNo);

                    receipt.VchNo = (vch ?? 0) + 1;
                }

                var res = new dssFunctions(db, MainDb).GenerateChallanNo(receipt.FirmId, receipt.DivId, 02, receipt.VchNo, "");

                if (res.status_cd == 0)
                    return Ok(res);

                if (res.status_cd != 2)
                    receipt.ChallanNo = (string?)res.data;

                if (receipt.ChallanNo?.Length > 15)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Challan No Lenght(" + receipt.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                    };

                    return Ok(rtn);
                }
 
                acc999 = new CreateLedger(db).receipt(receipt);

                if (receipt.Acc00300 != null)
                    receipt.Acc00300.CreatedDt = DateTime.Now.ToString();
 
                db.Acc003s.Add(receipt);
                await db.SaveChangesAsync();

                acc999.ChallanId = receipt.VchId;
                db.Acc999s.Add(acc999);
                await db.SaveChangesAsync();

                t.Commit();
                rtn.data = receipt;

            }
            catch (Exception ex)
            {
                t.Rollback();

                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }

            return Ok(rtn);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            rtn.status_cd = 1;
            Acc003? acc003;

            try
            {
                
                var ledger = db.Acc999s.Where(c => c.ChallanId == id && c.VchType == 2).ToList();

                if (ledger.Count() > 0)
                    db.Acc999s.RemoveRange(ledger);
 
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }

            return Ok(rtn);
        }

        [HttpGet]
        public IActionResult chqRtn(int FirmId, string id)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = db.Acc003s.AsNoTracking()
                    .Where(w => w.ChallanNo == id && w.FirmId == FirmId)
                    .Include(i => i.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation).AsNoTracking()
                    .Include(i => i.Acc00300).AsNoTracking()
                  
                    .SingleOrDefault();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "422",
                        message = "Record Not Found"
                    };
                    return Ok(rtn);
                }
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }
  

        private void changedLog(Acc003 voucher)
        {
            var identity = User.Identity as ClaimsIdentity;
            var c = identity.Claims.Select(s => new { s.Subject, s.Type, s.Value }).FirstOrDefault();

            var tbls = new List<entityLog>();
            tbls.Add(new entityLog
            {
                name = "acc003",
                value = "Receipt"
            });

            tbls.Add(new entityLog
            {
                name = "RecPayees",
                value = "Receipt Items"
            });

            ChangeTracker(db, tbls, voucher.VchId.ToString(), voucher.FirmId, voucher.ChallanNo, c?.Value);
        }

        [HttpGet]
        public async Task<IActionResult> reciptrecord(string challan_no)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc003s.AsNoTracking()
                   .Where(w => w.ChallanNo == challan_no)
                   
                   .Include(i => i.Acc00301).ThenInclude(i => i.AccCodeNavigation).AsNoTracking() 
               
                   .Include(i => i.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation).AsNoTracking()
                   .Include(c => c.AccCodeNavigation.SgCodeNavigation.GrpCodeNavigation.MgCodeNavigation).AsNoTracking()
                   .SingleOrDefaultAsync();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "422",
                        message = "Record Not Found"
                    };
                }

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Acc003 receipt)
        {

            receipt.Motormemo = null;
            rtn.status_cd = 1;
             
            if (id != receipt.VchId)
            {
                rtn.status_cd = 0;
                rtn.errors = new errors
                {
                    error_cd = "400",
                    message = "Bad Request",
                    exception = ModelState
                };

                return Ok(rtn);
            }


            try
            {

                var existingParent = await db.Acc003s
                        .Where(a => a.VchId == id)
                        .Include(a => a.Acc00301)
                        .Include(a => a.Acc00300)
                        .SingleOrDefaultAsync();

                if (existingParent == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Record Not Found"
                    };

                    return Ok(rtn);
                }

                if (receipt.Acc00300 != null)
                    receipt.Acc00300.ModifiedDt = DateTime.Now.ToString();


                var ledger = await db.Acc999s
                        .Where(c => c.ChallanId == receipt.VchId && c.VchType == 2)
                        .SingleOrDefaultAsync();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);

                db.Acc999s.Add(new CreateLedger(db).receipt(receipt));


                if (receipt.VchNo != existingParent.VchNo)
                {

                    var res = new dssFunctions(db, MainDb).GenerateChallanNo(receipt.FirmId, receipt.DivId, 02, receipt.VchNo, "");

                    if (res.status_cd == 0)
                        return Ok(res);

                    if (res.status_cd != 2)
                        receipt.ChallanNo = (string?)res.data;

                    if (receipt.ChallanNo?.Length > 15)
                    {
                        rtn.status_cd = 0;
                        rtn.errors = new errors
                        {
                            error_cd = "404",
                            message = "Challan No Lenght(" + receipt.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                        };

                        return Ok(res);
                    }

                }

                db.Entry(existingParent).CurrentValues.SetValues(receipt);

                if (existingParent.Acc00300 != null)
                    db.Entry(existingParent.Acc00300).CurrentValues.SetValues(receipt.Acc00300);


                if(existingParent.Acc00301.VchId == receipt.Acc00301.VchId)
                {
                    db.Entry(existingParent.Acc00301).CurrentValues.SetValues(receipt.Acc00301);

                }
                else
                {
                    db.Acc00301s.Add(receipt.Acc00301);
                }

                changedLog(receipt);
                await db.SaveChangesAsync();
                rtn.data = receipt;

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }

    }
}

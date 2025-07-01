using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using MotorMemo.Services;
using System;
using System.Collections.Generic;
using System.Data;

using System.Threading.Tasks;
using System.Security.Claims; 
using MotorMemo.Models; 
using Microsoft.EntityFrameworkCore;

using static MotorMemo.Models.Helper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; 
using Microsoft.AspNetCore.Authorization; 

using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using MotorMemo.Models.MotorMemoEntities;
using Microsoft.Extensions.Logging;


namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public PaymentController(MotorMemoDbContext context, MainDbContext mainDb, IOptions<UserSettings> op)
        {
            db = context;
            MainDb = mainDb;
            Settings = op;
        }


        [HttpPost]
        public async Task<IActionResult> insert(Acc002 payment)
        {
            var t = db.Database.BeginTransaction();

            rtn.status_cd = 1;
            Acc999 lgr;

            try
            {
                if (payment.VchNo == 0)
                {
                    var vch = db.Acc002s
                         .Where(w => w.FirmId == payment.FirmId && w.DivId == payment.DivId)
                         .Max(c => (int?)c.VchNo);

                    payment.VchNo = (vch ?? 0) + 1;
                }

                var chlnNo = new dssFunctions(db, MainDb).GenerateChallanNo(payment.FirmId, payment.DivId, 03, payment.VchNo, "");

                if (chlnNo.status_cd == 0)
                    return Ok(chlnNo);

                if (chlnNo.status_cd != 2)
                    payment.ChallanNo = (string?)chlnNo.data;
                 
                if (payment.ChallanNo?.Length > 15)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Challan No Lenght(" + payment.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                    };

                    return Ok(rtn);
                }
                if (payment.Acc00200 != null)
                    payment.Acc00200.CreatedDt = DateTime.Now.ToString("yyyy/MM/dd");
            
                lgr = new CreateLedger(db).payment(payment);

                db.Acc002s.Add(payment);

                await db.SaveChangesAsync();

                lgr.ChallanId = payment.VchId;

                db.Acc999s.Add(lgr);

                await db.SaveChangesAsync();
                t.Commit();

                rtn.data = payment;

            }
            catch (Exception ex)
            {
                t.Rollback();
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }
            return Ok(rtn);
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id )
        {
            try
            {
                var filter = new EntityFrameworkFilter<Acc002>();

                var query = db.Acc002s.Include(i => i.Acc00201).
                    Where(w => w.FirmId == firm_id &&  w.DivId == div_id);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderByDescending(o => o.VchDate)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FirmId, 
                        i.DivId,
                        i.VchId,
                        i.VchDate,
                        i.VchNo,
                        i.ChallanNo,
                        i.AccCode,

                        i.AccCodeNavigation.AccName,
                        toDebit = i.Acc00201.AccCodeNavigation.AccName,
                        Amount = i.Acc00201.Amount

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Acc002>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<IActionResult> payments(int firm_id, string div_id, string username, bool isApproval = false)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc002s.AsNoTracking()
                        .Where(w => w.FirmId == firm_id && w.DivId == div_id )
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
        public async Task<IActionResult> payment(long id)
        {
            rtn.status_cd = 1;

            try
            {
                rtn.data = await db.Acc002s.AsNoTracking()
                    .Where(w => w.VchId == id)
                    .Include(a => a.Acc00200).AsNoTracking()
                    .Include(a=>a.AccCodeNavigation).AsNoTracking()
                    .Include(c => c.Acc00201).ThenInclude(i => i.AccCodeNavigation).AsNoTracking()
                    .SingleOrDefaultAsync();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
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
        [HttpGet] 
        public async Task<IActionResult> GetPayByChallan(string challan_no,int firm_id, string year)
        {
            rtn.status_cd = 1;
            rtn.data = await db.Acc002s
                .Where(a => a.ChallanNo == challan_no && a.FirmId == firm_id &&  a.DivId == year)
               .Select(s => new
               {
                   s.VchId,
                   s.FirmId, 
                   s.DivId,
                   s.VchNo,
                   s.VchDate,
                   s.ChallanNo,
                   s.Amount,
                   s.AccCode,
                   s.Acc00201 

               }).SingleOrDefaultAsync();

            if (rtn.data == null)
            {
                return NotFound();

            }

            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            rtn.status_cd = 1;
            Acc002? pay;
            try
            {
                pay = await db.Acc002s.Where(w => w.VchId == id).SingleOrDefaultAsync();
 

                var ledger = db.Acc999s.Where(c => c.ChallanId == id && c.VchType == 3).ToList();

                if (ledger.Count() > 0)
                    db.Acc999s.RemoveRange(ledger);
     
                db.Acc002s.Remove(pay);
                await db.SaveChangesAsync();
                rtn.data = pay;

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            rtn.data = pay;

            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Acc002 pay)
        {
            rtn.status_cd = 1;
             
            if (id != pay.VchId)
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
                 
                var existingParent = await db.Acc002s
                        .Where(a => a.VchId == id)
                        .Include(a => a.Acc00201)
                        .Include(a => a.Acc00200)
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

                if (pay.Acc00200 != null)
                    pay.Acc00200.ModifiedDt = DateTime.Now.ToString();


                var ledger = await db.Acc999s
                        .Where(c => c.ChallanId == pay.VchId && c.VchType == 2)
                        .SingleOrDefaultAsync();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);

                db.Acc999s.Add(new CreateLedger(db).payment(pay));


                if (pay.VchNo != existingParent.VchNo)
                {

                    var res = new dssFunctions(db, MainDb).GenerateChallanNo(pay.FirmId, pay.DivId, 02, pay.VchNo, "");

                    if (res.status_cd == 0)
                        return Ok(res);

                    if (res.status_cd != 2)
                        pay.ChallanNo = (string?)res.data;

                    if (pay.ChallanNo?.Length > 15)
                    {
                        rtn.status_cd = 0;
                        rtn.errors = new errors
                        {
                            error_cd = "404",
                            message = "Challan No Lenght(" + pay.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                        };

                        return Ok(res);
                    }

                }

                db.Entry(existingParent).CurrentValues.SetValues(pay);

                if (existingParent.Acc00200 != null)
                    db.Entry(existingParent.Acc00200).CurrentValues.SetValues(pay.Acc00200);


                if (existingParent.Acc00201.VchId == pay.Acc00201.VchId)
                {
                    db.Entry(existingParent.Acc00201).CurrentValues.SetValues(pay.Acc00201);

                }
                else
                {
                    db.Acc00201s.Add(pay.Acc00201);
                }

                changedLog(pay);
                await db.SaveChangesAsync();
                rtn.data = pay;

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }

        private void changedLog(Acc002 voucher)
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
    }
}

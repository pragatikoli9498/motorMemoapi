using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services; 
using System.Data;
  
using  MotorMemo.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;
using static MotorMemo.Models.Helper;
using System.Security.Claims;


namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ContraController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;

        private respayload rtn = new respayload();

        public ContraController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            db = context;
            MainDb = mainDb;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Acc006>();

                var query = db.Acc006s
                    .Where(w => w.FirmId == firm_id && w.DivId == div_id );
              
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderByDescending(o => o.VchDate)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchNo,
                        i.FirmId, 
                        i.DivId,
                        i.VchId,
                        i.ChallanNo,
                        i.VchDate,
                        i.Amount,
                        i.AccCodeNavigation.AccName,
                        TransTypeNm = i.TransType == 0 ? "Debit" : (i.TransType == 1 ? "Credit" : "CNG")


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Acc006>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<IActionResult> contras(int firm_id, string div_id, bool isApproval = false)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc006s.AsNoTracking()
                     .Where(w => w.FirmId == firm_id && w.DivId == div_id && (isApproval == false || w.Acc00607 == null))
                     .Select(s => new
                     {
                         s.VchNo,
                         s.FirmId, 
                         s.DivId,
                         s.VchId,
                         s.ChallanNo,
                         s.VchDate,
                         s.Amount,
                         s.AccCodeNavigation.AccName,
                         TransTypeNm = s.TransType == 0 ? "Debit" : (s.TransType == 1 ? "Credit" : "CNG")
                     })
                     .ToListAsync();
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
        public async Task<IActionResult> contra(long id)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc006s.AsNoTracking()
                   .Where(w => w.VchId == id)
                   .Include(c => c.Acc00600).AsNoTracking()
                   .Include(c => c.Acc00601s).ThenInclude(c => c.AccCodeNavigation).AsNoTracking()

                   .Include(c => c.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation).AsNoTracking()

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

        [HttpPut]
        public async Task<IActionResult> Put(int id, Acc006 contra)
        {

            if (!ModelState.IsValid)
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

            if (id != contra.VchId)
            {
                rtn.status_cd = 0;
                rtn.errors = new errors
                {
                    error_cd = "400",
                    message = "Record Not Match"
                };
                return Ok(rtn);
            }

            try
            {
                var old = await db.Acc006s
                    .Where(a => a.VchId == id)
                    .Include(a => a.Acc00601s)
                    .Include(a => a.Acc00607)
                    .SingleOrDefaultAsync();

                if (old == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Record Not Found"
                    };
                    return Ok(rtn);
                }

                var ledger = await db.Acc999s
                    .Where(c => c.ChallanId == contra.VchId && c.VchType == 1)
                    .SingleOrDefaultAsync();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);

                db.Acc999s.Add(new CreateLedger(db).contra(contra));

                contra.AccCodeNavigation = null;
                if (contra.Acc00607 != null)
                    if (contra.Acc00607.ApprovedDt == null)
                        contra.Acc00607 = null;

                db.Entry(old).CurrentValues.SetValues(contra);
                if (old.Acc00600 != null)
                    db.Entry(old.Acc00600).CurrentValues.SetValues(contra.Acc00600);

                foreach (var old_01 in old.Acc00601s.ToList())
                {
                    if (!contra.Acc00601s.Any(a => a.DetlId == old_01.DetlId))
                        db.Acc00601s.Remove(old_01);
                }

                foreach (var n_01 in contra.Acc00601s)
                {
                    n_01.AccCodeNavigation = null;

                    var old_01 = old.Acc00601s
                        .Where(a => (a.DetlId == 0 ? -1 : a.DetlId) == n_01.DetlId)
                        .SingleOrDefault();

                    if (old_01 != null)
                        db.Entry(old_01).CurrentValues.SetValues(n_01);
                    else
                    {

                        n_01.VchId = contra.VchId;

                        old.Acc00601s.Add(n_01);
                    }
                }
               
                await db.SaveChangesAsync();
                rtn.data = contra;
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
        public async Task<IActionResult> Post(Acc006 contra)
        {
             Acc999? lgr;
 
            try
            {
                foreach(var x in contra.Acc00601s.ToList()){

                    x.AccCodeNavigation = null;
                }

                if (contra.VchNo == 0)
                {
                    var vch = await db.Acc006s
                        .Where(w => w.FirmId == contra.FirmId && w.DivId == contra.DivId )
                             .MaxAsync(c => (int?)c.VchNo);

                    contra.VchNo = (vch ?? 0) + 1;

                }

                string? prefix = await MainDb.Mst005s
                    .Where(w => w.DivId == contra.DivId && w.FirmCode == contra.FirmId )
                    .Select(c => c.Prefix)
                    .SingleOrDefaultAsync();

                 var chlnNo = new dssFunctions(db,MainDb).GenerateChallanNo(contra.FirmId, contra.DivId, 01, contra.VchNo, "");

                if (chlnNo.status_cd == 0)
                    return Ok(chlnNo);

                if (chlnNo.status_cd != 2)
                    contra.ChallanNo = (string?)chlnNo.data;

                if (contra.ChallanNo?.Length > 15)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Challan No Lenght(" + contra.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                    };

                    return Ok(rtn);
                }

                if (!ModelState.IsValid)
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

                lgr = new CreateLedger(db).contra(contra);

                contra.AccCodeNavigation = null;
                if (contra.Acc00607 != null)
                    if (contra.Acc00607.ApprovedDt == null)
                        contra.Acc00607 = null;

                foreach (var childModel in contra.Acc00601s)
                {
                    childModel.AccCodeNavigation = null;
                }

                db.Acc006s.Add(contra);

                await db.SaveChangesAsync();

                lgr.ChallanId = contra.VchId;
                db.Acc999s.Add(lgr);

                await db.SaveChangesAsync();
             

            }
            catch (Exception ex)
            { 
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            rtn.data = contra;

            return Ok(rtn);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            rtn.status_cd = 1;
            Acc006? cntr = null;
            try
            {
                cntr = await db.Acc006s.Where(w => w.VchId == id).SingleOrDefaultAsync();

                if (cntr == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Record Not Found"
                    };
                    return Ok(rtn);
                }
 
                var ledger = db.Acc999s
                    .Where(c => c.ChallanId == id && c.VchType == 1)
                    .SingleOrDefault();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);
                changedLog(cntr);
                db.Acc006s.Remove(cntr);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            return Ok(rtn);
        }

        private void changedLog(Acc006 voucher)
        {
            var identity = User.Identity as ClaimsIdentity;
            var c = identity.Claims.Select(s => new { s.Subject, s.Type, s.Value }).FirstOrDefault();
            if (c != null)
            {
                var tbls = new List<entityLog>();

                tbls.Add(new entityLog
                {
                    name = "Contra",
                    value = "Contra"
                });

                tbls.Add(new entityLog
                {
                    name = "ContraItems",
                    value = "Contra Items"
                });

                ChangeTracker(db, tbls, voucher.VchId.ToString(), voucher.FirmId, voucher.ChallanNo, c?.Value);
            }
        }
    }
}
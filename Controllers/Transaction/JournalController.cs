using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorMemo.Models;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using static MotorMemo.Models.Helper;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public JournalController(MotorMemoDbContext context, MainDbContext mainDb, IOptions<UserSettings> op)
        {
            db = context;
            MainDb = mainDb;
            Settings = op;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id, bool isApproval = false)
        {
            try
            { 
                var filter = new EntityFrameworkFilter<Acc005>();

                var query = db.Acc005s.Where(w => w.FirmId == firm_id && w.DivId == div_id && (isApproval == false || w.Acc00507 == null));
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderByDescending(o => o.VchDate)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchId,
                        i.FirmId, 
                        i.DivId,
                        i.VchNo,
                        i.AccCodeNavigation.AccName,
                        i.ChallanNo,
                        i.VchDate,
                        i.Amount,
                        i.TransType,
                        i.InvType

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Acc005>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        
        [HttpPost]
        public async Task<IActionResult> insert(Acc005 jrn)
        {
 
            Acc999 lgr;
 
            try

            {
                if (jrn.InvType == 0)
                    jrn.InvType = 1;

                if (jrn.VchNo == 0)
                {
                    var vch = await db.Acc005s
                        .Where(w => w.FirmId == jrn.FirmId && w.DivId == jrn.DivId )
                        .MaxAsync(c => (int?)c.VchNo);

                    jrn.VchNo = (vch ?? 0) + 1;
                }

                var chlnNo = new dssFunctions(db, MainDb).GenerateChallanNo(jrn.FirmId, jrn.DivId, 25, jrn.VchNo, "");

                if (chlnNo.status_cd == 0)
                    return Ok(chlnNo);

                if (chlnNo.status_cd != 2)
                    jrn.ChallanNo = (string?)chlnNo.data;

                if (jrn.ChallanNo?.Length > 15)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Challan No Lenght(" + jrn.ChallanNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                    };

                    return Ok(rtn);
                }

                lgr = new CreateLedger(db).journal(jrn);
                if (jrn.Acc00500 != null)
                    jrn.Acc00500.CreatedDt = DateTime.Now.ToShortDateString();

                jrn.AccCodeNavigation = null;
                jrn.Acc00507 = null;

                foreach (var child in jrn.Acc00501s)
                {
                    child.AccCodeNavigation = null;
             
                }

                var old = await db.Acc005s
                   .Where(a => a.VchId == jrn.VchId)
                   .Include(a => a.Acc00501s)
                   .Include(a => a.Acc00507)
                   .SingleOrDefaultAsync();

                if (old != null)
                {
                    foreach (var old_01 in old.Acc00501s)
                    {
                        if (!jrn.Acc00501s.Any(a => a.VchId == old_01.VchId))
                            db.Acc00501s.Remove(old_01);
                    }
                }

                foreach (var n_01 in jrn.Acc00501s)
                {
                    n_01.AccCodeNavigation = null;

                    if (old != null)
                    {
                        var old_01 = old.Acc00501s
                            .Where(a => (a.VchId == 0 ? -1 : a.VchId) == n_01.VchId)
                            .SingleOrDefault();
                        if (old_01 != null)
                            db.Entry(old_01).CurrentValues.SetValues(n_01);
                        else
                        {

                            n_01.VchId = jrn.VchId;

                            old.Acc00501s.Add(n_01);
                        }
                    }
                    else
                    {
                        n_01.VchId = jrn.VchId;

                        db.Acc00501s.Add(n_01);
                    }
 
                }

                db.Acc005s.Add(jrn);

                lgr.ChallanId = jrn.VchId;

                db.Acc999s.Add(lgr);

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
                return Ok(rtn);
            }

            rtn.data = jrn;

            return Ok(rtn);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {

            rtn.status_cd = 1;
            Acc005? jrn = null;
            try
            {
                jrn = await db.Acc005s.Where(w => w.VchId == id).SingleOrDefaultAsync();

                if (jrn == null)
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
                    .Where(c => c.ChallanId == id && c.VchType == 25)
                    .SingleOrDefault();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);
                changedLog(jrn);
                db.Acc005s.Remove(jrn);
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

        private void changedLog(Acc005 voucher)
        {
            var identity = User.Identity as ClaimsIdentity;
            var c = identity.Claims.Select(s => new { s.Subject, s.Type, s.Value }).FirstOrDefault();
            if (c != null)
            {
                var tbls = new List<entityLog>();
                tbls.Add(new entityLog
                {
                    name = "Jrn",
                    value = "Journal"
                });

                tbls.Add(new entityLog
                {
                    name = "JrnItems",
                    value = "Journal Items"
                });

                ChangeTracker(db, tbls, voucher.VchId.ToString(), voucher.FirmId, voucher.ChallanNo, c?.Value);
            }
        }

        [HttpGet]
        public async Task<IActionResult> jrnrecord(string challan_no)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc005s.AsNoTracking()
                    .Where(w => w.ChallanNo == challan_no)
                    .Include(c => c.Acc00500).AsNoTracking()
                    .Include(c => c.Acc00501s).ThenInclude(c => c.AccCodeNavigation).AsNoTracking()
                    .Include(c => c.Acc00507).AsNoTracking() 
                    .Include(c => c.AccCodeNavigation.SgCodeNavigation.GrpCodeNavigation.MgCodeNavigation).AsNoTracking()
                    .Include(c => c.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation).AsNoTracking()
                    .SingleOrDefaultAsync();

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

        [HttpPut]
        public async Task<IActionResult> update(long id, Acc005 jrn)
        {

            if (id != jrn.VchId)
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

                var existingParent = await db.Acc005s
                    .Where(a => a.VchId == id)
                    .Include(a => a.Acc00500)
                    .Include(a => a.Acc00501s)
                    .Include(a => a.Acc00507)
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
                if (existingParent.Acc00507 != null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {

                        message = "Please cancel approval before editing"
                    };
                    return Ok(rtn);
                }

                var ledger = await db.Acc999s
                        .Where(c => c.ChallanId == jrn.VchId && c.VchType == 25)
                        .SingleOrDefaultAsync();

                if (ledger != null)
                    db.Acc999s.Remove(ledger);

                db.Acc999s.Add(new CreateLedger(db).journal(jrn));

                jrn.AccCodeNavigation = null;
                if (jrn.Acc00507 != null)
                    if (jrn.Acc00507.ApprovedDt == null)
                        jrn.Acc00507 = null;


                db.Entry(existingParent).CurrentValues.SetValues(jrn);

                if (existingParent.Acc00500 != null)
                    db.Entry(existingParent.Acc00500).CurrentValues.SetValues(jrn.Acc00500);


                foreach (var existingChild in existingParent.Acc00501s.ToList())
                {
                    if (!jrn.Acc00501s.Any(a => a.DetlId == existingChild.DetlId))
                        db.Acc00501s.Remove(existingChild);
                }

                long autoID = -1;

                foreach (var childModel in jrn.Acc00501s)
                {
                    childModel.AccCodeNavigation = null;

                    var existingChild = existingParent.Acc00501s
                        .Where(a => (a.DetlId == 0 ? -1 : a.DetlId) == childModel.DetlId)
                        .SingleOrDefault();

                    if (existingChild != null)
                    {
                        db.Entry(existingChild).CurrentValues.SetValues(childModel);

                    }
                    else
                    {
                        childModel.VchId = jrn.VchId;

                        existingParent.Acc00501s.Add(childModel);
                    }


                }

                changedLog(jrn);
                await db.SaveChangesAsync();
                rtn.data = jrn;
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
        public async Task<IActionResult> journal(long id)
        {
            rtn.status_cd = 1;
            try
            {
                rtn.data = await db.Acc005s.AsNoTracking()
                    .Where(w => w.VchId == id)
                    .Include(c => c.Acc00500).AsNoTracking()
                    .Include(c => c.Acc00501s).ThenInclude(c => c.AccCodeNavigation).AsNoTracking()
                    .Include(c => c.Acc00507).AsNoTracking() 
                    .Include(c => c.AccCodeNavigation.SgCodeNavigation.GrpCodeNavigation.MgCodeNavigation).AsNoTracking()
                    .Include(c => c.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation).AsNoTracking()
                    .SingleOrDefaultAsync();

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

    }
}

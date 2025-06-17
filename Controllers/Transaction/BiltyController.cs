using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Data;

using MotorMemo.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;
using static MotorMemo.Models.Helper;
using System.Security.Claims;
using SkiaSharp;
using MotorMemo.Models.MainDbEntities;
using Syncfusion.XlsIO.Implementation.PivotAnalysis;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BiltyController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;

        private respayload rtn = new respayload();

        public BiltyController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            db = context;
            MainDb = mainDb;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id)
          {
            try
            {

                var filter = new EntityFrameworkFilter<Bilty>();

                var query = db.Bilties.Where(w => w.FirmId == firm_id && w.DivId == div_id).
                Include((Bilty s) => s.BiltyDetails).AsNoTracking()
                                         .Include((Bilty s) => s.BiltyAudit).AsNoTracking()
                                         .Include((Bilty s) => s.BiltyCommodities).AsNoTracking()
                                         .Include((Bilty s) => s.BiltyGstDetails).AsNoTracking();
                                         
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VchId)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.BiltyNo,
                        i.VchId,
                        i.BiltyAudit,
                        i.BiltyCommodities,
                        i.BiltyDetails,
                        i.BiltyGstDetails,
                        i.To_Dstn,
                        i.From_Dstn,
                       
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Bilty>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(Bilty bilty)
        {

            rtn.status_cd = 1;

            try
            {
                if (bilty.BiltyNo == 0)
                {
                    var vch = db.Bilties
                         .Where(w => w.FirmId == bilty.FirmId && w.DivId == bilty.DivId)
                         .Max(c => (int?)c.BiltyNo);

                    bilty.BiltyNo = (vch ?? 0) + 1;
                }

                db.Bilties.Add(bilty);
                await db.SaveChangesAsync();


                rtn.data = bilty;

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }

            return Ok(rtn);

        }

        [HttpGet]
        public async Task<ActionResult> edit(int id)
        {
            try
            {

                rtn.data = await db.Bilties.Where(s => s.VchId == id).Include(s => s.BiltyAudit).AsNoTracking()
                        .Include(s => s.BiltyCommodities).AsNoTracking()
                        .Include(s => s.BiltyDetails).AsNoTracking()
                        .Include(s => s.BiltyGstDetails).AsNoTracking()

                       
                        .AsNoTracking().Select(i => new
                        {
                            i.BiltyNo,
                            i.BiltyAudit,
                            i.BiltyCommodities,
                            i.BiltyDetails,
                            i.BiltyGstDetails,
                            i.vchDate,
                            i.From_Dstn,
                            i.To_Dstn,
                            i.VchId,
                            senderStateId = db.Mst00603s.Where(w => w.StateCode == i.BiltyDetails.SenderStateId).FirstOrDefault(),
                            receiverStateId = db.Mst00603s.Where(w => w.StateCode == i.BiltyDetails.ReceiverStateId).FirstOrDefault(),

                        }).SingleOrDefaultAsync();
                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";
                }
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var value = await db.Bilties.FindAsync(id);

                if (value == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                db.Bilties.Remove(value);
                await db.SaveChangesAsync();
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(int id, Bilty data)
        {
            try
            {
               

                var s = await db.Bilties.Include(s => s.BiltyDetails)
                    .Include(s => s.BiltyCommodities)
                    .Include(s => s.BiltyGstDetails)
                    .Include(s => s.BiltyAudit)
                    .Where(w => w.VchId == id).FirstOrDefaultAsync();

               

                if (s != null)
                {

                    db.Entry(s).CurrentValues.SetValues(data);

                    db.Entry(s.BiltyDetails).CurrentValues.SetValues(data.BiltyDetails);

                    foreach (var existingChild in s.BiltyCommodities.ToList())
                    {
                        if (!data.BiltyCommodities.Any(a => a.DetlId == existingChild.DetlId))
                            db.BiltyCommodities.Remove(existingChild);
                    }



                    foreach (var childModel in data.BiltyCommodities.ToList())
                    {


                        var existingChild = s.BiltyCommodities
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            db.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        {

                            childModel.VchId = data.VchId;

                            s.BiltyCommodities.Add(childModel);
                        }


                    }

                    // _context.Entry(s.MotormemoExpenses).CurrentValues.SetValues(data.MotormemoExpenses);

                    //foreach (var existingChild in s.BiltyGstDetails.ToList())
                    //{
                    //    if (!data.BiltyGstDetails.Any(a => a.DetlId == existingChild.DetlId))
                    //        db.BiltyGstDetails.Remove(existingChild);
                    //}

                    //foreach (var childModel in data.BiltyGstDetails.ToList())
                    //{
                    //    //childModel.AccCodeNavigation = null;
                    //    //childModel.Sundries = null;

                    //    var existingChild = s.BiltyGstDetails
                    //        .Where(a => a.DetlId == childModel.DetlId)
                    //        .SingleOrDefault();

                    //    if (existingChild != null)
                    //    {
                    //        db.Entry(existingChild).CurrentValues.SetValues(childModel);

                    //    }
                    //    else
                    //    {
                    //        childModel.VchId = data.VchId;

                    //        s.BiltyGstDetails.Add(childModel);
                    //    }

                    //}
                    db.Entry(s.BiltyGstDetails).CurrentValues.SetValues(data.BiltyGstDetails);

                    //_context.Entry(s.MotormemoOtherCharges).CurrentValues.SetValues(data.MotormemoOtherCharges);



                }
                else
                {
                    db.Bilties.Add(data);
                }


                await db.SaveChangesAsync();
                rtn.data = data;
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        


        [HttpGet]
        public async Task<ActionResult> pendinglists(int firmCode,string div_id)
        {
            try
            {
                rtn.data = await db.Bilties
             .Where(w => w.FirmId == firmCode && w.DivId == div_id && !w.Motormemo2Childe.Any(m => m.BiltyId == w.VchId))
             .Include(s => s.BiltyDetails)
             .Include(s => s.BiltyAudit)
             .Include(s => s.BiltyCommodities)
             .Include(s => s.BiltyGstDetails)
             .AsNoTracking()
             .Select(s=> new
             {
                 BiltyId= s.VchId,
                  s.BiltyNo,
                  s.vchDate,
                  s.To_Dstn,
                 SenderName= s.BiltyDetails.SenderName,
                 ReceiverName=s.BiltyDetails.ReceiverName,
                 Weight=s.BiltyCommodities.Sum(sm=>sm.ActWeight),
                 EwayNo=s.BiltyDetails.EwayNo,
                 
             }).ToListAsync();

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);

        }

        [HttpGet]
        public async Task<ActionResult> list()
        {
            try
            {
                respayload respayload = rtn;

                respayload.data = await (from i in db.Mst011s.AsNoTracking()
                                         .Include((Mst011 s) => s.Mst01109).AsNoTracking()
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking()
                                         select new
                                         {
                                             i.AccCode,
                                             i.AccAlias,
                                             i.AccName,
                                             i.SgCodeNavigation,
                                             i.Place,
                                             i.Mst01109
                                         }).ToListAsync();

            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Any;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Diagnostics.Contracts;
using System.Net;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MotorMemoController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext _mainDb;

        private respayload rtn = new respayload();

        public MotorMemoController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            _mainDb = mainDb;
        }
       
        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Motormemo>();

                var query = _context.Motormemos.Where(w=>w.FirmId==firm_id && w.DivId==div_id).
                Include((Motormemo s) => s.MotormemoDetails).AsNoTracking()
                                         .Include((Motormemo s) => s.MotormemoAudit).AsNoTracking()
                                         .Include((Motormemo s) => s.MotormemoCommodities).AsNoTracking()
                                         .Include((Motormemo s) => s.MotormemoExpenses).AsNoTracking()
                                         .Include((Motormemo s) => s.MotormemoOtherCharges).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VchId)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                     i.MemoNo,
                     i.VchId,
                     i.MotormemoAudit,
                     i.MotormemoCommodities,
                        i.MotormemoDetails,
                        i.MotormemoExpenses,
                        i.VehicleNo,
                     i.To_Dstn,
                     i.From_Dstn,  
                     i.MotormemoOtherCharges,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Motormemo>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(Motormemo motormemo)
        {
            rtn.status_cd = 1;

            foreach (var childModel in motormemo.MotormemoExpenses)
            {
                childModel.AccCodeNavigation = null;
                childModel.Sundries = null;
            }


            foreach (var childModel in motormemo.MotormemoOtherCharges)
            {
                childModel.AccCodeNavigation = null;
                childModel.Sundries = null;
                
            }

            try
            {
                if (motormemo.MemoNo == 0)
                {
                    var vch = _context.Motormemos
                         .Where(w => w.FirmId == motormemo.FirmId && w.DivId == motormemo.DivId)
                         .Max(c => (int?)c.MemoNo);

                    motormemo.MemoNo = (vch ?? 0) + 1;
                }

                _context.Motormemos.Add(motormemo);
                await _context.SaveChangesAsync();

              
                rtn.data = motormemo;

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
               
                rtn.data = await  _context.Motormemos.Where( s => s.VchId == id).Include(s=> s.MotormemoAudit).AsNoTracking()
                        .Include(s=> s.MotormemoCommodities).AsNoTracking()
                        .Include(s=> s.MotormemoExpenses).AsNoTracking()  
                        .Include( s => s.MotormemoOtherCharges).AsNoTracking()
                     
                        .Include(s=>s.MotormemoExpenses).ThenInclude(ti=>ti.AccCodeNavigation)
                        .Include(s => s.MotormemoExpenses).ThenInclude(ti => ti.Sundries)
                        .Include(s => s.MotormemoOtherCharges).ThenInclude(ti => ti.AccCodeNavigation)
                        .Include(s => s.MotormemoOtherCharges).ThenInclude(ti => ti.Sundries)
                        .Include( s => s.MotormemoOtherCharges).AsNoTracking()
                          .Include(s => s.Acc003s)
                        .AsNoTracking().Select(i=>new 
                                         {
                                          i.MemoNo,
                                          i.MotormemoAudit,
                                          i.MotormemoCommodities,
                                          i.TotalFreight,
                                          i.LeftAmount,
                                          i.AdvAmount,
                                          i.MotormemoExpenses,
                                          i.FreightType,
                                          i.MotormemoDetails,
                                          Acc003s= i.Acc003s.FirstOrDefault(),
                                          //i.MobileNo,
                                          i.BillAmt,
                                          i.VehicleNo,
                                          i.Dt,
                                          i.From_Dstn,
                                          i.To_Dstn,  
                                          i.MotormemoOtherCharges,
                                          i.VchId,
                                             senderStateId = _context.Mst00603s.Where(w=>w.StateCode == i.MotormemoDetails.SenderStateId).FirstOrDefault(),
                                             receiverStateId = _context.Mst00603s.Where(w=>w.StateCode == i.MotormemoDetails.ReceiverStateId).FirstOrDefault(),
                                         
                                             senderaccount = _context.Mst011s.Where(w => w.AccCode == i.MotormemoDetails.senderAccount).FirstOrDefault(),
                                             receiveraccout = _context.Mst011s.Where(w => w.AccCode == i.MotormemoDetails.receiverAccount).FirstOrDefault(),

                                             oweraccount = _context.Mst011s.Where(w => w.AccCode == i.MotormemoDetails.ownerAccount).FirstOrDefault(),
                    
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
                var value = await _context.Motormemos.FindAsync(id);

                if (value == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Motormemos.Remove(value);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> update(int id, Motormemo data)
        {
            try
            {
                data.Acc003s = null;

                var s = await _context.Motormemos.Include(s=>s.MotormemoDetails)
                    .Include( s => s.MotormemoCommodities)
                    .Include(s => s.MotormemoExpenses)
                    .Include(s=>s.MotormemoOtherCharges)
                    .Where(w => w.VchId == id).FirstOrDefaultAsync();

                foreach (var item in data.MotormemoExpenses)
                {

                    item.AccCodeNavigation = null;
                }

                if (s != null)
                {

                    _context.Entry(s).CurrentValues.SetValues(data);

                    _context.Entry(s.MotormemoDetails).CurrentValues.SetValues(data.MotormemoDetails);

                  //  _context.Entry(s.MotormemoCommodities).CurrentValues.SetValues(data.MotormemoCommodities);

                     

                    foreach (var existingChild in s.MotormemoCommodities.ToList())
                    {
                        if (!data.MotormemoCommodities.Any(a => a.DetlId == existingChild.DetlId))
                            _context.MotormemoCommodities.Remove(existingChild);
                    }

                   

                    foreach (var childModel in data.MotormemoCommodities.ToList())
                    {
                       

                        var existingChild = s.MotormemoCommodities
                            .Where(a =>  a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);
 
                        }
                        else
                        {

                            childModel.VchId = data.VchId;

                            s.MotormemoCommodities.Add(childModel);
                        }


                    }

                    // _context.Entry(s.MotormemoExpenses).CurrentValues.SetValues(data.MotormemoExpenses);

                    foreach (var existingChild in s.MotormemoExpenses.ToList())
                    {
                        if (!data.MotormemoExpenses.Any(a => a.DetlId == existingChild.DetlId))
                            _context.MotormemoExpenses.Remove(existingChild);
                    }
                     
                    foreach (var childModel in data.MotormemoExpenses.ToList())
                    {
                        childModel.AccCodeNavigation = null;
                        childModel.Sundries = null;

                        var existingChild = s.MotormemoExpenses
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        { 
                            childModel.VchId = data.VchId;

                            s.MotormemoExpenses.Add(childModel);
                        }
                         
                    }

                    //_context.Entry(s.MotormemoOtherCharges).CurrentValues.SetValues(data.MotormemoOtherCharges);

                    foreach (var existingChild in s.MotormemoOtherCharges.ToList())
                    {
                        if (!data.MotormemoOtherCharges.Any(a => a.DetlId == existingChild.DetlId))
                            _context.MotormemoOtherCharges.Remove(existingChild);
                    }

                    foreach (var childModel in data.MotormemoOtherCharges.ToList())
                    {

                        childModel.AccCodeNavigation = null;
                        

                        var existingChild = s.MotormemoOtherCharges
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        {

                            childModel.VchId = data.VchId;

                            s.MotormemoOtherCharges.Add(childModel);
                        }


                    }

                }
                else
                {
                    _context.Motormemos.Add(data);
                }


                await _context.SaveChangesAsync();
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
        public ActionResult LRgetdata(int firm_id,string div_id ,int memoNo)
        {

            try
            {
                rtn.data = _context.Motormemos
                       .Where(i => i.FirmId==firm_id && i.DivId==div_id &&  i.MemoNo ==  memoNo && !i.Acc003s.Any() )

                       .Select(i => new
                       {
                           i.VchId,
                           i.MemoNo,
                           i.DivId,
                           i.BillAmt,
                           i.MotormemoDetails.ownerAccount,
                           accCodeNavigation = i.MotormemoExpenses.Select(s=>s.AccCodeNavigation).FirstOrDefault()
                          
                       }).FirstOrDefault();
            }
            catch (WebException ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }

            return Ok(rtn);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Diagnostics.Contracts;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Principal;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TransportInvoiceController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext MainDb;

        private respayload rtn = new respayload();

        public TransportInvoiceController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            MainDb = mainDb;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Tms011>();

                var query = _context.Tms011s.Where(w => w.FirmId == firm_id && w.DivId == div_id).
                Include((Tms011 s) => s.Tms01101s).ThenInclude(i => i.Motormemo).AsNoTracking();
                                         
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VchId)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchDt,
                        i.VchId,
                        i.Tms01101s,
                        i.BillNo,
                        i.AccCode,
                        i.AccCodeNavigation,
                        i.Sac,
                        i.GrossAmt,
                        i.NetAmt
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Tms011>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;
            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(Tms011 tms011)
        {

            tms011.AccCodeNavigation = null;
            tms011.Mst00603 = null;
            var t = _context.Database.BeginTransaction();
            rtn.status_cd = 1;

            try
            {
                if (tms011.VchNo == 0)
                {
                    var vch = await _context.Tms011s
                    .Where(w => w.FirmId == tms011.FirmId && w.DivId == tms011.DivId)
                             .MaxAsync(c => (int?)c.VchNo);

                    tms011.VchNo = (vch ?? 0) + 1;

                }

                string? prefix = await MainDb.Mst005s
                   .Where(w => w.DivId == tms011.DivId && w.FirmCode == tms011.FirmId)
                   .Select(c => c.Prefix)
                   .SingleOrDefaultAsync();

                var chlnNo = new dssFunctions(_context, MainDb).GenerateChallanNo(tms011.FirmId, tms011.DivId, 08, tms011.VchNo, "");

                if (chlnNo.status_cd == 0)
                    return Ok(chlnNo);

                if (chlnNo.status_cd != 2)
                    tms011.BillNo = (string?)chlnNo.data;

                if (tms011.BillNo?.Length > 15)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Challan No Lenght(" + tms011.BillNo.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                    };

                    return Ok(rtn);
                }

                _context.Tms011s.Add(tms011);
                await _context.SaveChangesAsync();

                t.Commit();
                rtn.data = tms011;

            }
            catch (Exception ex)
            {
                t.Rollback();

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
                rtn.data = await _context.Tms011s.Where(s => s.VchId == id).Include(s => s.Tms01101s).ThenInclude(i => i.Motormemo).AsNoTracking()
                        .Include(s => s.AccCodeNavigation).ThenInclude(p => p.Place.Taluka.District.StateCodeNavigation).AsNoTracking()
                        .AsNoTracking().Select(i => new
                        {
                            i.VchId,
                            i.VchNo,
                            i.VchDt,
                            i.BillNo,
                            i.FirmId,
                            i.DivId,
                            i.IsRcm,
                            i.FromDt,
                            i.ToDt,
                            i.AccCode,
                            i.AccCodeNavigation,
                            i.CgstAmt,
                            i.CgstRate,
                            i.IgstRate,
                            i.IgstAmt,
                            i.SgstRate,
                            i.SgstAmt,
                            i.Sac,
                            i.GrossAmt,
                            i.RoundOff,
                            i.NetAmt,
                            state=i.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation,
                            i.Tms01101s
                         
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

        [HttpPut]
        public async Task<IActionResult> update(int id,Tms011 data)
        {
            try
            {
               
                data.AccCodeNavigation = null;
                data.Mst00603 = null;
                var s = await _context.Tms011s
                    .Include(s => s.Tms01101s)
                    .Where(w => w.VchId == id).FirstOrDefaultAsync();

                foreach (var item in data.Tms01101s)
                {

                    item.Motormemo = null;
                    
                }

                if (s != null)
                {
                    _context.Entry(s).CurrentValues.SetValues(data);

                   

                    foreach (var existingChild in s.Tms01101s.ToList())
                    {
                        if (!data.Tms01101s.Any(a => a.DetlId == existingChild.DetlId))
                            _context.Tms01101s.Remove(existingChild);
                    }

                    foreach (var childModel in data.Tms01101s.ToList())
                    {

                        var existingChild = s.Tms01101s
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        {

                            childModel.VchId = data.VchId;

                            s.Tms01101s.Add(childModel);
                        }

                    }
                    
                }
                else
                {
                    _context.Tms011s.Add(data);
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

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var value = await _context.Tms011s.FindAsync(id);

                if (value == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Tms011s.Remove(value);
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccOpblController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public AccOpblController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page, int firm_id, string div_id)
       {
            try
            {
                 
                var filter = new EntityFrameworkFilter<Mst01100>();

                var query = _context.Mst01100s.Include(i => i.AccCodeNavigation)
                    .Where(w => w.FirmId == firm_id && w.DivId == div_id);



                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VchId)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchId,
                        i.AccCode,
                        i.Crbal,
                        i.Drbal,
                        i.AccCodeNavigation, 
                        i.FirmId,
                        i.DivId


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst01100>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<ActionResult> insert(Mst01100 accOpningBal)
        {
            try
            { 
                accOpningBal.AccCodeNavigation = null;
                
                _context.Mst01100s.Add(accOpningBal);
                Acc999 lgr;
                await _context.SaveChangesAsync();
                lgr = new CreateLedger(_context).opening(accOpningBal);

                lgr.ChallanId = accOpningBal.VchId;

                _context.Acc999s.Add(lgr);
                await _context.SaveChangesAsync();
                rtn.data = accOpningBal;
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
        public async Task<IActionResult> update(long id, Mst01100 accOpningBal)
        {
            if (id != accOpningBal.VchId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }

            accOpningBal.AccCodeNavigation = null;
            _context.Entry(accOpningBal).State = EntityState.Modified;

            try
            {
                Acc999 lgr;
                var ledger = await _context.Acc999s
                       .Where(c => c.ChallanId == accOpningBal.VchId && c.VchType == 0)
                       .SingleOrDefaultAsync();

                if (ledger != null)
                    _context.Acc999s.Remove(ledger);



                _context.Acc999s.Add(new CreateLedger(_context).opening(accOpningBal));
                await _context.SaveChangesAsync();
                rtn.data = accOpningBal;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AccOpblExists(id))
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }
        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            Mst01100? accOpningBal = await _context.Mst01100s.FindAsync(id);
            if (accOpningBal == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            var ledger = _context.Acc999s.Where(c => c.ChallanId == id).ToList();

            if (ledger.Count() > 0)
                _context.Acc999s.RemoveRange(ledger);
            _context.Mst01100s.Remove(accOpningBal);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }
 

        private bool AccOpblExists(long id)
        {
            return _context.Mst01100s.Any((Mst01100 e) => e.VchId == id);
        }
    }
}

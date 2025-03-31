using Microsoft.AspNetCore.Mvc; 
using MotorMemo.Models.Context;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TdsController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public TdsController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst031>();

                var query = _context.Mst031s.Include(i => i.AccCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccCode)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchId,
                        i.SfCess,
                        i.HeCess,
                        i.ECess,
                        i.AccCode,
                        i.AccCodeNavigation,
                        i.TdsName,
                        i.Tds
                         
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst031>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Mst031 mst031)
        { 
            _context.Entry(mst031).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = mst031;
            }
            catch (DbUpdateConcurrencyException ex)
            {  
                 
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<ActionResult> insert(Mst031 mst031)
        {
            try
            { 
                _context.Mst031s.Add(mst031); 
                _context.SaveChanges();
                rtn.data = mst031;
            }
            catch (WebException ex2)
            {
                WebException ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

       
        [HttpDelete]
        public async Task<IActionResult> delete(long id)
        {
            try
            {
                Mst031? mst031 = await _context.Mst031s.FindAsync(id);
                if (mst031 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst031s.Remove(mst031);
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

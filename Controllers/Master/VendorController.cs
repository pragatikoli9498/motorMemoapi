using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public VendorController(MotorMemoDbContext context)
        {
            _context = context;
        }
 
        [HttpPost]
        public  ActionResult insert(Mst030 value)
        {
            try
            {
                value.AccCodeNavigation = null;
                _context.Mst030s.Add(value);
                _context.SaveChanges();
                rtn.data = value;
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


        [HttpGet]
        public ActionResult GstinbyInfo(string? gstin, string? mobileno)
      {

            try
            {
                rtn.data = _context.Mst030s
                       .Where(i => i.GstinNo == gstin  ||   i.MobileNo == mobileno)
                                    
                       .Select(i => new 
                {
                    i.Id,
                    i.AccCode,
                    i.AccCodeNavigation,
                           i.Name,
                    i.Address,
                    i.MobileNo,
                    i.StateCode,
                    i.GstinNo, 
                    i.pincode,
                    i.EmailId,
                    state = _context.Mst00603s.Where(w => w.StateCode == i.StateCode).FirstOrDefault(),
                }).FirstOrDefault();
}
            catch (WebException ex) {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }

            return Ok(rtn);
        }

       
        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst030>();

                var query = _context.Mst030s.Include(i => i.AccCodeNavigation);
                    
               
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccCode)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    { 
                        i.Id,
                        i.AccCode,
                        i.AccCodeNavigation,
                        i.Name,
                        i.Address,
                        i.MobileNo,
                        i.StateCode,
                        i.GstinNo,
                        i.pincode,
                        i.EmailId, 
                        state = _context.Mst00603s.Where(w=>w.StateCode == i.StateCode).FirstOrDefault(),
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst030>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                Mst030? x = await _context.Mst030s.FindAsync(id);
                if (x == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst030s.Remove(x);
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

        [HttpGet]
        public async Task<ActionResult<Mst030>> edit(int id)
        {
            try
            { 
                respayload respayload = rtn;
                respayload.data = await (from i in _context.Mst030s.Where(w => w.Id == id)
                        .Include(s => s.AccCodeNavigation).ThenInclude(ti => ti.Place).ThenInclude(ti => ti.Taluka).ThenInclude(ti => ti.District).ThenInclude(ti => ti.StateCodeNavigation).AsNoTracking()
                        .AsNoTracking()
                                         select new
                                         {
                                             i.StateCode,
                                             i.AccCodeNavigation,
                                             i.AccCode,
                                             state=i.AccCodeNavigation.Place.Taluka.District.StateCodeNavigation,
                                             i.AccCodeNavigation.Place,
                                             i.Id,
                                             i.GstinNo,
                                             i.Address,
                                             i.MobileNo,
                                             i.EmailId,
                                             i.pincode,
                                             i.Name

                                         }).SingleOrDefaultAsync();
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
        public async Task<IActionResult> update(long id, Mst030 mst030)
        {
            if (id != mst030.Id)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(mst030).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = mst030;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Mst002Exists(id))
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
        private bool Mst002Exists(long id)
        {
            return _context.Mst030s.Any((e) => e.Id == id);
        }
 

    }
}

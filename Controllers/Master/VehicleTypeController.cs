using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using MotorMemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Models.MainDbEntities;
using System.Data;
using Newtonsoft.Json;
using static MotorMemo.Models.Helper;
using MotorMemo.models;
using Microsoft.Data.Sqlite;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public VehicleTypeController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult insert(Mst107 value)
        {
            rtn.status_cd = 1;
 
            try
            { 
                _context.Mst107s.Add(value);
                _context.SaveChanges();
                rtn.data = value;
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex.InnerException?.Message ?? ex.Message;
                return Ok(rtn);
            }
            rtn.data = value;
            return Ok(rtn);
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst107>();

                var query = _context.Mst107s;

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VtypeName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {

                         i.VtypeName,
                         i.Capacity,
                         i.Vheight,
                         i.Vlength,
                         i.Vwidth,
                         i.VtypeId

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst107>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Mst107>> edit(int id)
        {
          rtn.data=await _context.Mst107s.Where (w=>w.VtypeId == id).FirstOrDefaultAsync();
            if (rtn.data == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var s = await _context.Mst107s.FindAsync(id);

                if (s == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst107s.Remove(s);
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
        public async Task<IActionResult> update(Mst107 value)
        {

            Mst107 value1 = value;
            Mst107? InvItemUnit = _context.Mst107s.Where((Mst107 w) => w.VtypeId == value1.VtypeId).SingleOrDefault();
            if (InvItemUnit != null)
            {
                _context.Entry(InvItemUnit).CurrentValues.SetValues(value1);
            }
            else
            {
                _context.Mst107s.Add(value1);
            }
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = value1;
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
        public async Task<ActionResult> list()
        {
            try
            {
                 
                rtn.data = await _context.Mst107s.AsNoTracking().ToListAsync();
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

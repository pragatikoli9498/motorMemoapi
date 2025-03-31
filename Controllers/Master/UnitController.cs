using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UnitController : ControllerBase
    {

        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public UnitController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getUnit(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Mst012>();

                var query = _context.Mst012s;
 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.UnitName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {

                        i.UnitCode,
                        i.UnitName,


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst012>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult> list()
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await _context.Mst012s.AsNoTracking().ToListAsync();
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
        public async Task<ActionResult<Mst012>> Unitedit(string id)
        {
            await _context.Mst012s.FindAsync(id);
            if (rtn.data == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }
        [HttpPut]
        public async Task<IActionResult> update(Mst012 unit)
        {

            Mst012 unit2 = unit;
            Mst012? InvItemUnit = _context.Mst012s.Where((Mst012 w) => w.UnitCode == unit2.UnitCode).SingleOrDefault();
            if (InvItemUnit != null)
            {
                _context.Entry(InvItemUnit).CurrentValues.SetValues(unit2);
            }
            else
            {
                _context.Mst012s.Add(unit2);
            }
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = unit2;
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
        public async Task<IActionResult> delete(string id)
        {
            Mst012? mst012 = await _context.Mst012s.FindAsync(id);
            if (mst012 == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            _context.Mst012s.Remove(mst012);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }

        private bool InvItemUnitExists(string id)
        {
           
            return _context.Mst012s.Any(e => e.UnitCode == id);
        }
    }
}

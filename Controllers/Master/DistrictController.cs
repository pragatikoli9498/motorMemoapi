using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;


namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {

        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public DistrictController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getDistrict(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst00602>();

                var query = _context.Mst00602s.Include(i => i.StateCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.DistrictName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.DistrictId,
                        i.StateCode,
                        i.DistrictName,
                        i.StateCodeNavigation

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00602>.ToPagedList(data, page.PageNumber, page.PageSize);
            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }


        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Mst00602>();

                var query = _context.Mst00602s.Include(i => i.StateCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.DistrictName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.DistrictId,
                        i.StateCode,
                        i.DistrictName,
                        i.StateCodeNavigation


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00602>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
       
        [HttpPost]
        public async Task<ActionResult> getdistrictbystateCode(QueryStringParameters page, long stateCode, bool OrElse = false)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst00602>();
                var query = _context.Mst00602s
                           .Include(i => i.StateCodeNavigation)
                           .AsNoTracking()
                          .Where(w => w.StateCode == stateCode);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.DistrictName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.DistrictId,
                        i.DistrictName,
                        i.StateCode,
                        i.StateCodeNavigation,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00602>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex.Message;
            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult> list()
        {
            respayload respayload = rtn;
            respayload.data = await _context.Mst00602s.Include((s) => s.StateCodeNavigation).ToListAsync();
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Mst00602>> Distedit(long id)
        {
            await _context.Mst00602s.FindAsync(id);
            if (rtn == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Mst00602 district)
        {
            if (id != district.DistrictId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(district).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = district;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Mst00602Exists(id))
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

        [HttpPost]
        public async Task<ActionResult> insert(Mst00602 district)
        {
            try
            {
                district.StateCodeNavigation = null;
                _context.Mst00602s.Add(district);
                await _context.SaveChangesAsync();
                rtn.data = district;
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
            Mst00602? mst00602 = await _context.Mst00602s.FindAsync(id);
            if (mst00602 == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            _context.Mst00602s.Remove(mst00602);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }
        private bool Mst00602Exists(long id)
        {
            return _context.Mst00602s.Any((e) => e.DistrictId == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public PlaceController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getPlace(QueryStringParameters page)
        {
            try
            {
                 
                var filter = new EntityFrameworkFilter<Mst006>();

                var query = _context.Mst006s.Include(i => i.Taluka)
                                     .ThenInclude(i => i.District).ThenInclude(i => i.StateCodeNavigation) ;
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.CityName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.CityId,
                        i.CityPin,
                        i.CityName,
                        i.Taluka

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst006>.ToPagedList(data, page.PageNumber, page.PageSize);

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
            respayload respayload = rtn;
            respayload.data = await (from s in _context.Mst006s.AsNoTracking().Include((Mst006 i) => i.Taluka).ThenInclude((Mst00601 i) => i.District)
                    .ThenInclude((Mst00602 i) => i.StateCodeNavigation)
                    .AsNoTracking()
                                     select new { s.CityId, s.CityPin, s.CityName, s.Taluka }).ToListAsync();
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult> Placeedit(long id)
        { 
            rtn.status_cd = 1;
            try
            {
                rtn.data = await _context.Mst006s.AsNoTracking()
                   .Where(w => w.CityId == id)

                   .Include(i => i.Taluka).AsNoTracking()
                  .Select(s => new
                  {
                      s.CityId,
                      s.CityPin,
                      s.CityName,
                      s.TalukaId,
                      Taluka = s.Taluka,
 

                  })
                   .SingleOrDefaultAsync();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "422",
                        message = "Record Not Found"
                    };
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
        public async Task<IActionResult> update(long id, Mst006 place)
        {
            if (id != place.CityId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(place).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = place;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PlaceExists(id))
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
        public async Task<ActionResult> insert(Mst006 mst006)
        {
            try
            {
                mst006.Taluka = null;
                _context.Mst006s.Add(mst006);
                await _context.SaveChangesAsync();
                rtn.data = mst006;
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
        public async Task<IActionResult> delete(int id)
        {
            Mst006? mst006 = await _context.Mst006s.FindAsync(id);
            if (mst006 == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            _context.Mst006s.Remove(mst006);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }
        private bool PlaceExists(long id)
        {
            return _context.Mst006s.Any((Mst006 e) => e.CityId == id);
        }
    }
}

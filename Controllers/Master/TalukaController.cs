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
    public class TalukaController : ControllerBase
    {

        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public TalukaController(MotorMemoDbContext context)
        {
            _context = context;
        }

        public ActionResult getTaluka(QueryStringParameters page)
        {
            try
            {
                 
                var filter = new EntityFrameworkFilter<Mst00601>();

                var query = _context.Mst00601s.Include(i => i.District).ThenInclude(i => i.StateCodeNavigation);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.TalukaName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.TalukaId,
                        i.DistrictId,
                        i.TalukaName,
                        i.District,
  
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00601>.ToPagedList(data, page.PageNumber, page.PageSize);

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

                var filter = new EntityFrameworkFilter<Mst00601>();
                var a= _context.Database.GetConnectionString();

                var query = _context.Mst00601s.Include(i => i.District).ThenInclude(s => s.StateCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.TalukaName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.TalukaId,
                        i.DistrictId,
                        i.TalukaName,
                        i.District,
                        i.District.StateCodeNavigation

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00601>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;
            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<ActionResult> gettalukabydistCode(QueryStringParameters page, long districtId, bool OrElse = false)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst00601>();
                var query = _context.Mst00601s
                      .Include(i => i.District)
                      .AsNoTracking()
                     .Where(w => w.DistrictId == districtId);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.TalukaName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.TalukaId,
                        i.TalukaName,
                        i.DistrictId,
                        i.District,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00601>.ToPagedList(data, page.PageNumber, page.PageSize);

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
            respayload.data = await _context.Mst00601s.Include(s => s.District).ThenInclude(s => s.StateCodeNavigation).ToListAsync();
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Mst00601>> Talukaedit(long id)
        {
            try
            {
                await _context.Mst00602s.FindAsync(id);
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
        public async Task<IActionResult> update(long id, Mst00601 taluka)
        {
            if (id != taluka.TalukaId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(taluka).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = taluka;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TalukaExists(id))
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
        public async Task<ActionResult> insert(Mst00601 taluka)
        {
            try
            {
                taluka.District = null;
                _context.Mst00601s.Add(taluka);
                await _context.SaveChangesAsync();
                rtn.data = taluka;
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
                Mst00601? Mst00601 = await _context.Mst00601s.FindAsync(id);
                if (Mst00601 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst00601s.Remove(Mst00601);
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
      
        private bool TalukaExists(long id)
        {
            return _context.Mst00601s.Any((e) => e.TalukaId == id);
        }

    }
}

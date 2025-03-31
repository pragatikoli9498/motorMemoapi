using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using MotorMemo.Models.MainDbEntities;  

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FirmController : ControllerBase
    {
        private readonly MainDbContext _context;

        private respayload rtn = new respayload();

        public FirmController(MainDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Mst004>();

                var query = _context.Mst004s.Include(i => i.Mst00401).Include(i => i.Mst00603);




                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.FirmName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FirmCode,
                        i.FirmName,
                        i.FirmPlace,
                        i.FirmStateCode,
                        i.FirmFno,
                        i.EmailId,
                        i.Mst00603,
                        //i.FinYears,
                        i.Mst00401,
                        i.FirmAlias
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst004>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                respayload.data = await _context.Mst004s.Include((Mst004 i) => i.Mst00401).Include((Mst004 i) => i.Mst00603)
                    .ToListAsync();
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult<Mst004>> FirmEdit(long id)
        {
            try
            {
                respayload respayload = rtn;
                //respayload.data = await _context.Mst004s
                //     .Where(w => w.FirmCode == id)
                //        .Include(i => i.Mst00401)
                //          .Include(i => i.Mst00603)
                //            .Include(i => i.Mst00409s)
                //              .Include(i => i.Mst00403s)
                //                .AsNoTracking()
                //.SingleOrDefaultAsync();
                respayload.data = await _context.Mst004s.Where(w => w.FirmCode == id)
                    .Include(i => i.Mst00401).AsTracking()
                       .Include(i => i.Mst00603).AsTracking()
                          .Include(i => i.Mst00409).AsTracking()
                             .Include(i => i.Mst00403s).AsTracking()
                    .SingleOrDefaultAsync();
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
            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<ObjectResult> insert(Mst004 firm)
        {
            try
            {
                firm.CreatedDt = DateTime.Now.ToString("yyyy/MM/dd"); ;
                if (firm.FirmCode == 0)
                {
                    firm.FirmCode = (await _context.Mst004s.MaxAsync((Mst004 c) => (int?)c.FirmCode)).GetValueOrDefault() + 1;
                }
                _context.Add(firm);
                await _context.SaveChangesAsync();
                rtn.status_cd = 1;
                rtn.data = firm;
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<ObjectResult> update(Mst004 mst004, int firm_code)
        {
            if (firm_code != mst004.FirmCode)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            try
            {
                mst004.Mst00603 = null;
                Mst004? ExistingParent = await _context.Mst004s.Where((Mst004 a) => a.FirmCode == (long)firm_code).Include((Mst004 a) => a.Mst00401)
                    .SingleOrDefaultAsync();
                if (ExistingParent == null)
                {
                    mst004.CreatedDt = DateTime.Now.ToString("yyyy/MM/dd");
                    mst004.ModifiedUser = null;
                    _context.Mst004s.Add(mst004);
                }
                else
                {
                    mst004.ModifiedDt = DateTime.Now.ToString("yyyy/MM/dd"); ;
                    _context.Entry(ExistingParent).CurrentValues.SetValues(mst004);
                    Mst00401? ExistingChild3 = ExistingParent.Mst00401;
                    if (ExistingChild3 != null)
                    {
                        _context.Entry(ExistingChild3).CurrentValues.SetValues(mst004.Mst00401);
                    }
                    else if (mst004.Mst00401 != null)
                    {
                        mst004.Mst00401.FirmCode = firm_code;
                        _context.Mst00401s.Add(mst004.Mst00401);
                    }

                }
                await _context.SaveChangesAsync();
                rtn.data = mst004;
            }
            catch (Exception ex)
            {
                if (!FirmsExists(firm_code))
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
        public async Task<IActionResult> delete(int firm_code)
        {
            try
            {
                Mst004? firms = await _context.Mst004s.FindAsync(firm_code);
                if (firms == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst004s.Remove(firms);
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
        private bool FirmsExists(long id)
        {
            return _context.Mst004s.Any((Mst004 e) => e.FirmCode == id);
        }
    }
}

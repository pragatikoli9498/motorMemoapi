using Microsoft.AspNetCore.Mvc; 
using MotorMemo.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using MotorMemo.Models.MainDbEntities;
using Microsoft.AspNetCore.Http;  

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FinYearsController : ControllerBase
    {
        private readonly MainDbContext _context;

        private respayload rtn = new respayload();

        public FinYearsController(MainDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Mst005>();

                var query = _context.Mst005s.Include(i => i.Mst004 );




                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.FirmCode)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FirmCode, 
                        i.DivId,
                        i.FromDivId,
                        i.Fdt,
                        i.Tdt,  
                        i.Mst004



                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst005>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                rtn.data = await _context.Mst005s.Include((Mst005 i) => i.Mst004).

                    ToListAsync();

            }

            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }
            return Ok(rtn);

        }
        [HttpGet]
        public async Task<ActionResult> lists(long firmCode)
        {
            try
            {
                rtn.data = await _context.Mst005s.Where(w => w.FirmCode == firmCode)
                    .Include((Mst005 i) => i.Mst004).

                    ToListAsync();

            }


            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;


            }
            return Ok(rtn);

        }
        [HttpGet]
        public async Task<ActionResult<Mst005>> get(long firmCode, string divId)
        {

            try
            {
                rtn.data = await _context.Mst005s.Where(w => w.DivId == divId && w.FirmCode == firmCode ).SingleOrDefaultAsync();
                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;


            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult<Mst005>> getCurrToNewYear(long firmCode, string branchCode)
        {

            try
            {
                rtn.data = await _context.Mst005s.Where(w => w.FirmCode == firmCode )
                    .OrderByDescending(o => o.Fdt)
                    .Select(s => new Mst005
                    {
                        FirmCode = s.FirmCode, 
                        FromDivId = s.DivId,
                        DivId = (s.Fdt.Year + 1).ToString() + (s.Tdt.Year + 1).ToString(),
                        Fdt = s.Fdt.AddYears(1),
                        Tdt = s.Tdt.AddYears(1),
                        Prefix = s.Fdt.AddYears(1).Year.ToString().Substring(2) + s.Tdt.AddYears(1).Year.ToString().Substring(2)

                    })
                    .FirstOrDefaultAsync();

                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;


            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Mst005>> geLastTworYear(long firmCode)
        {

            try
            {
                rtn.data = await _context.Mst005s.Where(w => w.FirmCode == firmCode)
                    .OrderByDescending(o => o.Fdt)
                    .Select(s => new Mst005
                    {
                        FirmCode = s.FirmCode,
                        DivId = s.DivId,


                    }).Take(2).ToListAsync();
                   

               
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;


            }
            return Ok(rtn);
        }
        [HttpPut]
        public async Task<ObjectResult> update(Mst005 finyear)
        {



            try
            {

                var ExistingParent = await _context.Mst005s

                        .Where(a => a.DivId == finyear.DivId && a.FirmCode == finyear.FirmCode )
                        .SingleOrDefaultAsync();

                if (ExistingParent == null)
                {
                    finyear.CreatedDt = DateTime.Now.ToString("yyyy/mm/dd");
                    finyear.ModifiedUser = null;

                    _context.Mst005s.Add(finyear);



                }
                else
                {
                    finyear.ModifiedDt = DateTime.Now.ToString("yyyy/mm/dd");
                    _context.Entry(ExistingParent).CurrentValues.SetValues(finyear);
                }

                await _context.SaveChangesAsync();
                rtn.data = finyear;
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
        public async Task<IActionResult> delete(int firmCode, string divId)
        {
            try
            {
                 

                Mst005 mst005 = await _context.Mst005s.Where(w => w.FirmCode == firmCode && w.DivId == divId).FirstOrDefaultAsync();

                if (mst005 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }

                _context.Mst005s.Remove(mst005);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);

            }
            return Ok(rtn);
        }

    }
}

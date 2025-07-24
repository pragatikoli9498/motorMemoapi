using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;

using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;


namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public StateController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getStates(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Mst00603>();

                var query = _context.Mst00603s;

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.StateName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.StateCode,

                        i.StateName,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst00603>.ToPagedList(data, page.PageNumber, page.PageSize);

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
            respayload.data = await _context.Mst00603s.ToListAsync();
            return Ok(respayload);
        }



        [HttpGet]
        public async Task<ActionResult> ListFromPlace(int placeId)
        {
            var respayload = new respayload(); // or use 'var respayload = rtn;' if 'rtn' is predefined

            try
            {
                var result = await _context.Mst00603s
                    .Include(i => i.Mst00602s)
                        .ThenInclude(i1 => i1.Mst00601s)
                            .ThenInclude(i2 => i2.Mst006s)
                    .Where(w => w.Mst00602s.Any(a =>
                        a.Mst00601s.Any(a1 =>
                            a1.Mst006s.Any(a2 => a2.CityId == placeId))))
                    .SingleOrDefaultAsync();

                respayload.data = result;
            }
            catch (Exception ex)
            {
                respayload.status_cd = 0;
                respayload.errors.message = ex.Message;
            }

            return Ok(respayload);
        }

        [HttpGet]
        public async Task<ActionResult> ListFromAcc(int accCode)
        {
            var respayload = new respayload(); // or use 'var respayload = rtn;' if 'rtn' is predefined

            try
            {
                var result = await _context.Mst00603s
                    .Include(i => i.Mst00602s)
                        .ThenInclude(i1 => i1.Mst00601s)
                            .ThenInclude(i2 => i2.Mst006s).
                            ThenInclude(i3 => i3.Mst011s)
                    .Where(w => w.Mst00602s.Any(a =>
                        a.Mst00601s.Any(a1 => a1.Mst006s.Any(a2 => a2.Mst011s.Any(a3 => a3.AccCode==accCode)))))
                    .SingleOrDefaultAsync();

                respayload.data = result;
            }
            catch (Exception ex)
            {
                respayload.status_cd = 0;
                respayload.errors.message = ex.Message;
            }

            return Ok(respayload);
        }

        //[HttpPost]
        //public ActionResult ListFromPlace(QueryStringParameters page, int accCode)
        //{
        //    try
        //    {

        //        var filter = new EntityFrameworkFilter<Mst00603>();

        //        var query = _context.Mst00603s.Include(i => i.Accc)
        //                .ThenInclude(i1 => i1.Mst00601s)
        //                    .ThenInclude(i2 => i2.Mst006s).AsNoTracking()
        //            .Where(w => w.Mst00602s.Any(i => i.Mst00601s.Any(i1 => i1.Mst006s.Any(i3 => i3.CityId == placeId))))
        //           ;

        //        var data = filter.Filter(query, page.keys);

        //        rtn.data = data.OrderBy(o => o.StateName)
        //             .Skip((page.PageNumber - 1) * page.PageSize)
        //            .Take(page.PageSize).Select(i => new
        //            {
        //                i.StateCode,
        //                i.StateName,

        //            }).ToList();
        //        if (page.PageNumber == 1)
        //            rtn.PageDetails = PageDetail<Mst00603>.ToPagedList(data, page.PageNumber, page.PageSize);

        //    }
        //    catch (Exception ex2)
        //    {

        //        rtn.status_cd = 0;
        //        rtn.errors.message = ex2.Message;

        //    }
        //    return Ok(rtn);
        //}

    }
}

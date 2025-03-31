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
            return Ok(rtn);
        }
    }
}

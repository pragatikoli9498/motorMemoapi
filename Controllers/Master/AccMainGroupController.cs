using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccMainGroupController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public AccMainGroupController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getGroups(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Mst001>();

                var query = _context.Mst001s;



                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.MgName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.MgCode,

                        i.MgName,


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst001>.ToPagedList(data, page.PageNumber, page.PageSize);

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
            respayload.data = await _context.Mst001s.ToListAsync();
            return Ok(rtn);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SubGroupController : Controller
    {

        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public SubGroupController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getGroup(QueryStringParameters page)
        {
            try
            {
                 
                var filter = new EntityFrameworkFilter<Mst003>();

                var query = _context.Mst003s.Include(i => i.GrpCodeNavigation);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.Include(i => i.GrpCodeNavigation).ThenInclude(i => i.MgCodeNavigation).OrderBy(o => o.SgName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SgCode,
                        i.GrpCode,
                        i.SgName,
                        i.GrpCodeNavigation


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst003>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                var filter = new EntityFrameworkFilter<Mst003>();

                var query = _context.Mst003s.Include(i => i.GrpCodeNavigation);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.SgName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SgCode,
                        i.GrpCode,
                        i.SgName,
                        i.SrNo,
                        i.GrpCodeNavigation

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst003>.ToPagedList(data, page.PageNumber, page.PageSize);

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
            respayload.data = await _context.Mst003s.Include((Mst003 s) => s.GrpCodeNavigation).ToListAsync();
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Mst003>> SubGroup(long id)
        {
            await _context.Mst003s.FindAsync(id);
            if (rtn.data == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Mst003 subGroup)
        {
            if (id != subGroup.SgCode)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(subGroup).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = subGroup;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AccSubGroupExists(id))
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
        public async Task<ActionResult> insert(Mst003 mst003)
        {
            try
            {
                mst003.GrpCodeNavigation = null;
                _context.Mst003s.Add(mst003);
                await _context.SaveChangesAsync();
                rtn.data = mst003;
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
            Mst003? AccSubGroup = await _context.Mst003s.FindAsync(id);
            if (AccSubGroup == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            _context.Mst003s.Remove(AccSubGroup);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }
        private bool AccSubGroupExists(long id)
        {
            return _context.Mst003s.Any((Mst003 e) => e.SgCode == id);
        }
    }
}

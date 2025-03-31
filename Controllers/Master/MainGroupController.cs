using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context; 
using Microsoft.EntityFrameworkCore; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MainGroupController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public MainGroupController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getGroups(QueryStringParameters page)
        {
            try
            { 
                var filter = new EntityFrameworkFilter<Mst002>();

                var query = _context.Mst002s.Include(i => i.MgCodeNavigation);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.GrpName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.MgCode,
                        i.GrpCode,
                        i.GrpName,
                        i.MgCodeNavigation

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst002>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                var filter = new EntityFrameworkFilter<Mst002>();

                var query = _context.Mst002s.Include(i => i.MgCodeNavigation);
                 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.GrpName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.MgCode,
                        i.GrpCode,
                        i.GrpName,
                        i.MgCodeNavigation,
                        i.SrNo


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst002>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                respayload.data = await _context.Mst002s.Include((Mst002 s) => s.MgCodeNavigation).ToListAsync();
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
        public async Task<ActionResult> getGroup(long id)
        {
            respayload respayload = rtn;
            respayload.data = await _context.Mst002s.FindAsync(id);
            if (rtn.data == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }
        [HttpPut]
        public async Task<IActionResult> update(long id, Mst002 mst002)
        {
            if (id != mst002.GrpCode)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(mst002).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = mst002;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Mst002Exists(id))
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
        public async Task<ActionResult> insert(Mst002 mst002)
        {
            try
            {
                //mst002.MgCodeNavigation = null;
                _context.Mst002s.Add(mst002);
                //await _context.SaveChangesAsync();
                rtn.data = mst002;
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
            try
            {
                Mst002? mst002 = await _context.Mst002s.FindAsync(id);
                if (mst002 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst002s.Remove(mst002);
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

        private bool Mst002Exists(long id)
        {
            return _context.Mst002s.Any((Mst002 e) => e.GrpCode == id);
        }
    }
}

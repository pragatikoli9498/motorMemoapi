using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.IO;

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleInfoController : ControllerBase
    {
        private MainDbContext _context;

        private respayload rtn = new respayload();


        public RoleInfoController(MainDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Sys00202>();

                var query = _context.Sys00202s;




                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.RoleName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.RoleId,
                        i.RoleName,
                        i.Comments,
                        i.Sys00203s,
                        //i.UserRoleAuth




                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Sys00202>.ToPagedList(data, page.PageNumber, page.PageSize);

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

                respayload.data = await _context.Sys00202s.ToListAsync();
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }

        //[HttpGet]
        //public async Task<ActionResult<Firms>> getroleinfo(long roleid)
        //{
        //    try
        //    {
        //        respayload respayload = rtn;
        //        respayload.data = await _context.RoleInfos.Where(w => w.RoleId == roleid
        //          ).SingleOrDefaultAsync();




        //        if (rtn.data == null)
        //        {
        //            rtn.status_cd = 0;
        //            rtn.errors.message = "Record Not Found";
        //        }
        //    }
        //    catch (Exception ex2)
        //    {
        //        Exception ex = ex2;
        //        rtn.status_cd = 0;
        //        rtn.errors.exception = ex;
        //    }
        //    return Ok(rtn);
        //}
        [HttpPost]
        public async Task<ObjectResult> insert(Sys00202 roleInfo)
        {
            try
            {

                _context.Add(roleInfo);
                await _context.SaveChangesAsync();
                rtn.status_cd = 1;
                rtn.data = roleInfo;
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
        public async Task<ObjectResult> update(Sys00202 roleInfo, long roleid)
        {
            if (roleid != roleInfo.RoleId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            try
            {

                var ExistingParent = await _context.Sys00202s.Where((a) => a.RoleId == (long)roleid).SingleOrDefaultAsync();
               
                _context.Entry(ExistingParent).CurrentValues.SetValues(roleInfo);




                await _context.SaveChangesAsync();
                rtn.data = roleInfo;
            }
            catch (Exception ex)
            {
               
                  
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        private bool RoleInfoExists(Sys00202 roleInfo)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int roleid)
        {
            try
            {
                Sys00202? roleInfo = await _context.Sys00202s.FindAsync(roleid);
                if (roleInfo == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Sys00202s.Remove(roleInfo);
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
    }
}

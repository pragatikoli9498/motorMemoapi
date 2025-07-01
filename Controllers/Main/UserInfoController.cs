using dsserp.commans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Services;

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private MainDbContext db;
        private respayload rtn = new respayload();


        public UserInfoController(MainDbContext context)
        {
            db = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Sys00203>();

                var query = db.Sys00203s.Include(i => i.Role);
                    
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.UserName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.UserName,
                        i.UserId,
                        i.RoleId,
                        i.Role,
                        i.Mobileno,
                        i.UserLongname
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Sys00203>.ToPagedList(data, page.PageNumber, page.PageSize);

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

                rtn.data = await db.Sys00203s.Include((Sys00203 i) => i.Role).ToListAsync();

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
        public async Task<ActionResult<Sys00203>> getuserinfo(long userid)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await db.Sys00203s.Where(w => w.UserId == userid
                  ).Include(i => i.Role).SingleOrDefaultAsync();




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

        [HttpGet]
        public async Task<ActionResult> UserByRole(long roleid)
        {
            try
            {
                respayload respayload = rtn;

                respayload.data = await db.Sys00203s.Where(w => w.RoleId == roleid
                )
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


        [HttpPost]
        public async Task<ObjectResult> insert(Sys00203 userInfo)
        {

            userInfo.Sys00201.Password = password.EncryptPass(userInfo.Sys00201.Password);

            try
            {
                var Existing = await db.Sys00203s.Where(w => w.UserId == userInfo.UserId).SingleOrDefaultAsync();
                if (Existing == null)

                    db.Sys00203s.Add(userInfo);
                else
                    db.Entry(Existing).CurrentValues.SetValues(userInfo);

                await db.SaveChangesAsync();
                rtn.data = userInfo;

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
        public async Task<ObjectResult> update(Sys00203 userInfo, long userid)
        {
            if (userid != userInfo.UserId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            try
            {

                Sys00203? ExistingParent = await db.Sys00203s.Where((a) => a.UserId == (long)userid).SingleOrDefaultAsync();
                db.Entry(ExistingParent).CurrentValues.SetValues(userInfo);

                await db.SaveChangesAsync();
                rtn.data = userInfo;
            }
            catch (Exception ex)
            {
   
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        private bool UserInfoExists(Sys00203 userInfo)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int userid)
        {
            try
            {
                Sys00203? userInfo = await db.Sys00203s.FindAsync(userid);
                if (userInfo == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                db.Sys00203s.Remove(userInfo);
                await db.SaveChangesAsync();
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

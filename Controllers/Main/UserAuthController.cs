using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailDesk.Models;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.Context;
namespace RetailDesk.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserAuthController : Controller
    {
        private MainDbContext db;
        private respayload rtn = new respayload();

        public UserAuthController(MainDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult> list()
        {
            try
            {

                respayload respayload = rtn;

                respayload.data = await db.Sys00204s.Include(i => i.User).
                    Select(s => new
                    {

                        s.UserId,
                        s.A,
                        s.O,
                        s.P,
                        s.L,
                        s.EAdmin,
                        s.Sysadmin,
                        s.E,
                        s.D,
                        s.J,
                        s.User,

                    }).OrderBy(o => o.UserId).ToListAsync();
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
        public async Task<ObjectResult> userauth()
        {

            try
            {
                rtn.data = await db.Sys00204s

                    .Select(s => new
                    {
                        s.UserId,
                        s.A,
                        s.O,
                        s.P,
                        s.L,
                        s.EAdmin,
                        s.Sysadmin,
                        s.E,
                        Delete = s.D,
                        s.J

                    }).ToListAsync();
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;


            }

            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Sys00204>> get(long userid)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await db.Sys00204s.Where(w => w.UserId == userid
                  ).SingleOrDefaultAsync();
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
        public async Task<ObjectResult> insert(List<Sys00204> userAuths)
        {
            try
            {


                foreach (var item in userAuths)
                {
                    var existingItem = db.Sys00204s.Where(w => w.UserId == item.UserId).FirstOrDefault();
                    if (existingItem != null)
                        db.Entry(existingItem).CurrentValues.SetValues(item);
                    else
                        db.Sys00204s.Add(item);



                };

                await db.SaveChangesAsync();
                rtn.data = userAuths;
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }



        private bool UserAuthExists(Sys00204 userAccess)
        {
            throw new NotImplementedException();
        }

    }
}

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
    public class UserRoleAuthController : Controller
    {
        private MainDbContext db;
        private respayload rtn = new respayload();
        public UserRoleAuthController(MainDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult> list()
        {
            try
            {

                respayload respayload = rtn;

                respayload.data = await db.Sys00205s
                    .Include(i => i.User)
                    .
                    Select(s => new

                    {


                        s.UserId,
                        s.A,
                        s.O,
                        s.P,
                        s.L,
                        s.R,
                        s.E,
                        s.D,
                        s.B,
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


        [HttpPost]
        public async Task<ObjectResult> insert(List<Sys00205> userRoleAuths)
        {
            try
            {


                foreach (var item in userRoleAuths)
                {
                    var existingItem = db.Sys00205s.Where(w => w.UserId == item.UserId).FirstOrDefault();
                    if (existingItem != null)
                        db.Entry(existingItem).CurrentValues.SetValues(item);
                    else
                        db.Sys00205s.Add(item);



                };

                await db.SaveChangesAsync();
                rtn.data = userRoleAuths;

            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailDesk.Models;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.Context; 

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ModulesController : Controller
    {
        private MainDbContext db;
        private respayload rtn = new respayload();
        public ModulesController(MainDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ObjectResult> list(int userid)
        {




            try
            {
                respayload respayload = rtn;
                respayload.data = await db.Modules.OrderBy(o => o.Modulename).Select(s => new ModulePermission
                {
                    Id = s.Sys00207s.Where(w => w.UserId == userid).Select(i=>i.Id).FirstOrDefault(),
                    ModuleId = s.Id,
                    UserId = userid,
                    ModuleName = s.Modulename,
                    Status = s.Sys00207s.Where(w => w.UserId == userid).Count() > 0 ? true : false

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
        public async Task<ObjectResult> getmodules()
        {




            try
            {
                respayload respayload = rtn;
                respayload.data = await db.Modules.OrderBy(o => o.Modulename).Select(s => new ModulePermission
                {

                    ModuleId = s.Id,

                    ModuleName = s.Modulename,
                    Status = false

                }).ToListAsync();

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }

            return Ok(rtn);


        }

        [HttpPut]
        public async Task<ObjectResult> update(List<ModulePermission> modulePermissions)
        {


            if (!ModelState.IsValid)
            {
                rtn.status_cd = 0;
                rtn.errors.error_cd = "500";
                rtn.errors.exception = ModelState;

                return Ok(rtn);

            }

            try
            {
                foreach (var module in modulePermissions)
                {
                    var existingmodule = await db.ModuleUsers.Where(w => w.Id == module.Id).SingleOrDefaultAsync();


                    
                        if (module.Status)
                        {
                            if (existingmodule == null)
                                db.ModuleUsers.Add(module);
                            else
                                db.Entry(existingmodule).CurrentValues.SetValues(existingmodule);
                        }
                        else

                        {
                            if (existingmodule != null)
                            {

                                db.ModuleUsers.Remove(existingmodule);

                            }

                        }
                    
                    await db.SaveChangesAsync();
                    rtn.data = modulePermissions;
                }
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.error_cd = "500";
                rtn.errors.exception = ex;

            }

            return Ok(rtn);

        }


        public class ModulePermission : Sys00207
        {
            public bool Status { get; set; }
            public string ModuleName { get; set; } = null!;

        }
    }
}

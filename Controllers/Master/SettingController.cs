using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Reflection.Emit;
using System.Security.Principal;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext _mainDb;

        private respayload rtn = new respayload();

        public SettingController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            _mainDb = mainDb;
        }

        //[HttpGet]
        //public async Task<ActionResult> list()
        //{
        //    try
        //    {
        //        respayload respayload = rtn;
        //        respayload.data = await _context.Settings.AsNoTracking().ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        rtn.status_cd = 0;
        //        rtn.errors.message = ex.Message;
        //    }
        //    return Ok(rtn);
        //}

        [HttpGet]
        public async Task<ActionResult> list()
        {
            try
            {
                respayload respayload = rtn;

                var settings = await _context.Settings.AsNoTracking().ToListAsync();
                var result = new List<object>();

                // Only process settings with SetCode = 101
                var filteredSettings = settings.Where(a => a.SetCode == 101).ToList();

                foreach (var setting in filteredSettings)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int accCode))
                    {
                        var account = await _context.Mst011s
                            .AsNoTracking()
                            .Where(a => a.AccCode == accCode)
                            .Select(a => new
                            {
                                a.AccCode,
                                a.AccAlias,
                                a.AccName
                            })
                            .FirstOrDefaultAsync();

                        if (account != null)
                        {
                            result.Add(new
                            {
                                Id=setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                AccCodeNavigation = account,
                                AccCode = account.AccCode,
                                AccName = account.AccName
                            });
                        }
                    }
                }

                var filteredSettings1 = settings.Where(a => a.SetCode == 102).ToList();

                foreach (var setting in filteredSettings1)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int accCode))
                    {
                        var account = await _context.Mst011s
                            .AsNoTracking()
                            .Where(a => a.AccCode == accCode)
                            .Select(a => new
                            {
                                a.AccCode,
                                a.AccAlias,
                                a.AccName
                            })
                            .FirstOrDefaultAsync();

                        if (account != null)
                        {
                            result.Add(new
                            {
                                Id = setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                AccCodeNavigation = account,
                                AccCode = account.AccCode,
                                AccName = account.AccName
                            });
                        }
                    }
                }

                var filteredSettings2 = settings.Where(a => a.SetCode == 103).ToList();

                foreach (var setting in filteredSettings2)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int sgCode))
                    {
                        var group = await _context.Mst003s.AsNoTracking().Include(i => i.GrpCodeNavigation)
                            .AsNoTracking()
                            .Where(a => a.SgCode == sgCode)
                            .Select(a => new
                            {
                                a.SgCode,
                                a.SgName,
                                a.GrpCodeNavigation
                            })
                            .FirstOrDefaultAsync();

                        if (group != null)
                        {
                            result.Add(new
                            {
                                Id = setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                SgCodeNavigation = group,
                               
                            });
                        }
                    }
                }

                respayload.data = result;
                return Ok(respayload);
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex.Message;
                return Ok(rtn);
            }
        }


        [HttpGet]
        public async Task<ActionResult<setting>> get(long id)
        {
            try
            {
                rtn.data = await _context.Settings.AsNoTracking().
                    Where(w => w.Id == id).SingleOrDefaultAsync();
                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";

                }

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);

        }

        [HttpGet]
        public async Task<ActionResult<setting>> setcode(int setcode)
        {
            try
            {
                rtn.data = await _context.Settings.AsNoTracking().
                    Where(w => w.SetCode == setcode).SingleOrDefaultAsync();
                if (rtn.data == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";

                }

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);

        }

        [HttpPut]
        public async Task<IActionResult> update(List<setting> settings)
        {
            try
            {

                foreach (var setting in settings)
                {
                    var existingParent = _context.Settings.Where(w => w.Id == setting.Id).Single();


                    if (existingParent != null)
                    {
                        _context.Entry(existingParent).CurrentValues.SetValues(setting);
                    }
                    else
                    {
                        _context.Settings.Add(setting);
                    }
                }
                await _context.SaveChangesAsync();
                var setting1 = settings;

                var result = new List<object>();

                var filteredSettings = setting1.Where(a => a.SetCode == 101).ToList();

                foreach (var setting in filteredSettings)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int accCode))
                    {
                        var account = await _context.Mst011s
                            .AsNoTracking()
                            .Where(a => a.AccCode == accCode)
                            .Select(a => new
                            {
                                a.AccCode,
                                a.AccAlias,
                                a.AccName
                            })
                            .FirstOrDefaultAsync();

                        if (account != null)
                        {
                            result.Add(new
                            {
                                Id = setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                AccCodeNavigation = account,
                                AccCode = account.AccCode,
                                AccName = account.AccName
                            });
                        }
                    }
                }

                var filteredSettings1 = setting1.Where(a => a.SetCode == 102).ToList();

                foreach (var setting in filteredSettings1)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int accCode))
                    {
                        var account = await _context.Mst011s
                            .AsNoTracking()
                            .Where(a => a.AccCode == accCode)
                            .Select(a => new
                            {
                                a.AccCode,
                                a.AccAlias,
                                a.AccName
                            })
                            .FirstOrDefaultAsync();

                        if (account != null)
                        {
                            result.Add(new
                            {
                                Id = setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                AccCodeNavigation = account,
                                AccCode = account.AccCode,
                                AccName = account.AccName
                            });
                        }
                    }
                }

                var filteredSettings2 = setting1.Where(a => a.SetCode == 103).ToList();

                foreach (var setting in filteredSettings2)
                {
                    // Try to convert SetValue to int
                    if (int.TryParse(setting.SetValue?.ToString(), out int sgCode))
                    {
                        var group = await _context.Mst003s.AsNoTracking().Include(i => i.GrpCodeNavigation)
                            .AsNoTracking()
                            .Where(a => a.SgCode == sgCode)
                            .Select(a => new
                            {
                                a.SgCode,
                                a.SgName,
                                a.GrpCodeNavigation
                            })
                            .FirstOrDefaultAsync();

                        if (group != null)
                        {
                            result.Add(new
                            {
                                Id = setting.Id,
                                SetValue = setting.SetValue,   // This is an integer
                                SetCode = setting.SetCode,
                                SetDesc = setting.SetDesc,
                                SgCodeNavigation = group,

                            });
                        }
                    }
                }

                rtn.data = result;
                return Ok(rtn);


            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }

            return Ok(rtn);

        }


    }
}

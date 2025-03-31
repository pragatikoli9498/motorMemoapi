using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using MotorMemo.Models.MainDbEntities;

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly MainDbContext _context;

        private respayload rtn = new respayload();

        public BranchController(MainDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Mst00402>();

                var query = _context.Mst00402s.Include(i => i.Mst00403s).Include(i => i.BranchState);



                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.BranchName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FirmCode,
                        i.BranchCode,
                        i.BranchName,
                        i.BranchStateId,
                        i.BranchPlace,
                        i.BranchMobNo,
                        i.BranchState,
                        i.FirmCodeNavigation,
                        i.Mst005s,
                        i.Mst00403s



                    }).ToList();
                if (page.PageNumber == 1)
                {
                    rtn.PageDetails = PageDetail<Mst00402>.ToPagedList(data, page.PageNumber, page.PageSize);
                }
                   

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
                respayload.data = await _context.Mst00402s.Include(i => i.Mst00403s)
                    .Include(i => i.BranchState)
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult> branchbyFirm(long firmCode)
        {
            try
            {
                 
                rtn.status_cd = 1;

                var x = _context.Mst00409s.Where(s => s.FirmId == firmCode).OrderByDescending(s => s.GstNo).FirstOrDefault();
                 
                rtn.data = await _context.Mst00402s.Include(s => s.Mst00409s).Where(w => w.FirmCode == firmCode).Select(s => new {

                    s.BranchCode,
                    s.BranchName,
                    s.Mst00409s,
                    branchGstin = x.GstNo,
 
                })
                    .ToListAsync();
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }
        [HttpGet]
        public async Task<ActionResult> getBranch(long firmCode, string branchCode)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await _context.Mst00402s.Include(i => i.Mst00409s).Include(i => i.Mst00403s).Include(i => i.BranchState)
                    .Where(w => w.BranchCode == branchCode && w.FirmCode == firmCode).SingleOrDefaultAsync();


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

        [HttpPost]
        public async Task<ObjectResult> insert(Mst00402 branch)
        {
            try
            {
                branch.CreatedDt = DateTime.Now.ToString("yyyy/mm/dd");

                _context.Add(branch);
                await _context.SaveChangesAsync();
                rtn.status_cd = 1;
                rtn.data = branch;
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }
        [HttpPut]
        public async Task<ObjectResult> update(Mst00402 branch, long firmCode, string branchCode)
        {
            if (!(branchCode == branch.BranchCode && branch.FirmCode == firmCode))
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            try
            {
                branch.BranchState = null;
                Mst00402? ExistingParent = await _context.Mst00402s.Where((Mst00402 a) => a.FirmCode == branch.FirmCode && a.BranchCode == branchCode)
                    .SingleOrDefaultAsync();
                if (ExistingParent == null)
                {
                    branch.CreatedDt = DateTime.Now.ToString("yyyy/mm/dd"); 
                    branch.ModifiedUser = null;
                    _context.Mst00402s.Add(branch);
                }
                else
                {
                    branch.ModifiedDt = DateTime.Now.ToString("yyyy/mm/dd");
                    _context.Entry(ExistingParent).CurrentValues.SetValues(branch);

                    foreach (Mst00403 ExistingChild2 in ExistingParent.Mst00403s.ToList())
                    {
                        if (!branch.Mst00403s.Any((Mst00403 a) => a.LicId == ExistingChild2.LicId))
                        {
                            _context.Mst00403s.Remove(ExistingChild2);
                        }
                    }
                    foreach (Mst00403 childModel in branch.Mst00403s)
                    {
                        Mst00403? ExistingChild = ExistingParent.Mst00403s.Where((Mst00403 a) => a.LicId == childModel.LicId).SingleOrDefault();
                        if (ExistingChild != null)
                        {
                            _context.Entry(ExistingChild).CurrentValues.SetValues(childModel);
                            continue;
                        }
                        childModel.FirmCode = (int)firmCode;
                        childModel.BranchCode = branchCode;
                        ExistingParent.Mst00403s.Add(childModel);
                    }


                    foreach (Mst00409 ExistingChild2 in ExistingParent.Mst00409s.ToList())
                    {
                        if (!branch.Mst00409s.Any((Mst00409 a) => a.GstId == ExistingChild2.GstId ))
                        {
                            _context.Mst00409s.Remove(ExistingChild2);
                        }
                    }
                    foreach (Mst00409 childModel in branch.Mst00409s)
                    {
                        Mst00409? ExistingChild = ExistingParent.Mst00409s.Where((Mst00409 a) => a.GstId == childModel.GstId).SingleOrDefault();
                        if (ExistingChild != null)
                        {
                            _context.Entry(ExistingChild).CurrentValues.SetValues(childModel);
                            continue;
                        }
                        childModel.FirmId = (int)firmCode;
                        childModel.BranchId = branchCode;
                        ExistingParent.Mst00409s.Add(childModel);
                    }
                }

                await _context.SaveChangesAsync();
                rtn.data = branch;
            }
            catch (Exception ex)
            {
                if (!BranchExists(branch.FirmCode, branchCode))
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

        [HttpDelete]
        public async Task<IActionResult> delete(long firmcode, string branchCode)
        {
            try
            {
                Mst00402? branch = await _context.Mst00402s.Where(w => w.FirmCode == firmcode && w.BranchCode == branchCode).SingleOrDefaultAsync();

                if (branch == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst00402s.Remove(branch);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }
            return Ok(rtn);
        }


        private bool BranchExists(long id, string branchCode)
        {
            return _context.Mst00402s.Any((Mst00402 e) => e.FirmCode == id && e.BranchCode == branchCode);
        }
    }
}

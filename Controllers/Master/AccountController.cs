using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;
using System.Security.Principal;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext _mainDb;

        private respayload rtn = new respayload();

        public AccountController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            _mainDb = mainDb;
        }

        [HttpPost]
        public ActionResult getAccount(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Include(s => s.SgCodeNavigation).AsNoTracking()
                         .Include((Mst011 s) => s.Place).ThenInclude(s => s.Taluka).AsNoTracking()
                          .Include((Mst011 s) => s.Place).ThenInclude(s => s.Taluka).ThenInclude(s => s.District).AsNoTracking();


                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {

                        i.AccCode,
                        i.AccName,
                        i.SgCodeNavigation,
                        i.Place

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                 
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Include((Mst011 s) => s.Mst01109).AsNoTracking()
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                         .Include((Mst011 s) => s.Mst01101).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccAlias,
                        i.AccName,
                        i.SgCodeNavigation, 
                        i.Mst01101,
                        i.Place,
                        i.Mst01109


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

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

                respayload.data = await (from i in _context.Mst011s.AsNoTracking() 
                                         .Include((Mst011 s) => s.Mst01109).AsNoTracking()
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking()
                                         select new
                                         {
                                             i.AccCode,
                                             i.AccAlias,
                                             i.AccName,
                                             i.SgCodeNavigation,
                                             i.Place,
                                           
                                             i.Mst01109
                                         }).ToListAsync();

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


        [HttpPost]
        public async Task<ActionResult> getAcclist(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();
                var query = _context.Mst011s.AsNoTracking() 
                                         .Include((Mst011 s) => s.Mst01109).AsNoTracking()
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking();
                                
                var data = filter.Filter(query, page.keys, true);

                rtn.data = await data
                    .OrderBy(o => o.AccName).ThenBy(o => o.Place.CityName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccAlias,
                        i.AccName,
                        i.SgCodeNavigation,
                        i.Place, 
                        i.PanNo,
                        i.Mst01109
                    }).ToListAsync();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

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
        [HttpPost]
        public async Task<ActionResult> getAcclistbyMgCode(QueryStringParameters page, string mgCode, bool OrElse = false)
        {
            try
            {
                long[] _mgCode = Array.ConvertAll(mgCode.Split(','), long.Parse);

                var filter = new EntityFrameworkFilter<Mst011>();
                var query = _context.Mst011s.AsNoTracking().Where(w => _mgCode.Contains(w.SgCodeNavigation.GrpCodeNavigation.MgCode))
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking();

                var data = filter.Filter(query, page.keys, OrElse);

                rtn.data = await data
                    .OrderBy(o => o.AccName).ThenBy(o => o.Place.CityName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccAlias,
                        i.AccName,
                        i.SgCodeNavigation,
                        i.Place,

                    }).ToListAsync();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

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
        [HttpPost]
        public async Task<ActionResult> getAcclistbySgCode(QueryStringParameters page, long? sgCode, bool OrElse = false)
        {
            try
            {
                 var filter = new EntityFrameworkFilter<Mst011>();
                var query = _context.Mst011s.AsNoTracking().Where(w => (sgCode == null || w.SgCode == sgCode))
                                         .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                                        .Include((Mst011 s) => s.Place).AsNoTracking();

                var data = filter.Filter(query, page.keys, OrElse);

                rtn.data = await data
                    .OrderBy(o => o.AccName).ThenBy(o => o.Place.CityName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccAlias,
                        i.AccName,
                        i.SgCodeNavigation,
                        i.Place,

                    }).ToListAsync();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

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
        public async Task<ActionResult> supplier()
        {
            respayload respayload = rtn;
            respayload.data = await (from w in _context.Mst011s.Include(i => i.Mst01109)
                                     .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District)
                                     where w.SgCodeNavigation.GrpCodeNavigation.MgCode == 14
                                     select w into s
                                     select new
                                     {
                                         s.AccCode,
                                         s.AccName,
                                         s.Place,
                                         s.Mst01109
                                     }).ToListAsync();
            return Ok(rtn);
        }
        [HttpPost]
        public ActionResult getsupplier(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Include(i => i.Mst01109)
                                     .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District); 
                          
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccName,
                        i.Place,
                        i.Mst01109

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
 
        [HttpGet]
        public async Task<ActionResult> Accountedit(long id)
        {
            try
            {
               
                var accountData = await _context.Mst011s
                    .Where(s => s.AccCode == id)
                    .Include(s => s.Mst01101)
                    .Include(s => s.Mst01104)
                    .Include(s => s.Mst01109)
                    .Include(s=>s.Place)
                    .Include(s => s.SgCodeNavigation)
                        .ThenInclude(s => s.GrpCodeNavigation)
                    .Include(s => s.Mst01110s)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();

                if (accountData == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "Record Not Found";
                    return Ok(rtn);
                }

                var firmCodes = accountData.Mst01110s.Select(s => s.firmCode).ToList();

                var firmData = await _mainDb.Mst004s
                    .Where(w => firmCodes.Contains(w.FirmCode))
                    .Select(s => new
                    {
                        s.FirmCode,
                        s.FirmName
                    })
                    .ToListAsync();

                // Step 3: Combine Data Manually
                rtn.data = new
                { 
                        accountData.AccAlias,
                        accountData.AccCode,
                        accountData.AccName,
                        accountData.CreatedDt,
                        accountData.CreatedUser,
                        accountData.ModifiedDt,
                        accountData.ModifiedUser,
                        accountData.SgCode,
                        accountData.Mst01101,
                        accountData.TanNo,
                        accountData.PanNo,
                        accountData.CinNo,
                        accountData.CreditLimit,
                        accountData.Mst01104,
                        accountData.Mst01109,
                        accountData.Place,
                        accountData.SgCodeNavigation,
                        accountData.PlaceId,
                        accountData.IsDisabled,

                    mst01110s = firmData
                };

                return Ok(rtn);
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                 var mst11 = await _context.Mst011s.FindAsync(id);

                if (mst11 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst011s.Remove(mst11);
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
        [HttpPost]
        public ActionResult getBankCash(QueryStringParameters page)
        {
            long[] MgCode = { 3, 4, 25 };
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Where(w => MgCode.Contains(w.SgCodeNavigation.GrpCodeNavigation.MgCode)).AsNoTracking()
                    .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                    .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SgCode,
                        i.SgCodeNavigation.GrpCodeNavigation.MgCode,
                        i.AccName,
                        i.AccCode,
                        i.SgCodeNavigation,
                        i.Place,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public ActionResult getExpensesAcc(QueryStringParameters page)
        {
            long[] MgCode = { 19, 20, 21, 22 };
            try
            {

                var query = _context.Mst011s.Where(w => MgCode.Contains(w.SgCodeNavigation.GrpCodeNavigation.MgCode)).AsNoTracking()
                    .Include((Mst011 s) => s.SgCodeNavigation).AsNoTracking()
                    .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District);

                var filter = new EntityFrameworkFilter<Mst011>();
 
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SgCode,
                        i.SgCodeNavigation.GrpCodeNavigation.MgCode,
                        i.AccName,
                        i.AccCode,
                        i.SgCodeNavigation,
                        i.Place,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
        [HttpPost]
        public ActionResult getWithOutBankCash(QueryStringParameters page)
        {
            long[] MgCode = { 3, 4, 25 };
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Where(w => !MgCode.Contains(w.SgCodeNavigation.GrpCodeNavigation.MgCode)).AsNoTracking()

                .Include((Mst011 s) => s.Place).ThenInclude(s => s.Taluka).AsNoTracking()
                          .Include((Mst011 s) => s.Place).ThenInclude(s => s.Taluka).ThenInclude(s => s.District).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {

                        i.SgCode,
                        i.SgCodeNavigation.GrpCodeNavigation.MgCode,
                        i.AccName,
                        i.AccCode,
                        i.Place

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);
            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }


        [HttpPost]
        public ActionResult getsuppliers(QueryStringParameters page)
        {
            long[] MgCode = { 3, 4, 5, 14, 25 };
            try
            {
                var filter = new EntityFrameworkFilter<Mst011>();

                var query = _context.Mst011s.Include(i => i.Mst01109)
                                     .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District)
                .Where(w => MgCode.Contains(w.SgCodeNavigation.GrpCodeNavigation.MgCode)).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.AccName,
                        i.Place,
                        i.Mst01109

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst011>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }


        [HttpGet]
        public async Task<ActionResult> supplierDetails(long accCode)
        {
            respayload respayload = rtn;
            try
            {
                respayload.data = await (from w in _context.Mst011s.Include(i => i.Mst01109)
                                         .Include(i => i.Mst01101)
                                         .Include(i => i.Place).ThenInclude(i => i.Taluka).ThenInclude(i => i.District)
                                         where w.AccCode == accCode
                                         select w into s
                                         select new
                                         {
                                             s.AccCode,
                                             s.AccName,
                                             s.Place.CityName,
                                             s.Mst01101.EmailId,
                                             s.Mst01101.ContactMobileNo,
                                             s.Place.Taluka.District.StateCodeNavigation
                                         }).OrderByDescending(o => o.AccCode).SingleOrDefaultAsync();

            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(Mst011 mst011)
        {

            mst011.SgCodeNavigation = null;
            var t = _context.Database.BeginTransaction(); 
            rtn.status_cd = 1;
              
            try
            {  
                _context.Mst011s.Add(mst011);
                await _context.SaveChangesAsync();

                t.Commit();
                rtn.data = mst011;

            }
            catch (Exception ex)
            {
                t.Rollback();

                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }

            return Ok(rtn);

        }

        [HttpPut]
        public async Task<IActionResult> update(int id, Mst011 acc)
        {

            try
            {
                if (id != acc.AccCode)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = BadRequest();
                    return Ok(rtn);
                }
            
                var old = await _context.Mst011s.Where(s => s.AccCode == id)
                    .Include(s => s.Mst01101)
                    .Include(s => s.Mst01104)
                    .Include(s => s.Mst01109) 
                    .Include(s=>s.Mst01110s)
                    .SingleOrDefaultAsync();
                if (old == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Record Not Found"
                    };
                    return Ok(rtn);
                }
                _context.Entry(old).CurrentValues.SetValues(acc);
                if (old.Mst01101 != null)
                {
                    _context.Entry(old.Mst01101).CurrentValues.SetValues(acc.Mst01101);
                }
                else
                {
                    if (acc.Mst01101 != null)
                    {
                        acc.Mst01101.AccCode = id;
                        _context.Mst01101s.Add(acc.Mst01101);
                    }
                }
                if (old.Mst01104 != null)
                {
                    _context.Entry(old.Mst01104).CurrentValues.SetValues(acc.Mst01104);
                }
                else
                {
                    if (acc.Mst01104 != null)
                    {
                        acc.Mst01104.AccCode = id;
                        _context.Mst01104s.Add(acc.Mst01104);
                    }
                }

                    foreach (var existingChild in old.Mst01110s.ToList())
                    {
                        if (!acc.Mst01110s.Any(a => a.firmCode == existingChild.firmCode))
                            _context.Mst01110s.Remove(existingChild);
                    }

                foreach (var child in acc.Mst01110s)
                {
 
                    var existingChild = old.Mst01110s .Where(w => w.firmCode == child.firmCode) .SingleOrDefault();

                    if (existingChild != null)
                    {
                        _context.Entry(existingChild).CurrentValues.SetValues(child);

                    }
                    else
                    {
                        _context.Mst01110s.Add(child);
                    }

                }

                await _context.SaveChangesAsync();
                rtn.data = acc;
            }

            catch (Exception ex2)
            {
                Exception ex = ex2;
                if (!AccountExists(id))
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
        private bool AccountExists(long id)
        {
            return _context.Mst011s.Any((Mst011 e) => e.AccCode == id);
        }

        [HttpGet]
        public ActionResult getvender(string? no)
        {
            try
            {
                rtn.data = _context.Mst011s
                       .Where(i => (no == null || i.Mst01109.AccGstn == no))
                       .Select(i => new
                       { 
                           i.AccCode, 
                           i.AccName,
                           i.PanNo,
                           i.Mst01101.AccAddress,
                           i.Mst01101.EmailId,
                           i.Mst01101.ContactMobileNo,
                           place=_context.Mst006s.Where(w=>w.CityId == i.PlaceId).Select(s=>new { s.CityId, s.CityName, s.Taluka.District.StateCode,s.Taluka.District.StateCodeNavigation }).FirstOrDefault(),
                           i.Mst01109.AccGstn,
                      
                       }).FirstOrDefault();
            }
            catch (WebException ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }

            return Ok(rtn);
        }

    }
}

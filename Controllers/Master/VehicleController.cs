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
    public class VehicleController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public VehicleController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst108>();

                var query = _context.Mst108s
                                         .Include((Mst108 s) => s.Mst10803).AsNoTracking()
                                         .Include((Mst108 s) => s.Mst10804).AsNoTracking()
                                        .Include((Mst108 s) => s.Mst10801s).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VehicleNo)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.AccCode,
                        i.Chassisno,
                        i.Enginno,
                        i.VtypeId,
                        i.VehicleNo,
                        i.CapacityMts,
                        i.DriverName,
 
                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst108>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(Mst108 mst108)
        {
             
            var t = _context.Database.BeginTransaction();
            rtn.status_cd = 1;

            try
            {
                _context.Mst108s.Add(mst108);
                await _context.SaveChangesAsync();

                t.Commit();
                rtn.data = mst108;

            }
            catch (Exception ex)
            {
                t.Rollback();

                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);

            }

            return Ok(rtn);

        }

        [HttpDelete]
        public async Task<IActionResult> delete(string id)
        {
            try
            {
                var s = await _context.Mst108s.FindAsync(id);

                if (s == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst108s.Remove(s);
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

        [HttpGet]
        public async Task<ActionResult> edit(string id)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await (from i in _context.Mst108s.Where((Mst108 s) => s.VehicleNo == id).Include((Mst108 s) => s.Mst10801s).AsNoTracking()
                        .Include((Mst108 s) => s.Mst10805s).AsNoTracking()
                   
                        .Include( s=> s.Mst10805s).ThenInclude(ti=>ti.State).AsNoTracking() 
                        .Include((Mst108 s) => s.Mst10804)
                        .AsNoTracking()
                                         select new
                                         {
                                             i.VehicleNo,
                                             i.AccCode,
                                             i.Alias,
                                             i.CapacityMts,
                                             i.Chassisno,
                                             i.CreditLimitAmt,
                                             i.DriverAddress,
                                             i.DriverLicNo,
                                             i.DriverMobileNo,
                                             i.DriverName,
                                             i.Enginno,
                                             i.Vtype,
                                             i.AccCodeNavigation,
                                             i.CreatedDt,
                                             i.CreatedUser,
                                             i.ModifiedDt,
                                             i.ModifiedUser,
                                             i.Mst10801s,
                                             i.Mst10804,
                                             i.Mst10805s,
                                             i.PanNo,
                                             i.IsOwn,
                                             i.VtypeId
              
                                         }).SingleOrDefaultAsync();
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
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(string id, Mst108 mst108)
        {
            try
            {
                var mst108s = await _context.Mst108s.Where(w => w.VehicleNo == id).FirstOrDefaultAsync();

                foreach (var item in mst108.Mst10805s)
                {

                    item.State = null;
                }

                if (mst108s !=null)
                {
                    _context.Entry(mst108).State = EntityState.Modified;
                }
                else
                {
                    _context.Mst108s.Add(mst108);
                }
                 
                await _context.SaveChangesAsync();
                rtn.data = mst108
                        ;
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<ActionResult> vehiclelist(QueryStringParameters page)
        {
            try
            { 
                var filter = new EntityFrameworkFilter<Mst108>();

                var query = _context.Mst108s.AsNoTracking();
                 
                var data = filter.Filter(query, page.keys, true);
                 
                rtn.data = await data
                    .OrderBy(o => o.VehicleNo)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VehicleNo, 
                        i.AccCodeNavigation

                    }).ToListAsync();
                if (page.PageNumber == 1)

                    rtn.PageDetails = PageDetail<Mst108>.ToPagedList(data, page.PageNumber, page.PageSize);

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
        public ActionResult vehiclebyInfo(string? vehicleno)
        {

            try
            {
                rtn.data = _context.Mst108s
                       .Where(i => (vehicleno == null || i.VehicleNo == vehicleno))
                       .Select(i => new
                       {
                           i.VehicleNo,
                           i.AccCode, 
                           i.AccCodeNavigation
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

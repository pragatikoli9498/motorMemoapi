using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Net;
using MotorMemo.Models.MainDbEntities;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                                        .Include((Mst108 s) => s.Mst10801s).AsNoTracking()
                                        .Include((Mst108 s) => s.Mst10806s).AsNoTracking();

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VehicleNo)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        AccCode = i.Mst10806s.Select(a => a.AccCode),
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
                        .Include(s => s.Mst10806s).ThenInclude(m => m.AccCodeNavigation).AsNoTracking()
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
                                             i.VtypeId,
                                             i.Mst10806s,
                                             
              
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

        //[HttpPut]
        //public async Task<IActionResult> update(string id, Mst108 mst108)
        //{
        //    if (id != mst108.VehicleNo)
        //    {
        //        rtn.status_cd = 0;
        //        rtn.errors.exception = BadRequest();
        //        return Ok(rtn);
        //    }
        //    try
        //    {
        //        mst108.AccCodeNavigation = null;

        //        Mst108? ExistingParent = await _context.Mst108s.Where((Mst108 a) => a.VehicleNo == (string)id)
        //            .Include(i => i.Mst10801s).Include(i=>i.Mst10803).Include(i => i.Mst10805s).Include(i => i.Mst10804).Include(i => i.Mst10806s)
        //            .SingleOrDefaultAsync();

        //        if (ExistingParent == null) { 

        //        var mst108s = await _context.Mst108s.Where(w => w.VehicleNo == id).FirstOrDefaultAsync();

        //            _context.Mst108s.Add(mst108);
        //            if (mst108.Mst10805s != null)
        //            {
        //                foreach (var item in mst108.Mst10805s)
        //                {
        //                    item.State = null;
        //                }
        //            }
        //            if (mst108.Mst10806s != null)
        //            {
        //                foreach (var item in mst108.Mst10806s)
        //                {
        //                    item.AccCodeNavigation = null;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            _context.Entry(ExistingParent).CurrentValues.SetValues(mst108);
        //            if (ExistingParent.Mst10803 != null)
        //            {
        //                _context.Entry(ExistingParent.Mst10803).CurrentValues.SetValues(mst108.Mst10803);
        //            }
        //            else
        //            {
        //                if (mst108.Mst10803 != null)
        //                {
        //                    mst108.Mst10803.VehicleNo = id;
        //                    _context.Mst10803s.Add(mst108.Mst10803);
        //                }
        //            }

        //            _context.Entry(ExistingParent).CurrentValues.SetValues(mst108);
        //            if (ExistingParent.Mst10804 != null)
        //            {
        //                _context.Entry(ExistingParent.Mst10804).CurrentValues.SetValues(mst108.Mst10804);
        //            }
        //            else
        //            {
        //                if (mst108.Mst10804 != null)
        //                {
        //                    mst108.Mst10804.VehicleNo = id;
        //                    _context.Mst10804s.Add(mst108.Mst10804);
        //                }
        //            }

        //            _context.Entry(ExistingParent).CurrentValues.SetValues(mst108);
        //            if (ExistingParent.Mst10801s != null)
        //            {
        //                _context.Entry(ExistingParent.Mst10801s).CurrentValues.SetValues(mst108.Mst10801s);
        //            }
        //            else
        //            {
        //                if (mst108.Mst10801s != null)
        //                {
        //                    mst108.Mst10801s.VehicleNo = id;
        //                    _context.Mst10801s.Add(mst108.Mst10801s);
        //                }
        //            }

        //            foreach (var existingChild in ExistingParent.Mst10805s.ToList())
        //            {
        //                if (!mst108.Mst10805s.Any(a => a.VehicleNo == existingChild.VehicleNo))
        //                    _context.Mst10805s.Remove(existingChild);
        //            }

        //            foreach (var child in mst108.Mst10805s)
        //            {

        //                var existingChild = ExistingParent.Mst10805s.Where(w => w.VehicleNo == child.VehicleNo).SingleOrDefault();

        //                if (existingChild != null)
        //                {
        //                    _context.Entry(existingChild).CurrentValues.SetValues(child);

        //                }
        //                else
        //                {
        //                    _context.Mst10805s.Add(child);
        //                }
        //            }

        //            foreach (var existingChild in ExistingParent.Mst10806s.ToList())
        //            {
        //                if (!mst108.Mst10806s.Any(a => a.Vehicle_No == existingChild.Vehicle_No))
        //                    _context.Mst10806s.Remove(existingChild);
        //            }

        //            foreach (var child in mst108.Mst10806s)
        //            {

        //                var existingChild = ExistingParent.Mst10806s.Where(w => w.Vehicle_No == child.Vehicle_No).SingleOrDefault();

        //                if (existingChild != null)
        //                {
        //                    _context.Entry(existingChild).CurrentValues.SetValues(child);

        //                }
        //                else
        //                {
        //                    _context.Mst10806s.Add(child);
        //                }

        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        rtn.data = mst108;
        //    }
        //    catch (Exception ex)
        //    {

        //        rtn.status_cd = 0;
        //        rtn.errors.exception = ex;
        //        return Ok(rtn);
        //    }
        //    return Ok(rtn);
        //}


        [HttpPut]
        public async Task<IActionResult> update(string id, Mst108 mst108)
        {
            if (id != mst108.VehicleNo)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }

            try
            {
                // Avoid circular reference
                mst108.AccCodeNavigation = null;

                var existing = await _context.Mst108s
                    .Include(x => x.Mst10801s)
                    .Include(x => x.Mst10803)
                    .Include(x => x.Mst10804)
                    .Include(x => x.Mst10805s)
                    .Include(x => x.Mst10806s)
                    .SingleOrDefaultAsync(x => x.VehicleNo == id);

                if (existing == null)
                {
                    // New insert
                    if (mst108.Mst10805s != null)
                    {
                        foreach (var item in mst108.Mst10805s)
                        {
                            item.State = null;
                            item.VehicleNo = id; // ensure FK is set
                        }
                    }

                    if (mst108.Mst10806s != null)
                    {
                        foreach (var item in mst108.Mst10806s)
                        {
                            item.AccCodeNavigation = null;
                            item.VehicleNo = id;
                        }
                    }

                    if (mst108.Mst10803 != null)
                        mst108.Mst10803.VehicleNo = id;

                    if (mst108.Mst10804 != null)
                        mst108.Mst10804.VehicleNo = id;

                    if (mst108.Mst10801s != null)
                        mst108.Mst10801s.VehicleNo = id;

                    _context.Mst108s.Add(mst108);
                }
                else
                {
                    // Update parent
                    _context.Entry(existing).CurrentValues.SetValues(mst108);

                    // Mst10803
                    if (mst108.Mst10803 != null)
                    {
                        if (existing.Mst10803 != null)
                        {
                            _context.Entry(existing.Mst10803).CurrentValues.SetValues(mst108.Mst10803);
                        }
                        else
                        {
                            mst108.Mst10803.VehicleNo = id;
                            _context.Mst10803s.Add(mst108.Mst10803);
                        }
                    }

                    // Mst10804
                    if (mst108.Mst10804 != null)
                    {
                        if (existing.Mst10804 != null)
                        {
                            _context.Entry(existing.Mst10804).CurrentValues.SetValues(mst108.Mst10804);
                        }
                        else
                        {
                            mst108.Mst10804.VehicleNo = id;
                            _context.Mst10804s.Add(mst108.Mst10804);
                        }
                    }

                    // Mst10801
                    if (mst108.Mst10801s != null)
                    {
                        if (existing.Mst10801s != null)
                        {
                            _context.Entry(existing.Mst10801s).CurrentValues.SetValues(mst108.Mst10801s);
                        }
                        else
                        {
                            mst108.Mst10801s.VehicleNo = id;
                            _context.Mst10801s.Add(mst108.Mst10801s);
                        }
                    }

                    // Mst10805s
                    if (mst108.Mst10805s != null)
                    {
                        foreach (var item in existing.Mst10805s.ToList())
                        {
                            if (!mst108.Mst10805s.Any(a => a.VehicleNo == item.VehicleNo))
                                _context.Mst10805s.Remove(item);
                        }

                        foreach (var child in mst108.Mst10805s)
                        {
                            child.State = null;
                            var existingChild = existing.Mst10805s.SingleOrDefault(c => c.VehicleNo == child.VehicleNo);
                            if (existingChild != null)
                                _context.Entry(existingChild).CurrentValues.SetValues(child);
                            else
                                _context.Mst10805s.Add(child);
                        }
                    }

                    // Mst10806s
                    if (mst108.Mst10806s != null)
                    {
                        foreach (var item in existing.Mst10806s.ToList())
                        {
                            if (!mst108.Mst10806s.Any(a => a.VehicleNo == item.VehicleNo))
                                _context.Mst10806s.Remove(item);
                        }

                        foreach (var child in mst108.Mst10806s)
                        {
                            child.AccCodeNavigation = null;
                            var existingChild = existing.Mst10806s.SingleOrDefault(c => c.VehicleNo == child.VehicleNo);
                            if (existingChild != null)
                                _context.Entry(existingChild).CurrentValues.SetValues(child);
                            else
                                _context.Mst10806s.Add(child);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                rtn.data = mst108;
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

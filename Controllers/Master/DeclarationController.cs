using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using MotorMemo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Models.MainDbEntities;
using System.Data;

using Newtonsoft.Json;

using static MotorMemo.Models.Helper;
using MotorMemo.models;
using Microsoft.Data.Sqlite;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeclarationController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public DeclarationController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult insert(Mst103 value)
        {
            rtn.status_cd = 1;
           

            if (value.Mst10300 != null)
                value.Mst10300.CreatedDt = DateTime.Now.ToString("yyyy/mm/dd");


            try
            {

                _context.Mst103s.Add(value);
                _context.SaveChanges();
                rtn.data = value;
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.message = ex.InnerException?.Message ?? ex.Message;
                return Ok(rtn);
            }
            rtn.data = value;
            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                Mst103? s = await _context.Mst103s.FindAsync(id);
                if (s == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst103s.Remove(s);
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
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst103>();

                var query = _context.Mst103s.Include(i => i.AccCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.AccCode)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        
                        i.AccCode,
                        i.AccCodeNavigation,
                        i.NoOfVehicles,
                        i.DeclrNo,
                        i.Ishuf,
                        i.FromDt,
                        i.DeclrId

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst103>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult<Boolean>> cheackvehicle(string id)
        {

           var data = await _context.Mst108s.Where(a=>a.VehicleNo == id).FirstOrDefaultAsync();

            if(data !=null)
            {
                rtn.data = true;
            }
            else
            {
                rtn.data = false;
            }
            return Ok(rtn);
        }

            [HttpGet]
        public async Task<ActionResult<Mst103>> edit(long id)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await _context.Mst103s.Where(w => w.DeclrId == id)
                    .Include(i => i.AccCodeNavigation)
                    .Include(i => i.Mst10301s).AsTracking()
                       //.Include(i => i.Mst10300).AsTracking()
         
                    .SingleOrDefaultAsync();
                //    rtn.data= await _context.Mst103s.Where(w=>w.DeclrId == id).FirstOrDefaultAsync();
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
        public async Task<IActionResult> update(int id, Mst103 mst103)
        {
         
            
            try
            {

               

                var existingParent = await _context.Mst103s.Include(i => i.Mst10301s).Where(w => w.DeclrId == id).FirstOrDefaultAsync();

                _context.Entry(existingParent).CurrentValues.SetValues(mst103);

                foreach (var existingChild in existingParent.Mst10301s.ToList())
                {
                    if (!mst103.Mst10301s.Any(a => (a.Id == 0 ? -1 : a.Id) == existingChild.Id))
                        _context.Mst10301s.Remove(existingChild);
                }


                foreach (var childModel in mst103.Mst10301s.ToList())
                {
                   
                    var existingChildbb = _context.Mst10301s
                        .Where(a => a.detlid == childModel.detlid)
                        .SingleOrDefault();


                    

                        if (existingChildbb != null)
                        {

                            childModel.Id = mst103.DeclrId;
                            _context.Entry(existingChildbb).CurrentValues.SetValues(childModel);

                        }
                        else
                        {

                            childModel.Id = id;

                            _context.Mst10301s.Add(childModel);
                        }
                    


                }
 

                await _context.SaveChangesAsync();
                rtn.data = mst103;
              
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }
    }
}

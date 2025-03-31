using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
namespace MotorMemo.Controllers.Transaction
{

    [Route("[controller]/[action]")]
    [ApiController]
    public class SundryController : Controller
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public SundryController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Sundry>();

                var query = _context.Sundries.Include(i => i.AccCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.SundryName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SundryName,
                        i.ExpaccCode,
                        i.Operation,
                        i.AccCodeNavigation.AccName,
                        i.AccCodeNavigation,
                    


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Sundry>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }


        [HttpPost]
        public ActionResult Create(Sundry item)
        {
            try
            {
                _context.Sundries.Add(item);
                 _context.SaveChangesAsync();

                rtn.data = item;
            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
         
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            { 
                var sundry = await _context.Sundries.FindAsync(id);
                if (sundry == null)
                {
                    return NotFound(new
                    {
                        status_cd = 0,
                        message = "Sundry record not found."
                    });
                }
                 
                _context.Sundries.Remove(sundry);
                await _context.SaveChangesAsync();
                 
                return Ok(new
                {
                    status_cd = 1,
                    message = "Sundry record deleted successfully."
                });
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new
                {
                    status_cd = 0,
                    message = "An error occurred while deleting the record.",
                    error = ex.Message
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id,Sundry item)
        {

            item.AccCodeNavigation = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status_cd = 0,
                    message = "Invalid input data.",
                    errors = ModelState
                });
            }

            try
            {
                // Find the Sundry record by ID
               
                var sundry = await _context.Sundries.Where(s=>s.ExpaccCode == id).FirstOrDefaultAsync();
                if (sundry == null)
                {
                    return NotFound(new
                    {
                        status_cd = 0,
                        message = "Sundry record not found."
                    });
                }

                //_context.Entry(item).State = EntityState.Modified;

                _context.Entry(sundry).CurrentValues.SetValues(item);

                await _context.SaveChangesAsync();
                 
                return Ok(new
                {
                    status_cd = 1,
                    message = "Sundry record updated successfully.",
                    data = sundry
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, new
                {
                    status_cd = 0,
                    message = "An error occurred while updating the record.",
                    error = ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<ActionResult> getExpAcclist(QueryStringParameters page)
        {
            try
            {


                var filter = new EntityFrameworkFilter<Sundry>();

                var query = _context.Sundries.AsNoTracking()
                             .Include(s => s.AccCodeNavigation).AsNoTracking();


                var data = filter.Filter(query, page.keys, true);


                rtn.data = await data
                    .OrderBy(o => o.SundryName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SundryName,
                        i.SId,
                        i.AccCodeNavigation,



                    }).ToListAsync();
                if (page.PageNumber == 1)

                    rtn.PageDetails = PageDetail<Sundry>.ToPagedList(data, page.PageNumber, page.PageSize);

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

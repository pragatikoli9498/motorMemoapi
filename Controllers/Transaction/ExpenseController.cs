using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context; 
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public ExpenseController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {

                var filter = new EntityFrameworkFilter<Expense>();

                var query = _context.Expenses.Include(i => i.AccCodeNavigation);

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.ExpenseName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.ExpenseName, 
                        i.ExpaccCode,  
                        i.AccCodeNavigation,
           


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Expense>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public ActionResult Create(Expense item)
        {
            try
            {
                _context.Expenses.Add(item);
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
                var s = await _context.Expenses.FindAsync(id);

                if (s == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Expenses.Remove(s);
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

            //try
            //{
            //    var s = await _context.Expenses.FindAsync(id);
            //    if (s == null)
            //    {
            //        return NotFound(new
            //        {
            //            status_cd = 0,
            //            message = "Sundry record not found."
            //        });
            //    }

            //    _context.Expenses.Remove(s);
            //    await _context.SaveChangesAsync();

            //    return Ok(new
            //    {
            //        status_cd = 1,
            //        message = "Sundry record deleted successfully."
            //    });
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new
            //    {
            //        status_cd = 0,
            //        message = "An error occurred while deleting the record.",
            //        error = ex.Message
            //    });
            //}
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Expense item)
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

                var s = await _context.Expenses.Where(s => s.ExpaccCode == id).FirstOrDefaultAsync();
                if (s == null)
                {
                    return NotFound(new
                    {
                        status_cd = 0,
                        message = "Sundry record not found."
                    });
                } 
                 
                _context.Entry(s).CurrentValues.SetValues(item);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status_cd = 1,
                    message = "Sundry record updated successfully.",
                    data = s
                });
            }
            catch (Exception ex)
            { 

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


                var filter = new EntityFrameworkFilter<Expense>();

                var query = _context.Expenses.AsNoTracking()
                             .Include(s => s.AccCodeNavigation).AsNoTracking();


                var data = filter.Filter(query, page.keys, true);


                rtn.data = await data
                    .OrderBy(o => o.ExpenseName) 
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.ExpaccCode,
                        i.ExpenseName, 
                        i.AccCodeNavigation,
                        
                        
              
                    }).ToListAsync();
                if (page.PageNumber == 1)
                    
                rtn.PageDetails = PageDetail<Expense>.ToPagedList(data, page.PageNumber, page.PageSize);

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

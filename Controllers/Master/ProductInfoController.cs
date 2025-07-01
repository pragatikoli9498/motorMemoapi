using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductInfoController : ControllerBase
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public ProductInfoController(MotorMemoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst010>();

                var query = _context.Mst010s;
       
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.IName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                      i.IHsn,
                      i.IName,
                      i.IHsnDescription,
                      i.IId,
                      i.IUnit,
                      i.IUnitNavigation

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst010>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }


        [HttpPost]
        public ActionResult getProduct(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst010>();

                var query = _context.Mst010s;

                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.IName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {

                        i.IId,
                        i.IName,
                        i.IUnitNavigation,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst010>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }
 

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {

            try
            {
                Mst010? Mst010 = await _context.Mst010s.FindAsync(id);

                if (Mst010 == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Mst010s.Remove(Mst010);
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
        public async Task<ActionResult> ProductInfoedit(long id)
        {
            try
            {
                respayload respayload = rtn;
                respayload.data = await _context.Mst010s.Where((Mst010 s) => s.IId == id)
                .SingleOrDefaultAsync();
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

        [HttpPost]
        public async Task<ActionResult> insert(Mst010 product)
        {
            try
            {
                _context.Mst010s.Add(product);
                await _context.SaveChangesAsync();
                rtn.data = product;
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
        public async Task<IActionResult> update(long id, Mst010 Mst010)
        {
            _context.Entry(Mst010).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = Mst010;
            }
            catch (DbUpdateConcurrencyException ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        private bool ProductExists(long id)
        {
            return _context.Mst010s.Any((Mst010 e) => e.IId == id);
        }
    }
}

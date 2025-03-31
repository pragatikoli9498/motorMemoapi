using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Services; 
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Http;
using MotorMemo.Models.MotorMemoEntities; 

namespace MotorMemo.Controllers.RetailDesk
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FirmTypeController : Controller
    {
        private readonly MotorMemoDbContext _context;

        private respayload rtn = new respayload();

        public FirmTypeController(MotorMemoDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public ActionResult getList(QueryStringParameters page)
       {
            try
            {


                var filter = new EntityFrameworkFilter<Mst152>();

                var query = _context.Mst152s;



                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.FtName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.FtId,
                       
                        i.FtName,
                        


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst152>.ToPagedList(data, page.PageNumber, page.PageSize);

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
                respayload.data = await _context.Mst152s.ToListAsync();
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
        public async Task<ActionResult<Mst152>> firmtypeedit(long id)
        {
            await _context.Mst152s.FindAsync(id);
            if (rtn == null)
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Record Not Found";
            }
            return Ok(rtn);
        }

        [HttpPut]
        public async Task<IActionResult> update(long id, Mst152 firmtype)
        {
            if (id != firmtype.FtId)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = BadRequest();
                return Ok(rtn);
            }
            _context.Entry(firmtype).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                rtn.data = firmtype;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Mst152Exists(id))
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

        [HttpPost]
        public async Task<ActionResult> insert(Mst152 firmtype)
        {
            try
            {
                _context.Mst152s.Add(firmtype);
                await _context.SaveChangesAsync();
                rtn.data = firmtype;
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

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            Mst152? FirtmType = await _context.Mst152s.FindAsync(id);
            if (FirtmType == null)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = NotFound();
                return Ok(rtn);
            }
            _context.Mst152s.Remove(FirtmType);
            await _context.SaveChangesAsync();
            return Ok(rtn);
        }

        private bool Mst152Exists(long id)
        {
            return _context.Mst152s.Any((Mst152 e) => e.FtId == id);
        }
    }

}

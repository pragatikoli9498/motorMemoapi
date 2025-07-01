using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 
using System.Data; 
using MotorMemo.Models.Context;
using Microsoft.AspNetCore.Http; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using MotorMemo.Models.MainDbEntities;

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class BalanceForwardController : Controller
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext _mainDb;

        private respayload rtn = new respayload();

        public class acccBatch
        {
            public List<Mst01100>? batch { get; set; }

        }

        public BalanceForwardController(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            _mainDb = mainDb;
        }


        [HttpGet]
        public async Task<ActionResult> accOpeningbalanceForward(long? firmcode, string divid, string user)
        {
            rtn.status_cd = 1;

            {
                var oldyear = await _mainDb.Mst005s.Where(w => w.DivId == divid && !string.IsNullOrEmpty(w.FromDivId)
                  && (firmcode == null || w.FirmCode == firmcode))
                   .Select(c => new { c.FirmCode, c.FromDivId }).ToListAsync();

                if (oldyear.Count > 0)
                {
                    foreach (var firm in oldyear)
                    {
                        decimal?[] bs = { 0, 1, 2 };


                        var PrevAccOpbl = (await _context.Acc99901s.Include(i => i.Vch).Include(i => i.AccCodeNavigation.SgCodeNavigation.GrpCodeNavigation.MgCodeNavigation)
                        .Where(w => w.Vch.FirmId == firm.FirmCode && w.Vch.DivId == firm.FromDivId

                             && bs.Contains(w.AccCodeNavigation.SgCodeNavigation.GrpCodeNavigation.MgCodeNavigation.MgBs)).ToListAsync()).AsEnumerable()
                             .GroupBy(g => g.Vch.BranchId)
                             .Select(b => new {

                                 branchcode = b.Key,
                                 AccOpbl = b.GroupBy(g1 => g1.AccCode).Select(s =>
                                    new Mst01100
                                    {
                                        FirmId = firm.FirmCode,
                                  
                                        DivId = divid,
                                        AccCode = s.Key,
                                        Drbal = s.Sum(sm => sm.Cramt - sm.Dramt) < 0 ? s.Sum(sm => sm.Cramt - sm.Dramt) * -1 : 0,
                                        Crbal = s.Sum(sm => sm.Cramt - sm.Dramt) > 0 ? s.Sum(sm => sm.Cramt - sm.Dramt) : 0,
                              
                                        CreatedUser = user,
                                        CreatedDt = DateTime.Now.ToString("yyy/mm/dd")
                                    }).ToList()
                             }).ToList();

                        var dummarray = new List<acccBatch>();

                        foreach (var prev in PrevAccOpbl)
                        {
                            if (prev.AccOpbl.Count > 0)
                            {

                                int cnt = 0;
                                var dummylist = prev.AccOpbl;
                                int batch = dummylist.Count > 100 ? 100 : dummylist.Count;
                                while (cnt < prev.AccOpbl.Count)

                                {
                                    dummarray.Add(new acccBatch { batch = dummylist.Take(batch).ToList() });
                                    dummylist = dummylist.Skip(batch).ToList();
                                    cnt += batch;
                                    batch = dummylist.Count > 100 ? 100 : dummylist.Count;
                                }



                                try
                                {


                                    var CurrentMst011_00 = await _context.Mst01100s.Where(w => w.FirmId == firm.FirmCode
                                             && w.DivId == divid
                                               ).ToListAsync();


                                    foreach (var accOpbl in CurrentMst011_00)
                                    {
                                        if (!prev.AccOpbl.Any(a => a.AccCode == accOpbl.AccCode))
                                        {

                                            var ledger = _context.Acc999s.Where(w => w.ChallanId == accOpbl.VchId && w.VchType == 0).SingleOrDefault();
                                            if (ledger != null)
                                                _context.Acc999s.Remove(ledger);
                                            _context.Mst01100s.Remove(accOpbl);

                                        }

                                    }
                                    await _context.SaveChangesAsync();

                                }


                                catch (Exception ex2)
                                {
                                    Exception ex = ex2;
                                    rtn.status_cd = 0;
                                    rtn.errors.exception = ex;
                                }


                            }
                        }

                        foreach (var bt in dummarray)

                        {

                            try
                            {
                                foreach (var m in bt.batch)
                                {
                                    {
                                        var current = _context.Mst01100s.Where(w => w.AccCode == m.AccCode && w.FirmId == m.FirmId && w.DivId == divid).SingleOrDefault();
                                        if (current != null)
                                        {

                                            var ledger = _context.Acc999s.Where(w => w.ChallanId == current.VchId && w.VchType == 0).SingleOrDefault();
                                            if (ledger != null)
                                                _context.Acc999s.Remove(ledger);

                                            m.VchId = current.VchId;
                                            _context.Entry(current).CurrentValues.SetValues(m);


                                        }
                                        else
                                        {
                                            _context.Mst01100s.Add(m);

                                        }
                                        await _context.SaveChangesAsync();
                                       

                                    }

                                }


                                await _context.SaveChangesAsync();

                            }

                            catch (Exception ex2)
                            {
                                Exception ex = ex2;
                                rtn.status_cd = 0;
                                rtn.errors.exception = ex;
                            }



                        }



                    }
                }
            }
            return Ok(rtn);
        }

 

    }
 
}


﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.OpenApi.Any;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using System.Diagnostics.Contracts;
using System.Net;

namespace MotorMemo.Controllers.Transaction
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class Motormemo2Controller : ControllerBase
    {
        private readonly MotorMemoDbContext _context;
        private readonly MainDbContext _mainDb;

        private respayload rtn = new respayload();

        public Motormemo2Controller(MotorMemoDbContext context, MainDbContext mainDb)
        {
            _context = context;
            _mainDb = mainDb;
        }


        [HttpPost]
        public async Task<IActionResult> getList(QueryStringParameters page, int firm_id, string div_id)
        {

            rtn.status_cd = 1;

            try
            {
                var filter = new EntityFrameworkFilter<motormemo2>();

                var query = _context.Motormemo2s.Where(w => w.FirmId == firm_id && w.DivId == div_id).
                Include((motormemo2 s) => s.Motormemo2Childe).AsNoTracking()
               .Include((motormemo2 s) => s.Motormemo2AdvDetails).AsNoTracking();
                                         
                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.VchId)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.VchId,
                        i.VchNo,
                        i.Motormemo2Childe,
                        i.To_Dstn,
                        i.From_Dstn,
                        i.TotalWet,
                        i.VehicleNo,
                        i.VchDate,
                        i.Motormemo2AdvDetails,

                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<motormemo2>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpPost]
        public async Task<IActionResult> insert(motormemo2 motormemo2)
        {
            rtn.status_cd = 1;

            try
            {

                foreach (var childModel in motormemo2.Motormemo2AdvDetails)
                {
                    childModel.AccCodeNavigation = null;
                }

                if (motormemo2.VchNo == 0)
                {
                    var vch = _context.Motormemo2s
                         .Where(w => w.FirmId == motormemo2.FirmId && w.DivId == motormemo2.DivId)
                         .Max(c => (int?)c.VchNo);

                    motormemo2.VchNo = (vch ?? 0) + 1;
                }

                _context.Motormemo2s.Add(motormemo2);
                
                await _context.SaveChangesAsync(); 

                rtn.data = motormemo2;
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new dssFunctions().getException(ex);
            }

            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult> edit(int id)
        {
            try
            {
                rtn.data = await _context.Motormemo2s.Where(s => s.VchId == id).Include(i => i.Motormemo2Childe).ThenInclude(d => d.Bilty).ThenInclude(b => b.BiltyDetails).Include(b => b.Motormemo2AdvDetails).Include(i => i.Motormemo2Audit)
                    .Select(i => new
                    {
                        i.VchId,
                        i.VchNo,
                        i.From_Dstn,
                        i.To_Dstn,
                        i.VchDate,
                        i.VehicleNo,
                        i.TotalWet,
                        i.FreightperWet,
                        i.FreightTotal,
                        i.TotalAdv,
                        i.RemAmt,
                        i.Motormemo2Audit,
                        i.VehicleAccNavigation,
                        Motormemo2Childe = i.Motormemo2Childe.Select(child => new
                        {
                            child.DetlId,
                            child.VchId,
                            child.BiltyId,
                            child.Weight,
                            child.EwayNo,
                            BiltyNo = child.Bilty != null ? (int?)child.Bilty.BiltyNo : null,
                            VchDate = child.Bilty != null ? child.Bilty.vchDate.ToString() : null,
                            SenderName = child.Bilty != null ? child.Bilty.BiltyDetails.SenderName : null,
                            ReceiverName = child.Bilty != null ? child.Bilty.BiltyDetails.ReceiverName : null,
                            To_Dstn = child.Bilty != null ? child.Bilty.To_Dstn : null
                        }),
                        Motormemo2AdvDetails = i.Motormemo2AdvDetails.Select(s => new
                        {
                            s.DetlId,
                            s.VchId,
                            s.AccCode,
                            s.Amount,
                            s.Narration,
                            s.AccCodeNavigation
                        }).ToList()
                    })
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

        [HttpPut]
        public async Task<IActionResult> update(int id, motormemo2 data)
        {
            try
            {

                var s = await _context.Motormemo2s.Include(s => s.Motormemo2Childe).Include(s => s.Motormemo2Audit).Include(s => s.Motormemo2AdvDetails)
                    .Where(w => w.VchId == id).FirstOrDefaultAsync();

                if (s != null)
                {
                    _context.Entry(s).CurrentValues.SetValues(data);

                    foreach (var existingChild in s.Motormemo2Childe.ToList())
                    {
                        existingChild.Bilty = null;

                        if (!data.Motormemo2Childe.Any(a => a.DetlId == existingChild.DetlId))
                            _context.Motormemo2Childes.Remove(existingChild);
                    }

                    foreach (var childModel in data.Motormemo2Childe.ToList())
                    {
                        var existingChild = s.Motormemo2Childe
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        {
                            childModel.VchId = data.VchId;

                            s.Motormemo2Childe.Add(childModel);
                        }

                    }

                    foreach (var existingChild in s.Motormemo2AdvDetails.ToList())
                    {
                        existingChild.AccCodeNavigation = null;

                        if (!data.Motormemo2AdvDetails.Any(a => a.DetlId == existingChild.DetlId))
                            _context.Motormemo2AdvDetails.Remove(existingChild);
                    }

                    foreach (var childModel in data.Motormemo2AdvDetails.ToList())
                    {

                        var existingChild = s.Motormemo2AdvDetails
                            .Where(a => a.DetlId == childModel.DetlId)
                            .SingleOrDefault();

                        if (existingChild != null)
                        {
                            _context.Entry(existingChild).CurrentValues.SetValues(childModel);

                        }
                        else
                        {
                            childModel.VchId = data.VchId;

                            s.Motormemo2AdvDetails.Add(childModel);
                        }


                    }

                }
                else
                {
                    _context.Motormemo2s.Add(data);
                }

                await _context.SaveChangesAsync();
                rtn.data = data;
            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return Ok(rtn);
            }
            return Ok(rtn);
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var value = await _context.Motormemo2s.FindAsync(id);

                if (value == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Motormemo2s.Remove(value);
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

        [HttpDelete]
        public async Task<IActionResult> deletechild(int id)
        {
            try
            {
                var value = await _context.Motormemo2Childes.FindAsync(id);

                if (value == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = NotFound();
                    return Ok(rtn);
                }
                _context.Motormemo2Childes.Remove(value);
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
        public ActionResult PendingLorryRec(int firm_id, string div_id, string veh_no)
        {
            try
            {
                rtn.data = _context.Motormemo2s.Where(w => w.FirmId == firm_id && w.DivId == div_id && w.VehicleNo == veh_no && w.RemAmt > 0 && w.ConfDate == null).
                 Include((motormemo2 s) => s.Motormemo2Childe).ThenInclude(i => i.Bilty).ThenInclude(b => b.BiltyDetails).AsNoTracking()
                                          .Include((motormemo2 s) => s.Motormemo2Audit).AsNoTracking()
                                          .Include((motormemo2 s) => s.Motormemo2AdvDetails).AsNoTracking()

                         .Select(i => new
                         {
                             i.VchId,
                             i.VchNo,
                             i.From_Dstn,
                             i.To_Dstn,
                             i.VchDate,
                             i.VehicleNo,
                             i.TotalWet,
                             i.FreightperWet,
                             i.FreightTotal,
                             i.TotalAdv,
                             i.RemAmt,
                             i.Motormemo2Audit,
                             i.VehicleAccNavigation,
                             Motormemo2Childe = i.Motormemo2Childe.Select(child => new
                             {
                                 child.DetlId,
                                 child.VchId,
                                 child.BiltyId,
                                 child.Weight,
                                 child.EwayNo,
                                 BiltyNo = child.Bilty != null ? (int?)child.Bilty.BiltyNo : null,
                                 VchDate = child.Bilty != null ? child.Bilty.vchDate.ToString() : null,
                                 SenderName = child.Bilty != null ? child.Bilty.BiltyDetails.SenderName : null,
                                 ReceiverName = child.Bilty != null ? child.Bilty.BiltyDetails.ReceiverName : null,
                                 To_Dstn = child.Bilty != null ? child.Bilty.To_Dstn : null
                             }),
                             Motormemo2AdvDetails = i.Motormemo2AdvDetails.Select(s => new
                             {
                                 s.DetlId,
                                 s.VchId,
                                 s.AccCode,
                                 s.Amount,
                                 s.Narration,
                                 s.AccCodeNavigation
                             }).ToList()

                         });

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<ActionResult> PendingAmountedit(int id)
        {
            try
            {
                rtn.data = await _context.Motormemo2s.Where(s => s.VchId == id)
                    .Select(i => new
                    {
                        i.VchId,
                        i.VchNo,
                        i.From_Dstn,
                        i.To_Dstn,
                        i.VchDate,
                        i.VehicleNo,
                        i.TotalWet,
                        i.FreightperWet,
                        i.FreightTotal,
                        i.TotalAdv,
                        i.RemAmt,
                        i.Motormemo2Audit
                    })
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

        [HttpPut]
        public async Task<IActionResult> updatepayment(int id, motormemo2 data)
        {
            try
            {
                var s = await _context.Motormemo2s
                    .Include(s => s.Motormemo2AdvDetails)
                    .Where(w => w.VchId == id)
                    .FirstOrDefaultAsync();

                foreach (var item in data.Motormemo2AdvDetails)
                {
                    item.AccCodeNavigation = null;
                }

                if (s != null)
                {
                    _context.Entry(s).CurrentValues.SetValues(data);

                    foreach (var childModel in data.Motormemo2AdvDetails)
                    {
                        childModel.VchId = data.VchId;
                        _context.Motormemo2AdvDetails.Add(childModel);
                    }
                }
                else
                {
                    _context.Motormemo2s.Add(data);
                }

                await _context.SaveChangesAsync();
                rtn.data = data;
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

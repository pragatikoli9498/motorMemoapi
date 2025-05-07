using Microsoft.AspNetCore.Mvc;
using MotorMemo.Models.Context;
using MotorMemo.Services;
using System;
using System.Collections.Generic;
using System.Data;

using System.Threading.Tasks;
using System.Security.Claims;
using MotorMemo.Models;
using Microsoft.EntityFrameworkCore;

using static MotorMemo.Models.Helper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.AspNetCore.Authorization;

using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using MotorMemo.Models.MotorMemoEntities;
using Microsoft.Extensions.Logging;
using MotorMemo.Models.Procedures;
using SkiaSharp;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TrialBalanceController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly TrialBalanceProc _proc;
        public TrialBalanceController(MotorMemoDbContext context, TrialBalanceProc proc)
        {
            db = context;
            _proc = proc;
        }

        [HttpGet]
        public async Task<IActionResult> getTrialBalItems(int firm_id, string div_id, DateTime? sdt, DateTime edt)
        {

           
            try
            {
                var data = await _proc.Data(firm_id, div_id, sdt, edt) as IEnumerable<dynamic>;
                if (data == null)
                {
                    throw new Exception("Data returned is not a valid enumerable object.");
                }

                var trialDash = data
    .GroupBy(g => new
    {
        sg_code = (int?)g.sg_code,
        sg_sr_no = (int?)g.sg_sr_no,
        sg_name = (string)g.sg_name
    })
    .Select(s => new
    {
        srNo = s.Key.sg_sr_no ?? 0,
        sg_code = s.Key.sg_code ?? 0,
        Group = s.Key.sg_name,
        Debit = s.Sum(a =>
        {
            var bal = a.balance;
            if (bal == null || Convert.ToDecimal(bal) >= 0) return 0m;
            return -Convert.ToDecimal(bal);
        }),
        Credit = s.Sum(a =>
        {
            var bal = a.balance;
            if (bal == null || Convert.ToDecimal(bal) <= 0) return 0m;
            return Convert.ToDecimal(bal);
        })
    });

                rtn.data = trialDash.ToList();
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }

            return Ok(rtn);
        }
    }
}

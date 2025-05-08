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
using MotorMemo.Models.MainDbEntities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MotorMemo.Controllers.Master
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProfitLossController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext _MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly ProfitLossProc _proc;
        public ProfitLossController(MotorMemoDbContext context, ProfitLossProc proc)
        {
            db = context;
            _proc = proc;

        }

        [HttpGet]
        public async Task<IActionResult> getProfitLossItems(int firm_id, string div_id, DateTime edt)
        {
            try
            {

                var data = await _proc.Data(firm_id, div_id, edt)as IEnumerable<dynamic>;

                if (data == null)
                {
                    throw new Exception("Data returned is not a valid enumerable object.");
                }

                var prfLoos = data
             .GroupBy(g => new
             {
                 sg_code = (int?)g.sg_code,
                 sr_no = (int?)g.sr_no,
                 sg_name = (string)g.sg_name,
                 mg_bs = (int?)g.mg_bs,
                 isclosing = (decimal?)g.isclosing,
                 idx = (int?)g.idx,
                 show = (int?)g.show,

             })
             .Select(s => new
             {
                s.Key.sr_no,
                s.Key.sg_code,
                s.Key.sg_name,
                s.Key.mg_bs,
                s.Key.isclosing,
                s.Key.show,
               s.Key.idx,
                 accounts = s.Select(c => new
                 {
                     acc_code = (int?)c.acc_code, // Nullable type for acc_code
                     acc_name = c.Acc_name?.ToString(),
                     balance = (decimal?)c.balance ?? 0 // Default to 0 if NULL
                 }),
                 balance = s.Sum(a => (decimal?)a.balance ?? 0)
             })
             .Select(c => new
             {
                 c.sr_no,
                 c.sg_code,
                 c.sg_name,
                 c.balance,
                 c.isclosing,
                 c.show,
                 c.idx,
                 debit = c.mg_bs == 5 ? c.balance : 0,
                 credit = c.mg_bs == 6 ? c.balance : 0,

                 account = c.accounts.Select(x => new
                 {
                     x.acc_code,
                     x.acc_name,
                     x.balance,
                     cramt = x.balance * (c.mg_bs == 5 ? -1 : 1) < 0 ? x.balance * (c.mg_bs == 5 ? -1 : 1) * -1 : 0,
                     dramt = x.balance * (c.mg_bs == 5 ? -1 : 1) > 0 ? x.balance * (c.mg_bs == 5 ? -1 : 1) : 0
                 }).ToList()

             })
                 .ToList();





                rtn.data = prfLoos.ToList();

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

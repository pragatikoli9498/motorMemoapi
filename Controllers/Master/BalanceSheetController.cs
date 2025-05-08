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
    public class BalanceSheetController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext _MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly BalanceSheetProc _proc;
        public BalanceSheetController(MotorMemoDbContext context, BalanceSheetProc proc)
        {
            db = context;
            _proc = proc;
          
        }
        [HttpGet]
        public async Task<IActionResult> getBalanceSheet(int firm_id, string div_id, DateTime edt, string username)
        {
          
            try
            {
                var report = await _proc.Data(firm_id, div_id, edt) as IEnumerable<dynamic>;

                if (report == null)
                {
                    throw new Exception("Data returned is not a valid enumerable object.");
                }
                var balSheetDash = report
                            .GroupBy(g => new
                            {

                                sg_code = (int?)g.sg_code,
                                sg_sr_no = (int?)g.sg_sr_no ?? 0,
                                sg_name = (string)g.sg_name,
                                mg_bs = (int?)g.mg_bs ?? 0,
                                mg_head = (string)g.mg_head,
                                mg_name = (string)g.mg_name,
                                grp_name = (string)g.grp_name,
                                isclosing = (decimal?)g.isclosing ?? 0,
                                isopdifference = (decimal?)g.isopdifference ?? 0,
                                show = (int?)g.show ?? 0
                            })
                            .Select(s => new
                            {
                                s.Key.sg_sr_no,
                                s.Key.sg_code,
                                s.Key.sg_name,
                                s.Key.mg_bs,
                                s.Key.mg_head,
                                s.Key.mg_name,
                                s.Key.grp_name,
                                s.Key.isclosing,
                                s.Key.isopdifference,
                                s.Key.show,
                                accounts = s.Select(c => new
                                {
                                    acc_code = (int?)c.acc_code, // Nullable type for acc_code
                                    acc_name = (string)c.acc_name,
                                    balance = (decimal?)c.balance ?? 0 // Default to 0 if NULL
                                }),
                                balance = s.Sum(a => (decimal?)a.balance ?? 0) // Handle NULL values
                            })
                            .Select(c => new
                            {
                                c.sg_sr_no,
                                c.sg_code,
                                c.sg_name,
                                c.isclosing,
                                c.isopdifference,
                                c.show,
                                mg_head = c.mg_head == null ? (c.mg_name == null ? c.grp_name : c.mg_name) : c.mg_head,
                                debit = c.mg_bs == 1 ? c.balance : 0,
                                credit = c.mg_bs == 2 ? c.balance : 0,
                                account = c.accounts.Select(x => new
                                {
                                    x.acc_code,
                                    x.acc_name,
                                    dramt = c.mg_bs == 1 ? x.balance : 0,
                                    cramt = c.mg_bs == 2 ? x.balance : 0
                                })
                            })
                            .ToList();
                            rtn.data = balSheetDash.ToList();
                          }
                           catch (Exception e)
                           {
                          rtn.status_cd = 0;
                          rtn.errors.message = e.Message ?? e.InnerException?.Message;
                          }
                          return Ok(rtn);
                        }
    }
}

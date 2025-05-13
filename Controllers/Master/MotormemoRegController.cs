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
    public class MotormemoRegController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly MotormemoRegProc _proc;
        public MotormemoRegController(MotorMemoDbContext context, MotormemoRegProc proc)
        {
            db = context;
            _proc = proc;

        }
        [HttpGet]
        public async Task<IActionResult> getmotormemoitem(QueryStringParameters page, int firm_id, string div_id, DateTime? sdt, DateTime edt)
        {

            try
            {

                var data = await _proc.Data(firm_id, div_id, sdt,edt) as IEnumerable<dynamic>;
                //DataTable data = await _proc.Data(firm_id, branch_id, div_id, sg_code, acc_code, sdt, edt, Inventory);
                //decimal runningTotal = 0;
                if (data != null)
                {
                    rtn.data = data.Select(g => new
                    {
                        dt = (string)g.dt,
                        LRNO=(decimal)g.memo_No,
                        from = (string)g.from_dstn,
                        to = (string)g.to_dstn,
                        vehicle_no = (string)g.vehicle_no,
                        SundryName = (string)g.sundry_name,
                        AdvanceAmount = (decimal)g.AdvAmount,
                        freight = (decimal)g.TotalFreight,
                        charges = (decimal)g.charges,
                        leftAmount=(decimal)g.LeftAmount
                    });
                }
                else
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = "No data returned or invalid format.";
                }

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

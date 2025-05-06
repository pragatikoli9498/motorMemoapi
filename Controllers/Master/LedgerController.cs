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
    public class LedgerController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly LedgerProc _proc;
        public LedgerController(MotorMemoDbContext context, LedgerProc proc)
        {
            db = context;
            _proc = proc;

        }

        [HttpGet]
        public async Task<IActionResult> getGroupByAcc(long? SgCode)
        {
            try
            {
                rtn.data = await db.Mst011s
                    .Where(w => SgCode == null || w.SgCode == SgCode).ToListAsync();

            }
            catch (Exception ex)
            {

                rtn.status_cd = 0;
                rtn.errors.exception = ex;

            }
            return Ok(rtn);
        }



        [HttpGet]
        public async Task<IActionResult> getLedgerItems(QueryStringParameters page, int firm_id,string div_id, int? sg_code, int? acc_code, DateTime sdt, DateTime edt, Boolean Inventory)
        {

            try
            {

                var data = await _proc.Data(firm_id, div_id, sg_code, acc_code, sdt, edt, Inventory) as IEnumerable<dynamic>;
                //DataTable data = await _proc.Data(firm_id, branch_id, div_id, sg_code, acc_code, sdt, edt, Inventory);
                //decimal runningTotal = 0;
                if (data != null)
                {
                    rtn.data = data.Select(g => new
                    {
                        vch_type = (int)g.vch_type,
                        vch_type_name = (string)g.vch_type_name,
                        challan_no = (string)g.challan_no,
                        vch_date = (string)g.vch_date,
                        acc_name = (string)g.acc_name,
                        cramt = (decimal)g.cramt,
                        dramt = (decimal)g.dramt,
                        refName = (string)g.refName,
                        balance = (decimal)g.cramt - (decimal)g.dramt
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

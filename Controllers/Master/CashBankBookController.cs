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
    public class CashBankBookController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly CashBankBookProc _proc;
        public CashBankBookController(MotorMemoDbContext context, CashBankBookProc proc)
        {
            db = context;
            _proc = proc;
        }

        [HttpGet]
        public async Task<IActionResult> getcashbankbookItems(QueryStringParameters page, int firm_id, string div_id, DateTime sdt, DateTime edt, int? acc_code)
        {

            try
            {
              

                    var opBalRows = await _proc.getCashBankOpening(firm_id, div_id, sdt, edt, acc_code) as IEnumerable<dynamic>;
                int opBal = 0;
                if (opBalRows != null)
                {
                    var firstRow = opBalRows.FirstOrDefault();
                    if (firstRow != null && firstRow.balance != null)
                    {
                        opBal = Convert.ToInt16(firstRow.balance);
                    }
                }

                //var report = await _proc.getCashBankBook(firm_id,div_id, sdt, edt, acc_code);
                var report = await _proc.getCashBankBook(firm_id, div_id, sdt, edt, acc_code) as IEnumerable<dynamic>;
                var clbaln = report.AsEnumerable().Sum(row => row.Field<int?>("balance") ?? 0) + opBal;
                int clBal = Convert.ToInt16(clbaln);

                var rpt = new List<CashBankBook>();

                var opening = new CashBankBook();

                opening.acc_code = acc_code;
                opening.acc_name = "Opening";
                opening.balance = opBal;
                opening.challan_no = "";
                opening.cramt = opBal > 0 ? opBal : 0;
                opening.dramt = opBal < 0 ? -opBal : 0;
                opening.refName = "Balance B/F";
                opening.ref_code = null;
                opening.vch_date = sdt;
                opening.vch_id = null;
                opening.vch_no = null;
                opening.vch_type = 0;
                opening.vch_type_name = null;
                opening.challan_id = null;

                var closing = new CashBankBook();

                closing.acc_code = acc_code;
                closing.acc_name = "Closing";
                closing.balance = clBal;
                closing.challan_no = "";
                closing.cramt = clBal < 0 ? -clBal : 0;
                closing.dramt = clBal > 0 ? clBal : 0;
                closing.Naration = null;
                closing.refName = "Balance C/F";
                closing.ref_code = null;
                closing.vch_date = edt;
                closing.vch_id = null;
                closing.vch_no = null;
                closing.vch_type = 0;
                closing.vch_type_name = null;
                closing.challan_id = null;

                var reportList = new List<CashBankBook>();

               
                foreach (var row in report)
                {
                    int balance = row.balance != null ? Convert.ToInt32(row.balance) : 0;

                    var cashBankBook = new CashBankBook
                    {

                        acc_code = row.acc_code != null ? (int?)Convert.ToInt32(row.acc_code) : null,
                        acc_name = row.acc_name,
                        balance = balance,
                        cramt = balance > 0 ? balance : 0,
                        dramt = balance < 0 ? -balance : 0,
                        vch_type_name = row.vch_type_name,
                        vch_date = row.vch_date != null ? Convert.ToDateTime(row.vch_date) : DateTime.MinValue,
                        challan_no = row.challan_no,
                        refName = row.refName

                    };

                    reportList.Add(cashBankBook);
                }



                rpt.Add(opening);

                rpt.AddRange(reportList);

                rpt.Add(closing);

                rtn.data = rpt;

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

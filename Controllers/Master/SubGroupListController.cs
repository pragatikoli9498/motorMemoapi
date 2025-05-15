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
    public class SubGroupListController : ControllerBase
    {
        private readonly MotorMemoDbContext db;
        private readonly MainDbContext MainDb;
        private respayload rtn = new respayload();
        private readonly IOptions<UserSettings> Settings;
        public readonly SubGroupListProc _proc;
        public SubGroupListController(MotorMemoDbContext context, SubGroupListProc proc)
        {
            db = context;
            _proc = proc;
        }

        [HttpGet]
        public async Task<IActionResult> getSubGroupListItems(int firm_id, string div_id, int? sg_code, DateTime? sdt, DateTime? edt)
        {

            try
            {

                var data = await _proc.Data(firm_id, div_id, sg_code, sdt, edt) as IEnumerable<dynamic>;
                //if (data == null)
                //{
                //    throw new Exception("Data returned is not a valid enumerable object.");
                //}

                var grplist = data
                   .GroupBy(g => new
                   {
                       sg_code = g.sg_code != null ? (int)(long)g.sg_code : 0,
                       sg_name = g.sg_name?.ToString() ?? "",
                       acc_code = g.acc_code != null ? (int)(long)g.acc_code : 0,
                       acc_name = g.Acc_name?.ToString() ?? "",
                       city_name = g.City_name?.ToString() ?? "",
                       mobile_no = g.mobile_no?.ToString() ?? "",
                       email_id = g.email_id?.ToString() ?? "",
                       op_credit = g.op_credit != null ? (double)g.op_credit : 0.00,
                       op_debit = g.op_debit != null ? (double)g.op_debit : 0.00,
                       curr_credit = g.curr_credit != null ? (double)g.curr_credit : 0.00,
                       curr_debit = g.curr_debit != null ? (double)g.curr_debit : 0.00,
                       cl_credit = g.cl_credit != null ? (double)g.cl_credit : 0.00,
                       cl_debit = g.cl_debit != null ? (double)g.cl_debit : 0.00,
                   })
                    .Select(c => new
                    {
                        c.Key.sg_code,
                        c.Key.sg_name,
                        c.Key.acc_code,
                        c.Key.acc_name,
                        c.Key.city_name,
                        c.Key.mobile_no,
                        c.Key.email_id,
                        c.Key.op_credit,
                        c.Key.op_debit,
                        c.Key.curr_credit,
                        c.Key.curr_debit,
                        c.Key.cl_credit,
                        c.Key.cl_debit,
                    })
                    .GroupBy(g => g.acc_name)
                    .Select(s =>
                    {
                        var first = s.FirstOrDefault();
                        double openingbal = s.Sum(item => item.op_credit != 0 ? item.op_credit : item.op_debit);
                        double currentbal = s.Sum(item => item.curr_credit != 0 ? item.curr_credit : item.curr_debit);
                        double closingbal = s.Sum(item => item.cl_credit != 0 ? item.cl_credit : item.cl_debit);
                        return new GroupListReport
                        {
                            acc_name = s.Key,
                            acc_code = first.acc_code,
                            city_name = first.city_name,
                            mobile_no = first.mobile_no,
                            opening = openingbal,
                            current = currentbal,
                            closing = closingbal,
                        };
                    })
                    .ToList();
                rtn.data = grplist.ToList();
            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }

            return Ok(rtn);
        }

        [HttpPost]
        public ActionResult getItems(QueryStringParameters page)
        {
            try
            {
                var filter = new EntityFrameworkFilter<Mst003>();

                var query = db.Mst003s.Include(i => i.GrpCodeNavigation);



                var data = filter.Filter(query, page.keys);

                rtn.data = data.OrderBy(o => o.SgName)
                     .Skip((page.PageNumber - 1) * page.PageSize)
                    .Take(page.PageSize).Select(i => new
                    {
                        i.SgCode,
                        i.GrpCode,
                        i.SgName,
                        i.SrNo,
                        i.GrpCodeNavigation


                    }).ToList();
                if (page.PageNumber == 1)
                    rtn.PageDetails = PageDetail<Mst003>.ToPagedList(data, page.PageNumber, page.PageSize);

            }
            catch (Exception ex2)
            {

                rtn.status_cd = 0;
                rtn.errors.message = ex2.Message;

            }
            return Ok(rtn);
        }

        [HttpGet]
        public async Task<IActionResult> SubGroupListItems(int firm_id, string div_id, int? sg_code)
        {
            try
            {
                object data = await _proc.Datas(firm_id, div_id, sg_code);
                var rows = data as IEnumerable<dynamic>;
                if (rows == null)
                {
                    throw new Exception("Expected IEnumerable<dynamic> but got a different type.");
                }

                var grplist = rows
                    .GroupBy(g => new
                    {
                        sg_code = (int)g.sg_code,
                        sg_name = (string)g.sg_name,
                        acc_code = (int)g.acc_code,
                        acc_name = (string)g.acc_name,
                        city_name = (string)g.city_name,
                        mobile_no = (string)g.mobile_no,
                        email_id = (string)g.email_id,
                        op_credit = (double)g.op_credit,
                        op_debit = (double)g.op_debit,
                        curr_credit = (double)g.curr_credit,
                        curr_debit = (double)g.curr_debit,
                        cl_credit = (double)g.cl_credit,
                        cl_debit = (double)g.cl_debit
                    })
                    .Select(c => new
                    {
                        c.Key.sg_code,
                        c.Key.sg_name,
                        c.Key.acc_code,
                        c.Key.acc_name,
                        c.Key.city_name,
                        c.Key.mobile_no,
                        c.Key.email_id,
                        c.Key.op_credit,
                        c.Key.op_debit,
                        c.Key.curr_credit,
                        c.Key.curr_debit,
                        c.Key.cl_credit,
                        c.Key.cl_debit
                    })
                    .OrderBy(o => o.acc_name)
                    .GroupBy(g => g.acc_name)
                    .Select(s =>
                    {
                        double openingbal = s.Sum(item => item.op_credit != 0 ? item.op_credit : item.op_debit);
                        double currentbal = s.Sum(item => item.curr_credit != 0 ? item.curr_credit : item.curr_debit);
                        double closingbal = s.Sum(item => item.cl_credit != 0 ? item.cl_credit : item.cl_debit);
                        return new GroupListReport
                        {
                            acc_name = s.Key,
                            acc_code = s.First().acc_code,
                            city_name = s.First().city_name,
                            mobile_no = s.First().mobile_no,
                            opening = openingbal,
                            current = currentbal,
                            closing = closingbal,
                        };
                    })
                    .ToList();

                rtn.data = grplist;
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

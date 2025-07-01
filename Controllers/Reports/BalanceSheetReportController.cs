using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MotorMemo.Models.Procedures;
using MotorMemo.Services;
using Newtonsoft.Json;
using dss_reporting_services.Models;
using dss_reporting_services;
using Microsoft.Reporting.NETCore;
using MotorMemo.Models;
using System.Data;
namespace MotorMemo.Controllers.Reports
{
    [Route("[controller]")]
    public class BalanceSheetReportController : ControllerBase
    {
        private IMemoryCache _cache;
        private IWebHostEnvironment Environment;

        private string? _exportType = "PDF";
        private string? _reportCacheId;
        public readonly BalanceSheetProc _proc;
        public readonly CommanProc _comman;
        private respayload rtn = new respayload();
        public BalanceSheetReportController(IMemoryCache cache, IWebHostEnvironment environment, BalanceSheetProc proc, CommanProc comman)
        {
            _cache = cache;
            Environment = environment;
            _proc = proc;
            _comman = comman;
        }

        [HttpPost]
        public async Task<IActionResult> Report([FromBody] Dictionary<string, object> jsonArray)
        {
            var repoService = new ReportService();
            List<ReportParams> reportParams = JsonConvert.DeserializeObject<List<ReportParams>>(jsonArray["reportParams"].ToString());

            _reportCacheId = jsonArray["reportCacheId"].ToString();
            if (_reportCacheId != null)
            {
                if (jsonArray.ContainsKey("exportType"))
                    _exportType = jsonArray["exportType"].ToString();


                MailNav? _mailArray = null;

                if (jsonArray.ContainsKey("mail"))
                    _mailArray = JsonConvert.DeserializeObject<MailNav>(jsonArray["mail"].ToString());
                WappNav? _wappArray = null;

                if (jsonArray.ContainsKey("wapp"))
                    _wappArray = JsonConvert.DeserializeObject<WappNav>(jsonArray["wapp"].ToString());

                string docName = "document";
                if (jsonArray.ContainsKey("docName"))
                    docName = jsonArray["docName"].ToString();

                string[] DbParamNames = new string[] { "firm_id", "div_id", "edt", "reportType" };
                var DbParamsWithValue = repoService.getdbParams(reportParams, DbParamNames);

                string[] RpParamNames = new string[] { "edt" };
                var RpParamsWithValue = repoService.getReportParams(reportParams, RpParamNames);

                string basePath = Environment.ContentRootPath;

                string? reportType = reportParams.Where(w => w.key == "reportType").Select(s => s.value).SingleOrDefault();

                if (reportType == null)
                {
                    reportType = "1";
                }

                string RdlPath = basePath + ((Convert.ToInt16(reportType) == 1) ? @"\\Models\\Rdlc\\Balancesheet1.rdl" : @"\\Models\\Rdlc\\Balancesheet2.rdl");

                string MemType = "PDF";
                string contentType = "application/pdf";

                try
                {
                    CacheData? ReportData = repoService.ExistingCache(_reportCacheId, _cache, _exportType, _mailArray, _wappArray);

                    if (ReportData == null)
                    {
                        ReportData = new CacheData();

                        object dataset1 = await _proc.Data(Convert.ToInt16(DbParamsWithValue["firm_id"]),
                                            DbParamsWithValue["div_id"].ToString(),
                                             Convert.ToDateTime(DbParamsWithValue["edt"])
                                              );

                        object dataset2 = await _comman.firm(Convert.ToInt32(DbParamsWithValue["firm_id"]));

                        object dataset3 = await _proc.Data(Convert.ToInt16(DbParamsWithValue["firm_id"]),
                                           DbParamsWithValue["div_id"].ToString(),
                                            Convert.ToDateTime(DbParamsWithValue["edt"]));

                        var rdc = new List<ReportDataSource>();
                        rdc.Add(new ReportDataSource("DataSet1", dataset1));
                        rdc.Add(new ReportDataSource("DataSet2", dataset2));
                        rdc.Add(new ReportDataSource("DataSet3", dataset3));

                        if (_mailArray == null && _wappArray == null)
                        {
                            MemType = _exportType ?? "PDF";
                        }
                        else
                        {
                            if (_mailArray != null)
                            {

                                MemType = _mailArray.fileType;
                            }

                            else if (_wappArray != null)
                                if (_wappArray.fileType == "PDF")
                                {

                                    MemType = _wappArray.fileType;
                                }
                        }

                        ReportData.DataSources = rdc;
                        ReportData.Parameters = RpParamsWithValue;
                        ReportData.ExportType = MemType;
                        ReportData.ContentType = contentType;

                    }

                    if (ReportData.Data == null)
                    {
                        jsonArray = repoService.execReport(RdlPath, ReportData);
                        ReportData.ContentType = (string)jsonArray["contentType"];
                        ReportData.Data = (byte[])jsonArray["fileBytes"];

                        repoService.SetCache(_reportCacheId, _cache, ReportData);

                    }

                    if (_mailArray == null && _wappArray == null)
                        rtn.data = new { ArrayBuffer = File(ReportData.Data, contentType), ContentType = contentType, FileType = _exportType };

                    else
                    {
                        if (_mailArray != null)
                        {
                            repoService.smtpConfig = ReportHelper.getSmtpSetting();
                            rtn.data = repoService.sendEmail(ReportData.Data, _mailArray, docName);
                        }
                        else if (_wappArray != null)
                        {
                            repoService.whatsAppConfig = ReportHelper.getWappSetting();
                            rtn.data = repoService.sendWapp(ReportData.Data, _wappArray, docName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    rtn.status_cd = 0;
                    rtn.errors.message = ex.Message ?? ex.InnerException?.Message;
                }
            }
            else
            {
                rtn.status_cd = 0;
                rtn.errors.message = "Report Token Id Not Found";
            }

            return Ok(rtn);
        }
    }
}

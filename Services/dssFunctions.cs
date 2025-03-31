 
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


namespace MotorMemo.Services
{
    public class dssFunctions
    {
       
        static respayload rtn = new respayload();
        private readonly MotorMemoDbContext ? db;
        private readonly MainDbContext? MainDb;
        public dssFunctions(MotorMemoDbContext? context=null, MainDbContext? mainDb=null)
        {
            db = context;
            MainDb = mainDb;
        }

        public respayload GenerateChallanNo(long firm_id, string div_id, int vch_type, long vch_no, string tp = "")
        {
            rtn.status_cd = 1;

            if (db != null)
            {
                var Config = db.Sys002s
                    .Where(w => w.VchType == vch_type)
                    .AsEnumerable()
                    .SingleOrDefault();

                if (Config == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Voucher Configuration Not Found"
                    };
                    return rtn;
                }

                if (Config.AutoNo)
                {
                    if (string.IsNullOrEmpty(Config.Pattern) || Config.Pattern.Trim().Length == 0)
                    {
                        rtn.status_cd = 0;
                        rtn.errors = new errors
                        {
                            error_cd = "404",
                            message = "Voucher Pattern Not Found"
                        };
                        return rtn;
                    }
                    string? firm_alias = null;
                    string[]? ptrn = null;
                    string challan = "";
                    string? finYear = null;
                    if (MainDb != null)
                    {

                        finYear = MainDb.Mst005s
                           .Where(w => w.DivId == div_id && w.FirmCode == firm_id)
                           .Select(c => c.Prefix)
                           .SingleOrDefault();


                        ptrn = Config.Pattern.Split(',');



                        var frm = MainDb.Mst004s.Where(w => w.FirmCode == firm_id).FirstOrDefault();


                        if (frm == null)
                        {
                            rtn.status_cd = 0;
                            rtn.errors = new errors
                            {
                                error_cd = "404",
                                message = "Firm Details Not Found"
                            };
                            return rtn;
                        }


                        //var firm_alias = db.mst004.Where(w => w.firm_code == firm_id).First().firm_alias;

                        firm_alias = frm.FirmAlias;
                    }
                    if (ptrn != null)
                        foreach (var p in ptrn)
                        {
                            switch (p)
                            {
                                case "COMPANYCODE":
                                    challan += firm_id.ToString();
                                    break;

                                case "COMPANYALIAS":
                                    challan += (firm_alias ?? "").Trim();
                                    break;

                                //case "BRANCHCODE":
                                //    challan += branch_id.Trim();
                                //    break;

                                case "VOUCHERCODE":
                                    challan += vch_type.ToString().PadLeft(2, '0');
                                    break;

                                case "VOUCHERNO":
                                    int padLen = 0;

                                    if (Config.Padding)
                                        padLen = Config.VchLength - tp.Trim().Length;

                                    challan += tp.Trim() + vch_no.ToString().PadLeft(padLen, '0');

                                    break;

                                case "YEAR":
                                    challan += (finYear ?? "").Trim();
                                    break;

                                case "/":
                                    challan += "/";
                                    break;

                                case "-":
                                    challan += "-";
                                    break;

                                case "PREFIX":
                                    challan += (Config.Prefix ?? "").Trim();
                                    break;

                            }
                        }


                    if (challan.Length > 15)
                    {
                        rtn.status_cd = 0;
                        rtn.errors = new errors
                        {
                            error_cd = "404",
                            message = "Challan No Lenght(" + challan.Length + ") is Greater Than 15 Characters.\r\n Please Contact Your Administrator."
                        };

                        return rtn;
                    }


                    rtn.data = challan;
                }
                else
                {
                    rtn.status_cd = 2;
                }
            }
            return rtn;
        }

        public errors getException(Exception ex)
        {
            var rtn = new errors();

            rtn.error_cd = ex.HResult.ToString();
            rtn.message = ex.Message;

            if (ex.InnerException != null)
            {
                var a = ex.InnerException;

                rtn.error_cd = a.HResult.ToString();
                rtn.message = a.Message;

                if (a.InnerException != null)
                {
                    var b = a.InnerException;

                    rtn.error_cd = b.HResult.ToString();
                    rtn.message = b.Message;
                }

            }

            rtn.exception = ex;
            return rtn;
        }


     

    }
    

    public class respayload
    {

        public object? PageDetails { get; set; }
        public byte status_cd { get; set; } = 1;
        public object? data { get; set; }
        public errors errors { get; set; } = new errors();
       


    }
    public class errors
    {


        public  string? error_cd { get; set; }
        public string? message { get; set; }

        public  object? exception { get; set; }
    }

    public class UserSettings
    {
        public wapp wapp { get; set; }=new wapp();
        public uploadfile uploadfile { get; set; }= new uploadfile();
        public string? dbPath { get; set; }

    }
    public class wapp
    {
        public string url { get; set; } = null!;
        public string sender { get; set; } = null!;

    }
    public class uploadfile
    {
        public string path { get; set; } = null!;


    }

}

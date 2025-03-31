 
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.MotorMemoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
 

namespace RetailDesk.Models
{
    public class dssFunctions
    {

        static respayload rtn = new respayload();
        private readonly MotorMemoDbContext? db;
        private readonly MainDbContext? MainDb;
        public dssFunctions(MotorMemoDbContext? context=null, MainDbContext? mainDb=null)
        {
            db = context;
            MainDb = mainDb;
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

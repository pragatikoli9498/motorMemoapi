using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MotorMemo.Models.Context;
using MotorMemo.models;
using MotorMemo.ReportModels;
using Dapper;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace MotorMemo.Models.Procedures
{
    public class DayBookProc
    {
        private readonly MotorMemoDbContext _context;
        public DayBookProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id, DateTime sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select vch_type, vch_type_name, vch_id, ref_code, challan_no, vch_date, acc_code, Acc_name, COALESCE(refName, 'Nar') AS ref_name, 
                                		cramt, dramt, CASE WHEN refName IS NOT NULL THEN Nar ELSE NULL END AS naration, challan_id,email_id as d_email,contact_mobile_no
                                from Ledger 
                                where (firm_id = @firm_id) and (div_id=@div_id) and (vch_date between @sdt and @edt)
                                order by vch_date, vch_type, challan_no, challan_id";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt });
                return data;
            }

        }
    }
}

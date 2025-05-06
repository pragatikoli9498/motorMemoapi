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
    public class LedgerProc
    {
        private readonly MotorMemoDbContext _context;
        public LedgerProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id, int? sg_code, int? acc_code, DateTime sdt, DateTime edt, bool? Inventory)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT vch_type,vch_type_name,vch_id,vch_sr_no,refName,vch_date,challan_id,challan_no, ac_date,acc_code, Acc_name,
                                           acc_alias,City_id,City_name,City_pin,Taluka_id,Taluka_name,District_id,District_name,State_name,State_Code,Nar, cramt,dramt,
                                           linkedchallan,singlename,acc_address,contact_mobile_no,email_id,cost_center,
                                           iif(0,'', inventory)as inventory,
                                           iif(null ,'',sg_name) as sg_name
                                   FROM Ledger  
                                   WHERE firm_id =@firm_id and div_id=@div_id and (@sg_code is null or sg_code=@sg_code) and
                                           (@acc_code is null or acc_code = @acc_code) AND vch_date BETWEEN @sdt AND @edt 
 
                                   UNION ALL
 
                                   SELECT 0 as vch_type,'Opbl' AS vch_type_name,0 as vch_id,0 AS vch_sr_no,'Opening Balance' AS refName,vch_date,NULL AS challan_id,
                                           null as challan_no, null as ac_date,acc_code, Acc_name, acc_alias,City_id,City_name,City_pin,Taluka_id,Taluka_name,District_id,District_name,State_name,
                                           State_Code, null as Nar,CASE WHEN (SUM(cramt) - SUM(dramt)) > 0 THEN SUM(cramt) - SUM(dramt) ELSE 0 END AS cramt,CASE WHEN (SUM(cramt) - SUM(dramt)) < 0 THEN (SUM(cramt) - SUM(dramt)) * - 1 ELSE 0 END AS dramt,
                                           null as linkedchallan, null as inventory,singlename,acc_address, contact_mobile_no,email_id,cast(null as varchar(100)) as cost_center,
                                           iif( null ,'',sg_name) as sg_name
                                   FROM Ledger  
                                   WHERE firm_id =@firm_id and div_id=@div_id and  (@sg_code is null or sg_code=@sg_code) and
                                           (@acc_code is null or acc_code = @acc_code) AND  vch_date <@sdt
								   GROUP BY acc_code,Acc_name,acc_alias,City_id,City_name,City_pin,Taluka_id,Taluka_name,District_id,District_name,State_name,State_Code,
                                          singlename,acc_address,contact_mobile_no,email_id
                                   ORDER by acc_name,vch_date,vch_sr_no,challan_no";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sg_code, acc_code, sdt, edt, Inventory });
                return data;
            }

        }
    }
}

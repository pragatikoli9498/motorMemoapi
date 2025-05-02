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
    public class CashBankBookProc
    {
        private readonly MotorMemoDbContext _context;
        public CashBankBookProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> getCashBankOpening(int firm_id,string div_id,DateTime sdt,DateTime edt,int? acc_code)
        {
           
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT SUM(cramt-dramt) AS balance
                FROM Ledger
                WHERE firm_id=@firm_id
                    AND div_id =@div_id 
                    AND vch_date<@sdt 
                    AND acc_code= @acc_code
                GROUP BY acc_code";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id,div_id,sdt,edt,acc_code });
                return data;
            }

        }

        public async Task<object> getCashBankBook(int firm_id, string div_id, DateTime sdt, DateTime edt, int? acc_code)
        {
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select d.acc_name, d.vch_type_name, d.challan_no, d.acc_code, d.vch_date, d.ref_code, d.vch_id, d.vch_type, COALESCE(d.refName, d.Nar) AS refName,
                                 		case when d.refName is not null then d.Nar else null end as Naration, d.vch_no, d.dramt, d.cramt as cramt,
										 d.balance, d.challan_id,d.email_id,d.contact_mobile_no
                                from (
										select acc_code, acc_name, vch_type_name, challan_no, vch_date, ref_code, vch_id, vch_type, refName, Nar, vch_no, SUM(cramt)-SUM(dramt) as balance, 
                                				sum(dramt) as dramt, sum(cramt) as cramt, challan_id ,email_id,contact_mobile_no
                                 		from Ledger
                                            where firm_id=@firm_id 
                                            and div_id =@div_id 
                                            and (vch_date between @sdt and @edt) 
                                            and vch_type not in(4,5) 
                                            and (@acc_code is null or acc_code= @acc_code)
										
										group by acc_code, acc_name,vch_type_name, challan_no,vch_date, ref_code, vch_id, vch_type, refName, Nar, vch_no, challan_id,email_id,contact_mobile_no
										UNION ALL
										 select acc_code, acc_name,vch_type_name, challan_no,vch_date, null, null, vch_type,
										 'As Per '+case when vch_type=4 then 'Sales' else 'Purchase' end +' Register', null, null, 
										 SUM(cramt)-SUM(dramt) as balance, sum(dramt) as dramt, sum(cramt) as cramt, challan_id,email_id,contact_mobile_no
                                 		from Ledger
										    where firm_id=@firm_id 
                                            and div_id =@div_id
                                            and (vch_date between @sdt and @edt)
                                            and vch_type in(4,5) 
                                            and (@acc_code is null or acc_code= @acc_code)
										group by acc_code,acc_name,vch_type_name, challan_no, vch_date, vch_type, challan_id,email_id,contact_mobile_no
										 ) as d";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt, acc_code });
                return data;
            }
        }

    }
}

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
    public class TrialBalanceProc
    {
        private readonly MotorMemoDbContext _context;
        public TrialBalanceProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id,string div_id, DateTime? sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT grp_name, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, COALESCE(City_name,'') as City_name, div_id, 
		                                sum(dramt) as dr_amt, sum(cramt) as cr_amt, sum(cramt-dramt) as balance, show, grp_code, sg_code
                                from viewLedger   
                                where firm_id=@firm_id and div_id=@div_id and (vch_date <= @edt)

                                GROUP BY  grp_name, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, City_name, div_id, show, grp_code, sg_code
                                having  SUM(cramt-dramt)<>0

                                union all

                                select 'Opening Balance Difference', 'Opening Balance Difference', -1, -1, null, '', '', div_id,
		                                case when sum(drbal-crbal) < 0 then sum(drbal-crbal) * -1 else 0 end as dr_amt,
		                                case when sum(drbal-crbal) > 0 then sum(drbal-crbal) else 0 end as cr_amt, 
		                                sum(drbal-crbal)  as balance, cast(0 as bit) as show, null, null
                                from mst011_00
                                where firm_id=@firm_id and div_id=@div_id
                                group by div_id                              
                                having sum(drbal-crbal) <> 0
                                order by sg_code";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt });
                return data;
            }

        }

        
    }
}

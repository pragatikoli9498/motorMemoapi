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
    public class TrialBalance1Proc
    {
        private readonly MotorMemoDbContext _context;
        public TrialBalance1Proc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data1(int firm_id, string div_id, DateTime? sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select  grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, City_name, div_id,
										sum(balance)as balance, sum(op_dramt)as op_dramt, sum(op_cramt)as op_cramt, sum(cramt)as cramt, sum(dramt)as dramt, show
								from (
										select vch_date, grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, COALESCE(City_name, '') as City_name, 
												div_id, sum(cramt-dramt) as balance,
												case when sum(cramt-dramt) < 0 then sum(cramt-dramt) * -1 else 0 end as op_dramt,
												case when sum(cramt-dramt) > 0 then sum(cramt-dramt) else 0 end as op_cramt,
												0 as cramt, 0 as dramt, show
										from viewLedger
 										where firm_id =@firm_id and div_id=@div_id and vch_date < @sdt
		
										group by grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, City_name, div_id, show
										having case when  sum(cramt-dramt) < 0 then sum(cramt-dramt) * -1 else 0 end <> 0 or
												case when sum(cramt-dramt) > 0 then sum(cramt-dramt) else 0 end <> 0

										union all

										select vch_date, grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, COALESCE(City_name, '') as City_name, 
												div_id, sum(cramt-dramt) as balance, 0 as op_dramt, 0 as op_cramt, sum(cramt) as cramt, sum(dramt) as dramt, show
										from viewLedger
 										where firm_id =@firm_id and div_id=@div_id and (vch_date between @sdt and @edt)

										group by vch_date, grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, City_name, div_id, show
										having sum(cramt) <> 0 or sum(dramt) <> 0

										union all
		
										select null, null, 'Opening Balance Difference', null, 'Opening Balance Difference', -1, -1, null, '', '', 
												div_id, sum(drbal - crbal), 
												case when sum(drbal - crbal) < 0 then sum(drbal - crbal) * -1 else 0 end as dramt,
												case when sum(drbal - crbal) > 0 then sum(drbal - crbal) else 0 end as cramt,
												0, 0, cast(0 as bit) as show
										from mst011_00
 										where firm_id =@firm_id and div_id =@div_id
										
										group by div_id
										having sum(drbal-crbal) <> 0
								)as d
								group by grp_code, grp_name, sg_code, sg_name, grp_sr_no, sg_sr_no, acc_code, Acc_name, City_name, div_id, show";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt });
                return data;
            }

        }
    }
}

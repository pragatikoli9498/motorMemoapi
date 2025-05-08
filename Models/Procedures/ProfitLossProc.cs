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
    public class ProfitLossProc
    {

        private readonly MotorMemoDbContext _context;
        public ProfitLossProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT mst011.Acc_name, SUM(acc999_01.cramt - acc999_01.dramt)*iif(mst001.mg_bs =5 , 1 ,-1 ) AS balance,
                            				mst003.sg_name, mst003.sr_no, 1 as idx, mst011.acc_code, mst003.sg_code, mst001.mg_bs, mst003.show,
                                            mst002.mg_code,mst002.sr_no as grp_sr_no,
                                            cast(0 as bit)  as isclosing
                            		FROM acc999
                            				inner join acc999_01 on acc999.vch_id=acc999_01.vch_id 
                            				INNER JOIN mst011 ON acc999_01.acc_Code = mst011.acc_code 
                            				INNER JOIN mst003 ON mst011.sg_code = mst003.sg_code 
                            				INNER JOIN mst002 ON mst003.grp_code = mst002.grp_code 
                            				INNER JOIN mst001 ON mst002.mg_code = mst001.mg_code  
                            		WHERE mst001.mg_bs in(5,6) and acc999.div_id=@div_id and  acc999.firm_id=@firm_id 	
                            				and acc999.vch_date<=@edt
                            		GROUP BY mst011.Acc_name, mst003.sg_name,
                                            mst003.sr_no,mst011.acc_code,mst003.sg_code,mst003.show,mst001.mg_bs,   mst002.mg_code,mst002.sr_no 
                                     having SUM(acc999_01.cramt - acc999_01.dramt)<>0";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, edt });
                return data;
            }

        }

    }
}

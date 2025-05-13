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
    public class MotormemoRegProc
    {
        private readonly MotorMemoDbContext _context;
        public MotormemoRegProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id,DateTime? sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT MEMO.memo_No, memo.dt,MEMO.from_dstn,MEMO.to_dstn,MEMO.vehicle_no,Sundry.sundry_name,EXP.charges,memo.TotalFreight,memo.AdvAmount,memo.LeftAmount FROM motormemo MEMO
                                        LEFT JOIN motormemo_expense EXP ON MEMO.vch_id = EXP.vch_id
                                        LEFT JOIN Sundry ON EXP.S_id=Sundry.S_Id
                                        Where MEMO.firm_id = @firm_id
                                            and MEMO.div_id = @div_id
                                            and (MEMO.dt between @sdt and @edt)";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt });
                return data;
            }

        }
    }
}

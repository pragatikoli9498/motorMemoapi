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
    public class ExpensesProc
    {
        private readonly MotorMemoDbContext _context;
        public ExpensesProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }
        public async Task<object> Data(int firm_id, string div_id,int s_id, DateTime sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select motormemo.*,Sundry.sundry_name,mst011.Acc_name,motormemo_expense.charges,motormemo_expense.s_id from motormemo
                                        inner join motormemo_expense on motormemo.vch_id=motormemo_expense.vch_id
                                        left join Sundry on motormemo_expense.s_id=Sundry.S_Id
                                        left join mst011 on motormemo_expense.acc_code=mst011.acc_code
                                       
                                where (motormemo.firm_id = @firm_id) and (motormemo.div_id=@div_id) and (motormemo.dt between @sdt and @edt) and (motormemo_expense.s_id = @s_id)
                                order by motormemo.dt";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id,s_id, sdt, edt });
                return data;
            }

        }
    }
}

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
    public class LorryReceipt2RegProc
    {
        private readonly MotorMemoDbContext _context;
        public LorryReceipt2RegProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }
        public async Task<object> Data(int firm_id, string div_id, DateTime? sdt, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select motormemo2.* from motormemo2
                                        Where motormemo2.firm_id = @firm_id
                                            and motormemo2.div_id = @div_id
                                            and (motormemo2.Vch_date between @sdt and @edt)";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt });
                return data;
            }

        }
    }
}

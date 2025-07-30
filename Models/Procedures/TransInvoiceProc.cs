using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MotorMemo.Models.Context;
using MotorMemo.models;
using MotorMemo.ReportModels;
using Dapper;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MotorMemo.Models.Procedures
{
    public class TransInvoiceProc : CreateDataSet
    {
        private readonly MotorMemoDbContext _context;
        public TransInvoiceProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context) :
               base(SqlLiteConnstring, configuration, context)
        {
            _context = context;
        }
        public async Task<object> Data(int vch_id)
        {
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT tms011.*,mst011.Acc_name,mst006_03.State_name,tms011_01.memo_No,tms011_01.dt,
                                    tms011_01.from_dstn,tms011_01.to_dstn,tms011_01.vehicle_no,tms011_01.KiloMiters,tms011_01.BillAmt
                                    FROM tms011
                                    INNER JOIN tms011_01 ON tms011.vch_id = tms011_01.vch_id
                                    LEFT JOIN mst011 ON tms011.AccCode = mst011.acc_code
                                    left join mst006_03 on tms011.StateCode = mst006_03.State_Code
                                where (tms011.vch_id=@vch_id)";

                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id});
                return data;
            }
        }
    }
}

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
    public class TransInvoiceRegProc
    {
        private readonly MotorMemoDbContext _context;
        public TransInvoiceRegProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }
        public async Task<object> Data(int firm_id, string div_id, DateTime sdt, DateTime edt,int accCode)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {
                var CommandText = @"select tms011.*,mst011.Acc_name,mst006_03.State_name from tms011 
                                            inner join tms011_01 on tms011.Vch_id=tms011_01.Vch_id
                                            left join mst011 on tms011.AccCode=mst011.acc_code
                                            left join mst006_03 on tms011.StateCode=mst006_03.State_Code
                                            where (tms011.Firm_id = @firm_id) and (tms011.Div_id=@div_id) and (tms011.vch_dt between @sdt and @edt) and (tms011.AccCode=@accCode)";

                    var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sdt, edt, accCode });
                    return data;
                }
            }
        }

    
}

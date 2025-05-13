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
    public class MotorememoProc
    {
        private readonly MotorMemoDbContext _context;
        public MotorememoProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int vch_id)
        {
            //var oParams = new SqliteParameter[1];
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT motormemo.*, d.type,d.commodity,d.uom,d.qty,d.chrgWeight,d.actWeight,d.rate,d.freight,
	                                    motormemo_details.ReceiverGstin,motormemo_details.ReceiverName,motormemo_details.ReceiverMobileNo,motormemo_details.ReceiverMail,motormemo_details.EwayNo,
	                                    motormemo_details.SenderName,motormemo_details.SenderGstin,motormemo_details.SenderMobileNo,motormemo_details.SenderMail,motormemo_details.SenderBillNo,motormemo_details.SenderBillDt
                                        FROM motormemo
                                    INNER JOIN (
                                        SELECT 'Common' AS type,motormemo_commodity.vch_id,motormemo_commodity.commodity,motormemo_commodity.uom,motormemo_commodity.qty,motormemo_commodity.chrgWeight,
                                            motormemo_commodity.actWeight,motormemo_commodity.rate,motormemo_commodity.freight
                                            FROM motormemo_commodity 
                                    WHERE motormemo_commodity.vch_id = @vch_id
 
                                        UNION ALL

                                    SELECT 'Expenses' AS type,motormemo_expense.vch_id,Sundry.sundry_name AS commodity,NULL AS uom,NULL AS qty,NULL AS chrgWeight,NULL AS actWeight,
                                        NULL AS rate,-motormemo_expense.charges AS freight
                                        FROM motormemo_expense
                                    INNER JOIN Sundry ON motormemo_expense.S_id = Sundry.S_Id
                                    WHERE motormemo_expense.vch_id = @vch_id
                                    ) d ON motormemo.vch_id = d.vch_id
                                    inner join motormemo_details on motormemo.vch_id=motormemo_details.vch_id
                                    WHERE motormemo.vch_id = @vch_id";

                //oParams[0] = new SqliteParameter("vch_id", vch_id);

                //DataTable dataTable = await CreateTableMototrmemo(CommandText, oParams);
                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id });
                return data;
            }
        }
    }
}

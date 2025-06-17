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
    public class BiltyProc
    {
        private readonly MotorMemoDbContext _context;
        public BiltyProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int vch_id)
        {
            //var oParams = new SqliteParameter[1];
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select bilty.*,bilty_details.ReceiverName,bilty_details.ReceiverAddress,bilty_details.ReceiverMobileNo,bilty_details.ReceiverGstin,bilty_details.ReceiverMail,recState.State_name as ReceiverState,bilty_details.SenderName,
                                        bilty_details.SenderAddress,bilty_details.SenderMobileNo,bilty_details.SenderGstin,bilty_details.SenderMail,sendState.State_name as SenderState,bilty_Gst.igst,bilty_Gst.IgstAmt,bilty_Gst.cgst,bilty_Gst.CgstAmt,bilty_Gst.sgst,
                                        bilty_Gst.SgstAmt,bilty_Gst.cess,bilty_Gst.CessAmt,bilty_Gst.TotalAmt,d.commodity,d.uom,d.actweight, d.chrgWeight,d.qty,d.rate,d.freight
                                        from bilty

                                        inner join(Select bilty_commodity.Detl_id,bilty_commodity.vch_id,bilty_commodity.commodity,bilty_commodity.uom,
                                        bilty_commodity.actweight,bilty_commodity.chrgWeight,bilty_commodity.qty,bilty_commodity.rate,bilty_commodity.freight from bilty_commodity) d on bilty.vch_id=d.vch_id

                                        INNER join bilty_details on bilty.vch_id = bilty_details.vch_id
                                        left join mst006_03 as recState on bilty_details.ReceiverStateId=recState.State_Code
                                        left join mst006_03 as sendState on bilty_details.SenderStateId=sendState.State_Code
                                        inner join bilty_Gst on bilty.vch_id =bilty_Gst.vch_id
                                        
                                    WHERE bilty.vch_id = @vch_id";

                //oParams[0] = new SqliteParameter("vch_id", vch_id);

                //DataTable dataTable = await CreateTableMototrmemo(CommandText, oParams);
                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id });
                return data;
            }
        }
    }
}

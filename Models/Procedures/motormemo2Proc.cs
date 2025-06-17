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
    public class motormemo2Proc
    {
        private readonly MotorMemoDbContext _context;
        public motormemo2Proc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }
        public async Task<object> Data(int vch_id)
        {
            //var oParams = new SqliteParameter[1];
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select  motormemo2.*,bilty.bilty_no,bilty.bill_dt,bilty_details.SenderName,bilty_details.ReceiverName,bilty_details.ReceiverPlace,motormemo2_child.detl_id,motormemo2_child.Weight,motormemo2_child.EwayNo,motormemo2_child.ValidUpTo
                                         from motormemo2
                                        inner join motormemo2_child on motormemo2.vch_id=motormemo2_child.vch_id
                                        left join bilty on motormemo2_child.bilty_id=bilty.vch_id
                                        left join bilty_details on bilty.vch_id=bilty_details.vch_id

                                    WHERE motormemo2.vch_id = @vch_id";

                //oParams[0] = new SqliteParameter("vch_id", vch_id);

                //DataTable dataTable = await CreateTableMototrmemo(CommandText, oParams);
                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id });
                return data;
            }
        }
    }
}

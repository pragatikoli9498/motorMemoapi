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

                var CommandText = @"select row_number() OVER (PARTITION by acc002.vch_id ORDER BY acc002_01.detl_id) AS Sr_No,acc002.vch_id,acc002.vch_date,acc002.vch_no,acc002.challan_no,cr.Acc_name,acc002.amount as net_amt,acc002.Nar,
                                    case when acc002.txn_type=1 then 'Cheque' when acc002.txn_type=2 then 'RTGS' when acc002.txn_type=3 then 'NEFT'
			                                    when acc002.txn_type=4 then 'Debit Card' when acc002.txn_type=5 then 'Credit Card' else 'Other' end as trns_type,
                                    acc002.txn_no,acc002.txn_date,acc002.txn_drawnon,acc002.txn_bywhome,
                                    case when acc002.against=2 then'Purchase' when acc002.against=3 
                                    then 'Advanced' else 'Unknow' end as against,acc002.ref_no,acc002.ref_date,dr.Acc_name as bank_name,dr.City_name,acc002_01.rec_amt as dr_amount,
                                    acc002_01.tds_rate,acc002_01.tds_amt,acc002_01.amount,mst010.I_Name,acc002_01.detl_id
                                    from acc002
                                    INNER JOIN acc002_01 on acc002.vch_id=acc002_01.vch_id
                                    INNER JOIN Accmst as cr on acc002.acc_code=cr.acc_code
                                    INNER join Accmst as dr on acc002_01.acc_code=dr.acc_code
                                    LEFT JOIN mst010 on acc002_01.cost_id=mst010.I_id
--                                  where acc002.vch_id=@vch_id";

                //oParams[0] = new SqliteParameter("vch_id", vch_id);

                //DataTable dataTable = await CreateTableMototrmemo(CommandText, oParams);
                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id });
                return data;
            }
        }
    }
}

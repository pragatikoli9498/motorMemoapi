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

    public class ReceiptProc : CreateDataSet
    {
        private readonly MotorMemoDbContext _context;
        public ReceiptProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context) :
               base(SqlLiteConnstring, configuration, context)
        {
            _context = context;
        }

        public async Task<object> Data(int vch_id)
        {
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select ROW_NUMBER() OVER (PARTITION BY acc003.vch_id ORDER BY acc003_01.detl_id) AS Sr_No,acc003.vch_id,acc003.vch_no,acc003.vch_date,acc003.challan_no,acc003.amount as net_amt,acc003.Nar,acc003.LrId,
                                case when acc003.txn_type=1 then 'Cheque' when acc003.txn_type=2 then 'RTGS' when acc003.txn_type=3 then 'NEFT'
			                    when acc003.txn_type=4 then 'Debit Card' when acc003.txn_type=5 then 'Credit Card' else 'Other'
		                        end as trns_type,acc003.txn_no,acc003.txn_date,acc003.txn_drawnon,acc003.txn_bywhome,
                                case when acc003.against=1 then'Sale' when acc003.against=3 then 'Advanced' else 'Unknow' end as against,acc003.ref_no,acc003.ref_date,
                                Cr.Acc_name as acc_name,Cr.City_name,acc003_01.rec_amt as cr_amount,acc003_01.tds_rate,acc003_01.tds_amt,acc003_01.amount,acc003_01.detl_id
                                from acc003
                                INNER JOIN acc003_01 on acc003.vch_id=acc003_01.vch_id
                                INNER JOIN Accmst as Cr on acc003_01.acc_code=Cr.acc_code
                                where acc003.vch_id=@vch_id";

                var data = await ocomm.QueryAsync<object>(CommandText, new { vch_id });
                return data;
            }
        }
    }
}
    


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
    public class SubGroupListProc
    {
        private readonly MotorMemoDbContext _context;
        public SubGroupListProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id, int? sg_code, DateTime? sdt, DateTime? edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT sg_code, sg_name, acc_code, Acc_name,City_name,mobile_no,cast(SUM(op_credit)*1.00 as decimal(12,3)) AS op_credit, cast(SUM(op_debit)*1.00 as decimal(12,3)) AS op_debit, cast(SUM(curr_credit)*1.00 as decimal(12,3)) AS curr_credit, 
                                cast(SUM(curr_debit)*1.00 as decimal(12,3)) AS curr_debit,cast((CASE WHEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) > 0 THEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) ELSE 0 END) *1.00 as decimal(12,3)) AS cl_credit,
                                		                                cast((CASE WHEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) < 0 THEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) * - 1 ELSE 0 END)*1.00 as decimal(12,3)) AS cl_debit,
										                                email_id
                                 FROM
                                      (
                                         SELECT	sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no as mobile_no,email_id,
                                         CASE WHEN (SUM(cramt) - SUM(dramt)) > 0 THEN SUM(cramt) - SUM(dramt) ELSE 0 END AS op_credit, 
                                         CASE WHEN (SUM(cramt) - SUM(dramt)) < 0 THEN (SUM(cramt) - SUM(dramt)) * - 1 ELSE 0 END AS op_debit, 0 as curr_credit, 0 as curr_debit
                                         FROM Ledger 
                                          WHERE	(firm_id = @firm_id) AND (div_id = @div_id) AND (vch_date < @sdt) AND (@sg_code IS NULL OR sg_code=@sg_code)
		                                GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no,email_id
                                        union all
                                        SELECT	sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no as mobile_no,email_id, 0 as op_credit, 0 as op_debit,
                                        SUM(cramt) AS curr_credit, SUM(dramt) AS curr_debit
                                        FROM Ledger 
                                        WHERE	(firm_id = @firm_id) AND (div_id = @div_id) AND (vch_date BETWEEN @sdt AND @edt) AND (@sg_code IS NULL OR sg_code=@sg_code)                                  
                                         GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no,email_id
                                         ) as drlst
                                GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, mobile_no,email_id ";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sg_code, sdt, edt });
                return data;
            }
        }

             public async Task<object> Datas(int firm_id, string div_id, int? sg_code)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"SELECT sg_code, sg_name, acc_code, Acc_name,City_name,mobile_no,cast(SUM(op_credit)*1.00 as decimal(12,3)) AS op_credit,cast(SUM(op_debit)*1.00 as decimal(12,3)) AS op_debit, cast(SUM(curr_credit)*1.00 as decimal(12,3)) AS curr_credit, 
                                cast(SUM(curr_debit)*1.00 as decimal(12,3)) AS curr_debit,cast((CASE WHEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) > 0 THEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) ELSE 0 END)*1.00 as decimal(12,3)) AS cl_credit,
                                		                                cast((CASE WHEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) < 0 THEN ((SUM(op_credit) + SUM(curr_credit))- (SUM(op_debit) + SUM(curr_debit))) * - 1 ELSE 0 END)*1.00 as decimal(12,3))  AS cl_debit,
										                                email_id
                                 FROM
                                      (
                                         SELECT	sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no as mobile_no,email_id,
                                         CASE WHEN (SUM(cramt) - SUM(dramt)) > 0 THEN SUM(cramt) - SUM(dramt) ELSE 0 END AS op_credit, 
                                         CASE WHEN (SUM(cramt) - SUM(dramt)) < 0 THEN (SUM(cramt) - SUM(dramt)) * - 1 ELSE 0 END AS op_debit, 0 as curr_credit, 0 as curr_debit
                                         FROM Ledger 
                                          WHERE	(firm_id = @firm_id) AND (@branch_id IS NULL OR branch_id = @branch_id) AND (div_id = @div_id)  AND ( sg_code=@sg_code)
		                                GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no,email_id
                                        union all
                                        SELECT	sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no as mobile_no,email_id, 0 as op_credit, 0 as op_debit,
                                        SUM(cramt) AS curr_credit, SUM(dramt) AS curr_debit
                                        FROM Ledger 
                                        WHERE	(firm_id = @firm_id) AND (@branch_id IS NULL OR branch_id = @branch_id) AND (div_id = @div_id)  AND ( sg_code=@sg_code)                                  
                                         GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, contact_mobile_no,email_id
                                         ) as drlst
                                        
                                GROUP BY  sg_code, sg_name, acc_code, Acc_name, City_name, mobile_no,email_id";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, sg_code});
                return data;
            }

        }
    }
}

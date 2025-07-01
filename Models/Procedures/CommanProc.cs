using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MotorMemo.models;
using MotorMemo.Models.Context;
using MotorMemo.ReportModels;
using System.Data;

namespace MotorMemo.Models.Procedures
{
    public class CommanProc
    {
        private readonly MainDbContext _context;
        public CommanProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MainDbContext context) 
           
        {
            _context = context;
        }

        public async Task<object> firm(int? firm_id)
        {
         
            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select mst004.*,mst004_01.logo
                                    FROM mst004
                                    INNER JOIN mst004_01 on mst004_01.firm_code=mst004.firm_code";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id });
                return data;
            }
           
        }
    }
}

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data.Common;
using System.Data;
using static SQLite.SQLite3;
using BoldReports.Web.ReportViewer.Internal;
using BoldReports.Web;
using MotorMemo.Models.Context;
using MotorMemo.ReportModels;

namespace MotorMemo.models
{ 
   public class CreateDataSet
    {
        public readonly SqliteConnection MainDbConnection;
        public readonly SqliteConnection MotormemoConnection;
        private readonly MotorMemoDbContext _context;
        public CreateDataSet(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)
        {

            MainDbConnection = new SqliteConnection(SqlLiteConnstring.Value.DefaultConnection);
            MotormemoConnection = new SqliteConnection(string.Format(SqlLiteConnstring.Value.motormemoConnection, configuration["DataBaseName"]));
            _context = context;
        }
        public async Task<DataTable> CreateTableMototrmemo(string CommandText, SqliteParameter[] oParams)
        {
            MotormemoConnection.Open();
            var ocmd = new SqliteCommand(CommandText, MotormemoConnection);

            ocmd.Parameters.AddRange(oParams);

            var reader =  ocmd.ExecuteReader();
            DataTable dataTable = createTable(reader);
            MotormemoConnection.Close();

            return dataTable;
        }
        public async Task<DataTable> CreateTableMainDb(string CommandText, SqliteParameter[] oParams)
        {
            MainDbConnection.Open();
            var ocmd = new SqliteCommand(CommandText, MainDbConnection);

            ocmd.Parameters.AddRange(oParams);


            var reader = await ocmd.ExecuteReaderAsync();
            DataTable dataTable = createTable(reader);

            MotormemoConnection.Close();

            return dataTable;
        }

        private DataTable createTable(DbDataReader reader)
        {
            DataTable dataTable = new DataTable();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var col = new DataColumn();
                col.ColumnName = reader.GetName(i);
                col.DataType = reader.GetFieldType(i);
                col.AllowDBNull = true;

                dataTable.Columns.Add(col);
            }


            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.IsDBNull(i))
                    {
                        row[i] = DBNull.Value; // Or set to some default value if needed
                    }
                    else
                    {
                        row[i] = reader.GetValue(i);
                    }
                }
                dataTable.Rows.Add(row);
            }



            reader.Close();
            return dataTable;
        }
        
    }
}

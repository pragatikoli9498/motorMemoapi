using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options; 
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Models.MainDbEntities;
using System.Data; 
using MotorMemo.Models.Context;

using Newtonsoft.Json;  
using RetailDesk.Models;  
using static MotorMemo.Models.Helper;
using MotorMemo.Services;
using MotorMemo.models;
using Microsoft.Data.Sqlite;




namespace MotorMemo.ReportModels.Procedures
{
    public class OpeningBalProc : CreateDataSet
    {
        public OpeningBalProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context) :
           base(SqlLiteConnstring, configuration, context)
        {
        }

        public async Task<DataTable> Data(long? firm_id, string? branch_id, string div_id)
        {
            var oParams = new SqliteParameter[3];
             

            var CommandText = @" SELECT mst002.grp_name,mst003.sg_name, mst002.Sr_no grp_sr_no,mst003.sr_no  sg_sr_no, mst011.acc_code, mst011.Acc_name,
                                COALESCE(mst006.City_name,'') as City_name, mst011_00.div_id, drbal as dr_amt, crbal as cr_amt,  crbal-drbal as balance, 
                                show, mst002.grp_code, mst003.sg_code
                                 from mst011_00
                                 inner join mst011 on mst011_00.acc_code=mst011.acc_code
                                 inner join mst003 on mst011.sg_code=mst003.sg_code 
                                 inner join mst002 on mst003.grp_code=mst002.grp_code
                                 left join mst011_01 on mst011.acc_code=mst011_01.acc_code
                                 left join mst006 on mst011.place_id=mst006.City_id 
                                where mst011_00.div_id=@div_id  
                                and (@firm_id is null or mst011_00.firm_id=@firm_id)
                                and (@branch_id IS NULL OR mst011_00.branch_id=@branch_id) 
                                and  crbal-drbal<>0";

            oParams[0] = new SqliteParameter("firm_id", firm_id);
            oParams[1] = new SqliteParameter("branch_id", branch_id);
            oParams[2] = new SqliteParameter("div_id", div_id);

            DataTable dataTable = await CreateTableMototrmemo(CommandText, oParams);

            return dataTable;
        }

    }
}

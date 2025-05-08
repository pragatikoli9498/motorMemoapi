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
    public class BalanceSheetProc
    {
        private readonly MotorMemoDbContext _context;
        public BalanceSheetProc(IOptions<ConnectionString> SqlLiteConnstring, IConfiguration configuration, MotorMemoDbContext context)

        {
            _context = context;
        }

        public async Task<object> Data(int firm_id, string div_id, DateTime edt)
        {

            using (var ocomm = new SqliteConnection(_context.Database.GetConnectionString()))
            {

                var CommandText = @"select * from (  SELECT mst011.Acc_name || COALESCE(',' || mst006.City_name, '') AS acc_name,mst003.sg_name, mst002.grp_name, mst001.mg_name,
                                        mst001.mg_head,cast((SUM(acc999_01.cramt - acc999_01.dramt)*case when mst001.mg_bs =1 then -1 else 1 end)*1.00 as decimal(12,3)) AS balance,
                                        mst003.sr_no AS sg_sr_no,mst002.Sr_no AS grp_sr_no,mst001.mg_type AS mg_sr_no, mst011.acc_code, mst011.sg_code, 1 as idx, mst001.mg_bs,mst003.show,
										cast(0 as bit) as isclosing,cast(0 as bit) as isopdifference
                                        FROM acc999
                                        inner join acc999_01 on acc999.vch_id=acc999_01.vch_id 
                                        INNER JOIN mst011 ON acc999_01.acc_Code = mst011.acc_code 
                                        INNER JOIN mst003 ON mst011.sg_code = mst003.sg_code 
                                        INNER JOIN mst002 ON mst003.grp_code = mst002.grp_code 
                                        INNER JOIN mst001 ON mst002.mg_code = mst001.mg_code 
                                        left join mst011_01 on mst011.acc_code=mst011_01.acc_code 
                                        left join mst006 on mst011.place_id=mst006.City_id 

                                   WHERE  mst001.mg_bs in (1,2) and (mst003.show = 1) and acc999.div_id=@div_id  and  acc999.firm_id=@firm_id and acc999.vch_date <=@edt 
 							            
								   GROUP BY mst011.Acc_name,mst006.City_name, mst003.sg_name, mst003.sr_no, mst002.grp_name, mst002.Sr_no, mst001.mg_name,   
                                                        mst001.mg_bs,mst001.mg_head, mst001.mg_type, mst001.mg_bs, mst011.acc_code, mst011.sg_code ,mst003.show 
                                                        having (SUM(acc999_01.cramt - acc999_01.dramt)) <>0    
														
								  union all 

								  SELECT mst003.sg_name, mst003.sg_name, mst002.grp_name,mst001.mg_name, mst001.mg_head,
                                        cast((SUM(acc999_01.cramt - acc999_01.dramt)*case when mst001.mg_bs  =1 then -1 else 1 end)* 1.00 as decimal(12,3)) AS balance,
                                        mst003.sr_no AS sg_sr_no, mst002.Sr_no AS grp_sr_no,mst001.mg_type AS mg_sr_no, mst011.acc_code,mst011.sg_code,
                                        2 as idx,mst001.mg_bs,mst003.show,cast(0 as bit) as isclosing,cast(0 as bit) as isopdifference
                                        FROM acc999 
                                        inner join acc999_01 on acc999.vch_id=acc999_01.vch_id 
                                        INNER JOIN mst011 ON acc999_01.acc_Code = mst011.acc_code 
                                        INNER JOIN mst003 ON mst011.sg_code = mst003.sg_code 
                                        INNER JOIN mst002 ON mst003.grp_code = mst002.grp_code 
                                        INNER JOIN mst001 ON mst002.mg_code = mst001.mg_code

                                 WHERE mst001.mg_bs in (1,2) and   (mst003.show = 0) and acc999.div_id=@div_id  and  acc999.firm_id=@firm_id and acc999.vch_date <=@edt 
						                
								 GROUP BY mst003.sg_name, mst003.sr_no, mst002.grp_name, mst002.Sr_no, mst001.mg_name,   
                                                        mst001.mg_bs, mst001.mg_head, mst001.mg_type, mst001.mg_bs,mst011.acc_code, mst011.sg_code ,mst003.show 
                                                        having SUM(acc999_01.cramt - acc999_01.dramt) <>0    
				                 union all 	
				
				                 SELECT mst003.sg_name AS acc_name, mst003.sg_name,mst002.grp_name, mst001.mg_name, mst001.mg_head, 0 AS balance,
				                         mst003.sr_no AS sg_sr_no,mst002.Sr_no AS grp_sr_no, mst001.mg_type AS mg_Sr_no,
                                         null,mst003.sg_code,3 as idx, 1 as mg_bs,0,cast(1 as bit) as isclosing,cast(0 as bit) as isopdifference
				                         FROM mst011_00 
                                         INNER JOIN mst011 ON mst011_00.acc_code = mst011.acc_code 
                                         INNER JOIN mst003 ON mst011.sg_code = mst003.sg_code 
                                         INNER JOIN mst002 ON mst003.grp_code = mst002.grp_code 
                                         INNER JOIN mst001 ON mst002.mg_code = mst001.mg_code

                                WHERE (mst002.mg_code = 6) and mst011_00.div_id=@div_id and mst011_00.firm_id=@firm_id 

				                GROUP BY mst003.sg_name, mst002.grp_name, mst001.mg_name, mst001.mg_head, mst003.sr_no, mst002.Sr_no,   
                                                        mst001.mg_bs, mst001.mg_type, mst003.sg_code  
														
														) blsheet  where balance<>0
                                                    ORDER BY mg_bs, mg_sr_no, mg_head, grp_sr_no, sg_sr_no, Acc_name";

                var data = await ocomm.QueryAsync<object>(CommandText, new { firm_id, div_id, edt });
                return data;
            }

        }
    }
}

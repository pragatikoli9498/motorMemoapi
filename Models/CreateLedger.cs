using Newtonsoft.Json; 
using MotorMemo.Models.MotorMemoEntities; 
using System.Data;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.MainDbEntities;
using MotorMemo.Models.Context; 
using static MotorMemo.Models.Helper;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;

namespace MotorMemo.Models
{
     
        public class CreateLedger
        {
            private readonly MotorMemoDbContext db;
       
        private respayload rtn = new respayload();
        public CreateLedger(MotorMemoDbContext context)
            {
                db = context;
           
        }
            private static string getException(string ac)
            {
                return "Tax Account of " + ac + " Not Set. Please Contact Your Administrator";
            }

            public  Acc999 opening(Mst01100 opening)
            {

                Acc999 ledger = new Acc999();
                ledger.FirmId = opening.FirmId;
                ledger.DivId = opening.DivId; 
                ledger.ChallanId = opening.VchId;
                ledger.VchType = 0;

                ledger.VchDate = new DateTime(1900, 1, 1);
                ledger.Nar = null;


                ledger.Acc99901s.Add(new Acc99901
                {
                    AccCode = opening.AccCode,

                    AcDate = new DateTime(1900, 1, 1).ToString(),

                    Dramt = opening.Drbal,
                    Cramt = opening.Crbal
                });


            return ledger;

        }

            public  Acc999 contra(Acc006 challan)
            {
                Acc999 ledger = new Acc999();
                ledger.FirmId = challan.FirmId;
                ledger.DivId = challan.DivId; 
                ledger.ChallanId = challan.VchId;
                ledger.VchType = 1;
                ledger.VchNo = challan.VchNo;
                ledger.ChallanNo = challan.ChallanNo;
                ledger.VchDate = challan.VchDate;
                ledger.Nar = challan.Nar;

                int i = 0;

                foreach (var voucher in challan.Acc00601s)
                {
                    if (i == 0)
                        ledger.Acc99901s.Add(new Acc99901
                        {
                            AccCode = challan.AccCode,
                            RefCode = voucher.AccCode,
                            AcDate = challan.ToString(),
                            Nar = challan.Nar ?? voucher.Nar,
                            Dramt = challan.TransType == 0 ? challan.Amount : 0,
                            Cramt = challan.TransType == 1 ? challan.Amount : 0,
                        });


                    ledger.Acc99901s.Add(new Acc99901
                    {
                        AccCode = voucher.AccCode,
                        RefCode = challan.AccCode,
                        AcDate = challan.ToString(),
                        Nar = voucher.Nar ?? challan.Nar,
                        Dramt = challan.TransType == 1 ? voucher.Amount : 0,
                        Cramt = challan.TransType == 0 ? voucher.Amount : 0,
                    });
                    i += 1;
                }
                return ledger;

            }

            public  Acc999 journal(Acc005 challan)
            {
                Acc999 ledger = new Acc999();

                ledger.FirmId = challan.FirmId;
                ledger.DivId = challan.DivId; 
                ledger.ChallanId = challan.VchId;
                ledger.VchType = 25;
                ledger.VchNo = challan.VchNo;
                ledger.ChallanNo = challan.ChallanNo;
                ledger.VchDate = challan.VchDate;
                ledger.Nar = challan.Nar;

                int i = 0;
                var itmCnt = challan.Acc00501s.Count();

                foreach (var voucher in challan.Acc00501s)
                {
                    if (voucher.Amount > 0)
                    {
                        ledger.Acc99901s.Add(new Acc99901
                        {
                            AccCode = challan.AccCode,
                            RefCode = voucher.AccCode,
                            AcDate =  challan.AcDate,
                            Nar = voucher.Nar ?? challan.Nar,
                            Dramt =  Convert.ToInt32(challan.TransType) == 1 ? voucher.Amount : 0,
                            Cramt = Convert.ToInt32(challan.TransType) == 2 ? voucher.Amount : 0,
                        });

                        ledger.Acc99901s.Add(new Acc99901
                        {
                            AccCode = voucher.AccCode,
                            RefCode = challan.AccCode,
                            AcDate = challan.AcDate,
                            Nar = voucher.Nar ?? challan.Nar,
                            Dramt = Convert.ToInt32(challan.TransType) == 2 ? voucher.Amount : 0,
                            Cramt = Convert.ToInt32(challan.TransType) == 1 ? voucher.Amount : 0,
                        });
                    }
                    else
                    {
                        ledger.Acc99901s.Add(new Acc99901
                        {
                            AccCode = challan.AccCode,
                            RefCode = voucher.AccCode,
                            AcDate = challan.AcDate,
                            Nar = voucher.Nar ?? challan.Nar,
                            Cramt = Convert.ToInt32(challan.TransType) == 1 ? -voucher.Amount : 0,
                            Dramt = Convert.ToInt32(challan.TransType) == 2 ? -voucher.Amount : 0,
                        });

                        ledger.Acc99901s.Add(new Acc99901
                        {
                            AccCode = voucher.AccCode,
                            RefCode = challan.AccCode,
                            AcDate = challan.AcDate,
                            Nar = voucher.Nar ?? challan.Nar,
                            Cramt = Convert.ToInt32(challan.TransType) == 2 ? -voucher.Amount : 0,
                            Dramt = Convert.ToInt32(challan.TransType) == 1 ? -voucher.Amount : 0,
                        });
                    }

                    i += 1;
                }

                return ledger;

            }

            public   Acc999 receipt(Acc003 challan)
            {
                string source = @"[{'value': 1,'name': 'Cheque'},{'value': 2,'name': 'RTGS'},{'value': 3,'name': 'NEFT'},{'value': 4,'name': 'Debit Card'},{'value': 5,'name': 'Credit Card'}]";
                string output = JsonConvert.SerializeObject(source);

                var data = JsonConvert.DeserializeObject<BankTxn[]>(source);

                string? nar = null;

                if (challan.TxnType < 9)
                {
                    string txtname = data.Where(c => c.value == challan.TxnType).Select(c => c.name).SingleOrDefault();
                    nar = txtname;

                    nar = nar + (challan.TxnNo != null ? "# " + challan.TxnNo : "");
                    nar = nar + (challan.TxnDate != null ? "/" + challan.TxnDate: "");
                    nar = nar + (challan.TxnDrawnon != null ? "/" + challan.TxnDrawnon : "");


                    if (!string.IsNullOrEmpty(challan.TxnBywhome))
                        nar = nar + ((nar != string.Empty ? " " : "") + (challan.TxnBywhome != string.Empty ? " by " + challan.TxnBywhome : ""));

                    if (challan.Nar != string.Empty)
                    {
                        nar = nar + "\r\n" + challan.Nar;

                    }

                }
                else
                {
                    nar = challan.Nar;
                }

                string? linkedchallan = null;
 

                Acc999 ledger = new Acc999();
                ledger.FirmId = challan.FirmId;
                ledger.DivId = challan.DivId; 
                ledger.ChallanId = challan.VchId;
                ledger.VchType = 2;
                ledger.VchNo = challan.VchNo;
                ledger.ChallanNo = challan.ChallanNo;
                ledger.VchDate = challan.VchDate;
                ledger.Linkedchallan = linkedchallan;
                ledger.Nar = nar;

                int i = 0;

               
               
                return ledger;
        }

            public Acc999 payment(Acc002 challan)
        {

            string source = @"[{'value': 1,'name': 'Cheque'},{'value': 2,'name': 'RTGS'},{'value': 3,'name': 'NEFT'},{'value': 4,'name': 'Debit Card'},{'value': 5,'name': 'Credit Card'}]";

            string output = JsonConvert.SerializeObject(source);

            var data = JsonConvert.DeserializeObject<BankTxn[]>(source);

            string? nar = null;

            if (challan.TxnType < 9)
            {
                string txtname = data.Where(c => c.value == challan.TxnType).Select(c => c.name).SingleOrDefault();
                nar = txtname;

                nar = nar + (challan.TxnNo != null ? "# " + challan.TxnNo : "");
                nar = nar + (challan.TxnDate != null ? "/" + challan.TxnDate : "");
                nar = nar + (challan.TxnDrawnon != null ? "/" + challan.TxnDrawnon : "");


                if (!string.IsNullOrEmpty(challan.TxnBywhome))
                    nar = nar + ((nar != string.Empty ? " " : "") + (challan.TxnBywhome != string.Empty ? " by " + challan.TxnBywhome : ""));


                if (!string.IsNullOrEmpty(challan.Nar))
                    nar = nar + "\r\n" + challan.Nar;
            }

            string? linkedchallan = null;

     
            Acc999 ledger = new Acc999();

            ledger.FirmId = challan.FirmId;
            ledger.DivId = challan.DivId; 
            ledger.ChallanId = challan.VchId;
            ledger.VchType = 3;
            ledger.VchNo = challan.VchNo;
            ledger.ChallanNo = challan.ChallanNo;
            ledger.VchDate = challan.VchDate;
            ledger.Linkedchallan = linkedchallan;
             
            ledger.Nar = nar;

            int i = 0;
            
            return ledger;
        }


    }
}
   


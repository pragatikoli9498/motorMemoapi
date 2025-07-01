using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MotorMemo.Models.Context;
using MotorMemo.Models.MotorMemoEntities;
using MotorMemo.Services;
using Microsoft.Extensions.Logging;
using MotorMemo.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using dsserp.commans;
using System.Net.Mail;
using System.Text;
using MotorMemo.Models.MainDbEntities;

namespace MotorMemo.Controllers.Main
{
    [Route("[controller]/[action]")]
    [ApiController]

    public class loginController : ControllerBase
    {
       
        private MainDbContext db;

        private respayload rtn = new respayload();

        public loginController(MainDbContext context)
        {
            db = context;

        }


        [HttpGet]
        public static string generatePass()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + numbers + small_alphabets;

            int length = 10;
            string pass = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (pass.IndexOf(character) != -1);
                pass += character;
            }

            return pass;
        }


        [HttpPut]
        public async Task<ObjectResult> SendOTPs(int custid)
        {
            rtn.status_cd = 1;

            try
            {
                var User = await db.Sys00203s.SingleOrDefaultAsync();

                if (User == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "400",
                        message = "User Not Found",

                    };

                    return Ok(rtn);
                }


                string otp = dsserp.commans.otp.generateOTP();

                User.Otp = otp;
               

                db.Entry(User).State = EntityState.Modified;





                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new errors
                {
                    error_cd = ex.HResult.ToString(),
                    message = ex.Message,
                    exception = ex.InnerException
                };
            }

            return Ok(rtn);

        }


        [HttpGet]
        private respayload SendEmailOtps(string FullName, string EmailID, string OTP)
        {
            rtn.status_cd = 1;

            try
            {
                var SmtpServer = new SmtpClient();

                SmtpServer.Port = 587;
                SmtpServer.Host = "mail.dsserp.in";
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("no-reply@dsserp.com", "Zakkas@1007");


                var mail = new MailMessage();

                mail.From = new MailAddress("no-reply@dsserp.com", "DSS ERP", Encoding.UTF8);

                mail.To.Add(new MailAddress(EmailID, FullName));

                mail.Subject = "DSS ERP - Generated login Password";


                string body = string.Empty;


                body = "Dear " + FullName + "\r\n" + "\r\n";
                body = body + "Please use this Password  " + OTP + " to sign in to your application.";
                body = body + "\r\n" + "\r\n Thanks, \r\n Dynamic Super Software";
                //}

                mail.Body = body;

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return rtn;
            }

            return rtn;

        }


        [HttpPut]
        public async Task<ObjectResult> SendOTP(string user, string email)
        {
            rtn.status_cd = 1;

            try
            {
                var User = await db.Sys00203s.AsNoTracking().Where(w => w.UserLongname == user && w.EmailId == email).SingleOrDefaultAsync();

                if (User == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "400",
                        message = "User Not Found",

                    };

                    return Ok(rtn);
                }


                string otp = dsserp.commans.otp.generateOTP();

                User.Otp = otp;
                


                db.Entry(User).CurrentValues.SetValues(user);


                var chk = SendEmailOtp(User.UserLongname, User.EmailId, otp);

                await db.SaveChangesAsync();
                rtn.data = User;

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors = new errors
                {
                    error_cd = ex.HResult.ToString(),
                    message = ex.Message,
                    exception = ex.InnerException
                };
            }

            return Ok(rtn);

        }

        [HttpGet]
        public async Task<ObjectResult> usergrants(string username, string psw)
        {

            rtn.status_cd = 1;


            var s = password.EncryptPass(psw);
            var d = password.DecryptPass("SFNp9LgOZOSUnhg7vgRZxQ==");

            try
            {
                rtn.data = (await db.Sys00203s.AsNoTracking()
                    .Include(c => c.Sys00207s)
                    .Include(s => s.Sys00204)
                    .Include(s => s.Sys00201)
                        .Where(c => c.UserName == username && c.Sys00201.Password == s).ToListAsync()).AsEnumerable()
                        .Select(s => new
                        {
                            user_id = s.UserId,
                            username = s.UserName,
                            userlongname = s.UserLongname,

                            A = s.Sys00204?.A,
                            D = s.Sys00204?.D,
                            E = s.Sys00204?.E,
                            L = s.Sys00204?.L,
                            O = s.Sys00204?.O,
                            P = s.Sys00204?.P,


                            sysadmin = s.Sys00204?.Sysadmin,
                            J = s.Sys00204?.J,

                            modules = s.Sys00207s.Select(c => new { c.Id, c.ModuleId }).ToList()

                        }).SingleOrDefault();


                if (rtn.data == null)
                {

                    rtn.status_cd = 0;
                    rtn.errors.message = "Invalid username or password";


                }
            }
            catch (Exception ex)

            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);

        }




        private respayload SendEmailOtp(string FullName, string EmailID, string OTP)
        {
            rtn.status_cd = 1;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

                var SmtpServer = new SmtpClient();

                SmtpServer.Port = 587;
                SmtpServer.Host = "mail.dsserp.in";
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("no-reply@dsserp.com", "Zakkas@1007");


                var mail = new MailMessage();

                mail.From = new MailAddress("no-reply@dsserp.com", "DSS ERP", Encoding.UTF8);

                mail.To.Add(new MailAddress(EmailID, FullName));

                mail.Subject = "DSS ERP - Login OTP";

                string body = string.Empty;


                var valid_upto = DateTime.Now.AddMinutes(10).ToString("dd/MM/yyyy hh:mm tt");


                body = "Dear " + FullName + "\r\n" + "\r\n";
                body = body + "Please use this One Time Password " + OTP + ", and is valid upto " + valid_upto + " to sign in to your application.";
                body = body + "\r\n" + "\r\n Thanks, \r\n Dynamic Super Software";

                // }

                mail.Body = body;

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
                return rtn;
            }

            return rtn;

        }




        [HttpGet]
        public async Task<ObjectResult> setforgotPassword(string id)
        {
            rtn.status_cd = 1;

            try
            {
                var user = await db.Sys00203s.AsNoTracking().Where(w => w.UserName == id || w.Mobileno == id || w.EmailId == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    rtn.status_cd = 0;
                    rtn.errors.exception = BadRequest();
                    return Ok(rtn);
                }

                if (string.IsNullOrEmpty(user.EmailId))
                {

                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Email Id Not Found...Please Contact With Your Adminstrator"
                    };
                    return Ok(rtn);
                }

                var pass = generatePass();

                var profile = db.Sys00201s.Find(user.UserId);

                profile.Password = dsserp.commans.password.EncryptPass(pass);

                db.Entry(profile).State = EntityState.Modified;

                await db.SaveChangesAsync();

              

            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);
        }


        [HttpGet]
        public async Task<ObjectResult> changePsw(string username, string exist, string current, string confirm)
        {
            respayload respayload = rtn;
            try
            {

                var user = await db.Sys00201s.Where(w => w.User.UserName == username).SingleOrDefaultAsync();

                var a = user.Password;
                string passworddecoded = dsserp.commans.password.DecryptPass(a);

                if (passworddecoded == exist && current == confirm)
                {
                    db.Entry(user).State = EntityState.Modified;
                    user.Password = dsserp.commans.password.EncryptPass(current);
                }

                else
                {

                    rtn.status_cd = 0;
                    rtn.errors = new errors
                    {
                        error_cd = "404",
                        message = "Invalid username or password"
                    };

                    return Ok(rtn);
                }
                await db.SaveChangesAsync();
            }
            catch (Exception ex2)
            {
                Exception ex = ex2;
                rtn.status_cd = 0;
                rtn.errors.exception = ex;
            }
            return Ok(rtn);

        }



        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {

            return true;
        }


        public class profile
        {

            public static MainDbContext db;
            public static int checkPassword(string username, string psw)
            {


                long? id = db.Sys00203s.Where(x => x.UserName.ToLower() == username.ToLower()).Select(x => x.UserId).FirstOrDefault();

                if (id != null)
                {

                    var a = db.Sys00201s.Where(x => x.UserId == id).Select(x => x.Password).FirstOrDefault();
                    if (a != null)
                    {
                        string passworddecoded = dsserp.commans.password.DecryptPass(a);
                        if (passworddecoded == psw)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {

                    return 3;
                }
            }


        }
    }
}

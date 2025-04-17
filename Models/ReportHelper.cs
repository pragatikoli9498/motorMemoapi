using dss_reporting_services.Models;

namespace MotorMemo.Models
{
    public class ReportHelper
    {
        public static SmtpConfig getSmtpSetting()
        {
            var s = new SmtpConfig();
            s.SmtpClient = "mail.dsserp.in";
            s.SmtpPort = 587;
            s.SmtpFromMailAddress = "no-reply@dsserp.com";
            s.DisplayName = "DSS ERP";
            s.SmtpUsername = "no-reply@dsserp.com";
            s.SmtpUserPassword = "Zakkas@1007";
            return s;
        }
        public static WhatsAppConfig getWappSetting()
        {
            var s = new WhatsAppConfig();
            s.wappUrl = "";
            s.wappKey = "";
            return s;
        }
    }
}

using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Exception = System.Exception;

namespace Utility
{
    public class MailUtil
    {
        string MailHost = string.Empty;
        string MailPort = string.Empty;
        string MailEnableSSL = string.Empty;
        string MailAcc = string.Empty;
        string MailPwd = string.Empty;
        public MailUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            MailHost = config.GetValue<string>("MailSetting:Host");
            MailPort = config.GetValue<string>("MailSetting:Port");
            MailEnableSSL = config.GetValue<string>("MailSetting:EnableSSL");
            MailAcc = config.GetValue<string>("MailSetting:Account");
            MailPwd = config.GetValue<string>("MailSetting:Password");
        }

        public bool ExecuteSendMail(string iRecvUserMail, string iSubject, string iBody, 
                                    MailPriority iMailPriority = MailPriority.Normal, AlternateView? altView = null, string ShowName = null)
        {
            bool result = true;
            // 郵件伺服器
            string MailServer = MailHost;
            //Port
            string iPort = MailPort;
            // 帳號
            string MailServerAccount = MailAcc;
            // 密碼
            string MailServerPassword = MailPwd;
            // 發送人郵件
            string SourceMail = MailAcc;
            // 郵件發送人名稱
            string displayName = "學務資訊系統";

            if(!string.IsNullOrEmpty(ShowName))
                displayName = ShowName;

            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SourceMail);
                msg.To.Add(iRecvUserMail);
                msg.Headers.Add("X-System-Auth", "VALID");
                msg.Subject = iSubject;
                msg.Body = iBody;
                msg.IsBodyHtml = true;
                msg.ReplyToList.Add(new MailAddress(SourceMail, displayName));

                switch (iMailPriority)
                {
                    case MailPriority.High:
                        msg.Priority = MailPriority.High;
                        break;
                    case MailPriority.Normal:
                        msg.Priority = MailPriority.Normal;
                        break;
                    case MailPriority.Low:
                        msg.Priority = MailPriority.Low;
                        break;
                }

                // 1. 強制讓應用程式支援現今主流的 TLS 1.2/1.3 加密協定（避免被攔截降級攻擊）
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls13;

                using (SmtpClient smtp = new SmtpClient(MailServer, int.Parse(iPort)))
                {
                    // 2. MailServerPassword 請務必改填剛剛申請的「應用程式密碼」
                    smtp.Credentials = new NetworkCredential(MailServerAccount, MailServerPassword);

                    // 3. Outlook (smtp.office365.com) 的 Port 587 必須啟用 SSL/TLS
                    smtp.EnableSsl = true;

                    smtp.Send(msg);
                }

                //using (SmtpClient smtp = new SmtpClient(MailServer, int.Parse(iPort)))
                //{
                //    smtp.Credentials = new NetworkCredential(MailServerAccount, MailServerPassword);
                //    smtp.EnableSsl = true;
                //    smtp.Send(msg);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("郵件發送錯誤：" + e.ToString());
                
                result = false;
            }

            return result;
        }
    }
}

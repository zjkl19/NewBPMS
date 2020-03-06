using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NewBPMS.Repository
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string user = "eamdfan@vip.126.com";//替换成你的126用户名
            string password = "********";//替换成你的126密码   这个密码是：你设置的客户端授权密码
            string host = "smtp.vip.126.com";//设置邮件的服务器
            string mailAddress = "eamdfan@vip.126.com"; //替换成你的126账户
            string ToAddress = email;//目标邮件地址。
                                     //初始化SMTP类
            var smtp = new SmtpClient(host)
            {
                EnableSsl = true, //开启安全连接。
                Credentials = new NetworkCredential(user, password), //创建用户凭证
                DeliveryMethod = SmtpDeliveryMethod.Network //使用网络传送
            };
            //MailMessage message = new MailMessage(mailAddress, ToAddress, "我的标题", "发送内容:AAA://218.66.5.89:8300/Account/ResetPassword?userId=34aada5d-f0b1-4fd2-af87-942376cac2dc&code=CfDJ8IC4GBuw1l9AiAWplkz%2BpjqDPWF5kSVa9qfpH%2FcitOyHuV%2FMKfq7u2dnUlxBpQSHEtIS%2FMSP5Ubvd4yPaSgIs3YSkuMFMiqI4zY4C7OhgQ3xdbN1Tl%2FBoGOsUmhcijiiRnxFzcImqWRuHjEdaqd%2FG%2BVT%2B2bILZ2egfxLJAHs6Qt47ulKQ5G%2FJBDWeKVOm5bP7jA8OJPE8VcYjoX0xRSPU2kMfY14y35vk3reRmYBmx7i89frdXb5S0QxTQFiyEYCIQ%3D%3D"); //创建邮件
            var message = new MailMessage(mailAddress, ToAddress, subject, htmlMessage); //创建邮件

            //发送附加件
            NEVER_EAT_POISON_Disable_CertificateValidation();
            smtp.Send(message); //发送邮件

            return;
        }

        //[Obsolete("Do not use this in Production code!!!", true)]
        static void NEVER_EAT_POISON_Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }
    }
}

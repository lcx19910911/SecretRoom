using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using Core;
using System.Data.Entity;
using Model;

namespace Web.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);

            //使用Elapsed事件，其中timer_Elapsed就是您需要处理的事情
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Ss);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Ss(object sender, System.Timers.ElapsedEventArgs e)
        {
            string ssff = "";
         }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiveAddress">接收人邮箱</param>
        /// <param name="topic">标题</param>
        /// <param name="body">内容</param>
        /// <param name="attachmentFilePath">附件文件路径</param>
        /// <returns></returns>
        public static bool SendMail(string receiveAddress, string topic, string body, string attachmentFilePath)
        {
            if (string.IsNullOrEmpty(topic) || string.IsNullOrEmpty(body))
                return false;
            string sendAddress = "a1013688053@163.com"; ;//发件者邮箱地址
            string sendPassword = "lcx123456"; ;//发件者邮箱密码
            string[] sendUsername = sendAddress.Split('@');

            SmtpClient client = new SmtpClient("smtp." + sendUsername[1].ToString()); //设置邮件协议
            client.UseDefaultCredentials = false;//这一句得写前面
            //client.EnableSsl = true;//服务器不支持SSL连接

            client.DeliveryMethod = SmtpDeliveryMethod.Network; //通过网络发送到Smtp服务器
            client.Credentials = new NetworkCredential(sendUsername[0].ToString(), sendPassword); //通过用户名和密码 认证
            MailMessage mmsg = new MailMessage(new MailAddress(sendAddress), new MailAddress(receiveAddress)); //发件人和收件人的邮箱地址
            mmsg.Subject = "订单统计-" + topic;//邮件主题
            mmsg.SubjectEncoding = Encoding.UTF8;//主题编码
            mmsg.Body = body;//邮件正文
            mmsg.BodyEncoding = Encoding.UTF8;//正文编码
            mmsg.IsBodyHtml = true;//设置为HTML格式 
            mmsg.Priority = MailPriority.High;//优先级
            if (attachmentFilePath.Trim() != "")
            {
                mmsg.Attachments.Add(new Attachment(attachmentFilePath));//增加附件
            }
            try
            {
                client.Send(mmsg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }



}

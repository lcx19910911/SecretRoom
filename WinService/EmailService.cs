using Core;
using Excel;
using Model;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinService
{
    partial class EmailService : ServiceBase
    {
        public EmailService()
        {
            InitializeComponent();
        }
        //  定义更新计时器 600 *  1000
        private System.Timers.Timer timer = new System.Timers.Timer(60 * 1000);

        protected override void OnStart(string[] args)
        {
            //使用Elapsed事件，其中timer_Elapsed就是您需要处理的事情
            timer.Elapsed += new System.Timers.ElapsedEventHandler(SendEmail);
            timer.AutoReset = true;
            timer.Enabled = true;

        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }


        /// <summary>
        /// 获取服务器上版本消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendEmail(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                //15分钟内用的是同一个验证码

                StringBuilder sb = new StringBuilder();
                string title = "订单统计-" + DateTime.Now.ToString("yyyy-MM-dd");
                sb.AppendLine("订单在附件中");

                using (DbRepository entities = new DbRepository())
                {
                    var query = entities.Order.AsQueryable().AsNoTracking().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                    var time = DateTime.Now.Date;
                    var endTime = DateTime.Now.AddDays(1).Date;
                    query = query.Where(x => x.CreatedTime >= time && x.CreatedTime < endTime);

                    var payDic = entities.Pay.ToDictionary(x => x.ID);
                    var userDic = entities.User.ToDictionary(x => x.ID);
                    var storeDic = entities.Store.ToDictionary(x => x.ID);
                    var themeDic = entities.Theme.ToDictionary(x => x.ID);
                    var companyDic = entities.User.Where(x => x.MenuFlag == -1).ToDictionary(x => x.CompanyId);

                    var count = query.Count();
                    var list = query.ToList();


                    Excel.Application app = new Excel.Application();
                    app.SheetsInNewWorkbook = 1;
                    app.Workbooks.Add();
                    Excel.Worksheet sheet = (Excel.Worksheet)app.ActiveWorkbook.Worksheets[1];

                    sheet.Cells[1, 1] = "公司名称";
                    sheet.Cells[1, 2] = "密室名称";
                    sheet.Cells[1, 3] = "主题名称";
                    sheet.Cells[1, 4] = "总金额";
                    sheet.Cells[1, 5] = "人数";
                    sheet.Cells[1, 6] = "预约时间";
                    sheet.Cells[1, 7] = "手机";
                    sheet.Cells[1, 8] = "支付名称";
                    sheet.Cells[1, 9] = "备注";

                    int index = 2;
                    list.ForEach(x =>
                    {
                        Pay payItem = new Pay();
                        if (!string.IsNullOrEmpty(x.PayId))
                        {
                            payDic.TryGetValue(x.PayId, out payItem);
                            x.PayName = payItem?.Name;
                        }
                        User userItem;
                        userDic.TryGetValue(x.CreaterId, out userItem);
                        Store storeItem;
                        storeDic.TryGetValue(x.StoreId, out storeItem);
                        User companyItem = new User();
                        if (userItem != null)
                            companyDic.TryGetValue(userItem.CompanyId, out userItem);
                        Theme themeItem;
                        themeDic.TryGetValue(x.ThemeId, out themeItem);
                        x.CreaterName = userItem?.Name;
                        x.CompanyName = companyItem?.Name;
                        x.StoreName = storeItem?.Name;
                        x.ThemeName = themeItem?.Name;
                    });

                    list.OrderByDescending(x => x.CompanyName);

                    list.ForEach(x =>
                    {
                        sheet.Cells[index, 1] = x.CompanyName;
                        sheet.Cells[index, 2] = x.StoreName;
                        sheet.Cells[index, 3] = x.ThemeName;
                        sheet.Cells[index, 4] = x.AllMoney;
                        sheet.Cells[index, 5] = x.PeopleCount;
                        sheet.Cells[index, 6] = x.AppointmentTime;
                        sheet.Cells[index, 7] = x.Mobile;
                        sheet.Cells[index, 8] = x.PayName;
                        sheet.Cells[index, 9] = x.Remark;
                        index++;
                    });

                    app.Visible = true;
                    System.Threading.Thread.Sleep(500);
                    try
                    {
                        app.ActiveWorkbook.SaveAs(@"C:\root\crm\OrderExecle\"+DateTime.Now.ToString("yyyy-MM-dd")+".xlsx");
                    }
                    catch { }
                    app.Quit();
                }


                SendMail(Params.EmailAccount, title, sb.ToString(), @"C:\root\crm\OrderExecle\" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex);
                throw;
            }
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
            mmsg.Subject = "投贝网-" + topic;//邮件主题
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

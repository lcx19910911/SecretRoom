using Core;
using Excel;
using Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Repository;
using Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
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
        //  定义更新计时器 6000 * 60*24  一天一次
        private System.Timers.Timer timer = new System.Timers.Timer(6000 * 60*24);

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
                StringBuilder sb = new StringBuilder();
                string title = "订单统计-" + DateTime.Now.ToString("yyyy-MM-dd");
                sb.AppendLine("订单在附件中");

                using (DbRepository entities = new DbRepository())
                {
                    var query = entities.Order.AsQueryable().AsNoTracking().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                    //var time = DateTime.Now.Date;
                    //var endTime = DateTime.Now.AddDays(1).Date;
                    //query = query.Where(x => x.CreatedTime >= time && x.CreatedTime < endTime);

                    var payDic = entities.Pay.ToDictionary(x => x.ID);
                    var userDic = entities.User.ToDictionary(x => x.ID);
                    var storeDic = entities.Store.ToDictionary(x => x.ID);
                    var themeDic = entities.Theme.ToDictionary(x => x.ID);
                    var companyDic = entities.User.Where(x => x.MenuFlag == -1).ToDictionary(x => x.CompanyId);

                    var list = query.ToList();

                    var newList = new List<OrderExecle>();
                    list.ForEach(x =>
                    {
                        Pay payItem = new Pay();
                        if (!string.IsNullOrEmpty(x.PayId))
                        {
                            payDic.TryGetValue(x.PayId, out payItem);
                            x.PayName = payItem?.Name;
                        }
                        Pay secondPayItem = new Pay();
                        if (!string.IsNullOrEmpty(x.SecondPayId))
                        {
                            payDic.TryGetValue(x.SecondPayId, out secondPayItem);
                            x.SecondPayName = secondPayItem?.Name;
                        }
                        User userItem;
                        userDic.TryGetValue(x.CreaterId, out userItem);
                        Store storeItem;
                        storeDic.TryGetValue(x.StoreId, out storeItem);
                        User companyItem = new User();
                        if (userItem != null)
                            companyDic.TryGetValue(userItem.CompanyId, out companyItem);
                        Theme themeItem;
                        themeDic.TryGetValue(x.ThemeId, out themeItem);
                        x.CreaterName = userItem?.Name;
                        x.CompanyName = companyItem?.Name;
                        x.StoreName = storeItem?.Name;
                        x.ThemeName = themeItem?.Name;

                        newList.Add(new OrderExecle()
                        {
                            CompanyName = x.CompanyName,
                            StoreName = x.StoreName,
                            ThemeName = x.ThemeName,
                            AppointmentTime = x.AppointmentTime,
                            PayName = x.PayName,
                            SecondPayName = x.SecondPayName,
                            Money = x.Money,
                            DrinkMoney = x.DrinkMoney,
                            AllMoney = x.AllMoney,
                            PeopleCount = x.PeopleCount,
                            Mobile = x.Mobile,
                            IsPlay = x.IsPlay == YesOrNoCode.Yes ? "已玩过" : "未玩过",
                            StartTime = x.StartTime,
                            OverTime = x.OverTime,
                        });

                    });

                    Hashtable hs = new Hashtable();
                    hs["CompanyName"] = "公司名称";
                    hs["StoreName"] = "密室名称";
                    hs["ThemeName"] = "主题名称";
                    hs["AppointmentTime"] = "预约时间";
                    hs["PayName"] = "支付名称";
                    hs["SecondPayName"] = "第二支付名称";
                    hs["Money"] = "补差额";
                    hs["DrinkMoney"] = "饮料消费";
                    hs["AllMoney"] = "总额";
                    hs["PeopleCount"] = "人数";
                    hs["Mobile"] = "手机号";
                    hs["IsPlay"] = "是否玩过";
                    hs["StartTime"] = "游戏开始时间";
                    hs["OverTime"] = "游戏结束时间";

                    Helper.ExcelHelper<OrderExecle>.getExcel(newList, hs, @"C:\root\crm\OrderExecle\" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
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

        public static void RenderToExcel<T>(List<T> datas)
        {
            MemoryStream ms = new MemoryStream();
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("导出数据");
            IRow headerRow = sheet.CreateRow(0);

            int rowIndex = 1, piIndex = 0;
            Type type = typeof(T);
            PropertyInfo[] pis = type.GetProperties();
            int pisLen = pis.Length - 2;//减2是多了2个外键引用  
            PropertyInfo pi = null;
            string displayName = string.Empty;
            while (piIndex < pisLen)
            {
                pi = pis[piIndex];
                displayName = "";
                //ExcelService.GetDisplayName(type, pi.Name);
                if (!displayName.Equals(string.Empty))
                {//如果该属性指定了DisplayName，则输出  
                    try
                    {
                        headerRow.CreateCell(piIndex).SetCellValue(displayName);
                    }
                    catch (Exception)
                    {
                        headerRow.CreateCell(piIndex).SetCellValue("");
                    }
                }
                piIndex++;
            }
            foreach (T data in datas)
            {
                piIndex = 0;
                IRow dataRow = sheet.CreateRow(rowIndex);
                while (piIndex < pisLen)
                {
                    pi = pis[piIndex];
                    try
                    {
                        dataRow.CreateCell(piIndex).SetCellValue(pi.GetValue(data, null).ToString());
                    }
                    catch (Exception)
                    {
                        dataRow.CreateCell(piIndex).SetCellValue("");
                    }
                    piIndex++;
                }
                rowIndex++;
            }
            workbook.Write(ms);
            FileStream dumpFile = new FileStream(@"C:\root\crm\OrderExecle" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx", FileMode.Create, FileAccess.ReadWrite);
            ms.WriteTo(dumpFile);
            ms.Flush();
            ms.Position = 0;
            dumpFile.Close();
        }
    }
}

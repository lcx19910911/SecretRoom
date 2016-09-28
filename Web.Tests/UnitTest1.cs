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
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using System.Reflection;
using NPOI.HSSF.UserModel;
using Helper;
using System.Collections;

namespace Web.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {

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
                var drinkDic = entities.Drink.ToDictionary(x => x.ID);
                var companyDic = entities.User.Where(x => x.MenuFlag == -1).ToDictionary(x => x.CompanyId);

                var list = query.ToList();

                var newList = new List<OrderExecle>();
                var drinkList = new List<DrinkExecle>();
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
                        IsPlay = x.IsPlay==YesOrNoCode.Yes?"已玩过":"未玩过",
                        StartTime = x.StartTime,
                        OverTime = x.OverTime,
                    });
                    if (!string.IsNullOrEmpty(x.DrinkJsonStr)&&!x.DrinkJsonStr.Equals("[]"))
                    {
                        var orderDrinkList = JsonExtensions.DeserializeJson<List<DrinkExecle>>(x.DrinkJsonStr);
                        orderDrinkList.ForEach(y =>
                        {
                            y.AllName = x.CompanyName+"-"+ x.StoreName;
                        });
                        drinkList.AddRange(orderDrinkList);
                    }
                });


                //Hashtable hs = new Hashtable();
                //hs["CompanyName"] = "公司名称";
                //hs["StoreName"] = "密室名称";
                //hs["ThemeName"] = "主题名称";
                //hs["AppointmentTime"] = "预约时间";
                //hs["PayName"] = "支付名称";
                //hs["SecondPayName"] = "第二支付名称";
                //hs["Money"] = "补差额";
                //hs["DrinkMoney"] = "饮料消费";
                //hs["AllMoney"] = "总额";
                //hs["PeopleCount"] = "人数";
                //hs["Mobile"] = "手机号";
                //hs["IsPlay"] = "是否玩过";
                //hs["StartTime"] = "游戏开始时间";
                //hs["OverTime"] = "游戏结束时间";

               //Helper.ExcelHelper<OrderExecle>.getExcel(newList, hs, @"C:\root\crm\OrderExecle\" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");

                Hashtable drinkHs = new Hashtable();
                drinkHs["CompanyName"] = "公司名称";
                drinkHs["StoreName"] = "密室";
                drinkHs["Name"] = "饮料名";
                drinkHs["Money"] = "单价";
                drinkHs["Count"] = "数量";

                List<DrinkModel> List = new List<DrinkModel>();

                var companyIdList = companyDic.Keys.ToList();

                var drinkIdList = entities.Drink.Where(x => companyIdList.Contains(x.CompanyId)).Select(x => x.ID).ToList();
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
                HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;


                bool h = false;
                int j = 1;
                int companyIndex = 0;
                companyIdList.ForEach(x =>
                {
                    HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;
                    var companyName = companyDic[x].Name;
                    headerRow.CreateCell(companyIndex).SetCellValue(companyName);


                });


                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                foreach (T item in lists)
                {
                    HSSFRow dataRow = sheet.CreateRow(j) as HSSFRow;
                    int i = 0;
                    foreach (PropertyInfo column in properties)
                    {
                        if (!h)
                        {
                            headerRow.CreateCell(i).SetCellValue(head[column.Name] == null ? column.Name : head[column.Name].ToString());
                            dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                        }
                        else
                        {
                            dataRow.CreateCell(i).SetCellValue(column.GetValue(item, null) == null ? "" : column.GetValue(item, null).ToString());
                        }

                        i++;
                    }
                    h = true;
                    j++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet = null;
                headerRow = null;
                workbook = null;
                FileStream fs = new FileStream(workbookFile, FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
               


                List<DrinkModel> dic = new List<DrinkModel>();
                companyNameList.ForEach(x =>
                {
                    decimal allMoneySum = 0;
                    var nameList = drinkList.Where(y => y.AllName.Equals(x)).ToList();
                   var ddd=nameList.GroupBy(y => y.Name).Select(y => {
                        var count = y.Sum(z => z.Count);
                        var money = y.FirstOrDefault().Money;
                        var allMoney = count * money;
                       allMoneySum += allMoney;
                       return new Tuple<string, decimal, int, decimal>(y.Key, money, count, allMoney);
                        }).ToList();

                    dic.Add(new DrinkModel()
                    {
                        AllName = x,
                        List = ddd,
                        AllMoney = allMoneySum
                    });
                });
                
            }
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

    public class DrinkModel
    {
        public string CompanyName { get; set; }

        public List<Tuple<string, decimal, int, decimal>> List = new List<Tuple<string, decimal, int, decimal>>();

        public decimal AllMoney { get; set; }
    }

}

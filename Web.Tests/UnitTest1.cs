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
using NPOI.SS.Util;

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
                            y.CompanyId = x.CompanyId;
                            y.CompanyName = x.CompanyName;
                        });
                        drinkList.AddRange(orderDrinkList);
                    }
                });
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                int rowIndex = 1;
                headerRow.CreateCell(0).SetCellValue("公司名称");
                headerRow.CreateCell(1).SetCellValue("密室名称");
                headerRow.CreateCell(2).SetCellValue("主题名称");
                headerRow.CreateCell(3).SetCellValue("预约时间");
                headerRow.CreateCell(4).SetCellValue("支付名称");
                headerRow.CreateCell(5).SetCellValue("第二支付名称");
                headerRow.CreateCell(6).SetCellValue("补差额");
                headerRow.CreateCell(7).SetCellValue("饮料消费");
                headerRow.CreateCell(8).SetCellValue("总额");
                headerRow.CreateCell(9).SetCellValue("人数");
                headerRow.CreateCell(10).SetCellValue("手机号");
                headerRow.CreateCell(11).SetCellValue("是否玩过");
                headerRow.CreateCell(12).SetCellValue("游戏开始时间");
                headerRow.CreateCell(13).SetCellValue("游戏结束时间");

                newList.ForEach(x =>
                {

                    IRow dataRow = sheet.CreateRow(rowIndex);

                    dataRow.CreateCell(0).SetCellValue(x.CompanyName);
                    dataRow.CreateCell(1).SetCellValue(x.StoreName);
                    dataRow.CreateCell(2).SetCellValue(x.ThemeName);
                    dataRow.CreateCell(3).SetCellValue(x.AppointmentTime);
                    dataRow.CreateCell(4).SetCellValue(x.PayName);
                    dataRow.CreateCell(5).SetCellValue(x.SecondPayName);
                    dataRow.CreateCell(6).SetCellValue(x.Money != null ? x.Money.ToString() : "");
                    dataRow.CreateCell(7).SetCellValue(x.DrinkMoney != null ? x.DrinkMoney.ToString() : "");
                    dataRow.CreateCell(8).SetCellValue(x.AllMoney.ToString());
                    dataRow.CreateCell(9).SetCellValue(x.PeopleCount);
                    dataRow.CreateCell(10).SetCellValue(x.Mobile);
                    dataRow.CreateCell(11).SetCellValue(x.IsPlay);
                    dataRow.CreateCell(12).SetCellValue(x.StartTime != null ? x.StartTime.ToString() : "");
                    dataRow.CreateCell(13).SetCellValue(x.OverTime != null ? x.OverTime.ToString() : "");

                    rowIndex++;
                });

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                FileStream fs = new FileStream(@"C:\root\crm\OrderExecle\订单统计-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx", FileMode.Create, FileAccess.Write);
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
            

                var companyIdList = companyDic.Keys.ToList();

                workbook = new HSSFWorkbook();
                sheet = workbook.CreateSheet();
                rowIndex = 0;

                companyIdList.ForEach(x =>
                {

                    decimal allMoneySum = 0;
                    int allCount = 0;
                    var companyName = companyDic[x].Name;

                    //卖出去的饮料名集合
                    var nameList = drinkList.Where(y => y.CompanyId.Equals(x)).ToList();
                    var drinkInfoList = nameList.GroupBy(y => y.ID).Select(y => {
                        var count = y.Sum(z => z.Count);
                        var money = y.FirstOrDefault().Money;
                        var allMoney = count * money;
                        allMoneySum += allMoney;
                        allCount += count;
                        return new Tuple<string, int, decimal>(y.Key, count, allMoney);
                    }).ToList();
                    
                    headerRow = sheet.CreateRow(rowIndex);
                    headerRow.CreateCell(0).SetCellValue("公司名称");
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 1, drinkInfoList.Count + 1));
                    headerRow.CreateCell(1).SetCellValue(companyName);
                    rowIndex++;
                    IRow namedataRow = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    IRow moneydataRow = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    IRow countdataRow = sheet.CreateRow(rowIndex);
                    rowIndex++;
                    IRow AllMoneydataRow = sheet.CreateRow(rowIndex);
                    rowIndex++;


                    namedataRow.CreateCell(0).SetCellValue("饮料名称");
                    moneydataRow.CreateCell(0).SetCellValue("单价（元）");
                    countdataRow.CreateCell(0).SetCellValue("数量");
                    AllMoneydataRow.CreateCell(0).SetCellValue("总计");

                    var drinkIdList = entities.Drink.Where(y => y.CompanyId.Equals(x)).ToList();
                    int index = 1;
                    if (drinkInfoList!=null&&drinkInfoList.Count != 0)
                    {
                        drinkIdList.ForEach(y =>
                        {
                            namedataRow.CreateCell(index).SetCellValue(y.Name);
                            moneydataRow.CreateCell(index).SetCellValue(y.Money.ToString());
                            var item = drinkInfoList.Where(z => z.Item1.Equals(y.ID)).FirstOrDefault();
                            if (item != null)
                            {
                                countdataRow.CreateCell(index).SetCellValue(item.Item2.ToString());
                                AllMoneydataRow.CreateCell(index).SetCellValue(item.Item3.ToString());
                            }
                            else
                            {
                                countdataRow.CreateCell(index).SetCellValue("0");
                                AllMoneydataRow.CreateCell(index).SetCellValue("0");
                            }
                            index++;
                        });
                        namedataRow.CreateCell(index).SetCellValue("总额");
                        moneydataRow.CreateCell(index).SetCellValue("");
                        countdataRow.CreateCell(index).SetCellValue(allCount.ToString());
                        AllMoneydataRow.CreateCell(index).SetCellValue(allMoneySum.ToString());
                    }
                    IRow emptydataRow = sheet.CreateRow(rowIndex);
                    rowIndex++;
                });

                ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet = null;
                headerRow = null;
                workbook = null;
                fs = new FileStream(@"C:\root\crm\DrinkExecle\饮料统计-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx", FileMode.Create, FileAccess.Write);
                data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                data = null;
                ms = null;
                fs = null;
            }

            SendMail(Params.EmailAccount, "1", "22", @"C:\root\crm\OrderExecle\订单统计-" + DateTime.Now.ToString("yyyy-MM-dd") + @".xlsx,C:\root\crm\DrinkExecle\饮料统计-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
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
                var list = attachmentFilePath.Split(',');
                foreach (var item in list)
                {
                    if(!string.IsNullOrEmpty(item))
                    mmsg.Attachments.Add(new Attachment(item));//增加附件
                }
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

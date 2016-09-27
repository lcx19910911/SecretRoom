using Core;
using Core.Model;
using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public partial class WebService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public List<Order> Get_OrderList(DateTime? searchTime, string storeId)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Order.AsQueryable().AsNoTracking().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (storeId.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.StoreId.Equals(storeId));
                }

                if (searchTime != null)
                {
                    var time = searchTime.Value.Date;
                    var endTime = searchTime.Value.AddDays(1).Date;
                    query = query.Where(x => x.CreatedTime >= time && x.CreatedTime < endTime);
                }
                var payDic = entities.Pay.Where(x => x.StoreId.Equals(storeId)).ToDictionary(x => x.ID);
                var userDic = entities.User.Where(x => x.CompanyId.Equals(Client.LoginUser.CompanyId)).ToDictionary(x => x.ID);
                var count = query.Count();
                var list = query.ToList();
                list.ForEach(x =>
                {
                    Pay payItem;
                    if (!string.IsNullOrEmpty(x.PayId))
                    {
                        payDic.TryGetValue(x.PayId, out payItem);
                        x.PayName = payItem?.Name;
                    }
                    User userItem;
                    userDic.TryGetValue(x.CreaterId, out userItem);
                    x.CreaterName = userItem?.Name;
                });
                return list;
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Add_Order(Order model)
        {
           
            using (DbRepository entities = new DbRepository())
            {
                var pay = entities.Pay.AsQueryable().AsNoTracking().Where(x=>x.ID.Equals(model.PayId)).FirstOrDefault();
                var secendPay = entities.Pay.AsQueryable().AsNoTracking().Where(x => x.ID.Equals(model.SecondPayId)).FirstOrDefault();
                var theme = entities.Theme.AsQueryable().AsNoTracking().Where(x => x.ID.Equals(model.ThemeId)).FirstOrDefault();
                if (theme == null)
                    return Result(false, ErrorCode.sys_param_format_error);
                var store = entities.Store.AsQueryable().AsNoTracking().Where(x => x.ID.Equals(model.StoreId)).FirstOrDefault();
                if (store == null)
                    return Result(false, ErrorCode.sys_param_format_error);
                model.AllMoney = (pay!=null? pay.RealMoney:0)+ (secendPay != null ? secendPay.RealMoney : 0) + (model.Money==null?0: model.Money.Value) + (model.DrinkMoney == null ? 0 : model.DrinkMoney.Value);
                model.ID = Guid.NewGuid().ToString("N");
                model.CreaterId = Client.LoginUser.ID;
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;
                model.IsPlay = YesOrNoCode.No;
                model.CompanyId = Client.LoginUser.CompanyId;
                entities.Order.Add(model);
                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }

        }

        /// <summary>
        /// 开始订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<string> Start_Order(string id)
        {

            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Order.Find(id);
                if (oldEntity != null)
                {
                    var theme = entities.Theme.Where(x => x.ID.Equals(oldEntity.ThemeId)).FirstOrDefault();
                    if(theme==null)
                        return Result("", ErrorCode.sys_param_format_error);
                    oldEntity.IsPlay = YesOrNoCode.Yes;
                    oldEntity.StartTime = DateTime.Now;
                    oldEntity.OverTime = DateTime.Now.AddMinutes(theme.GameMinute);
                    oldEntity.UpdatedTime = DateTime.Now;
                }
                else
                    return Result("", ErrorCode.sys_param_format_error);
                
                return entities.SaveChanges() > 0 ? Result(oldEntity.OverTime.Value.ToString("yyyy/MM/dd HH:mm:ss")) : Result("", ErrorCode.sys_fail);
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update_Order(Order model)
        {
            using (DbRepository entities = new DbRepository())
            {
                var pay = entities.Pay.AsQueryable().AsNoTracking().Where(x => x.ID.Equals(model.PayId)).FirstOrDefault();
                var secendPay = entities.Pay.AsQueryable().AsNoTracking().Where(x => x.ID.Equals(model.SecondPayId)).FirstOrDefault();
                var oldEntity = entities.Order.Find(model.ID);
                if (oldEntity != null)
                {
                    oldEntity.AllMoney = (pay != null ? pay.RealMoney : 0) + (secendPay != null ? secendPay.RealMoney : 0) + (model.Money == null ? 0 : model.Money.Value) + (model.DrinkMoney == null ? 0 : model.DrinkMoney.Value);
                    oldEntity.Money = model.Money;
                    oldEntity.Mobile = model.Mobile;
                    oldEntity.PeopleCount = model.PeopleCount;
                    oldEntity.ThemeId = model.ThemeId;
                    oldEntity.PayId = model.PayId;
                    oldEntity.Remark = model.Remark;
                    oldEntity.AppointmentTime = model.AppointmentTime;
                    oldEntity.UpdatedTime = DateTime.Now;
                    oldEntity.SecondPayId = model.SecondPayId;
                    oldEntity.DrinkMoney = model.DrinkMoney;
                    oldEntity.DrinkJsonStr = model.DrinkJsonStr;
                }
                else
                    return Result(false, ErrorCode.sys_param_format_error);

                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public WebResult<bool> Delete_Order(string ids)
        {
            if (!ids.IsNotNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Order.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
                {
                    x.Flag = x.Flag | (long)GlobalFlag.Removed;
                });
                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }
        }


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order Find_Order(string id)
        {
            if (!id.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Order.Find(id);
                return entity;
            }
        }

        /// <summary>
        /// 查找该客户预约的订单记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Find_MobileOrder(string mobile,string storeId)
        {
            if (!mobile.IsNotNullOrEmpty()&& mobile.Length!=11)
                return 0;
            using (DbRepository entities = new DbRepository())
            {
                return entities.Order.Where(x=>x.Mobile.Equals(mobile)&&x.StoreId.Equals(storeId)&&x.Flag==(int)GlobalFlag.Normal&&x.IsPlay==YesOrNoCode.Yes).Count();
            }
        }

        public WebResult<ReportModel> GetReport(int index, string storeId, DateTime start, DateTime end)
        {
            var model = new ReportModel();
            string storeName = string.Empty;
            StringBuilder html = new StringBuilder();
            using (DbRepository entities = new DbRepository())
            {
                end = end.AddDays(1);
                var orderList = entities.Order.AsNoTracking().Where(x => x.StoreId.Equals(storeId) && x.CreatedTime >= start && x.CreatedTime <= end && x.Flag == 0).ToList();
                //var orderList = entities.Order.AsNoTracking().Where(x =>  x.CreatedTime > start && x.CreatedTime < end && x.Flag == 0).ToList();
                storeName = entities.Store.AsNoTracking().Where(x=>x.ID.Equals(storeId)).Select(x => x.Name).FirstOrDefault();

                if (index == 1)
                {
                    var orderGroupList = orderList.GroupBy(x => x.CreatedTime.Date).Select(x =>
                        new OrderTotal()
                        {
                            Date = x.Key,
                            OrderCount = x.Count(),
                            PeopleCount = x.Sum(y => y.PeopleCount),
                            Money = x.Sum(y => y.AllMoney)
                        }
                    ).OrderBy(x => x.Date).ToList();


                    orderGroupList.ForEach(x =>
                    {
                        html = new StringBuilder();
                        html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", x.Date.ToString("yyyy-MM-dd"), x.OrderCount, x.PeopleCount, x.Money);
                        model.tbody = string.Format("{0}{1}", model.tbody, html);
                    });
                    model.time = new List<string>();
                    model.series = new List<Series>();
                    var oneSeries = new Series();
                    
                    for (var i = start.Date; i <= end.Date; i = i.AddDays(1))
                    {
                        model.time.Add(i.Date.ToString("yyyy-MM-dd"));

                    }
                    oneSeries.name = storeName;
                    oneSeries.data = new List<decimal>();

                    for (var i = start.Date; i <end.Date; i = i.AddDays(1))
                    {
                        oneSeries.data.Add(orderGroupList.Where(x => x.Date.Equals(i)).Select(x => x.Money).FirstOrDefault());
                    }

                    model.series.Add(oneSeries);
                }
                else if (index == 2)
                {
                    var themeList = entities.Theme.AsNoTracking().Where(x => x.StoreId.Equals(storeId)).ToList();
                    var themeReportList = orderList.GroupBy(x => x.ThemeId).Select(x =>
                        new ThemeTotal()
                        {
                            ThemeId = x.Key,
                            ThemeName = themeList.Where(y => y.ID.Equals(x.Key)).Select(y => y.Name).FirstOrDefault(),
                            Money = x.Sum(y => y.AllMoney),
                            OrderCount = x.Count(),
                            PeopleCount= x.Sum(y => y.PeopleCount)
                        }
                    ).ToList();
                    model.pieseries = new List<PieSeries>();
                    var oneSeries = new PieSeries();

                    oneSeries.name = "主题占比";
                    oneSeries.type = "pie";
                    oneSeries.data = new List<Series>();

                    themeReportList.ForEach(x =>
                    {
                        html = new StringBuilder();
                        html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", x.ThemeName, storeName ,x.OrderCount, x.PeopleCount, x.Money);
                        model.tbody = string.Format("{0}{1}", model.tbody, html);

                        oneSeries.data.Add(new Series()
                        {
                            y = x.Money,
                            name= x.ThemeName
                        });
                    });


                    model.pieseries.Add(oneSeries);
                }

                else if (index == 3)
                {
                    var payList = entities.Pay.AsNoTracking().Where(x => x.StoreId.Equals(storeId)).ToList();
                    var payReportList = orderList.GroupBy(x => x.PayId).Select(x =>
                        new PayTotal()
                        {
                            PayId = x.Key,
                            PayName = payList.Where(y => y.ID.Equals(x.Key)).Select(y => y.Name).FirstOrDefault(),
                            Money = x.Sum(y => y.AllMoney),
                            OrderCount = x.Count(),
                            PeopleCount = x.Sum(y => y.PeopleCount)
                        }
                    ).ToList();
                    model.pieseries = new List<PieSeries>();
                    var oneSeries = new PieSeries();

                    oneSeries.name = "支付占比";
                    oneSeries.type = "pie";
                    oneSeries.data = new List<Series>();

                    payReportList.ForEach(x =>
                    {
                        html = new StringBuilder();
                        html.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", x.PayName, storeName, x.OrderCount, x.PeopleCount, x.Money);
                        model.tbody = string.Format("{0}{1}", model.tbody, html);

                        oneSeries.data.Add(new Series()
                        {
                            y = x.Money,
                            name = x.PayName
                        });
                    });


                    model.pieseries.Add(oneSeries);
                }


                return Result(model);
            }
        }
    }
}

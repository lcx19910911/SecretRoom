using Core;
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
        /// <param name="provinceName">省份名 - 搜索项</param>
        /// <param name="cityName">城市名 - 搜索项</param>
        /// <param name="phone">手机号- 搜索项</param>
        /// <param name="startTime">营业开始时间 - 搜索项</param>
        /// <param name="endTime">营业结束时间 - 搜索项</param>
        /// <returns></returns>
        public WebResult<PageList<Store>> Get_StorePageList(int pageIndex, int pageSize, string name,string provinceName,string cityName,string phone,string startTime, string endTime)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Store.AsQueryable().AsNoTracking().Where(x=>x.UserId.Equals(Client.LoginUser.ID) && (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                if (provinceName.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Province.Contains(provinceName));
                }
                if (cityName.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.City.Contains(cityName));
                }
                if (phone.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Mobile.Contains(phone));
                }
                var list = query.ToList();
                var returnList = new List<Store>();
                bool isTrue = false;
                list.ForEach(x =>
                {

                    if (startTime.IsNotNullOrEmpty())
                    {
                        if (DateTime.Parse(x.StartTime) >= DateTime.Parse(startTime))
                            isTrue = true;
                        else
                            isTrue = false;
                    }
                    if (endTime.IsNotNullOrEmpty())
                    {
                        if (DateTime.Parse(x.EndTime) <= DateTime.Parse(endTime))
                            isTrue = true;
                        else
                            isTrue = false;
                    }
                    if (isTrue)
                        returnList.Add(x);
                });
                var count = query.Count();

                if (startTime.IsNotNullOrEmpty()|| endTime.IsNotNullOrEmpty())
                    returnList = returnList.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                else
                    returnList = list.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return ResultPageList(returnList, pageIndex, pageSize,count);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Add_Store(Store model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            if (DateTime.Parse(model.StartTime) > DateTime.Parse(model.EndTime))
                Result(false, ErrorCode.time_not_legal);
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Store.AsQueryable();
                if (entities.Store.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.UserId.Equals(Client.LoginUser.ID)).Any())
                    return Result(false, ErrorCode.datadatabase_name_had);
                model.ID = Guid.NewGuid().ToString("N");
                model.UserId = Client.LoginUser.ID;
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;

                var limitFlags = entities.Store.Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0).Select(x => x.LimitFlag==0?0: x.LimitFlag).ToList();
                var limitFlagAll = 0L;
                // 获取所有角色位值并集
                limitFlags.ForEach(x => limitFlagAll |= x);
                var limitFlag = 0L;
                // 从低位遍历是否为空
                for (var i = 0; i < 64; i++)
                {
                    if ((limitFlagAll & (1 << i)) == 0)
                    {
                        limitFlag = 1 << i;
                        break;
                    }
                }
                model.LimitFlag = limitFlag;
                entities.Store.Add(model);
                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update_Store(Store model)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            if (DateTime.Parse(model.StartTime) > DateTime.Parse(model.EndTime))
                Result(false, ErrorCode.time_not_legal);
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Store.Find(model.ID);
                if (oldEntity != null)
                {
                    if (!model.Name.Equals(oldEntity.Name))
                    {
                        if (entities.Pay.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.UserId.Equals(Client.LoginUser.ID)).Any())
                            return Result(false, ErrorCode.datadatabase_name_had);
                    }
                    oldEntity.Logo = model.Logo;
                    oldEntity.Name = model.Name;
                    oldEntity.Mobile = model.Mobile;
                    oldEntity.OrderSpaceMinute = model.OrderSpaceMinute;
                    oldEntity.Province = model.Province;
                    oldEntity.City = model.City;
                    oldEntity.StartTime = model.StartTime;
                    oldEntity.EndTime = model.EndTime;
                    oldEntity.UpdatedTime = DateTime.Now;
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
        public WebResult<bool> Delete_Store(string ids)
        {
            if (!ids.IsNotNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Store.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
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
        public Store Find_Store(string id)
        {
            if (!id.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Store.Find(id);
                return entity;
            }
        }


        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="">门店id</param>
        /// <returns></returns>
        public List<SelectItem> Get_StoreSelectItem(string storeId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Store.AsNoTracking().ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Selected = x.ID.Equals(storeId),
                        Text = x.Name,
                        Value = x.ID
                    });
                });
                return list;

            }
        }

        /// <summary>
        /// 获取ZTree子节点
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="groups">分组数据</param>
        /// <returns></returns>
        public List<ZTreeNode> Get_StoreZTree(long flag)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<ZTreeNode> ztreeNodes = new List<ZTreeNode>();
                var user = new User();
                if (this.Client.LoginUser.MenuFlag == -1)
                    user = CookieHelper.GetCurrentUser();
                else
                    user = entities.User.Where(x => x.CompanyId.Equals(this.Client.LoginUser.CompanyId) && x.MenuFlag == -1).FirstOrDefault();
                ztreeNodes = entities.Store.Where(x => x.UserId.Equals(user.ID) && (x.Flag & (long)GlobalFlag.Normal) == 0).Select(
                    x => new ZTreeNode()
                    {
                        name = x.Name,
                        value = x.LimitFlag.ToString(),
                        ischeck=(x.LimitFlag&flag)!=0,
                        nocheck=false
                    }).ToList();
                return ztreeNodes;
            }
        }
    }
}

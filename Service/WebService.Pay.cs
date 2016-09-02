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
        public WebResult<PageList<Pay>> Get_PayPageList(int pageIndex, int pageSize, string name,string no)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Pay.AsQueryable().AsNoTracking().Where(x=>x.UserId.Equals(Client.LoginUser.ID));
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                if (no.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NO.Contains(no));
                }
              
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return ResultPageList(list, pageIndex, pageSize);
            }
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Add_Pay(Pay model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Store.AsQueryable();
                if (entities.Pay.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.UserId.Equals(Client.LoginUser.ID)).Any())
                    return Result(false, ErrorCode.datadatabase_name_had);
                model.ID = Guid.NewGuid().ToString("N");
                model.UserId = Client.LoginUser.ID;
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;            
                entities.Pay.Add(model);
                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update_Pay(Pay model)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                if (entities.Pay.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.UserId.Equals(Client.LoginUser.ID)).Any())
                    return Result(false, ErrorCode.datadatabase_name_had);
                var oldEntity = entities.Pay.Find(model.ID);
                if (oldEntity != null)
                {

                    oldEntity.RealMoney = model.RealMoney;
                    oldEntity.NO = model.NO;
                    oldEntity.Name = model.Name;
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
        public WebResult<bool> Delete_Pay(string ids)
        {
            if (!ids.IsNotNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Pay.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
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
        public Pay Find_Pay(string id)
        {
            if (!id.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Pay.Find(id);
                return entity;
            }
        }


        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="">门店id</param>
        /// <returns></returns>
        public List<SelectItem> Get_PaySelectItem(string id)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Pay.AsNoTracking().ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Selected = x.ID.Equals(id),
                        Text = x.Name,
                        Value = x.ID
                    });
                });
                return list;

            }
        }
    }
}

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
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public WebResult<PageList<Pay>> Get_PayPageList(int pageIndex, int pageSize, string name,string no)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Pay.AsQueryable().AsNoTracking().Where(x=>x.CompanyId.Equals(Client.LoginUser.CompanyId) &&(x.Flag&(long)GlobalFlag.Removed)==0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                if (no.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NO.Contains(no));
                }
                var storeDic = entities.Store.Where(x => x.UserId.Equals(Client.LoginUser.ID)).ToDictionary(x=>x.ID);
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                list.ForEach(x =>
                {
                    Store stroreItem;
                    storeDic.TryGetValue(x.StoreId, out stroreItem);
                    x.StoreName = stroreItem?.Name;
                    x.State = EnumHelper.GetEnumDescription((GlobalFlag)x.Flag);
                });
                return ResultPageList(list, pageIndex, pageSize, count);
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
                if (entities.Pay.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.StoreId.Equals(model.StoreId)).Any())
                    return Result(false, ErrorCode.datadatabase_name_had);
                if (entities.Pay.AsNoTracking().Where(x => x.NO.Equals(model.NO) && x.StoreId.Equals(model.StoreId)).Any())
                    return Result(false, ErrorCode.datadatabase_no_had);
                model.ID = Guid.NewGuid().ToString("N");
                model.CompanyId = Client.LoginUser.CompanyId;
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
                var oldEntity = entities.Pay.Find(model.ID);
                if (oldEntity != null)
                {
                    if (entities.Pay.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.StoreId.Equals(model.StoreId)&&!x.ID.Equals(model.ID)).Any())
                        return Result(false, ErrorCode.datadatabase_name_had);
                    if (entities.Pay.AsNoTracking().Where(x => x.NO.Equals(model.NO) && x.StoreId.Equals(model.StoreId) && !x.ID.Equals(model.ID)).Any())
                        return Result(false, ErrorCode.datadatabase_no_had);
                    oldEntity.RealMoney = model.RealMoney;
                    oldEntity.NO = model.NO;
                    oldEntity.Name = model.Name;
                    oldEntity.StoreId = model.StoreId;
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
        /// 启用
        /// </summary>
        /// <param name="ids">id，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public WebResult<bool> Enable_Pay(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = ids.Split(',');

                entities.Pay.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
                {
                    x.Flag = x.Flag & ~(long)GlobalFlag.Unabled;
                });

                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }
        }


        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="ids">ids，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public WebResult<bool> Disable_Pay(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = ids.Split(',');

                entities.Pay.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
                {
                    x.Flag = x.Flag | (long)GlobalFlag.Unabled;
                });

                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
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
                entities.Pay.Where(x=>string.IsNullOrEmpty(id)?1==1:(x.StoreId.Equals(id)&&x.Flag==0)).AsNoTracking().OrderBy(x => x.CreatedTime).ToList().ForEach(x =>
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

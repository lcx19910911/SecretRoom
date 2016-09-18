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
        public WebResult<PageList<Theme>> Get_ThemePageList(int pageIndex, int pageSize, string name, string no)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Theme.AsQueryable().AsNoTracking().Where(x => x.UserId.Equals(Client.LoginUser.ID) && (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                if (no.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NO.Contains(no));
                }
                var storeDic = entities.Store.Where(x => x.UserId.Equals(Client.LoginUser.ID)).ToDictionary(x => x.ID);
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
        public WebResult<bool> Add_Theme(Theme model)
        {
            if (model == null
                || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Store.AsQueryable();
                if (entities.Theme.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.StoreId.Equals(model.StoreId)).Any())
                    return Result(false, ErrorCode.datadatabase_name_had);
                if (entities.Theme.AsNoTracking().Where(x => x.NO.Equals(model.NO) && x.StoreId.Equals(model.StoreId)).Any())
                    return Result(false, ErrorCode.datadatabase_no_had);
                model.ID = Guid.NewGuid().ToString("N");
                model.UserId = Client.LoginUser.ID;
                model.CreatedTime = DateTime.Now;
                model.UpdatedTime = DateTime.Now;
                model.Flag = (long)GlobalFlag.Normal;
                entities.Theme.Add(model);
                return entities.SaveChanges() > 0 ? Result(true) : Result(false, ErrorCode.sys_fail);
            }

        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update_Theme(Theme model)
        {
            if (model == null
                 || !model.Name.IsNotNullOrEmpty()
                )
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                var oldEntity = entities.Theme.Find(model.ID);
                if (oldEntity != null)
                {
                    if (entities.Theme.AsNoTracking().Where(x => x.Name.Equals(model.Name) && x.StoreId.Equals(model.StoreId) && !x.ID.Equals(model.ID)).Any())
                        return Result(false, ErrorCode.datadatabase_name_had);
                    if (entities.Theme.AsNoTracking().Where(x => x.NO.Equals(model.NO) && x.StoreId.Equals(model.StoreId) && !x.ID.Equals(model.ID)).Any())
                        return Result(false, ErrorCode.datadatabase_no_had);
                    oldEntity.GameMinute = model.GameMinute;
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
        public WebResult<bool> Delete_Theme(string ids)
        {
            if (!ids.IsNotNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            using (DbRepository entities = new DbRepository())
            {
                //找到实体
                entities.Theme.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
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
        public Theme Find_Theme(string id)
        {
            if (!id.IsNotNullOrEmpty())
                return null;
            using (DbRepository entities = new DbRepository())
            {
                var entity = entities.Theme.Find(id);
                return entity;
            }
        }


        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="ids">id，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public WebResult<bool> Enable_Theme(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = ids.Split(',');

                entities.Theme.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
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
        public WebResult<bool> Disable_Theme(string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return Result(false, ErrorCode.sys_param_format_error);
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = ids.Split(',');

                entities.Theme.Where(x => ids.Contains(x.ID)).ToList().ForEach(x =>
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
        public List<SelectItem> Get_ThemeSelectItem(string storeId)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                var query = entities.Store.AsNoTracking().Where(x => x.CompanyId.Equals(Client.LoginUser.CompanyId) && x.Flag == 0);
                if (string.IsNullOrEmpty(storeId))
                {
                    if (Client.LoginUser.StoreFlag != -1)
                    {
                        query = query.Where(x => (Client.LoginUser.StoreFlag & x.LimitFlag) != 0);
                    }
                    var storeIdList = query.Select(x => x.ID).ToList();
                    entities.Theme.AsNoTracking().OrderBy(x => x.CreatedTime).Where(x => storeIdList.Contains(x.StoreId) && x.Flag == 0).OrderBy(x => x.CreatedTime).ToList().ForEach(x =>
                    {
                        list.Add(new SelectItem()
                        {
                            Text = x.Name,
                            Value = x.ID
                        });
                    });
                }
                else
                {                  
                    entities.Theme.AsNoTracking().OrderBy(x => x.CreatedTime).Where(x => x.StoreId.Equals(storeId) && x.Flag == 0).OrderBy(x => x.CreatedTime).ToList().ForEach(x =>
                           {
                               list.Add(new SelectItem()
                               {
                                   Text = x.Name,
                                   Value = x.ID
                               });
                           });
                }
                return list;

            }
        }
    }
}

﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    /// <summary>
    /// 密室
    /// </summary>
    [LoginFilter]
    public class StoreController : BaseController
    {
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

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
        public ActionResult GetPageList(int pageIndex, int pageSize, string name, string provinceName, string cityName, string phone, string startTime, string endTime)
        {
            return JResult(WebService.Get_StorePageList(pageIndex, pageSize, name, provinceName, cityName, phone, startTime, endTime));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Store model)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var result = WebService.Add_Store(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Update(Store model)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var result = WebService.Update_Store(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(WebService.Delete_Store(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(WebService.Find_Store(id));
        }

        /// <summary>
        /// 页面全新枚举
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult GetFlagZTreeNodes(long flag = 0)
        {
            return JResult(WebService.Get_StoreZTree(flag));
        }



        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStoreSelectItem(string id)
        {
            return JResult(WebService.Get_StoreSelectItem(id));
        }
    }
}
using Model;
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
        // GET: Login
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
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
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string provinceName, string cityName, string phone, string startTime, string endTime)
        {
            return JResult(WebService.Get_StorePageList(pageIndex, pageSize, name, provinceName, cityName, phone, startTime, endTime));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public JsonResult Add(Store model)
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
                return ParamsErrorJResult();
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public JsonResult Update(Store model)
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
                return ParamsErrorJResult();
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult Delete(string ids)
        {
            var result = WebService.Delete_Store(ids);
            return JResult(result);
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public JsonResult Find(string id)
        {
            var result = WebService.Find_Store(id);
            return JResult(result);
        }
    }
}
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    [LoginFilter]
    public class OrderController : BaseController
    {
        // GET: Login
        public ActionResult Index(string storeId,DateTime? searchTime)
        {
            var model = new OrderIndexModel();
            model.StoreList = WebService.Get_StoreSelectItem(storeId);
            storeId = !string.IsNullOrEmpty(storeId) ? storeId : (model.StoreList.Count > 0 ? model.StoreList[0].Value : "");
            model.ThemeList = WebService.Get_ThemeSelectItem(storeId);
            model.OrderList = WebService.Get_OrderList(searchTime==null?DateTime.Now:searchTime, storeId);
            if (!string.IsNullOrEmpty(storeId))
            {
                var storeModel = WebService.Find_Store(storeId);
                if (storeModel != null)
                {
                    model.StartTime = Int32.Parse(storeModel.StartTime.Split(':')[0]);
                    model.EndTime = Int32.Parse(storeModel.EndTime.Split(':')[0]);
                }
            }
            return View(model);
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Order model)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var result = WebService.Add_Order(model);
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
        public ActionResult Update(Order model)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var result = WebService.Update_Order(model);
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
            return JResult(WebService.Delete_Order(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(WebService.Find_Order(id));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult StartOrder(string id)
        {
            return JResult(WebService.Start_Order(id));
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaySelectItem(string id)
        {
            return JResult(WebService.Get_PaySelectItem(id));
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public ActionResult GetThemeSelectItem(string id)
        {
            return JResult(WebService.Get_ThemeSelectItem(id));
        }
    }
}
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
    /// 饮料
    /// </summary>
    [LoginFilter]
    public class DrinkController : BaseController
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
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name)
        {
            return JResult(WebService.Get_DrinkPageList(pageIndex, pageSize, name));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Drink model)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var result = WebService.Add_Drink(model);
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
        public ActionResult Update(Drink model)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var result = WebService.Update_Drink(model);
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
            return JResult(WebService.Delete_Drink(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(WebService.Find_Drink(id));
        }


        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        public ActionResult Enable(string ids)
        {
            return JResult(WebService.Enable_Drink(ids));
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        public ActionResult Disable(string ids)
        {
            return JResult(WebService.Disable_Drink(ids));
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectItem(string id)
        {
            return JResult(WebService.Get_DrinkSelectItem(id));
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
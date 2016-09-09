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
    public class ThemeController : BaseController
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
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name, string no)
        {
            return JResult(WebService.Get_ThemePageList(pageIndex, pageSize, name, no));
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Theme model)
        {
            ModelState.Remove("ID");
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var result = WebService.Add_Theme(model);
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
        public ActionResult Update(Theme model)
        {
            ModelState.Remove("UpdatedTime");
            ModelState.Remove("CreatedTime");
            if (ModelState.IsValid)
            {
                var result = WebService.Update_Theme(model);
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
            return JResult(WebService.Delete_Theme(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(WebService.Find_Theme(id));
        }   

        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        public ActionResult Enable(string ids)
        {
            return JResult(WebService.Enable_Theme(ids));
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        public ActionResult Disable(string ids)
        {
            return JResult(WebService.Disable_Theme(ids));
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
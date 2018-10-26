using AutoMapper;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Model;
using Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class UserProductLibraryController : Controller
    {
        private BaseDbContext db;

        public UserProductLibraryController(BaseDbContext db)
        {
            this.db = db;
        }

        [Description("查看产品库页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("用户产品库列表")]
        public JsonResult List()
        {
            var userinfo = LoginUserInfo.GetUserInfo();
            List<UserProductLibrary> list = db.UserProductLibraries.Where(a=> a.ParentId == null).ToList();
            if (list.Count == 0)
            {
                list.Add(new UserProductLibrary
                {
                    ParentId = null,
                    Name = "我的文件夹",
                    Order = 0,
                    CreateBy = userinfo.UserId,
                    CreateLogin = userinfo.Login,
                    CreateName = userinfo.Name,
                    Desc = "系统创建节点"
                });
                list = db.UserProductLibraries.AddRange(list).ToList();
                db.SaveChanges();
            }
            var dtoList = Mapper.Map<List<UserProductLibraryView>>(list);
            return Json(ResultInfo.Success(dtoList));
        }
        [Description("添加用户产品库分类")]
        public JsonResult Add(UserProductLibraryView view)
        {
            var userinfo = LoginUserInfo.GetLoginUser();
            if (string.IsNullOrWhiteSpace(view.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //判断是否重名
            var list = db.UserProductLibraries.Where(where => where.ParentId == view.parentid && where.Name.Trim() == view.name.Trim()).ToList();
            if (list.Count > 0)
            {
                return Json(ResultInfo.Fail("之前已经创建过新节点，请先编辑新节点名称"));
            }
            var model = Mapper.Map<UserProductLibrary>(view);
            model.CreateBy = userinfo.UserId;
            model.CreateLogin = userinfo.Login;
            model.CreateName = userinfo.Name;
            model = db.UserProductLibraries.Add(model);
            db.SaveChanges();
            var rt = Mapper.Map<UserProductLibraryView>(model);
            return Json(ResultInfo.Success(rt));
        }
        [Description("删除用户产品库分类")]
        public JsonResult Delete(UserProductLibraryView view)
        {
            if (view.id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //判断是否有子分类
            var list = db.UserProductLibraries.Where(where => where.ParentId == view.id).ToList();
            if (list.Count > 0)
            {
                return Json(ResultInfo.Fail("文件夹下还有子分类，无法删除"));
            }
            //判断是否有引用
            var list2 = db.UserProductLibraryLink.Where(where => where.LibraryId == view.id).ToList();
            if (list2.Count > 0)
            {
                return Json(ResultInfo.Fail("文件夹下有产品，无法删除"));
            }
            db.UserProductLibraries.Remove(db.UserProductLibraries.Find(view.id));
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
        [Description("重命名用户产品库分类")]
        public JsonResult Rename(UserProductLibraryView view)
        {
            if (view.id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            else if (string.IsNullOrWhiteSpace(view.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //判断是否重名
            var list = db.UserProductLibraries.Where(where => where.ID != view.id && where.ParentId == view.parentid && where.Name.Trim() == view.name.Trim()).ToList();
            if (list.Count > 0)
            {
                return Json(ResultInfo.Fail("同级文件夹下具有相同名称分类"));
            }
            var model = Mapper.Map<UserProductLibrary>(view);
            db.UserProductLibraries.Attach(model);
            db.Entry(model).Property("Name").IsModified = true;
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
        [Description("添加用户产品库分类下产品")]
        public JsonResult LinkAdd(int libid, string ids)
        {
            if (libid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            if (string.IsNullOrWhiteSpace(ids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Success(ResultInfo.Parse(Proc.ProcUserProductLibraryLinkAdd(db, libid, ids, LoginUserInfo.GetUserInfo())));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("查询用户产品库分类下产品列表（分页）")]
        public JsonResult LinkList(int id, int page = 1, int rows = 10)
        {
            if (id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var query = db.UserProductLibraryLink.Where(w => w.LibraryId == id);
            var data = query.OrderBy(d => d.ID).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = data, total = count }));
        }
        [Description("删除用户产品库分类下产品列表")]
        public JsonResult LinkDelete(int[] ids)
        {
            if (ids.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            db.UserProductLibraryLink.RemoveRange(db.UserProductLibraryLink.Where(where => ids.Contains(where.ID)));
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
    }
}
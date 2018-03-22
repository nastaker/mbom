using AutoMapper;
using BLL;
using MBOM.Models;
using System.Linq;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Mvc;
using Localization;
using Model;
using System.ComponentModel;
using MBOM.Filters;

namespace MBOM.Controllers
{
    [UserAuth]
    public class MenuController : Controller
    {
        [Dependency]
        public SysMenuBLL bll { get; set; }
        [Dependency]
        public SysRightActionBLL rabll { get; set; }
        [Dependency]
        public SysActionBLL abll { get; set; }

        [Description("查看菜单管理界面")]
        public ActionResult Index()
        {
            return View();
        }
        [Description("查看[菜单->操作权限]界面")]
        public ActionResult MenuModuleIndex()
        {
            return View();
        }
        // GET: Menu
        [Description("获取菜单列表数据")]
        public JsonResult List()
        {
            var menus = bll.GetQueryable(m => m.ParentId == null).OrderBy(m => m.Order).ToList();
            var menuviews = Mapper.Map<List<SysMenuView>>(menus);
            return Json(ResultInfo.Success(menuviews));
        }
        [Description("获取菜单树数据")]
        public JsonResult Tree()
        {
            var menus = bll.GetQueryable(m => m.ParentId == null).OrderBy(m => m.Order).ToList();
            var menuviews = Mapper.Map<List<MenuView>>(menus);
            return Json(ResultInfo.Success(menuviews));
        }

        [Description("获取[操作权限]列表数据")]
        public JsonResult ActionList()
        {
            var list = abll.GetAll();
            var dtoList = Mapper.Map<List<SysActionView>>(list);
            return Json(ResultInfo.Success(dtoList));
        }

        [Description("获取[菜单->操作权限]列表数据")]
        public JsonResult MenuActionList(int menuid)
        {
            var list = rabll.GetList(ra => ra.MenuId == menuid).OrderBy(a=>a.ActionId);
            var dtoList = Mapper.Map<List<SysRightActionView>>(list);
            return Json(ResultInfo.Success(dtoList));
        }

        [Description("添加[菜单->操作权限]数据")]
        public JsonResult AddAction(int menuid, int[] actionids)
        {
            var actions = new List<SysRightAction>();
            foreach(var actionid in actionids)
            {
                actions.Add(new SysRightAction
                {
                    ActionId = actionid,
                    MenuId = menuid
                });
            }
            var result = rabll.AddRange(actions);
            rabll.SaveChanges();
            return Json(ResultInfo.Success(result));
        }

        [Description("删除[菜单->操作权限]数据")]
        public JsonResult RemoveAction(int[] ids)
        {
            rabll.Delete(c=> ids.Contains(c.ID));
            rabll.SaveChanges();
            return Json(ResultInfo.Success());
        }

        // GET: UserMenu
        [Description("获取用户菜单树数据")]
        public JsonResult UserMenuList()
        {
            var userInfo = LoginUserInfo.GetLoginUser();
            if (userInfo == null)
            {
                return Json(ResultInfo.Fail(Lang.NotLogin));
            }
            IEnumerable<int> rightIds = userInfo.RightIds;
            var menus = bll.GetQueryable(m => rightIds.Contains(m.ID)).OrderBy(m => m.Order);
            var menuviews = Mapper.Map<List<TreeMenuView>>(menus);
            var newmenuvies = new List<TreeMenuView>();
            ConstructTree(menuviews, newmenuvies, null);
            return Json(ResultInfo.Success(newmenuvies));
        }

        private void ConstructTree(List<TreeMenuView> list, List<TreeMenuView> parentlist, int? pid)
        {
            int i = 0;
            while(i < list.Count)
            {
                var child = list[i];
                if (child.parentid == pid)
                {
                    list.Remove(child);
                    parentlist.Add(child);
                    i--;
                }
                i++;
            }
            i = 0;
            while(i < parentlist.Count)
            {
                var parent = parentlist[i];
                if(parent.children == null)
                {
                    parent.children = new List<TreeMenuView>();
                }
                ConstructTree(list, parent.children, parent.id);
                i++;
            }
        }
        //
        [Description("添加新菜单")]
        public JsonResult New(MenuView menuView)
        {
            if (string.IsNullOrWhiteSpace(menuView.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            bll.Add(Mapper.Map<SysMenu>(menuView));
            bll.SaveChanges();
            return Json(ResultInfo.Success());
        }

        [Description("编辑菜单")]
        public JsonResult Edit(MenuView menuView)
        {
            if (string.IsNullOrWhiteSpace(menuView.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            else if (menuView.id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            bll.Edit(Mapper.Map<SysMenu>(menuView));
            bll.SaveChanges();
            return Json(ResultInfo.Success());
        }

        [Description("添加二级菜单")]
        public JsonResult AddChild(MenuView menuView)
        {
            if (string.IsNullOrWhiteSpace(menuView.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            else if (menuView.parentid == null || menuView.parentid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            bll.Add(Mapper.Map<SysMenu>(menuView));
            bll.SaveChanges();
            return Json(ResultInfo.Success());
        }

        [Description("删除菜单")]
        public JsonResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            bll.Delete(id);
            bll.SaveChanges();
            return Json(ResultInfo.Success());
        }
    }
}
using AutoMapper;
using Repository;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class MBOMController : Controller
    {
        private BaseDbContext db;

        public MBOMController(BaseDbContext db)
        {
            this.db = db;
        }
        // MBOM 维护数据列表页面
        [Description("查看MBOM 维护数据列表页面")]
        public ActionResult Index()
        {
            return View();
        }
        // MBOM 发布维护菜单列表页面
        [Description("查看MBOM 发布维护菜单列表页面")]
        public ActionResult MenuIndex()
        {
            return View();
        }

        [Description("查看产品库页面")]
        public ActionResult ProductLibraryIndex()
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM 维护主页面")]
        public ActionResult MaintenanceIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM 变更维护主页面")]
        public ActionResult ChangeMaintenanceIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM 完整性核查页面")]
        public ActionResult IntegrityCheckIndex(string code)
        {
            try
            {
                var prod = Proc.ProcMbomIntegrityCheck(db, code);
                var dtoModel = Mapper.Map<IntegrityCheckView>(prod);
                return View(ResultInfo.Success(dtoModel));
            }
            catch (SqlException ex)
            {
                return View(ResultInfo.Fail(ex.Message));
            }
        }
        // MBOM 产品看板
        [Description("MBOM 产品看板页面")]
        public ActionResult ProductBillboardsIndex()
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品PBOM 详情页面")]
        public ActionResult ProductPBomDetailIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品MBOM 详情页面")]
        public ActionResult ProductMBomDetailIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品详情页面")]
        public ActionResult ProductDetailIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品基本详情页面")]
        public ActionResult BaseInfoIndex(string code)
        {
            var viewModel = db.ViewProjectProductPboms.SingleOrDefault(m => m.CN_PRODUCT_CODE == code.Trim());
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<ViewProjectProductPbomView>(viewModel));
        }
        [Description("选装件列表页面")]
        public ActionResult OptionalItemsIndex()
        {
            return View();
        }
        [Description("产品选装关系设置页面")]
        public ActionResult OptionalItemSetIndex(int itemid)
        {
            return View(itemid);
        }
        // MBOM 物料看板
        [Description("MBOM 物料看板页面")]
        public ActionResult MaterialBillboardsIndex()
        {
            return View();
        }
        // MBOM 带有工序的物料看板
        [Description("MBOM 带有工序的物料看板页面")]
        public ActionResult ItemWithProcessIndex()
        {
            return View();
        }
        //BOM 信息比较
        [Description("BOM 信息比较页面")]
        public ActionResult BomDiffIndex()
        {
            return View();
        }
        [Description("PBOM变更（产品）")]
        public ActionResult PBomChangeProdIndex()
        {
            return View();
        }
        [Description("PBOM变更（部件）")]
        public ActionResult PBomChangeItemIndex()
        {
            return View();
        }
        [Description("MBOM变更维护（产品）")]
        public ActionResult ChangeIndex()
        {
            return View();
        }
        [Description("MBOM变更维护（部件）")]
        public ActionResult CreatePublishIndex()
        {
            return View();
        }
        [Description("MBOM制作及发布物料详情页面")]
        public ActionResult CreatePublishDetailIndex(string itemcode)
        {
            if (string.IsNullOrWhiteSpace(itemcode))
            {
                return HttpNotFound();
            }
            ViewData["itemcode"] = itemcode;
            return View();
        }
        //BOM 信息详情
        [Description("BOM 信息详情页面")]
        public ActionResult BomDiffDetailIndex(int bomid)
        {
            if(bomid == 0)
            {
                return HttpNotFound();
            }
            var list = Proc.ProcGetBomDiff(db, bomid);
            var dtoModels = Mapper.Map<List<BomDiffView>>(list);
            return View(dtoModels);
        }

        [Description("产品变更明细页面")]
        public ActionResult ProductChangeDetailIndex(string prodcode)
        {
            if (string.IsNullOrWhiteSpace(prodcode))
            {
                return HttpNotFound();
            }
            ViewData["prodcode"] = prodcode;
            return View();
        }

        [Description("物料变更明细页面")]
        public ActionResult ItemChangeDetailIndex(string itemcode)
        {
            if (string.IsNullOrWhiteSpace(itemcode))
            {
                return HttpNotFound();
            }
            ViewData["itemcode"] = itemcode;
            return View();
        }

        [Description("物料分类标识设置")]
        public ActionResult ItemHlinkSetIndex()
        {
            return View();
        }

        [Description("用户产品库列表")]
        public JsonResult UserProductLibraryList()
        {
            var userinfo = LoginUserInfo.GetLoginUser();
            List<UserProductLibrary> list = db.UserProductLibraries.ToList();
            if(list.Count == 0)
            {
                list.Add(new UserProductLibrary
                {
                    ParentId = null,
                    Name = "我的文件夹",
                    Order = 0,
                    CreateBy = userinfo.UserId,
                    CreateLogin = userinfo.LoginName,
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
        public JsonResult UserProductLibraryAdd(UserProductLibraryView view)
        {
            var userinfo = LoginUserInfo.GetLoginUser();
            if (string.IsNullOrWhiteSpace(view.name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //判断是否重名
            var list = db.UserProductLibraries.Where(where => where.ParentId == view.parentid && where.Name.Trim() == view.name.Trim()).ToList();
            if(list.Count > 0)
            {
                return Json(ResultInfo.Fail("之前已经创建过新节点，请先编辑新节点名称"));
            }
            var model = Mapper.Map<UserProductLibrary>(view);
            model.CreateBy = userinfo.UserId;
            model.CreateLogin = userinfo.LoginName;
            model.CreateName = userinfo.Name;
            model = db.UserProductLibraries.Add(model);
            db.SaveChanges();
            var rt = Mapper.Map<UserProductLibraryView>(model);
            return Json(ResultInfo.Success(rt));
        }
        [Description("删除用户产品库分类")]
        public JsonResult UserProductLibraryDelete(UserProductLibraryView view)
        {
            if (view.id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            //判断是否有子分类
            var list = db.UserProductLibraries.Where(where => where.ParentId == view.id).ToList();
            if(list.Count > 0)
            {
                return Json(ResultInfo.Fail("文件夹下还有子分类，无法删除"));
            }
            //判断是否有引用
            var list2 = db.UserProductLibraryLink.Where(where => where.LibraryId == view.id).ToList();
            if(list2.Count > 0)
            {
                return Json(ResultInfo.Fail("文件夹下有产品，无法删除"));
            }
            db.UserProductLibraries.Remove(db.UserProductLibraries.Find(view.id));
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
        [Description("重命名用户产品库分类")]
        public JsonResult UserProductLibraryRename(UserProductLibraryView view)
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
        public JsonResult UserProductLibraryLinkAdd(int libid, string ids)
        {
            if(libid == 0)
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
        public JsonResult UserProductLibraryLinkList(int id, int page = 1, int rows = 10)
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
        public JsonResult UserProductLibraryLinkDelete(int[] ids)
        {
            if (ids.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            db.UserProductLibraryLink.RemoveRange(db.UserProductLibraryLink.Where(where => ids.Contains(where.ID)));
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }

        // MBOM 维护进入检查
        [Description("MBOM 维护进入检查")]
        public JsonResult Maintenance(string code)
        {
            var procMsg = Proc.ProcMbomMaintenance(db, code, LoginUserInfo.GetUserInfo());
            return Json(ResultInfo.Parse(procMsg.success, procMsg.msg));
        }
        //
        [MaintenanceActionFilter]
        [Description("产品PBOM版本列表")]
        public JsonResult ProductPbomVerList(string code)
        {
            var list = db.AppPbomVers.Where(p => p.CN_CODE == code).ToList();
            return Json(ResultInfo.Success(list));
        }
        [MaintenanceActionFilter]
        [Description("产品MBOM版本列表")]
        public JsonResult ProductMbomVerList(string code)
        {
            var list = db.AppMbomVers.Where(p => p.CN_CODE == code).ToList();
            return Json(ResultInfo.Success(list));
        }

        // MBOM 数据
        [MaintenanceActionFilter]
        [Description("产品MBOM数据")]
        public JsonResult List(string code)
        {
            var list = Proc.ProcGetMbomList(db, code);
            var dtoModel = Mapper.Map<List<ProcItemTreeView>>(list);
            return Json(ResultInfo.Success(dtoModel));
        }
        /// <summary>
        /// 获取离散区信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [MaintenanceActionFilter]
        [Description("获取离散区信息")]
        public JsonResult DiscreteList(string code)
        {
            var list = Proc.ProcDiscreteList(db, code);
            var dtolist = Mapper.Map<List<ProcItemTreeView>>(list);
            return Json(ResultInfo.Success(dtolist));
        }

        [Description("引用自定义物料")]
        public JsonResult ItemLink(int pid, string plink, int itemid, float quantity)
        {
            if(pid == 0 || string.IsNullOrWhiteSpace(plink) || itemid == 0 || quantity == 0f)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemLink(db, pid, plink, itemid, quantity, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("删除引用的自定义物料")]
        public JsonResult ItemUnlink(int hlinkid)
        {
            if (hlinkid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemUnLink(db, hlinkid));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("修改引用的自定义物料数量")]
        public JsonResult ItemEditQuantity(int hlinkid, float quantity)
        {
            if (hlinkid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemEditQuantity(db, hlinkid, quantity));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        #region 虚件操作
        /// <summary>
        /// 设置为虚件
        /// </summary>
        /// <param name="bomid"></param>
        /// <param name="itemid"></param>
        /// <returns></returns>
        [Description("设置为虚件")]
        public JsonResult VirtualItemSet(string code, int bomid, int itemid, int show)
        {
            if(bomid == 0 || itemid == 0 || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemSet(db, code, bomid, itemid, show, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        /// <summary>
        /// 取消虚件设置
        /// </summary>
        /// <param name="bomid"></param>
        /// <param name="itemid"></param>
        /// <returns></returns>
        [Description("取消虚件设置")]
        public JsonResult VirtualItemDrop(string code, int itemid)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemDrop(db, code, itemid));
            }
            catch (SqlException ex)
            {   
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        /// <summary>
        /// 引用虚件
        /// </summary>
        /// <param name="parentcode"></param>
        /// <param name="code"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Description("引用虚件")]
        public JsonResult VirtualItemLink(string code, int parentitemid, int itemid, string parentlink, string link)
        {
            if(parentitemid == 0 || itemid == 0 || string.IsNullOrWhiteSpace(parentlink) || string.IsNullOrWhiteSpace(link) || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemLink(db, code, parentitemid, itemid, parentlink, link, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        /// <summary>
        /// 取消引用虚件
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        [Description("取消引用虚件")]
        public JsonResult VirtualItemUnlink(string code, int itemid, string link)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(link) || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemUnlink(db, code, itemid, link));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        #endregion

        #region 合件操作
        [Description("设置合件")]
        public JsonResult CompositeItemSet(string code, int bomid, string link, string itemids, string type)
        {
            if (bomid == 0 || string.IsNullOrWhiteSpace(link) || string.IsNullOrWhiteSpace(itemids) || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemSet(db, code, bomid, link, itemids, type, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("取消设置合件")]
        public JsonResult CompositeItemDrop(string code, int itemid)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemDrop(db, code, itemid));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("引用合件")]
        public JsonResult CompositeItemLink(string code, int parentitemid, int itemid, string parentlink, string link)
        {
            if (parentitemid == 0 || itemid == 0 || string.IsNullOrWhiteSpace(parentlink) || string.IsNullOrWhiteSpace(link))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemLink(db, code, parentitemid, itemid, parentlink, link, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("取消引用合件")]
        public JsonResult CompositeItemUnlink(string code, int itemid, int bomid, string link)
        {
            if (itemid == 0 || bomid == 0 || string.IsNullOrWhiteSpace(link) || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemUnlink(db, code, itemid, bomid, link));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        #endregion

        //修改合件名称
        [Description("修改合件名称")]
        public JsonResult EditCombineName(int itemid, string name)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcEditCombineName(db, itemid, name));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        //MBOM 发布
        [Description("MBOM 发布")]
        public JsonResult Release(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcMbomRelease(db, code, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("MBOM 创建和发布，添加子件（有关联）")]
        public JsonResult BomHlinkChildAdd(string parentitemcode, int itemid, int hlinkid, string bywhat)
        {
            if (itemid == 0 || hlinkid == 0 || string.IsNullOrWhiteSpace(parentitemcode) || string.IsNullOrWhiteSpace(bywhat))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcBomHlinkChildAdd(db, parentitemcode, itemid, hlinkid, bywhat, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("MBOM 创建和发布，添加子件（独立添加）")]
        public JsonResult BomHlinkAdd(string parentitemcode, int itemid, string bywhat)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(parentitemcode) || string.IsNullOrWhiteSpace(bywhat))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcBomHlinkAdd(db, parentitemcode, itemid, bywhat, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        public JsonResult ApplyBomChange(int hlinkid, string bywhat)
        {
            if (hlinkid == 0 || string.IsNullOrWhiteSpace(bywhat))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcApplyBomChange(db, hlinkid, bywhat, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("MBOM 创建和发布，删除子件（实际是修改状态)")]
        public JsonResult DisableBomHlink(int hlinkid)
        {
            if (hlinkid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcDisableBomHlink(db, hlinkid, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("MBOM工序设置")]
        public JsonResult ItemDeductionSet(string bomhids,int pvhid)
        {
            if (pvhid == 0 || string.IsNullOrWhiteSpace(bomhids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemDeductionSet(db, bomhids, pvhid, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("产品BOM所有子BOM_HLINK")]
        public JsonResult BomHlinkList(string itemcode)
        {
            if (string.IsNullOrWhiteSpace(itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var list = Proc.ProcGetBomHlinkChildren(db, itemcode);
            return Json(ResultInfo.Success(list));
        }
        [Description("产品BOM看板（分页）")]
        public JsonResult BomPageList(AppBomView view, int page = 1, int rows = 10)
        {
            var query = db.AppBoms.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.code))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(view.code));
            }
            if (!string.IsNullOrWhiteSpace(view.item_code))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.item_code));
            }
            if (!string.IsNullOrWhiteSpace(view.name))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.name));
            }
            var list = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var dtoModels = Mapper.Map<List<AppBomView>>(list);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = dtoModels, total = count }));
        }
        // MBOM 物料看板
        [Description("MBOM 物料看板（分页）")]
        public JsonResult MaterialBillboardsPageList(MaterialView view, int page = 1, int rows = 10)
        {
            var query = db.ViewMaterialBillboardses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.Code))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(view.Code));
            }
            if (!string.IsNullOrWhiteSpace(view.ItemCode))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.ItemCode));
            }
            if (!string.IsNullOrWhiteSpace(view.ItemName))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.ItemName));
            }
            if (view.ItemSale)
            {
                query = query.Where(obj => obj.销售件 != null);
            }
            if (view.ItemPurchase)
            {
                query = query.Where(obj => obj.采购件 != null);
            }
            if (view.ItemSelfmade)
            {
                query = query.Where(obj => obj.自制件 != null);
            }
            if (view.ItemCombine)
            {
                query = query.Where(obj => obj.MBOM合件 != null);
            }
            if (view.ItemVirtual)
            {
                query = query.Where(obj => obj.MBOM虚拟件 != null);
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            //var list = Mapper.Map<List<MaterialView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }
        // MBOM 产品看板
        [Description("MBOM 产品看板（分页）")]
        public JsonResult ProductBillboardsPageList(ProductView view, int page = 1, int rows = 10)
        {
            var query = db.ViewProductBillboardses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.Code))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(view.Code));
            }
            if (!string.IsNullOrWhiteSpace(view.ItemCode))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.ItemCode));
            }
            if (!string.IsNullOrWhiteSpace(view.Name))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.Name));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            //var list = Mapper.Map<List<MaterialView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }
        // MBOM 带有工序的物料看板
        [Description("MBOM 带有工序的物料看板（分页）")]
        public JsonResult ItemWithProcessPageList(ProductView view, int page = 1, int rows = 10)
        {
            var query = db.ViewItemWithProcesses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.Code))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(view.Code));
            }
            if (!string.IsNullOrWhiteSpace(view.ItemCode))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.ItemCode));
            }
            if (!string.IsNullOrWhiteSpace(view.Name))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.Name));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            //var list = Mapper.Map<List<MaterialView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }
        [Description("MBOM维护数据列表（分页）")]
        public JsonResult MaintenancePageList(ViewMbomMaintenanceView view, int page = 1, int rows = 10)
        {
            var query = db.ViewMbomMaintenances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(view.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.CN_PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var list = Mapper.Map<List<ViewMbomMaintenanceView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
        [Description("MBOM变更维护数据列表（分页）")]
        public JsonResult ChangeMaintenancePageList(ViewMbomMaintenanceView view, int page = 1, int rows = 10)
        {
            var query = db.ViewMbomMaintenances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(view.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.CN_PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var list = Mapper.Map<List<ViewMbomMaintenanceView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("查看PBOM变更的产品列表（分页）")]
        public JsonResult PBOMChangeProdPageList(ViewPbomChangeProduct view, int page = 1, int rows = 10)
        {
            var query = db.ViewPbomChangeProducts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.CN_PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(view.CN_PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.CN_NAME))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.CN_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_PRODUCT_CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }

        [Description("查看PBOM变更的物料列表（分页）")]
        public JsonResult PBOMChangeItemPageList(ViewPbomChangeItem view, int page = 1, int rows = 10)
        {
            var query = db.ViewPbomChangeItems.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.CN_ITEM_CODE))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.CN_ITEM_CODE));
            }
            var projs = query.OrderBy(obj => obj.CN_ITEM_CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }

        public JsonResult PBOMChangeItemAllPageList(ViewPbomChangeItemAll view, int page = 1, int rows = 10)
        {
            var query = db.ViewPbomChangeItemsAll.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.CN_ITEM_CODE))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.CN_ITEM_CODE));
            }
            var projs = query.OrderBy(obj => obj.CN_ITEM_CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = projs, total = count }));
        }

        [Description("查看产品的PBOM变更列表")]
        public JsonResult ProductChangeDetailList(string prodcode)
        {
            var list = Proc.ProcProductChangeDetail(db, prodcode);
            return Json(ResultInfo.Success(list));
        }

        [Description("查看物料的PBOM变更列表")]
        public JsonResult ItemChangeDetailList(string itemcode)
        {
            if (string.IsNullOrWhiteSpace(itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var list = db.ViewItemChangeDetails.Where(where=> where.itemcode == itemcode).ToList();
            return Json(ResultInfo.Success(list));
        }

        [Description("查看MBOM制作及发布的物料详情列表")]
        public JsonResult CreatePublishDetailList(string itemcode, short istoerp = -1, string changesign = null, string impactdo = null)
        {
            if (string.IsNullOrWhiteSpace(itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var query = db.ViewCreatePublishDetails.Where(where => where.pitemcode == itemcode);
            if (istoerp > -1)
            {
                query = query.Where(where => where.istoerp == istoerp);
            }
            if (changesign != null && changesign != "null")
            {
                query = query.Where(where => where.changesign == changesign);
            }
            if (impactdo != null && impactdo != "null")
            {
                query = query.Where(where => where.impactdo == impactdo);
            }
            return Json(ResultInfo.Success(query));
        }
    }
}
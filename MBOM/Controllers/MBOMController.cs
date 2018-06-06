using AutoMapper;
using Repository;
using Localization;
using MBOM.Filters;
using MBOM.Models;
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
        [Description("查看MBOM维护数据列表页面")]
        public ActionResult Index()
        {
            return View();
        }
        // MBOM 发布维护菜单列表页面
        [Description("查看MBOM发布维护菜单列表页面")]
        public ActionResult MenuIndex()
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM维护主页面")]
        public ActionResult MaintenanceIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM完整性核查页面")]
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
        [Description("MBOM产品看板页面")]
        public ActionResult ProductBillboardsIndex()
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品PBOM详情页面")]
        public ActionResult ProductPBomDetailIndex(string code)
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("产品MBOM详情页面")]
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
            var viewModel = db.ViewProjectProductPboms.Where(m => m.PRODUCT_CODE == code.Trim()).First();
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
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
        [Description("MBOM物料看板页面")]
        public ActionResult MaterialBillboardsIndex()
        {
            return View();
        }
        // MBOM 带有工序的物料看板
        [Description("MBOM带有工序的物料看板页面")]
        public ActionResult ItemWithProcessIndex()
        {
            return View();
        }
        //BOM 信息比较
        [Description("BOM信息比较页面")]
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
        [Description("BOM信息详情页面")]
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
        // MBOM 标记
        public JsonResult Mark(string code)
        {
            var prod = db.AppProducts.Where(w=>w.CN_CODE == code.TrimEnd()).First();
            if(prod == null)
            {
                return Json(ResultInfo.Fail("获取产品信息失败，标记失败"));
            }
            prod.CN_MARK = !prod.CN_MARK;
            db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
        // MBOM 维护进入检查
        [Description("MBOM维护进入检查")]
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
            ResultInfo rt = null;
            try
            {
                var list = Proc.ProcGetMbomList(db, code);
                rt = ResultInfo.Success(list);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
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
            ResultInfo rt = null;
            try
            {
                var list = Proc.ProcDiscreteList(db, code);
                rt = ResultInfo.Success(list);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
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
        public JsonResult CompositeItemSet(string code, int bomid, string parentlink, string itemids, string type)
        {
            if (bomid == 0 || string.IsNullOrWhiteSpace(parentlink) || string.IsNullOrWhiteSpace(itemids) || string.IsNullOrWhiteSpace(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemSet(db, code, bomid, parentlink, itemids, type, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("取消设置合件")]
        public JsonResult CompositeItemDrop(string code, int itemid, string parentlink)
        {
            if (itemid == 0 || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(parentlink))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemDrop(db, code, itemid, parentlink));
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
        [Description("MBOM发布")]
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

        [Description("MBOM创建和发布，添加子件（有关联）")]
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

        [Description("MBOM创建和发布，添加子件（独立添加）")]
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

        [Description("MBOM创建和发布，删除子件")]
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
            var list = db.AppBomHlinks
                .Join(db.AppBoms.Where(b=>b.CN_ITEM_CODE == itemcode), a => a.CN_BOM_ID, b => b.CN_ID, (a,b) => a)
                .Where(a => a.CN_SYS_STATUS == "" || a.CN_SYS_STATUS == "Y"
                        && (a.CN_ISDELETE == null || a.CN_ISDELETE != true))
                .ToList();
            return Json(ResultInfo.Success(list));
        }
        [Description("产品BOM看板列表（分页）")]
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
        [Description("MBOM物料看板列表（分页）")]
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
        [Description("MBOM产品看板列表（分页）")]
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
        [Description("MBOM带有工序的物料列表（分页）")]
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
        [Description("MBOM产品维护列表（分页）")]
        public JsonResult MaintenancePageList(ViewMbomMaintenance view, int page = 1, int rows = 10)
        {
            var query = db.ViewMbomMaintenances.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(view.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
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
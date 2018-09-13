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
using System;

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
        [Description("查看MBOM产品维护列表页面")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("查看MBOM物料维护列表页面")]
        public ActionResult ComponentIndex()
        {
            return View();
        }
        // MBOM 变更维护数据列表页面
        [Description("查看MBOM产品变更维护列表页面")]
        public ActionResult ChangeIndex()
        {
            return View();
        }

        [Description("查看MBOM发布维护菜单列表页面")]
        public ActionResult MenuIndex()
        {
            return View();
        }

        [MaintenanceActionFilter]
        [Description("MBOM维护主页面")]
        public ActionResult MaintenanceIndex(string prod_itemcode)
        {
            return View();
        }

        [MaintenanceActionFilter]
        [Description("MBOM维护主页面")]
        public ActionResult ComponentMaintenanceIndex(string itemcode)
        {
            return View();
        }

        [MaintenanceActionFilter]
        [Description("MBOM完整性核查页面")]
        public ActionResult IntegrityCheckIndex(string prod_itemcode)
        {
            try
            {
                var prod = Proc.ProcMbomIntegrityCheck(db, prod_itemcode);
                return View(ResultInfo.Success(prod));
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
        [Description("产品详情页面")]
        public ActionResult ProductDetailIndex(string prod_itemcode)
        {
            return View();
        }

        [MaintenanceActionFilter]
        [Description("产品基本详情页面")]
        public ActionResult BaseInfoIndex(string prod_itemcode)
        {
            var viewModel = db.ViewProjectProductPboms.Where(m => m.PRODUCT_ITEM_CODE == prod_itemcode.Trim());
            if (viewModel.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(viewModel.First());
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
        //BOM 信息详情
        [Description("BOM信息详情页面")]
        public ActionResult BomDiffDetailIndex(int bomid)
        {
            return HttpNotFound();
        }
        
        [Description("物料分类标识设置")]
        public ActionResult ItemHlinkSetIndex()
        {
            return View();
        }
        // MBOM 标记
        public JsonResult Mark(string prod_itemcode)
        {
            var prod = db.AppProducts.Where(w => w.CN_ITEM_CODE == prod_itemcode.TrimEnd()).First();
            if (prod == null)
            {
                return Json(ResultInfo.Fail("获取产品信息失败，标记失败"));
            }
            prod.CN_MARK = !prod.CN_MARK;
            db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(ResultInfo.Success());
        }
        // MBOM 创建新版本
        [Description("MBOM 创建新版本")]
        public JsonResult CreateVer(string prod_itemcode, string ver, DateTime dtef, DateTime dtex, string pbomver_guid, string desc)
        {
            if (String.IsNullOrWhiteSpace(prod_itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            if (!Guid.TryParse(pbomver_guid, out Guid pvguid))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCreateMbomVer(db, prod_itemcode, ver, dtef, dtex, pbomver_guid, desc, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        //
        [MaintenanceActionFilter]
        [Description("产品PBOM版本列表")]
        public JsonResult ProductPbomVerList(string prod_itemcode)
        {
            if (String.IsNullOrWhiteSpace(prod_itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Success(Proc.ProcGetPbomVerList(db, prod_itemcode));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [MaintenanceActionFilter]
        [Description("产品MBOM版本列表")]
        public JsonResult ProductMbomVerList(string prod_itemcode)
        {
            if (String.IsNullOrWhiteSpace(prod_itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Success(Proc.ProcGetMbomVerList(db, prod_itemcode));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        // MBOM 数据
        [MaintenanceActionFilter]
        [Description("产品MBOM数据")]
        public JsonResult List(string prod_itemcode)
        {
            ResultInfo rt = null;
            try
            {
                var list = Proc.ProcGetMbomList(db, prod_itemcode);
                rt = ResultInfo.Success(list);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        // MBOM 单物料数据
        [MaintenanceActionFilter]
        [Description("物料MBOM单层数据")]
        public JsonResult ItemList(string itemcode)
        {
            ResultInfo rt = null;
            try
            {
                var list = Proc.ProcGetItemList(db, itemcode);
                rt = ResultInfo.Success(list);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        // MBOM 单物料数据发布
        [MaintenanceActionFilter]
        [Description("物料MBOM单层数据发布")]
        public JsonResult ItemPublish(string itemcode)
        {
            ResultInfo rt = null;
            try
            {
                var list = Proc.ProcItemPublish(db, itemcode, LoginUserInfo.GetUserInfo());
                rt = ResultInfo.Success(list);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("引用自定义物料")]
        public JsonResult ItemLink(string prod_itemcode, string itemcode_parent, string itemcode, float quantity)
        {
            if (string.IsNullOrWhiteSpace(itemcode_parent) || string.IsNullOrWhiteSpace(itemcode) || quantity < 0f)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemLink(db, prod_itemcode, itemcode_parent, itemcode, quantity, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("删除引用的自定义物料")]
        public JsonResult ItemUnlink(string prod_itemcode, string guids)
        {
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemUnLink(db, prod_itemcode, guids, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("修改引用的自定义物料数量")]
        public JsonResult ItemEditQuantity(string guid, float quantity)
        {
            if (!Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcItemEditQuantity(db, g, quantity));
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
        public JsonResult VirtualItemSet(string prod_itemcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) || !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemSet(db, prod_itemcode, g, LoginUserInfo.GetUserInfo()));
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
        public JsonResult VirtualItemDrop(string prod_itemcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) || !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemDrop(db, prod_itemcode, g, LoginUserInfo.GetUserInfo()));
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
        /// <param name="prod_itemcode"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [Description("引用虚件")]
        public JsonResult VirtualItemLink(string prod_itemcode, string parentcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                string.IsNullOrWhiteSpace(parentcode) ||
                !Guid.TryParse(guid, out Guid g)
                )
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemLink(db, prod_itemcode, parentcode, g, LoginUserInfo.GetUserInfo()));
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
        public JsonResult VirtualItemUnlink(string prod_itemcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcVirtualItemUnlink(db, prod_itemcode, g, LoginUserInfo.GetUserInfo()));
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
        public JsonResult CompositeItemSet(string prod_itemcode, string guids, string type)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                string.IsNullOrWhiteSpace(guids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemSet(db, prod_itemcode, guids, type, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("取消设置合件")]
        public JsonResult CompositeItemDrop(string prod_itemcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemDrop(db, prod_itemcode, g, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("引用合件")]
        public JsonResult CompositeItemLink(string prod_itemcode, string parentcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                string.IsNullOrWhiteSpace(parentcode) ||
                !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemLink(db, prod_itemcode, parentcode, g, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("取消引用合件")]
        public JsonResult CompositeItemUnlink(string prod_itemcode, string guid)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode) ||
                !Guid.TryParse(guid, out Guid g))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcCompositeItemUnlink(db, prod_itemcode, g, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        #endregion

        //MBOM 发布
        [Description("MBOM发布")]
        public JsonResult Release(string prod_itemcode)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcMbomRelease(db, prod_itemcode, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
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
        public JsonResult ProductBillboardsPageList(ItemView view, int page = 1, int rows = 10)
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
        public JsonResult ItemWithProcessPageList(ItemView view, int page = 1, int rows = 10)
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
                if (view.PRODUCT_CODE.IndexOf(",") > 0)
                {
                    string[] prodcodes = view.PRODUCT_CODE.Split(',');
                    query = query.Where(obj => prodcodes.Contains(obj.PRODUCT_CODE));
                }
                else
                {
                    query = query.Where(obj => obj.PRODUCT_CODE.Contains(view.PRODUCT_CODE));
                }
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("MBOM物料维护列表（分页）")]
        public JsonResult ComponentMaintenancePageList(ItemView view, int page = 1, int rows = 10)
        {
            var query = db.AppItems.AsQueryable();
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
            var list = query.OrderBy(obj => obj.CN_ITEM_CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("MBOM产品变更维护列表（分页）")]
        public JsonResult ChangeMaintenancePageList(ViewMbomMaintenance view, int page = 1, int rows = 10)
        {
            var query = db.ViewMbomMaintenances.Where(v => v.MBOMVER == null || v.MBOMVER.Trim().Length == 0);
            if (!string.IsNullOrWhiteSpace(view.PRODUCT_CODE))
            {
                if (view.PRODUCT_CODE.IndexOf(",") > 0)
                {
                    string[] prodcodes = view.PRODUCT_CODE.Split(',');
                    query = query.Where(obj => prodcodes.Contains(obj.PRODUCT_CODE));
                }
                else
                {
                    query = query.Where(obj => obj.PRODUCT_CODE.Contains(view.PRODUCT_CODE));
                }
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
        
    }
}
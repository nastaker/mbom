using AutoMapper;
using BLL;
using DAL;
using DAL.Models;
using Localization;
using MBOM.Filters;
using MBOM.Models;
using Microsoft.Practices.Unity;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class ItemController : Controller
    {
        [Dependency]
        public AppItemBLL itembll { get; set; }
        [Dependency]
        public AppBomBLL bombll { get; set; }
        [Dependency]
        public AppItemHLinkBLL itemhlbll { get; set; }
        [Dependency]
        public AppProcessVerBLL processverbll { get; set; }
        [Dependency]
        public AppProcessVerHlinkBLL processbll { get; set; }
        [Dependency]
        public AppProductBLL prodbll { get; set; }
        [Dependency]
        public SysAttachmentBLL attachbll { get; set; }
        [Dependency]
        public PROCBLL procbll { get; set; }
        [Dependency]
        public ViewItemMaintenanceBLL viewbll { get; set; }
        [Dependency]
        public DictItemUnitBLL unitbll { get; set; }
        [Dependency]
        public DictItemTypeBLL typebll { get; set; }
        [Dependency]
        public AppBomHlinkBLL bomhlinkbll { get; set; }
        [Dependency]
        public ViewOptionalItemBLL viewoibll { get; set; }
        [Dependency]
        public AppOptionalItemHlinkBLL aoihbll { get; set; }
        [Dependency]
        public ViewNoOptionalItemBLL viewnoibll { get; set; }
        [Dependency]
        public ViewItemByTypeBLL vibytbll { get; set; }
        [Dependency]
        public ViewItemWithTypeBLL viwtbll { get; set; }
        [Dependency]
        public DictShippingAddrBLL dsabll { get; set; }

        const string PATH_ATTACHMENTS_FOLDER = "~/Upload/Attachments/";

        [Description("添加或修改自定义物料")]
        public ActionResult Edit(ViewItemMaintenance view, int[] CN_TYPE)
        {
            var msg = string.Empty;
            var user = LoginUserInfo.GetLoginUser();
            var now = DateTime.Now;
            AppItem item = null;
            if (view.CN_ID == 0)
            {
                item = Mapper.Map<AppItem>(view);
                if (string.IsNullOrWhiteSpace(view.CN_ITEM_CODE))
                {
                    return Json(ResultInfo.Fail("物料编码不能为空！"));
                }
                //添加
                item.CN_CODE = view.CN_ITEM_CODE;
                item.CN_DESIGN_PHASE = "";
                item.CN_PRODUCT_BASE = "";
                item.CN_SYS_STATUS = "Y";
                item.CN_IS_TOERP = 0;
                item.CN_CREATE_BY = user.UserId;
                item.CN_CREATE_LOGIN = user.LoginName;
                item.CN_CREATE_NAME = user.Name;

                var bom = new AppBom
                {
                    CN_CODE = item.CN_CODE,
                    CN_NAME = item.CN_NAME,
                    CN_ITEM_CODE = item.CN_ITEM_CODE,
                    CN_TYPE = "",
                    CN_DT_CREATE = now,
                    CN_CREATE_BY = user.UserId,
                    CN_CREATE_LOGIN = user.LoginName,
                    CN_CREATE_NAME = user.Name,
                    CN_SYS_STATUS = "Y"
                };

                itembll.Add(item);
                bombll.Add(bom);

                bombll.SaveChanges();
                msg = "添加成功";
            }
            else
            {
                //修改
                item = itembll.Get(where => where.CN_ID == view.CN_ID);
                if (item == null)
                {
                    return Json(ResultInfo.Fail("物料信息不存在，请联系管理员。"));
                }

                var bomh = bomhlinkbll.GetList(where => where.CN_COMPONENT_OBJECT_ID == view.CN_ID);
                if (bomh != null && bomh.Count > 0)
                {
                    return Json(ResultInfo.Fail("物料已经被引用，无法进行修改。"));
                }

                if (item.CN_IS_TOERP == 1)
                {
                    return Json(ResultInfo.Fail("物料已发布到ERP系统，无法修改！"));
                }
                item.CN_NAME = view.CN_NAME;
                item.CN_UNIT = view.CN_UNIT;
                item.CN_WEIGHT = view.CN_WEIGHT;
                itembll.Modify(item, "CN_NAME","CN_UNIT", "CN_WEIGHT");
                itemhlbll.Delete(where => where.CN_ID == item.CN_ID && where.CN_DISPLAYNAME == "自定义物料");
                msg = "修改成功";
            }
            List<AppItemHLink> typelist = new List<AppItemHLink>();
            foreach (int type in CN_TYPE)
            {
                typelist.Add(new AppItemHLink
                {
                    CN_ID = item.CN_ID,
                    CN_COMPONENT_CLASS_ID = 105,
                    CN_COMPONENT_OBJECT_ID = type,
                    CN_B_IS_ASSEMBLY = false,
                    CN_DT_CREATE = now,
                    CN_UNIT = item.CN_UNIT,
                    CN_DISPLAYNAME = "自定义物料",
                    CN_SYS_STATUS = "Y",
                    CN_CREATE_BY = user.UserId,
                    CN_CREATE_LOGIN = user.LoginName,
                    CN_CREATE_NAME = user.Name
                });
            }
            itemhlbll.AddRange(typelist);
            itembll.SaveChanges();
            itemhlbll.SaveChanges();
            return Json(ResultInfo.Success(msg));
        }

        [Description("删除手工添加的自定义物料")]
        public ActionResult Delete(int id)
        {
            var item = itembll.Get(where => where.CN_ID == id);
            if (item == null)
            {
                return Json(ResultInfo.Fail("物料不存在，或已经被删除，请刷新后重试。"));
            }

            var bomh = bomhlinkbll.GetList(where => where.CN_COMPONENT_OBJECT_ID == id);
            if (bomh != null && bomh.Count > 0)
            {
                return Json(ResultInfo.Fail("物料已经被引用，无法进行删除。"));
            }

            if (item.CN_PDM_CLS_ID != 0)
            {
                return Json(ResultInfo.Fail("物料非自定义添加的物料，无法删除！"));
            }
            if (item.CN_IS_TOERP == 1)
            {
                return Json(ResultInfo.Fail("物料已发布到ERP系统，无法删除！"));
            }
            itemhlbll.Delete(where => where.CN_ID == item.CN_ID);
            bombll.Delete(where => where.CN_CODE == item.CN_CODE);
            itembll.Delete(item);
            bombll.SaveChanges();
            itembll.SaveChanges();
            itemhlbll.SaveChanges();
            return Json(ResultInfo.Success("已成功删除自定义物料"));
        }

        // GET: Item
        [Description("查看[ITEM基本信息页面]")]
        public ActionResult BaseInfoIndex(string code)
        {
            var item = itembll.Get(where => where.CN_CODE == code);
            return View(item);
        }

        [Description("查看[附件上传页面]")]
        public ActionResult AttachmentsIndex()
        {
            return View();
        }

        [Description("查看[附件查看页面]")]
        public ActionResult AttachmentsViewIndex(string code, int filetype)
        {
            //是展示
            var attachs = attachbll.GetList(a => a.FileType == filetype && a.Code == code);
            var rt = Mapper.Map<List<FileResponse>>(attachs);
            return View(rt);
        }

        [Description("查看[ITEM详情页面]")]
        public ActionResult ItemDetailIndex()
        {
            return View();
        }

        [Description("物料维护页面")]
        public ActionResult MaintenanceIndex()
        {
            return View();
        }

        [HttpPost]
        [Description("上传附件")]
        public JsonResult UploadAttachment(string code, int filetype,HttpPostedFileBase file)
        {
            //如果file不为空，则是保存文件
            if (file != null)
            {
                string dir = Path.Combine(PATH_ATTACHMENTS_FOLDER,
                                                code,
                                                filetype.ToString());
                string filepath = Path.Combine(dir, file.FileName);
                //获取item数据
                var item = itembll.Get(i => i.CN_CODE == code);
                //写数据库
                var newAttach = attachbll.Add(new SysAttachment
                {
                    Code = item.CN_CODE,
                    ItemId = item.CN_ID,
                    ItemCode = item.CN_ITEM_CODE,
                    FileType = filetype,
                    FileName = file.FileName,
                    FilePath = filepath.Substring(1),
                    FileSize = file.ContentLength
                });
                attachbll.SaveChanges();
                FileResponse rt = Mapper.Map<FileResponse>(newAttach);
                //上传至/upload/attachments
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(Server.MapPath(dir));
                }
                file.SaveAs(Server.MapPath(filepath));
                return Json(new { files = new[] { rt } });
            }
            else
            {
                //是展示
                var attachs = attachbll.GetList(a => a.FileType == filetype && a.Code == code);
                var rt = Mapper.Map<List<FileResponse>>(attachs);
                return Json(ResultInfo.Success(rt));
            }
        }
        
        [Description("删除附件")]
        public JsonResult DeleteAttachment(int id)
        {
            attachbll.Delete(id);
            attachbll.SaveChanges();
            return Json(ResultInfo.Success());
        }

        [Description("获取物料计量单位列表")]
        public JsonResult UnitList()
        {
            var list = unitbll.GetAll();
            return Json(ResultInfo.Success(list));
        }

        [Description("获取物料类型列表表")]
        public JsonResult TypeList()
        {
            var list = typebll.GetList(where => 
            where.CN_NAME.Contains("自制") ||
            where.CN_NAME.Contains("MBOM合件") ||
            where.CN_NAME.Contains("采购"));
            return Json(ResultInfo.Success(list));
        }

        [Description("查看销售件")]
        public JsonResult SaleSetList(string code)
        {
            var list = procbll.ProcGetItemSaleSetInfo(code);
            return Json(ResultInfo.Success(list));
        }

        [Description("查看产品详情")]
        public JsonResult ProductTree(string code)
        {
            var prodTree = procbll.ProcGetItemTree(code);
            var dtoModel = Mapper.Map<List<ProcItemTreeView>>(prodTree);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看物料详情")]
        public JsonResult ProductList(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = procbll.ProcGetItemList(code);
            var dtoModel = Mapper.Map<List<ProcItemView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看工序")]
        public JsonResult ProductProcessList(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = procbll.ProcGetProcessItemList(code);
            var dtoModel = Mapper.Map<List<ProcProcessItemView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看物料工序详情")]
        public JsonResult ProductProcessInfo(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = procbll.ProcGetItemProcess(code);
            var dtoModel = Mapper.Map<List<ProcItemProcessView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看物料完整性数据")]
        public JsonResult IntegrityCheck(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var prod = prodbll.Get(p => p.CN_CODE == code);
            var dtoModel = Mapper.Map<IntegrityCheckView>(prod);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看产品分类")]
        public JsonResult ProductCateList(string code, string catename)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            else if (string.IsNullOrEmpty(catename))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = procbll.ProcGetItemCateList(code, catename);
            var dtoModel = Mapper.Map<List<ProcCateItemView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("销售件设置")]
        public JsonResult SaveSaleSetList(
            string code,
            List<ProcItemSetInfo> list)
        {
            if(list == null)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var str = ConstructSaleSetListString(list);
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.ProcSetSaleList(code, str, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        private string ConstructSaleSetListString(List<ProcItemSetInfo> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(String.Format("{3},{2},{1},{0};", item.ITEMID, item.F_QUANTITY, item.SHIPPINGADDR, item.TYPE));
            }
            return sb.ToString();
        }

        [Description("设置物料为选装件分类")]
        public JsonResult SetOptionalItems(string itemids)
        {
            if (string.IsNullOrWhiteSpace(itemids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.ProcSetOptionalItems(itemids, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("删除物料的选装件分类")]
        public JsonResult DeleteOptionalItem(int id)
        {
            itemhlbll.Delete(id);
            itemhlbll.SaveChanges();
            return Json(ResultInfo.Success("删除成功！"));
        }

        [Description("添加选装件对应关系")]
        public JsonResult OptionalItemMapAdd(int itemid, string itemids)
        {
            if(itemid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            if (string.IsNullOrWhiteSpace(itemids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.OptionalItemMapAdd(itemid, itemids, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("添加选装件对应关系")]
        public JsonResult OptionalItemMapRemove(string hlinkids)
        {
            if (string.IsNullOrWhiteSpace(hlinkids))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.OptionalItemMapRemove(hlinkids));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        [Description("查看产品选装关系列表")]
        public JsonResult OptionalItemMapList(int itemid)
        {
            var list = aoihbll.GetList(oih => oih.CN_ID == itemid);
            return Json(ResultInfo.Success(list));
        }

        [Description("查看物料工序版本")]
        public JsonResult ItemProcessVer(string code)
        {
            var list = processverbll.GetList(ver => ver.CN_CODE == code);
            var dtoModel = Mapper.Map<List<AppProcessVerView>>(list);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查看物料工序流程")]
        public JsonResult ItemProcessByVerId(int id)
        {
            if (id == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = processbll.GetList(process => process.CN_ID == id);
            var dtoModel = Mapper.Map<List<ProcItemProcessView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("查找Item的父级")]
        public JsonResult ItemParentList(int itemid)
        {
            if(itemid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var itemlist = itembll.GetParentList(itemid);
            var dtoModel = Mapper.Map<List<ProcProcessItemView>>(itemlist);
            return Json(ResultInfo.Success(dtoModel));
        }

        [Description("物料分类标识切换（采购或自制件）")]
        public JsonResult TypeSwitch(int itemid)
        {
            if (itemid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.ProcItemTypeSwitch(itemid, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("物料分类标识设置")]
        public JsonResult SetType(int itemid, int typeid)
        {
            if (itemid == 0 || typeid == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(procbll.ProcItemSetType(itemid, typeid, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        public JsonResult GetShippingAddr()
        {
            var query = dsabll.GetQueryable(where => where.CN_SYS_STATUS == "Y");
            return Json(ResultInfo.Success(query.ToList()));
        }

        [Description("物料维护分页列表")]
        public JsonResult MaintenancePageList(ViewItemMaintenance view, int page = 1, int rows = 10)
        {
            var query = viewbll.GetQueryable();
            if (!string.IsNullOrWhiteSpace(view.CN_CODE))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(view.CN_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.CN_ITEM_CODE))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(view.CN_ITEM_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.CN_NAME))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(view.CN_NAME));
            }
            var list = query.OrderByDescending(obj => obj.CN_DT_CREATE).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("物料列表（分页）")]
        public JsonResult PageList(AppItem item, int page = 1, int rows = 10)
        {
            var query = itembll.GetQueryable();
            if (!string.IsNullOrWhiteSpace(item.CN_CODE))
            {
                query = query.Where(obj => obj.CN_CODE.Contains(item.CN_CODE));
            }
            if (!string.IsNullOrWhiteSpace(item.CN_ITEM_CODE))
            {
                query = query.Where(obj => obj.CN_ITEM_CODE.Contains(item.CN_ITEM_CODE));
            }
            if (!string.IsNullOrWhiteSpace(item.CN_NAME))
            {
                query = query.Where(obj => obj.CN_NAME.Contains(item.CN_NAME));
            }
            var list = query.OrderByDescending(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("选装件物料列表（分页）")]
        public JsonResult OptionalItemPageList(ViewOptionalItem view, int page = 1, int rows = 10)
        {
            var query = viewoibll.GetQueryable();
            if (!string.IsNullOrWhiteSpace(view.code))
            {
                query = query.Where(obj => obj.code.Contains(view.code));
            }
            if (!string.IsNullOrWhiteSpace(view.itemcode))
            {
                query = query.Where(obj => obj.itemcode.Contains(view.itemcode));
            }
            if (!string.IsNullOrWhiteSpace(view.name))
            {
                query = query.Where(obj => obj.name.Contains(view.name));
            }
            var list = query.OrderBy(obj => obj.code).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("物料列表，选装件除外（分页）")]
        public JsonResult NoOptionalItemPageList(ViewNoOptionalItem view, int page = 1, int rows = 10)
        {
            var query = viewnoibll.GetQueryable();
            if (!string.IsNullOrWhiteSpace(view.code))
            {
                query = query.Where(obj => obj.code.Contains(view.code));
            }
            if (!string.IsNullOrWhiteSpace(view.name))
            {
                query = query.Where(obj => obj.name.Contains(view.name));
            }
            if (!string.IsNullOrWhiteSpace(view.itemcode))
            {
                query = query.Where(obj => obj.itemcode.Contains(view.itemcode));
            }
            var list = query.OrderByDescending(obj => obj.code).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("物料分类标识列表（分页）")]
        public JsonResult SearchByTypePageList(ViewItemByType view, string[] typenames, int page = 1, int rows = 10)
        {
            if (typenames == null || typenames.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }

            var query = vibytbll.GetQueryable(where => typenames.Contains(where.typename));
            if (!string.IsNullOrWhiteSpace(view.code))
            {
                var codes = view.code.Split(',');
                query = query.Where(obj => codes.Contains(obj.code));
            }
            if (!string.IsNullOrWhiteSpace(view.name))
            {
                query = query.Where(obj => obj.name.Contains(view.name));
            }
            if (!string.IsNullOrWhiteSpace(view.itemcode))
            {
                var itemcodes = view.itemcode.Split(',');
                query = query.Where(obj => itemcodes.Contains(obj.itemcode));
            }
            var list = query.OrderByDescending(obj => obj.code).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("物料分类标识列表，全部分类（分页）")]
        public JsonResult WithTypePageList(ViewItemWithType view, int page = 1, int rows = 10)
        {
            var query = viwtbll.GetQueryable();
            if (!string.IsNullOrWhiteSpace(view.code))
            {
                var codes = view.code.Split(',');
                query = query.Where(obj => codes.Contains(obj.code));
            }
            if (!string.IsNullOrWhiteSpace(view.name))
            {
                query = query.Where(obj => obj.name.Contains(view.name));
            }
            if (!string.IsNullOrWhiteSpace(view.itemcode))
            {
                var itemcodes = view.itemcode.Split(',');
                query = query.Where(obj => itemcodes.Contains(obj.itemcode));
            }
            var list = query.OrderByDescending(obj => obj.code).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
    }
}
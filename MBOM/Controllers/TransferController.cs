using Repository;
using MBOM.Filters;
using MBOM.Models;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MBOM.Unity;
using System;
using Localization;

namespace MBOM.Controllers
{
    [UserAuth]
    public class TransferController : Controller
    {
        private BaseDbContext db;
        public TransferController(BaseDbContext db)
        {
            this.db = db;
        }
        [Description("待发起项目页面")]
        public ActionResult InitiateIndex()
        {
            ViewBag.Param = "待发起";
            return View("~/Views/Maintenance/TransferIndex.cshtml");
        }

        [Description("已发起项目页面")]
        public ActionResult WorkingIndex()
        {
            ViewBag.Param = "已发起";
            return View("~/Views/Maintenance/TransferIndex.cshtml");
        }

        [Description("已转批项目页面")]
        public ActionResult DoneIndex()
        {
            ViewBag.Param = "已转批";
            return View("~/Views/Maintenance/TransferIndex.cshtml");
        }

        [Description("创建产品版本")]
        public JsonResult CreateProductVer(string prod_itemcode, string name, string desc)
        {
            if(string.IsNullOrWhiteSpace(prod_itemcode) || string.IsNullOrWhiteSpace(name))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var userinfo = LoginUserInfo.GetUserInfo();
            try
            {
                var proc = Proc.ProcCreateProductVer(db, prod_itemcode, name, desc, userinfo);
                return Json(ResultInfo.Parse(proc));
            }
            catch (Exception ex)
            {
                return Json(ResultInfo.Fail(ex.Message));
            }
        }

        [Description("转批发起操作")]
        public JsonResult Initiate(string prod_itemcode)
        {
            var userinfo = LoginUserInfo.GetUserInfo();
            ResultInfo resultInfo;
            try
            {
                resultInfo = ResultInfo.Success(Proc.ProcProductTransferInitiate(db, prod_itemcode, userinfo));
            }
            catch(Exception ex)
            {
                resultInfo = ResultInfo.Fail(ex.Message);
            }
            return Json(resultInfo);
        }

        [Description("转批发起操作")]
        public JsonResult Publish(string prod_itemcode)
        {
            var userinfo = LoginUserInfo.GetUserInfo();
            ResultInfo resultInfo;
            try
            {
                resultInfo = ResultInfo.Parse(Proc.ProcProductTransferPublish(db, prod_itemcode, userinfo));
            }
            catch (Exception ex)
            {
                resultInfo = ResultInfo.Fail(ex.Message);
            }
            return Json(resultInfo);
        }

        [Description("预转批列表（分页）")]
        public JsonResult PageList(
            ViewProjectProductPbom prod,
            DateTime? PdmBeginDate, DateTime? PdmEndDate,
            DateTime? PreBeginDate, DateTime? PreEndDate,
            int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = Common.GetQueryFilter(query);
            query = Common.GetQueryFilterUserId(query);
            if (PdmBeginDate != null && PdmBeginDate.Value.Year > 2000)
            {
                query = query.Where(obj => obj.DT_PDM >= PdmBeginDate);
            }
            if (PdmEndDate != null && PdmEndDate.Value.Year < 2100)
            {
                query = query.Where(obj => obj.DT_PDM <= PdmEndDate);
            }
            if (PreBeginDate != null && PreBeginDate.Value.Year > 2000)
            {
                query = query.Where(obj => obj.DT_PRE >= PreBeginDate);
            }
            if (PreEndDate != null && PreEndDate.Value.Year < 2100)
            {
                query = query.Where(obj => obj.DT_PRE <= PreEndDate);
            }
            if (prod.PRODUCT_STATUS == "待发起")
            {
                var inStatus = new string[]
                {
                    "待发起预转批","销售件已设置"
                };
                query = query.Where(obj => inStatus.Contains(obj.PRODUCT_STATUS));
            }
            else if (prod.PRODUCT_STATUS == "已发起")
            {
                var inStatus = new string[]
                {
                    "MBOM维护中","MBOM发布中","MBOM发布完成","转批中"
                };
                query = query.Where(obj => inStatus.Contains(obj.PRODUCT_STATUS));
            }
            else if (prod.PRODUCT_STATUS == "已转批")
            {
                var inStatus = new string[]
                {
                    "已转批"
                };
                query = query.Where(obj => inStatus.Contains(obj.PRODUCT_STATUS));
            }
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_ITEM_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_ITEM_CODE.Contains(prod.PRODUCT_ITEM_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_NAME))
            {
                query = query.Where(obj => obj.PRODUCT_NAME.Contains(prod.PRODUCT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
    }
}
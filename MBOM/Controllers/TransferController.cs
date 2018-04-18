using AutoMapper;
using Repository;
using MBOM.Filters;
using MBOM.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

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
        // GET: Transfer
        [Description("转批发起页面")]
        public ActionResult InitiateIndex()
        {
            return View();
        }

        [Description("转批中页面")]
        public ActionResult WorkingIndex()
        {
            return View();
        }

        [Description("已转批页面")]
        public ActionResult DoneIndex()
        {
            return View();
        }

        [Description("转批发起操作")]
        public JsonResult Initiate(string code)
        {
            var proc = Proc.ProcProductTransferInitiate(db, code);
            return Json(ResultInfo.Success(proc));
        }

        [Description("转批待发布列表（分页）")]
        public JsonResult WaitPageList(ViewProjectProductPbomView prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = query.Where(obj => obj.CN_PRODUCT_STATUS == "待发布");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.CN_PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var list = Mapper.Map<List<ViewProjectProductPbomView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("转批中列表（分页）")]
        public JsonResult WorkingPageList(ViewProjectProductPbomView prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = query.Where(obj => obj.CN_PRODUCT_STATUS == "转批中");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.CN_PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var list = Mapper.Map<List<ViewProjectProductPbomView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("已转批列表（分页）")]
        public JsonResult DonePageList(ViewProjectProductPbomView prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = query.Where(obj => obj.CN_PRODUCT_STATUS == "已转批");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.CN_PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.CN_PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var projs = query.OrderBy(obj => obj.CN_CODE).Skip((page - 1) * rows).Take(rows);
            var list = Mapper.Map<List<ViewProjectProductPbomView>>(projs);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
    }
}
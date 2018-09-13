using AutoMapper;
using Repository;
using MBOM.Filters;
using MBOM.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MBOM.Unity;
using System;

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

        [Description("创建产品版本")]
        public JsonResult CreateProductVer(string prod_itemcode, string name, string desc)
        {
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
            var proc = Proc.ProcProductTransferInitiate(db, prod_itemcode, userinfo);
            return Json(ResultInfo.Success(proc));
        }

        [Description("转批待发布列表（分页）")]
        public JsonResult WaitPageList(ViewProjectProductPbom prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = Common.GetQueryFilter(query);
            //query = Common.GetQueryFilterUserId(query);
            query = query.Where(obj => obj.PRODUCT_STATUS == "待发布");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("转批中列表（分页）")]
        public JsonResult WorkingPageList(ViewProjectProductPbom prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = Common.GetQueryFilter(query);
            //query = Common.GetQueryFilterUserId(query);
            query = query.Where(obj => obj.PRODUCT_STATUS == "转批中");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("已转批列表（分页）")]
        public JsonResult DonePageList(ViewProjectProductPbom prod, int page = 1, int rows = 10)
        {
            var query = db.ViewProjectProductPboms.AsQueryable();
            query = Common.GetQueryFilter(query);
            //query = Common.GetQueryFilterUserId(query);
            query = query.Where(obj => obj.PRODUCT_STATUS == "已转批");
            if (!string.IsNullOrWhiteSpace(prod.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(prod.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(prod.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(prod.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows);
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
    }
}
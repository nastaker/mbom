using Localization;
using MBOM.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    public class ProductionLineController : Controller
    {
        private BaseDbContext db;

        public ProductionLineController(BaseDbContext db)
        {
            this.db = db;
        }
        [Description("生产线管理")]
        public ActionResult Index()
        {
            return View();
        }
        [Description("生产线产品管理")]
        public ActionResult ProductIndex()
        {
            return View();
        }
        [Description("产品工序维护")]
        public ActionResult MaintenanceIndex()
        {
            return View();
        }

        [Description("产品工序版本列表")]
        public JsonResult ProcessVerList(string prod_itemcode)
        {
            if (string.IsNullOrWhiteSpace(prod_itemcode))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var date = DateTime.Now;
            var list = db.AppProductionLineProcessVers
                         .Where(w =>
                                w.CN_PRODUCT_ITEMCODE == prod_itemcode &&
                                w.CN_DT_EFFECTIVE < date &&
                                w.CN_DT_EXPIRY > date).ToList();
            return Json(ResultInfo.Success(list));
        }

        public JsonResult SetMbomProcess(Guid guid_ver, Guid guid_mbom, string guid_process, int type)
        {

            if (guid_ver == Guid.Empty || guid_mbom == Guid.Empty)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            if (type != 2 && string.IsNullOrWhiteSpace(guid_process))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            ResultInfo rt = null;
            try
            {
                rt = ResultInfo.Parse(Proc.ProcSetMbomProcess(db, guid_ver, guid_mbom, guid_process, type, LoginUserInfo.GetUserInfo()));
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [Description("添加产品工序版本")]
        public JsonResult AddProductVer(AppProductionLineProcessVer model)
        {
            if(model.CN_GUID_LINE_PRODUCT == Guid.Empty)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            if (string.IsNullOrWhiteSpace(model.CN_NAME))
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var user = LoginUserInfo.GetLoginUser();
            var date = DateTime.Now;
            model.CN_GUID = Guid.NewGuid();
            model.CN_DT_CREATE = date;
            model.CN_DT_EFFECTIVE = date;
            model.CN_DT_EXPIRY = DateTime.Parse("2100-01-01");
            model.CN_DT_TOERP = DateTime.Parse("2100-01-01");
            model.CN_CREATE_BY = user.UserId;
            model.CN_CREATE_LOGIN = user.Login;
            model.CN_CREATE_NAME = user.Name;
            db.AppProductionLineProcessVers.Add(model);
            db.SaveChanges();
            return Json(ResultInfo.Success("添加成功"));
        }

        [Description("添加、修改产品线")]
        public JsonResult Edit(AppProductionLine model)
        {
            string msg = "修改成功";
            var user = LoginUserInfo.GetLoginUser();
            if (model.CN_ID == 0)
            {
                msg = "添加成功";
                model.CN_GUID = Guid.NewGuid();
                model.CN_DT_CREATE = DateTime.Now;
                model.CN_CREATE_BY = user.UserId;
                model.CN_CREATE_LOGIN = user.Login;
                model.CN_CREATE_NAME = user.Name;
                db.AppProductionLines.Add(model);
            }
            else
            {
                db.AppProductionLines.Attach(model);
                db.Entry(model).Property(x => x.CN_LINE_NAME).IsModified = true;
                db.Entry(model).Property(x => x.CN_LINE_CODE).IsModified = true;
                db.Entry(model).Property(x => x.CN_DT_EFFECTIVE).IsModified = true;
                db.Entry(model).Property(x => x.CN_DT_EXPIRY).IsModified = true;
                db.Entry(model).Property(x => x.CN_LINE_INFO).IsModified = true;
            }
            db.SaveChanges();
            return Json(ResultInfo.Success(msg.ToString()));
        }

        [Description("添加产品线产品")]
        public JsonResult AddProduct(Guid guid_line, string[] itemcodes)
        {
            if (guid_line == Guid.Empty || itemcodes == null || itemcodes.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            List<AppProductionLineProduct> list = new List<AppProductionLineProduct>();
            DateTime now = DateTime.Now;
            var user = LoginUserInfo.GetLoginUser();
            for (int i = 0, j = itemcodes.Length; i < j; i++)
            {
                list.Add(new AppProductionLineProduct
                {
                    CN_GUID = Guid.NewGuid(),
                    CN_GUID_LINE = guid_line,
                    CN_PRODUCT_ITEMCODE = itemcodes[i],
                    CN_DT_EFFECTIVE = now,
                    CN_DT_EXPIRY = DateTime.Parse("2100-01-01"),
                    CN_DT_CREATE = now,
                    CN_CREATE_BY = user.UserId,
                    CN_CREATE_LOGIN = user.Login,
                    CN_CREATE_NAME = user.Name
                });
            }
            db.AppProductionLineProducts.AddRange(list);
            db.SaveChanges();
            return Json(ResultInfo.Success("添加成功！"));
        }

        [Description("删除产品线产品")]
        public JsonResult RemoveProduct(Guid[] guids)
        {
            if (guids == null || guids.Length == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            for(int i = 0, j = guids.Length; i < j; i++)
            {
                if(guids[i] == Guid.Empty)
                {
                    return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
                }
            }
            var list = db.AppProductionLineProducts.Where(w => guids.Contains(w.CN_GUID));
            db.AppProductionLineProducts.RemoveRange(list);
            db.SaveChanges();
            return Json(ResultInfo.Success("删除成功！"));
        }

        [Description("添加产品工序")]
        public JsonResult AddProcess(AppProductionLineProductProcess process)
        {
            if (process.CN_GUID_LINE_PRODUCT == Guid.Empty ||
                string.IsNullOrWhiteSpace(process.CN_PROCESS_NAME) ||
                string.IsNullOrWhiteSpace(process.CN_PROCESS_ORDER)
                )
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            var user = LoginUserInfo.GetLoginUser();
            var now = DateTime.Now;
            process.CN_DT_CREATE = now;
            process.CN_CREATE_BY = user.UserId;
            process.CN_CREATE_NAME = user.Name;
            process.CN_CREATE_LOGIN = user.Login;
            db.AppProductionLineProductProcesses.Add(process);
            db.SaveChanges();
            return Json(ResultInfo.Success("添加成功！"));
        }

        [Description("删除产品工序")]
        public JsonResult RemoveProcess(AppProductionLineProductProcess process)
        {
            if (process.CN_ID == 0)
            {
                return Json(ResultInfo.Fail(Lang.ParamIsEmpty));
            }
            db.AppProductionLineProductProcesses.Attach(process);
            db.AppProductionLineProductProcesses.Remove(process);
            db.SaveChanges();
            return Json(ResultInfo.Success("删除成功！"));
        }

        [Description("产品线列表")]
        public JsonResult PageList(AppProductionLine model, int page = 1, int rows = 10)
        {
            var query = db.AppProductionLines.AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.CN_LINE_NAME))
            {
                query = query.Where(m => m.CN_LINE_NAME.Contains(model.CN_LINE_NAME));
            }
            if (!string.IsNullOrWhiteSpace(model.CN_LINE_CODE))
            {
                query = query.Where(m => m.CN_LINE_CODE.Contains(model.CN_LINE_CODE));
            }
            var list = query.OrderBy(obj => obj.CN_LINE_NAME).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("产品线产品列表")]
        public JsonResult ProductPageList(AppProductionLineProduct model, int page = 1, int rows = 10)
        {
            var query = db.AppProductionLineProducts.AsQueryable();
            if (model.CN_GUID_LINE != Guid.Empty)
            {
                query = query.Where(m => m.CN_GUID_LINE == model.CN_GUID_LINE);
            }
            if (!string.IsNullOrWhiteSpace(model.CN_PRODUCT_ITEMCODE))
            {
                query = query.Where(m => m.CN_PRODUCT_ITEMCODE.Contains(model.CN_PRODUCT_ITEMCODE));
            }
            var list = query.OrderBy(obj => obj.CN_PRODUCT_ITEMCODE).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        [Description("产品线产品工序列表")]
        public JsonResult ProcessPageList(AppProductionLineProductProcess model, int page = 1, int rows = 10)
        {
            var query = db.AppProductionLineProductProcesses.AsQueryable();
            if (model.CN_GUID_LINE_PRODUCT != Guid.Empty)
            {
                query = query.Where(m => m.CN_GUID_LINE_PRODUCT == model.CN_GUID_LINE_PRODUCT);
            }
            if (!string.IsNullOrWhiteSpace(model.CN_PROCESS_NAME))
            {
                query = query.Where(m => m.CN_PROCESS_INFO.Contains(model.CN_PROCESS_INFO));
            }
            if (!string.IsNullOrWhiteSpace(model.CN_PROCESS_ORDER))
            {
                query = query.Where(m => m.CN_PROCESS_INFO.Contains(model.CN_PROCESS_INFO));
            }
            if (!string.IsNullOrWhiteSpace(model.CN_PROCESS_INFO))
            {
                query = query.Where(m => m.CN_PROCESS_INFO.Contains(model.CN_PROCESS_INFO));
            }
            var list = query.OrderBy(obj => obj.CN_PROCESS_ORDER).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }

        
    }
}
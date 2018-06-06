using MBOM.Filters;
using MBOM.Models;
using Repository;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    public class ProductChangeController : Controller
    {
        private BaseDbContext db;

        public ProductChangeController(BaseDbContext db)
        {
            this.db = db;
        }
        [Description("查看MBOM产品变更维护页面")]
        public ActionResult Index()
        {
            return View();
        }
        [Description("查看MBOM产品变更维护菜单列表页面")]
        public ActionResult MenuIndex()
        {
            return View();
        }
        [MaintenanceActionFilter]
        [Description("MBOM产品变更维护主页面")]
        public ActionResult MaintenanceIndex(string code)
        {
            return View();
        }

        [Description("查看产品的PBOM变更列表")]
        public JsonResult DetailList(string prodcode)
        {
            var list = Proc.ProcProductChangeDetail(db, prodcode);
            return Json(ResultInfo.Success(list));
        }

        [MaintenanceActionFilter]
        [Description("应用所有变更")]
        public JsonResult ApplyChanges(string code, string reason)
        {
            ResultInfo rt = null;
            try
            {
                var rtn = Proc.ProcProductChangeApplyChanges(db, code, reason, LoginUserInfo.GetUserInfo());
                rt = ResultInfo.Parse(rtn);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }

        [MaintenanceActionFilter]
        [Description("应用虚件变更")]
        public JsonResult ApplyVirtualChange(string code, string reason)
        {
            ResultInfo rt = null;
            try
            {
                var rtn = Proc.ProcProductChangeApplyVirtualChanges(db, code, reason, LoginUserInfo.GetUserInfo());
                rt = ResultInfo.Success(rtn);
            }
            catch (SqlException ex)
            {
                rt = ResultInfo.Fail(ex.Message);
            }
            return Json(rt);
        }
        
        [Description("MBOM产品变更列表（分页）")]
        public JsonResult PageList(ViewProductChange view, int page = 1, int rows = 10)
        {
            var query = db.ViewProductChanges.AsQueryable();
            if (!string.IsNullOrWhiteSpace(view.PRODUCT_CODE))
            {
                query = query.Where(obj => obj.PRODUCT_CODE.Contains(view.PRODUCT_CODE));
            }
            if (!string.IsNullOrWhiteSpace(view.PROJECT_NAME))
            {
                query = query.Where(obj => obj.PROJECT_NAME.Contains(view.PROJECT_NAME));
            }
            var list = query.OrderBy(obj => obj.CODE).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            return Json(ResultInfo.Success(new { rows = list, total = count }));
        }
    }
}
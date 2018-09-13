using AutoMapper;
using MBOM.Filters;
using MBOM.Models;
using Repository;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;

namespace MBOM.Controllers
{
    [UserAuth]
    [MaintenanceActionFilter]
    public class MaintenanceController : Controller
    {

        private BaseDbContext db;

        public MaintenanceController(BaseDbContext db)
        {
            this.db = db;
        }

        // GET: Maintenance
        [Description("查看产品发布维护页面")]
        public ActionResult Index()
        {
            return View();
        }
        //基本信息
        [Description("查看产品基本信息")]
        public ActionResult BaseInfoIndex(string prod_itemcode)
        {
            var viewModel = db.ViewProjectProductPboms.Where(m => m.PRODUCT_ITEM_CODE == prod_itemcode.Trim());
            if(viewModel.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(viewModel.First());
        }
        //销售件设置
        [Description("查看销售件设置页面")]
        public ActionResult SellIndex(string prod_itemcode)
        {
            return View();
        }

        //工艺BOM
        [Description("查看工艺BOM页面")]
        public ActionResult PBOMIndex(string prod_itemcode)
        {
            return View();
        }
        //制造BOM
        [Description("查看制造BOM页面")]
        public ActionResult MBOMIndex(string prod_itemcode)
        {
            return View();
        }

        [Description("查看物料父级引用页面")]
        public ActionResult BOMParentIndex(string prod_itemcode)
        {
            var item = db.AppItems.SingleOrDefault(where => where.CN_ITEM_CODE == prod_itemcode);
            return View(item);
        }

        //物料清单
        [Description("查看物料清单页面")]
        public ActionResult BOMListIndex(string prod_itemcode)
        {
            return View();
        }

        //自制件清单
        [Description("查看自制件清单页面")]
        public ActionResult SelfMadeListIndex(string prod_itemcode)
        {
            return View();
        }

        //标准件清单
        [Description("查看标准件清单页面")]
        public ActionResult StandardListIndex(string prod_itemcode)
        {
            return View();
        }

        //采购件清单
        [Description("查看采购件清单页面")]
        public ActionResult PurchaseListIndex(string prod_itemcode)
        {
            return View();
        }

        //销售件清单
        [Description("查看销售件清单页面")]
        public ActionResult SellListIndex(string prod_itemcode)
        {
            return View();
        }

        //工序流程清单
        [Description("查看工序流程清单页面")]
        public ActionResult ProcessFlowListIndex(string prod_itemcode)
        {
            return View();
        }
    }
}
using AutoMapper;
using BLL;
using MBOM.Filters;
using MBOM.Models;
using Microsoft.Practices.Unity;
using System.ComponentModel;
using System.Web.Mvc;

namespace MBOM.Controllers
{
    [UserAuth]
    [MaintenanceActionFilter]
    public class MaintenanceController : Controller
    {
        [Dependency]
        public ViewProjectProductPbomBLL viewbll { get; set; }
        [Dependency]
        public AppItemBLL itembll { get; set; }
        [Dependency]
        public AppProductBLL prodbll { get; set; }
        [Dependency]
        public PROCBLL procbll { get; set; }
        // GET: Maintenance
        [Description("查看产品发布维护页面")]
        public ActionResult Index()
        {
            return View();
        }
        //基本信息
        [Description("查看产品基本信息")]
        public ActionResult BaseInfoIndex(string code)
        {
            var viewModel = viewbll.Get(m => m.CN_PRODUCT_CODE == code.Trim());
            if(viewModel == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<ViewProjectProductPbomView>(viewModel));
        }
        //销售件设置
        [Description("查看销售件设置页面")]
        public ActionResult SaleSetIndex(string code)
        {
            return View();
        }

        //工艺BOM
        [Description("查看工艺BOM页面")]
        public ActionResult PBOMIndex(string code)
        {
            return View();
        }
        //制造BOM
        [Description("查看制造BOM页面")]
        public ActionResult MBOMIndex(string code)
        {
            return View();
        }

        [Description("查看物料父级引用页面")]
        public ActionResult BOMParentIndex(string code)
        {
            var item = itembll.Get(where => where.CN_CODE == code);
            return View(item);
        }

        //物料清单
        [Description("查看物料清单页面")]
        public ActionResult BOMListIndex(string code)
        {
            return View();
        }

        //自制件清单
        [Description("查看自制件清单页面")]
        public ActionResult SelfMadeListIndex(string code)
        {
            return View();
        }

        //标准件清单
        [Description("查看标准件清单页面")]
        public ActionResult StandardListIndex(string code)
        {
            return View();
        }

        //采购件清单
        [Description("查看采购件清单页面")]
        public ActionResult PurchaseListIndex(string code)
        {
            return View();
        }

        //销售件清单
        [Description("查看销售件清单页面")]
        public ActionResult SellListIndex(string code)
        {
            return View();
        }

        //工序流程清单
        [Description("查看工序流程清单页面")]
        public ActionResult ProcessFlowListIndex(string code)
        {
            return View();
        }
    }
}
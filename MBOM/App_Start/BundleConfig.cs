using System.Web;
using System.Web.Optimization;

namespace MBOM
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery")
				.Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
				.Include("~/Scripts/jquery.validate*"));
            // bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/umd/popper.js")
                .Include("~/Scripts/bootstrap.js"));
            // 检查浏览器标准
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
				.Include("~/Scripts/modernizr-*"));
            // 通用方法
            bundles.Add(new ScriptBundle("~/bundles/common")
                .Include("~/Scripts/views/lang.js",
                      "~/Scripts/views/common.js"));
            // ztree
            bundles.Add(new ScriptBundle("~/bundles/ztree")
                .Include("~/Scripts/ztree/jquery.ztree.core.js"));
            // jstree
            bundles.Add(new ScriptBundle("~/bundles/js/jstree")
                .Include("~/Scripts/jsTree3/jstree.js"));
            //
            //  MBOM详情
            //

            bundles.Add(new ScriptBundle("~/bundles/pbom")
				.Include("~/Scripts/views/maintenance/pbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbom")
                .Include("~/Scripts/views/maintenance/mbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/bom")
                .Include("~/Scripts/views/maintenance/bom.js"));
            bundles.Add(new ScriptBundle("~/bundles/bomlist")
				.Include("~/Scripts/views/maintenance/bomlist.js"));
            bundles.Add(new ScriptBundle("~/bundles/bomlist2")
                .Include("~/Scripts/views/maintenance/bomlist2.js"));
            bundles.Add(new ScriptBundle("~/bundles/processflow")
				.Include("~/Scripts/views/maintenance/processflow.js"));
            bundles.Add(new ScriptBundle("~/bundles/saleset")
				.Include("~/Scripts/views/maintenance/saleset.js"));
            bundles.Add(new ScriptBundle("~/bundles/sell")
				.Include("~/Scripts/views/maintenance/sell.js"));
            bundles.Add(new ScriptBundle("~/bundles/menudata1")
                .Include("~/Scripts/views/maintenance/menuData.js"));
            //
            //  MBOM维护
            //
            bundles.Add(new ScriptBundle("~/bundles/mbomindex")
                .Include("~/Scripts/views/mbom/index.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbommaintenance")
                .Include("~/Scripts/views/mbom/maintenance.js"));
            bundles.Add(new ScriptBundle("~/bundles/ComponentIndex")
                .Include("~/Scripts/views/mbom/ComponentIndex.js"));
            bundles.Add(new ScriptBundle("~/bundles/ComponentMaintenance")
                .Include("~/Scripts/views/mbom/ComponentMaintenance.js"));
            bundles.Add(new ScriptBundle("~/bundles/productpbom")
                .Include("~/Scripts/views/mbom/productpbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/productmbom")
                .Include("~/Scripts/views/mbom/productmbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/changeindex")
                .Include("~/Scripts/views/mbom/changeindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/optionalitemsindex")
				.Include("~/Scripts/views/mbom/optionalitemsindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/optionalitemsetindex")
				.Include("~/Scripts/views/mbom/optionalitemsetindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/productlibrary")
                .Include("~/Scripts/views/mbom/productlibrary.js"));
            bundles.Add(new ScriptBundle("~/bundles/itemchangedetail")
				.Include("~/Scripts/views/mbom/itemchangedetail.js"));
            bundles.Add(new ScriptBundle("~/bundles/menuData2")
                .Include("~/Scripts/views/mbom/menuData.js"));
            bundles.Add(new ScriptBundle("~/bundles/menuData3")
                .Include("~/Scripts/views/mbom/productmenuData.js"));
            bundles.Add(new ScriptBundle("~/bundles/itemhlinksetindex")
                .Include("~/Scripts/views/mbom/itemhlinksetindex.js"));
            // 首页
            bundles.Add(new ScriptBundle("~/bundles/menu")
                .Include("~/Scripts/views/menu.js"));
            // 域
            bundles.Add(new ScriptBundle("~/bundles/groupindex")
				.Include("~/Scripts/views/group/index.js"));
            //
            // 样式表
            //

            // 全局bootstrap样式
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/bootstrap.css", "~/Content/site.css"));
            // 全局font-awesome样式
            bundles.Add(new StyleBundle("~/bundles/fonts")
                .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));
            // 全局easyui样式
            bundles.Add(new StyleBundle("~/bundles/easyui")
                .Include("~/Scripts/easyui/themes/bootstrap/easyui.css", new CssRewriteUrlTransform())
                .Include("~/Scripts/easyui/themes/icon.css", new CssRewriteUrlTransform()));

            // ztree 样式
            bundles.Add(new StyleBundle("~/bundles/ztreecss")
                .Include("~/Content/ztree/awesomeStyle/awesome.css", new CssRewriteUrlTransform()));
            // jstree样式
            bundles.Add(new StyleBundle("~/bundles/css/jstree")
                .Include("~/Content/jsTree/themes/default/style.css", new CssRewriteUrlTransform()));

            // mbom制作样式
            bundles.Add(new StyleBundle("~/bundles/mbomcss")
                .Include("~/Content/easyui-icons.css", new CssRewriteUrlTransform())
                .Include("~/Content/views/mbom/maintenance.css"));
        }
    }
}

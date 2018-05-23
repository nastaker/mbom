using System.Web;
using System.Web.Optimization;

namespace MBOM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/ztree").Include(
                      "~/Scripts/ztree/jquery.ztree.core.js"));
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                      "~/Scripts/views/lang.js",
                      "~/Scripts/views/common.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbommaintenance").Include(
                      "~/Scripts/views/gradientColor.js",
                      "~/Scripts/views/mbom/maintenance.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbomchangemaintenance").Include(
                      "~/Scripts/views/mbom/changemaintenance.js"));
            bundles.Add(new ScriptBundle("~/bundles/pbom").Include(
                      "~/Scripts/views/maintenance/pbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbom").Include(
                      "~/Scripts/views/maintenance/mbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/bomlist").Include(
                      "~/Scripts/views/maintenance/bomlist.js"));
            bundles.Add(new ScriptBundle("~/bundles/productpbom").Include(
                      "~/Scripts/views/mbom/productpbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/productmbom").Include(
                      "~/Scripts/views/mbom/productmbom.js"));
            bundles.Add(new ScriptBundle("~/bundles/processflow").Include(
                      "~/Scripts/views/maintenance/processflow.js"));
            bundles.Add(new ScriptBundle("~/bundles/purchase").Include(
                      "~/Scripts/views/maintenance/purchase.js"));
            bundles.Add(new ScriptBundle("~/bundles/saleset").Include(
                      "~/Scripts/views/maintenance/saleset.js"));
            bundles.Add(new ScriptBundle("~/bundles/selfmade").Include(
                      "~/Scripts/views/maintenance/selfmade.js"));
            bundles.Add(new ScriptBundle("~/bundles/sell").Include(
                      "~/Scripts/views/maintenance/sell.js"));
            bundles.Add(new ScriptBundle("~/bundles/standard").Include(
                      "~/Scripts/views/maintenance/standard.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbomindex").Include(
                      "~/Scripts/views/mbom/index.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbomproductchangeindex").Include(
                      "~/Scripts/views/mbom/mbomproductchangeindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/changeindex").Include(
                      "~/Scripts/views/mbom/changeindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/optionalitemsindex").Include(
                      "~/Scripts/views/mbom/optionalitemsindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/optionalitemsetindex").Include(
                      "~/Scripts/views/mbom/optionalitemsetindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/pbomchangeprod").Include(
                      "~/Scripts/views/mbom/pbomchangeprod.js"));
            bundles.Add(new ScriptBundle("~/bundles/pbomchangeitem").Include(
                      "~/Scripts/views/mbom/pbomchangeitem.js"));
            bundles.Add(new ScriptBundle("~/bundles/prodchangedetail").Include(
                "~/Scripts/views/mbom/prodchangedetail.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbomcreatepublish").Include(
                "~/Scripts/views/mbom/mbomcreatepublish.js"));
            bundles.Add(new ScriptBundle("~/bundles/mbomcreatepublishdetail").Include(
                "~/Scripts/views/mbom/mbomcreatepublishdetail.js"));
            bundles.Add(new ScriptBundle("~/bundles/itemchangedetail").Include(
                "~/Scripts/views/mbom/itemchangedetail.js"));
            bundles.Add(new ScriptBundle("~/bundles/menu").Include(
                      "~/Scripts/views/menu.js"));
            bundles.Add(new ScriptBundle("~/bundles/menudata1").Include(
                      "~/Scripts/views/maintenance/menuData.js"));
            bundles.Add(new ScriptBundle("~/bundles/menuData2").Include(
                      "~/Scripts/views/mbom/menuData.js"));
            bundles.Add(new ScriptBundle("~/bundles/menuData3").Include(
                      "~/Scripts/views/mbom/productmenuData.js"));
            bundles.Add(new ScriptBundle("~/bundles/itemhlinksetindex").Include(
                        "~/Scripts/views/mbom/itemhlinksetindex.js"));
            bundles.Add(new ScriptBundle("~/bundles/groupindex").Include(
                      "~/Scripts/views/group/index.js"));


            bundles.Add(new StyleBundle("~/bundles/ztreecss").Include(
                      "~/Content/ztree/zTreeStyle/zTreeStyle.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/bundles/fonts")
                .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/bundles/easyui")
                .Include("~/Scripts/easyui/themes/bootstrap/easyui.css", new CssRewriteUrlTransform())
                .Include("~/Scripts/easyui/themes/icon.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/mbomcss")
                .Include("~/Content/easyui-icons.css", new CssRewriteUrlTransform())
                .Include("~/Content/views/mbom/maintenance.css"));



        }
    }
}

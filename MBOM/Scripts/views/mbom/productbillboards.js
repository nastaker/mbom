var URL_PAGELIST = "/MBOM/ProductBillboardsPageList"
var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        fitColumns: true,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 15 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 15 },
            { field: "CN_NAME", title: "物料名称", width: 20 },
            { field: "CN_STATUS", title: "状态", align: "center", width: 10 },
            {
                field: "PDM_RELEASE_DATE", title: "PDM发布日期", align: "center", width: 15,
                formatter: function (value, row, index) {
                    return ToJavaScriptDate(value);
                }
            },
            {
                field: "TOERP_DATE", title: "ERP发布日期", align: "center", width: 15,
                formatter: function (value, row, index) {
                    return ToJavaScriptDate(value);
                }
            }
        ]],
        loadFilter: function (result) {
            if (result.success) {
                return result.data;
            } else {
                return [];
            }
        }
    });
});

function query() {
    var param = $("#queryFrm").serializeJSON();
    dg.datagrid("load", param);
}

function productinfo() {
    var dgprod = $("#dgProducts");
    var prod = dgprod.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var prod_itemcode = prod.CN_ITEM_CODE;
    var title = "产品详情" + prod.CN_ITEM_CODE + " " + prod.CN_NAME;
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?prod_itemcode=" + prod_itemcode);
}
var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_MAINTENANCELIST = "/MBOM/MaterialBillboardsPageList"
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        height: "100%",
        fitColumns: true,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "PROJECT_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 15 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 15 },
            { field: "CN_NAME", title: "物料名称", width: 15 },
            {
                field: "CN_IS_TOERP", title: "是否发布", align: "center", width: 6,
                formatter: function (value, row, index) {
                    if (value == 0) {
                        return "";
                    }
                    return "已发";
                }
            },
            {
                field: "CN_DT_EFFECTIVE_ERP", title: "发布日期", align: "center", width: 10,
                formatter: function (value, row, index) {
                    if (row["CN_IS_TOERP"] == 1) {
                        return ToJavaScriptDate(value);
                    }
                    return "";
                }
            },
            { field:"销售件", title:"销售件", align: "center", width: 8 },
            { field: "采购件", title: "采购件", align: "center", width: 8 },
            { field: "自制件", title: "自制件", align: "center", width: 8 },
            { field: "MBOM虚拟件", title: "MBOM虚拟件", align: "center", width: 8 },
            { field: "MBOM合件", title: "MBOM合件", align: "center", width: 8 }
        ]],
        loadFilter: loadFilter
    });
    var opts = dg.datagrid("options");
    opts.url = URL_MAINTENANCELIST;
});

function query() {
    var param = $("#queryFrm").serializeJSON();
    dg.datagrid("load", param);
}

function showDetail() {
    var item = dg.datagrid("getSelected");
    if (!item) { return; }
    var title = "物料详情" + item.CN_ITEM_CODE + " " + item.CN_NAME;
    var prod_itemcode = item.CN_ITEM_CODE
    window.parent.openTab(title, URL_ITEMDETAIL + "?prod_itemcode=" + prod_itemcode);
}
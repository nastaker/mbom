var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_MAINTENANCELIST = "/MBOM/MaterialBillboardsPageList"
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "ItemCode",
        toolbar: '#toolbar',
        columns: [[
            { field: "Code", title: "代号", width: 150 },
            { field: "ItemCode", title: "物料编码", width: 150 },
            { field: "ItemName", title: "物料名称", width: 150 },
            {
                field: "ErpStatus", title: "发布状态", align: "center", width: 60,
                formatter: function (value, row, index) {
                    switch (value) {
                        case 0:
                            return "未发布";
                        case 1:
                            return "发布中";
                        case 2:
                            return "已发布";
                        default:
                            return "未知状态";
                    }
                }
            },
            {
                field: "ErpDate", title: "发布日期", align: "center", width: 70,
                formatter: function (value, row, index) {
                    if (row["ErpDate"]) {
                        return ToJavaScriptDate(value);
                    }
                    return "";
                }
            },
            {
                field: "Sell", title: "销售件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "Purchase", title: "采购件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "SelfMade", title: "自制件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "Standard", title: "标准件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "RawMaterial", title: "原材料", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "Package", title: "包装件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "Process", title: "工艺件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "Assembly", title: "装配件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            },
            {
                field: "MBOMOptional", title: "MBOM选装件", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "Y";
                    }
                    return "";
                }
            }
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
    var title = "物料详情" + item.ItemCode + " " + item.ItemName;
    var prod_itemcode = item.ItemCode
    window.parent.openTab(title, URL_ITEMDETAIL + "?prod_itemcode=" + prod_itemcode);
}
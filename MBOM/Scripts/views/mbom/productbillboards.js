var URL_PAGELIST = "/MBOM/ProductBillboardsPageList"
var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_ITEM_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 150 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 150 },
            { field: "CN_NAME", title: "物料名称", width: 150 },
            { field: "CN_STATUS", title: "状态", align: "center", width: 100 },
            {
                field: "CN_DT_PDM", title: "PDM日期", align: "center", width: 100,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                    return "";
                }
            },
            {
                field: "CN_DT_SELL", title: "销售件日期", align: "center", width: 100,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                    return "";
                }
            },
            {
                field: "CN_DT_PRE", title: "预转批日期", align: "center", width: 100,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                    return "";
                }
            },
            {
                field: "CN_DT_TOERP", title: "ERP发布日期", align: "center", width: 100,
                formatter: function (value, row, index) {
                    if (value) {
                        var d = ToJavaScriptDate(value);
                        if (d != "2100-01-01") {
                            return d;
                        }
                        return "";
                    }
                    return "";
                }
            },
            {
                field: "CN_IS_TOERP", title: "ERP状态", align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value == 0) {
                        return "未发布";
                    } else if (value == 1) {
                        return "发布中";
                    } else if (value == 2) {
                        return "已发布";
                    }
                    return "";
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


function clearDate() {
    $("#txtDtBeginPDM").datebox("setText", "2000-01-01");
    $("#txtDtBeginPRE").datebox("setText", "2000-01-01");
    $("#txtDtEndPDM").datebox("setText", "2100-01-01");
    $("#txtDtEndPRE").datebox("setText", "2100-01-01");
    $("#txtDtBeginPDM").datebox("setValue", "2000-01-01");
    $("#txtDtBeginPRE").datebox("setValue", "2000-01-01");
    $("#txtDtEndPDM").datebox("setValue", "2100-01-01");
    $("#txtDtEndPRE").datebox("setValue", "2100-01-01");
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
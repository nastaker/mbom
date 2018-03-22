var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var dgprod = $("#dgProducts");
$(function () {
    dgprod.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        fitColumns: true,
        striped: true,
        rownumbers: true,
        singleSelect: singleSelect,
        pagination: true,
        border: false,
        idField: "BOM_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: 'PROJECT_NAME', title: lang.projectName, width: 20 },
            { field: 'PRODUCT_NAME', title: lang.productName, width: 20 },
            { field: 'PRODUCT_CODE', title: lang.productCode, width: 20 },
            { field: 'PRODUCT_STATUS', title: lang.status, align: "center", width: 5 },
            { field: 'SALE_SET', title: lang.maintenance.saleSet, align: "center", width: 5 }
        ]],
        loadFilter: loadFilter,
        onLoadSuccess: function () {
            $(this).datagrid("clearSelections");
        }
    });
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dgprod.datagrid("load", data);
}
function queryDefault() {
    dgprod.datagrid("load", {});
}
function productReleaseMaintenance() {
    var prod = dgprod.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var code = prod.PRODUCT_CODE;
    var title = prod.PROJECT_NAME + " " + prod.PRODUCT_CODE + " " + prod.PRODUCT_NAME;
    window.parent.openTab(title, "/Maintenance/Index?code=" + code);
}
function transferInitiate() {
    var prods = dgprod.datagrid("getSelections");
    if (prods == null || prods.length == 0) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var prodcodes = "";
    for (var i in prods) {
        var prod = prods[i];
        prodcodes = $.trim(prod["PRODUCT_CODE"]) + "," + prodcodes;
    }
    prodcodes = prodcodes.substr(0, prodcodes.length - 1);
    var param = {
        code: prodcodes
    };
    postData(URL_PUBLISH, param, function (result) {
        if (result.success) {
            InfoWin(result.data.MSG);
        } else {
            AlertWin(result.msg != null ? result.msg : lang.initiateFailed);
        }
    });
}

function productinfo() {
    var prod = dgprod.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var code = prod["PRODUCT_CODE"];
    var title = "产品详情" + prod["PRODUCT_CODE"] + " " + prod["PRODUCT_NAME"];
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?code=" + code);
}
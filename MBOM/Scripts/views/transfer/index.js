var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var URL_CREATEPRODVER = "/Transfer/CreateProductVer";
var dgprod = $("#dgProducts");
var dlgProdVer = $("#dlgProdVerCreate");
$(function () {
    dgprod.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: singleSelect,
        pagination: true,
        border: false,
        idField: "BOM_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: 'PROJECT_NAME', title: lang.projectName, width: 250 },
            { field: 'PRODUCT_NAME', title: lang.productName, width: 140 },
            { field: 'PRODUCT_CODE', title: lang.productCode, width: 120 },
            { field: 'PRODUCT_STATUS', title: lang.status, align: "center", width: 80 },
            { field: 'PBOMVER', title: lang.pbom.ver, width: 80, align: "center" },
            { field: 'PBOMVER_CREATE_NAME', title: lang.pbom.createname, align: "center", width: 80 },
            { field: 'PRODVER_NAME', title: lang.transfer.prodver, align: "center", width: 80 },
            {
                field: 'PRODVER_STATUS', title: lang.transfer.prodverstatus, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value == null) {
                        return ""
                    } else if (value == 0) {
                        return "未发布"
                    } else {
                        return "已发布"
                    }
                }
            },
            { field: 'SALE_SET', title: lang.maintenance.saleSet, align: "center", width: 80 }
        ]],
        loadFilter: loadFilter,
        onLoadSuccess: function () {
            $(this).datagrid("clearSelections");
        }
    });

    dlgProdVer.dialog({
        title: '创建版本',
        footer: "#dlgCreateProdVerFooter",
        width: 350,
        height: 240,
        closed: true,
        modal: true
    });
});

function multiSelectSwitch(checked) {
    dgprod.datagrid({
        singleSelect: checked
    })
}

function query() {
    var data = $("#queryFrm").serializeJSON();
    dgprod.datagrid("load", data);
}

function productReleaseMaintenance() {
    var prod = dgprod.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    if (prod.PRODVER_NAME == null || prod.PRODVER_STATUS == null || prod.PRODVER_STATUS > 0) {
        dlgProdVer.dialog("open");
        dlgProdVer.dialog("center");
        return;
    }
    var prod_itemcode = prod.PRODUCT_ITEM_CODE;
    var title = prod.PROJECT_NAME + " " + prod.PRODUCT_ITEM_CODE + " " + prod.PRODUCT_NAME;
    window.parent.openTab(title, "/Maintenance/Index?prod_itemcode=" + prod_itemcode);
}
function createProdVerConfirm() {
    var prod = dgprod.datagrid("getSelected");
    var valid = $("#frmProdVerCreate").form("validate");
    if (!valid) {
        return;
    }
    var data = $("#frmProdVerCreate").serializeJSON();
    postData(URL_CREATEPRODVER, $.extend({ prod_itemcode: prod.PRODUCT_ITEM_CODE }, data), function (result) {
        if (!result.success) {
            AlertWin(result.msg);
            return;
        } else {
            dgprod.datagrid("reload");
            dlgProdVer.dialog("close");
            var prod_itemcode = prod.PRODUCT_ITEM_CODE;
            var title = prod.PROJECT_NAME + " " + prod.PRODUCT_ITEM_CODE + " " + prod.PRODUCT_NAME;
            window.parent.openTab(title, "/Maintenance/Index?prod_itemcode=" + prod_itemcode);
        }
    })
}

function transferInitiate() {
    var prods = dgprod.datagrid("getSelections");
    if (prods == null || prods.length == 0) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var prodcodes = "";
    for (var i = 0, len = prods.length; i < len; i++) {
        var prod = prods[i];
        prodcodes = $.trim(prod["PRODUCT_CODE"]) + "," + prodcodes;
    }
    prodcodes = prodcodes.substr(0, prodcodes.length - 1);
    var param = {
        prod_itemcode: prodcodes
    };
    postData(URL_PUBLISH, param, function (result) {
        if (result.success) {
            InfoWin(result.data.msg);
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
    var prod_itemcode = prod["PRODUCT_ITEM_CODE"];
    var title = "产品详情" + prod["PRODUCT_ITEM_CODE"] + " " + prod["PRODUCT_NAME"];
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?prod_itemcode=" + prod_itemcode);
}
var singleSelect = true;
var URL_PAGELIST = "/Transfer/PageList"
var URL_PUBLISH = "/Transfer/Initiate"
var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var URL_CREATEPRODVER = "/Transfer/CreateProductVer";
var URL_PRODUCT_PUBLISH = "/Transfer/Publish";
var dgprod = $("#dgProducts");
var dlgProdVer = $("#dlgProdVerCreate");
$(function () {
    dgprod.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: singleSelect,
        pagination: true,
        border: false,
        idField: "BOM_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: 'PROJECT_NAME', title: lang.transfer.projectName, width: 220 },
            { field: 'PRODUCT_NAME', title: lang.transfer.productName, width: 160 },
            { field: 'PRODUCT_CODE', title: lang.transfer.productCode, width: 120 },
            { field: 'PRODUCT_ITEM_CODE', title: lang.transfer.productItemCode, width: 120 },
            { field: 'PRODUCT_STATUS', title: lang.transfer.status, align: "center", width: 80 },
            {
                field: 'DT_PDM', title: lang.transfer.pdmDate, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: 'DT_PRE', title: lang.transfer.preDate, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            { field: 'PBOMVER', title: lang.pbom.ver, width: 80, align: "center" },
            { field: 'PBOMVER_CREATE_NAME', title: lang.pbom.createname, align: "center", width: 80 },
            { field: 'PRODVER_NAME', title: lang.transfer.prodver, align: "center", width: 80 },
            {
                field: 'PRODVER_STATUS', title: lang.transfer.prodverstatus, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value == 0) {
                        return "未发布"
                    } else if (value == 1) {
                        return "发布中"
                    } else if (value == 2) {
                        return "已发布"
                    } else {
                        return ""
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

    dgprod.datagrid("options").url = URL_PAGELIST;
    query();
});

function multiSelectSwitch(checked) {
    dgprod.datagrid({
        singleSelect: checked
    })
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

function query() {
    var data = $("#queryFrm").serializeJSON();
    dgprod.datagrid("load", data);
}

function productMaintenance() {
    var prod = dgprod.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    if (prod.PRODVER_NAME == null || prod.PRODVER_STATUS == null || prod.PRODVER_STATUS > 0) {
        dlgProdVer.dialog("open");
        dlgProdVer.dialog("center");
        var ver = "";
        if (prod["PRODVER_NAME"]) {
            var verNo = (parseInt(prod["PRODVER_NAME"].substr(1)) + 1);
            if (isNaN(verNo)) {
                ver = "S2"
            } else {
                ver = "S" + verNo;
            }
        } else {
            ver = "S1";
        }
        $("#txtName").textbox("setText", ver);
        $("#txtName").textbox("setValue", ver);
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

function transferPrePublish() {
    var prods = dgprod.datagrid("getSelections");
    if (prods == null || prods.length == 0) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var notSetSellItem = false;
    var prod_itemcodes = "";
    for (var i = 0, len = prods.length; i < len; i++) {
        var prod = prods[i];
        if ($.trim(prod["SALE_SET"]) != "设置完成") {
            notSetSellItem = true;
        }
        prod_itemcodes = $.trim(prod["PRODUCT_ITEM_CODE"]) + "," + prod_itemcodes;
    }
    prod_itemcodes = prod_itemcodes.substr(0, prod_itemcodes.length - 1);
    var param = {
        prod_itemcode: prod_itemcodes
    };
    if (notSetSellItem) {
        if (confirm("需要预转批的产品尚未设置销售件，是否仍然要预转批？")) {
            prePublish(param);
        }
    } else {
        if (confirm("是否确认预转批？")) {
            prePublish(param);
        }
    }
}

function prePublish(param) {
    postData(URL_PUBLISH, param, function (result) {
        if (result.success) {
            InfoWin(result.data.msg);
        } else {
            AlertWin(result.msg != null ? result.msg : lang.initiateFailed);
        }
        dgprod.datagrid("reload");
    });
}

function transferPublish() {
    var prods = dgprod.datagrid("getSelections");
    if (prods == null || prods.length == 0) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var notSetSellItem = false;
    var prod_itemcodes = "";
    for (var i = 0, len = prods.length; i < len; i++) {
        var prod = prods[i];
        if ($.trim(prod["SALE_SET"]) != "设置完成") {
            notSetSellItem = true;
        }
        prod_itemcodes = $.trim(prod["PRODUCT_ITEM_CODE"]) + "," + prod_itemcodes;
    }
    prod_itemcodes = prod_itemcodes.substr(0, prod_itemcodes.length - 1);
    var param = {
        prod_itemcode: prod_itemcodes
    };
    if (notSetSellItem) {
        AlertWin("产品未设置销售件");
        return;
    }
    if (confirm("是否确认直接发布？")) {
        postData(URL_PRODUCT_PUBLISH, param, function (result) {
            if (!result.success) {
                AlertWin(result.msg);
            } else {
                InfoWin(result.msg);
            }
            dgprod.datagrid("reload");
        })
    }
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
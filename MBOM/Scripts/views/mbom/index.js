var URL_MAINTENANCELIST = "/MBOM/MaintenancePageList";
var URL_MAINTENANCEPAGE = "/MBOM/MenuIndex";
var URL_MBOM_VER_CREATE = "/MBOM/CreateVer";
var URL_MBOMRELEASE = "/MBOM/Release";
var URL_MBOMMARK = "/MBOM/Mark";
var dg = $("#dgProducts");
var dlgCreateMBOMVer = $("#dlgCreateMBOMVer");
$(function () {
    dg.datagrid({
        url: URL_MAINTENANCELIST,
        height: "100%",
        queryParams: {
            MBOMVER_IS_TOERP: 0
        },
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "PBOMVER_GUID",
        toolbar: '#toolbar',
        rowStyler: function (index, row) {
            if(row["MARK"])
            return { style: "background-color:#66ccff" };
        },
        columns: [[
            { field: 'PROJECT_NAME', title: lang.transfer.projectName, width: 200 },
            { field: 'PRODUCT_NAME', title: lang.transfer.productName, width: 180 },
            { field: 'PRODUCT_CODE', title: lang.transfer.productCode, width: 120 },
            { field: 'PRODUCT_ITEM_CODE', title: lang.transfer.productItemCode, width: 120 },
            { field: 'PBOMVER', title: lang.pbom.ver, align: "center", width: 70 },
            { field: 'PBOM_CREATE_NAME', title: lang.pbom.createname, align: "center", width: 80 },
            { field: 'MBOMVER', title: lang.transfer.mbomVer, align: "center", width: 70 },
            {
                field: 'MBOMVER_IS_TOERP', title: lang.mbom.istoerp, align: "center", width: 70,
                formatter: function (value, row, index) {
                    switch (value) {
                        case 0:
                            return "未发布";
                        case 1:
                            return "发布中";
                        case 2:
                            return "已发布";
                        default:
                            break;
                    }
                }
            },
            { field: 'MBOM_CREATE_NAME', title: lang.mbom.createname, align: "center", width: 80 },
            { field: 'CHECK_STATUS', title: lang.mbom.checkStatus, align: "center", width: 70 },
            { field: 'TECH_STATUS', title: lang.mbom.techStatus, align: "center", width: 60 },
            { field: 'DESC', title: lang.remarks, width: 200 }
        ]],
        loadFilter: loadFilter
    });

    dlgCreateMBOMVer.dialog({
        title: '创建新的MBOM版本',
        footer: "#dlgCreateMBOMVerFooter",
        width: 360,
        height: 370,
        closed: true,
        modal: true
    });
});

function multiSelectSwitch(checked) {
    dg.datagrid({
        singleSelect: checked
    })
}
function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid("load", data);
}
function createMbomVerConfirm() {
    var prod = dg.datagrid("getSelected");
    if (prod === null) {
        AlertWin(lang.mbom.selectProductMbomVer);
        return false;
    }
    var ver = $("#txtVer").textbox("getText");
    var dtef = $("#txtEfDate").textbox("getText");
    var dtex = $("#txtExDate").textbox("getText");
    var pbomver_guid = prod["PBOMVER_GUID"];
    var desc = $("#txtDesc").textbox("getText");
    postData(URL_MBOM_VER_CREATE, {
        prod_itemcode: prod.PRODUCT_ITEM_CODE,
        ver: ver,
        dtef: dtef,
        dtex: dtex,
        pbomver_guid: pbomver_guid,
        desc: desc
    }, function (result) {
        $.messager.confirm("提示", "是否进入维护页面！", function (r) {
            if (r) {
                openMaintenanceTab();
            }
        });
        InfoWin(result.msg);
        dlgCreateMBOMVer.dialog("close");
    });
}
function publishMaintenance() {
    var prod = dg.datagrid("getSelected");
    if (prod === null) {
        AlertWin(lang.mbom.selectProductMbomVer);
        return false;
    }
    if (prod["MBOMVER_IS_TOERP"] > 0 || $.trim(prod["MBOMVER"]) == "") {
        var ver = "M1";
        if (prod.MBOMVER) {
            ver = "M" + (parseInt(prod.MBOMVER.substr(1, prod.MBOMVER.length)) + 1);
        }
        dlgCreateMBOMVer.dialog("open");
        $("#txtVer").textbox("setText", ver);
        $("#txtDesc").textbox("setText", "");
    } else {
        openMaintenanceTab();
    }
}

function openMaintenanceTab() {
    var prod = dg.datagrid("getSelected");
    if (prod === null) {
        AlertWin(lang.mbom.selectProductMbomVer);
        return false;
    }
    var param = "?prod_itemcode=" + prod.PRODUCT_ITEM_CODE;
    var title = $.trim(prod.PRODUCT_NAME) + "【" + $.trim(prod.PRODUCT_CODE) + "】";
    window.parent.openTab(title, URL_MAINTENANCEPAGE + param);
}

function publish() {
    var prods = dg.datagrid("getSelections");
    if (prods.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var prod_itemcodes = "";
    for (var i = 0, j = prods.length; i < j; i++) {
        var prod = prods[i];
        prod_itemcodes = prod.PRODUCT_ITEM_CODE + "," + prod_itemcodes;
    }
    prod_itemcodes = prod_itemcodes.substr(0, prod_itemcodes.length - 1);
    $.messager.confirm("提示", "您将发布产品[" + prod_itemcodes+"]，请您确认！", function (r) {
        if (r) {
            var param = {
                prod_itemcode: prod_itemcodes
            };
            postData(URL_MBOMRELEASE, param, function (result) {
                if (result.success) {
                    InfoWin(result.msg);
                } else {
                    AlertWin(result.msg);
                }
                dg.datagrid("clearSelections");
                dg.datagrid("reload");
            });
        }
    });
}
function mark() {
    var prod = dg.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var param = {
        prod_itemcode: prod.PRODUCT_ITEM_CODE
    };
    postData(URL_MBOMMARK, param, function (result) {
        if (!result.success) {
            AlertWin(result.msg);
        }
        dg.datagrid("clearSelections");
        dg.datagrid("reload");
    });
}
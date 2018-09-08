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
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "PRODUCT_CODE",
        toolbar: '#toolbar',
        rowStyler: function (index, row) {
            if(row["MARK"])
            return { style: "background-color:#66ccff" };
        },
        columns: [[
            { field: 'PRODUCT_CODE', title: lang.productCode, width: 150 },
            { field: 'PRODUCT_NAME', title: lang.productName, width: 200 },
            { field: 'PROJECT_NAME', title: lang.projectName, width: 250 },
            { field: 'PBOMVER', title: lang.pbomVer, align: "center", width: 70 },
            { field: 'PBOM_CREATE_NAME', title: 'PBOM创建人', align: "center", width: 80 },
            { field: 'MBOMVER', title: lang.mbomVer, align: "center", width: 70 },
            { field: 'MBOM_CREATE_NAME', title: 'MBOM创建人', align: "center", width: 80 },
            { field: 'CHECK_STATUS', title: lang.mbom.checkStatus, align: "center", width: 60 },
            { field: 'TECH_STATUS', title: lang.mbom.techStatus, align: "center", width: 60 },
            { field: 'DESC', title: lang.remarks, width: 200 }
        ]],
        loadFilter: loadFilter
    });

    dlgCreateMBOMVer.dialog({
        title: '创建新的MBOM版本',
        footer: "#dlgCreateMBOMVerFooter",
        width: 350,
        height: 300,
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
function createMbomVer() {
    var prod = dg.datagrid("getSelected");
    if (prod === null) {
        AlertWin(lang.mbom.selectProductMbomVer);
        return false;
    }
    var ver = "M1";
    if (prod.MBOMVER) {
        ver = "M" + (parseInt(prod.MBOMVER.substr(1, prod.MBOMVER.length)) + 1);
    }
    dlgCreateMBOMVer.dialog("open");
    $("#txtVer").textbox("setText", ver);
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
    var pbom_ver_guid = prod["PBOM_VER_GUID"];
    var desc = $("#txtDesc").textbox("getText");
    postData(URL_MBOM_VER_CREATE, {
        prodcode: prod.PRODUCT_CODE,
        ver: ver,
        dtef: dtef,
        dtex: dtex,
        pbom_ver_guid: pbom_ver_guid,
        desc: desc
    }, function (result) {
        $.messager.confirm("提示", "是否进入维护页面！", function (r) {
            if (r) {
                publishMaintenance();
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
    var param = "?code=" + prod.PRODUCT_CODE;
    var title = $.trim(prod.PRODUCT_NAME) + "【" + $.trim(prod.PRODUCT_CODE) + "】";
    window.parent.openTab(title, URL_MAINTENANCEPAGE + param);    
}
function publish() {
    var prods = dg.datagrid("getSelections");
    if (prods.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var prodcodes = "";
    for (var i = 0, j = prods.length; i < j; i++) {
        var prod = prods[i];
        prodcodes = prod.PRODUCT_CODE + "," + prodcodes;
    }
    prodcodes = prodcodes.substr(0, prodcodes.length - 1);
    $.messager.confirm("提示", "您将发布产品[" + prodcodes+"]，请您确认！", function (r) {
        if (r) {
            var param = {
                code: prodcodes
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
        code: prod.PRODUCT_CODE
    };
    postData(URL_MBOMMARK, param, function (result) {
        if (!result.success) {
            AlertWin(result.msg);
        }
        dg.datagrid("clearSelections");
        dg.datagrid("reload");
    });
}
var URL_MAINTENANCELIST = "/MBOM/MaintenancePageList"
var URL_MAINTENANCEPAGE = "/MBOM/MenuIndex"
var URL_MBOM_VER_CREATE = "/MBOM/CreateVer"
var URL_MBOMRELEASE = "/MBOM/Release"
var URL_MBOMMARK = "/MBOM/Mark"
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
        idField: "PROJECT_ID",
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
        height: 280,
        closed: true,
        modal: true
    });
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid("load", data);
}
function queryDefault() {
    dg.datagrid("clearSelections");
    dg.datagrid("load", {});
}
function createMbomVer() {
    //没有版本，需要用户创建MBOM版本，创建完成后才能进入维护
    dlgCreateMBOMVer.dialog("open");
    $("#txtVer").textbox("setText", "M1");
}
function createMbomVerConfirm() {
    var prod = dg.datagrid("getSelected");
    if (prod == null) { return false; }
    var ver = $("#txtVer").textbox("getText");
    var dtef = $("#txtEfDate").textbox("getText");
    var dtex = $("#txtExDate").textbox("getText");
    var pbom_ver_guid = prod["PBOM_VER_GUID"];
    var desc = $("#txtDesc").textbox("getText");
    postData(URL_MBOM_VER_CREATE, {
        code: prod.PRODUCT_CODE,
        name: prod.PRODUCT_NAME,
        itemcode: prod.PRODUCT_ITEMCODE,
        ver: ver,
        dtef: dtef,
        dtex: dtex,
        pbom_ver_guid: pbom_ver_guid,
        desc: desc
    }, function (result) {
        InfoWin(result.msg);
    });
}
function publishMaintenance() {
    var prod = dg.datagrid("getSelected");
    if (prod == null) { return false; }

    var param = "?code=" + prod.PRODUCT_CODE;
    var title = $.trim(prod.PRODUCT_NAME) + "【" + $.trim(prod.PRODUCT_CODE) + "】";
    window.parent.openTab(title, URL_MAINTENANCEPAGE + param);    
}
function publish() {
    var prod = dg.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var param = {
        code: prod.PRODUCT_CODE
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
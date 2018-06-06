var URL_MAINTENANCELIST = "/ProductChange/PageList"
var URL_MAINTENANCEPAGE = "/ProductChange/MenuIndex"
var URL_MBOMRELEASE = "/MBOM/Release"
var dg = $("#dgProducts");
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
            if (row["MARK"])
                return { style: "background-color:#66ccff" };
        },
        columns: [[
            { field: 'PRODUCT_CODE', title: lang.productCode, width: 150 },
            { field: 'PRODUCT_NAME', title: lang.productName, width: 200 },
            { field: 'PROJECT_NAME', title: lang.projectName, width: 250 },
            { field: 'PBOMVER', title: lang.pbomVer, align: "center", width: 70 },
            { field: 'MBOMVER', title: lang.mbomVer, align: "center", width: 70 },
            { field: 'CHECK_STATUS', title: lang.mbom.checkStatus, align: "center", width: 60 },
            { field: 'TECH_STATUS', title: lang.mbom.techStatus, align: "center", width: 60 },
            { field: 'DESC', title: lang.remarks, width: 200 }
        ]],
        loadFilter: loadFilter
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
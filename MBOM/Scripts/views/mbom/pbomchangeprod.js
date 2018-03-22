var URL_PAGELIST = "/MBOM/PBOMChangeProdPageList";
var URL_PRODCHANGEINDEX = "/MBOM/ProductChangeDetailIndex";
var dg = $("#dg");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        singleSelect: true,
        rownumbers: true,
        pagination: true,
        border: false,
        idField: "CN_PRODUCT_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: 'CN_PRODUCT_CODE', title: lang.pbom.prodcode, width: 120 },
            { field: 'CN_NAME', title: lang.pbom.name, width: 150 },
            { field: 'CN_VER', title: lang.pbom.ver, width: 80 },
            {
                field: 'CN_DT_VER', title: lang.pbom.dtver, width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: 'CN_DT_TOERP', title: lang.pbom.dttoerp, width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            { field: 'CN_CREATE_NAME', title: lang.pbom.createname, width: 100 }
        ]],
        loadFilter: loadFilter
    }); 
});

var timeout = 5;
var refreshable = true;
function reloadTable() {
    if (!refreshable) {
        return;
    }
    refreshable = false;
    setTimeout(function () {
        refreshable = true;
    }, timeout * 1000);
    dg.datagrid("reload");
}

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("load", data);
}

function openOptionalItemTab() {
    //设置选中的选装件关系
    var item = dg.datagrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var prodcode = item["CN_PRODUCT_CODE"];
    var title = prodcode + " PBOM变更明细";
    window.parent.openTab(title, URL_PRODCHANGEINDEX + "?prodcode=" + prodcode);
} 
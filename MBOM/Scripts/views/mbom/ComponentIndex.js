var URL_MAINTENANCELIST = "/MBOM/ComponentMaintenancePageList";
var URL_MAINTENANCEPAGE = "/MBOM/ComponentMaintenanceIndex"; 
var dg = $("#dgItems");
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
        columns: [[
            { field: 'CN_NAME', title: lang.item.name, width: 200 },
            { field: 'CN_CODE', title: lang.item.code, width: 120 },
            { field: 'CN_ITEM_CODE', title: lang.item.itemcode, width: 120 },
            { field: 'CN_XH', title: lang.item.xh, width: 140 },
            { field: 'CN_GG', title: lang.item.gg, width: 100 },
            { field: 'CN_UNIT', title: lang.item.unit, align: "center", width: 50 },
            {
                field: 'CN_DT_CREATE', title: lang.item.create, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: 'CN_IS_TOERP', title: lang.item.istoerp, align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value == "0") {
                        return "未发布";
                    } else if (value == "1") {
                        return "发布中……";
                    } else if (value == "2") {
                        return "已发布";
                    } else {
                        return "未知状态";
                    }
                }
            },
            {
                field: 'CN_DT_TOERP', title: lang.item.dttoerp, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (row["CN_IS_TOERP"] == "0") {
                        return "";
                    }else if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            }
        ]],
        loadFilter: loadFilter
    });
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid("load", data);
}

function maintenance() {
    var item = dg.datagrid("getSelected");
    if (item === null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var param = "?itemcode=" + item.CN_ITEM_CODE;
    var title = $.trim(item.CN_NAME) + "【" + $.trim(item.CN_ITEM_CODE) + "】";
    window.parent.openTab(title, URL_MAINTENANCEPAGE + param);    
}
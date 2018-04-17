var URL_PAGELIST = "/Item/SearchByTypePageList"
var URL_PAGELISTALL = "/Item/WithTypePageList"
var URL_CONFIRM = "/Item/TypeSwitch"
var URL_ITEMSETTYPE = "/Item/SetType"
var dg = $("#dg");
var dlg = $("#dlg");
var cboType = $("#cboType");
var queryParams = {
    typenames: ["采购件", "自制件"]
};
var defaultColumns = [[
    { field: 'code', title: lang.item.code, width: 120 },
    { field: 'itemcode', title: lang.item.itemcode, width: 120 },
    { field: 'name', title: lang.item.name, width: 150 },
    { field: 'xh', title: lang.item.xh, align: "center", width: 120 },
    { field: 'gg', title: lang.item.gg, align: "center", width: 120 },
    { field: 'weight', title: lang.item.weight, align: "center", width: 60 },
    { field: 'unit', title: lang.item.unit, align: "center", width: 40 },
    { field: 'typename', title: lang.item.type, align: "center", width: 80 }
]];
var allColumns = [[
    { field: 'code', title: lang.item.code, width: 120 },
    { field: 'itemcode', title: lang.item.itemcode, width: 120 },
    { field: 'name', title: lang.item.name, width: 150 },
    { field: 'xh', title: lang.item.xh, align: "center", width: 120 },
    { field: 'gg', title: lang.item.gg, align: "center", width: 120 },
    { field: 'weight', title: lang.item.weight, align: "center", width: 60 },
    { field: 'unit', title: lang.item.unit, align: "center", width: 40 },
    { field: 'typenames', title: lang.item.type, align: "center", width: 130 }
]];
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "id",
        toolbar: '#toolbar',
        queryParams: queryParams,
        columns: defaultColumns,
        loadFilter: loadFilter
    });

    dlg.dialog({
        title: '物料分类标识设置',
        closed: true,
        width: 300,
        height: 200,
        modal: true,
        footer: '#dlgFooter'
    })

    cboType.combobox({
        editable: false,
        panelHeight: "auto"
    })
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid({
        url: URL_PAGELIST,
        columns: defaultColumns,
        queryParams: $.extend({}, queryParams, data)
    });
}

function queryAll() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid({
        url: URL_PAGELISTALL,
        columns: allColumns,
        queryParams: $.extend({}, queryParams, data)
    });
}

function setItemType() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var dist = "";
    var typename = $.trim(item["typename"]);
    var typenames = $.trim(item["typenames"]);
    if (typename) {
        if (typename == "采购件") {
            dist = "自制件";
        }
        if (typename == "自制件") {
            dist = "采购件";
        }
        $.messager.confirm('提示', '是否将物料【' + item["itemcode"] + '】的类型修改为【' + dist + '】？', function (r) {
            if (r) {
                postData(URL_CONFIRM, {
                    itemid: item["id"]
                }, function (result) {
                    if (result.success) {
                        dg.datagrid("reload");
                    }
                    if (result.msg) {
                        InfoWin(result.msg);
                    }
                })
            }
        });
    } else {
        openDialog();
    }
}
function openDialog() {
    //打开设置选装件的窗口
    dlg.dialog('open');
    dlg.dialog('center');
    $("#dlgQueryFrm").form("clear");
}

function confirm() {
    //设置选中的物料为选装件
    //先获取选中的物料
    var item = dg.datagrid("getSelected");
    //判断是否选中了物料
    if (!item) {
        //未选中物料时弹出警告，程序停止执行
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    //获取选中物料的ID
    var id = item["id"];
    var typeid = $("#cboType").combobox("getValue");
    postData(URL_ITEMSETTYPE, {
        itemid: id,
        typeid: typeid
    }, function (result) {
        if (result.success) {
            //刷新表格 
            dg.datagrid("reload");
            dlg.dialog("close");
        }
        if (result.msg) {
            InfoWin(result.msg)
        }
    })
}

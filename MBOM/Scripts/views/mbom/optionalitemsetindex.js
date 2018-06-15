var URL_PAGELIST = "/Item/OptionalItemMapList"
var URL_PAGELIST2 = "/Item/PageList"
var URL_MAPADD = "/Item/OptionalItemMapAdd"
var URL_MAPREMOVE = "/Item/OptionalItemMapRemove"
var dg = $("#dgOptionalItems");
var dgItems = $("#dgItems");
var dlg = $("#dlgOptionalItems");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        border: false,
        idField: "CN_HLINK_ID",
        toolbar: '#toolbar',
        queryParams: {
            itemid: $("#itemid").val()
        },
        columns: [[
            { field: 'CN_ID', checkbox: true },
            { field: 'CN_CODE', title: lang.item.code, width: 120 },
            { field: 'CN_ITEM_CODE', title: lang.item.itemcode, width: 120 },
            { field: 'CN_NAME', title: lang.item.name, width: 150 },
            { field: 'CN_OPTIONAL_CODE', title: lang.oitem.code,  width: 120 },
            { field: 'CN_OPTIONAL_ITEM_CODE', title: lang.oitem.itemcode, width: 120 },
            { field: 'CN_OPTIONAL_NAME', title: lang.oitem.name, width: 150 },
            { field: 'CN_IS_TOERP', title: lang.item.istoerp, width: 50 },
            { field: 'CN_SYS_STATUS', title: lang.item.status, width: 50 },
            {
                field: 'CN_DT_EXPIRY_ERP', title: lang.item.expiryerp, width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: 'CN_DT_CREATE', title: lang.item.create, width: 80,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            { field: 'CN_CREATE_BY', title: lang.item.createby, width: 100 },
            { field: 'CN_CREATE_LOGIN', title: lang.item.createlogin, width: 100 },
            { field: 'CN_CREATE_NAME', title: lang.item.createname, width: 100 },
            { field: 'CN_DESC', title: lang.item.desc, width: 100 }
        ]],
        loadFilter: loadFilter
    });

    dgItems.datagrid({
        url: URL_PAGELIST2,
        height: "100%",
        striped: true,
        rownumbers: true,
        pagination: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#dlgToolbar',
        columns: [[
            { field: 'CN_ID', checkbox: true },
            { field: 'CN_CODE', title: lang.item.code, width: 120 },
            { field: 'CN_ITEM_CODE', title: lang.item.itemcode, width: 120 },
            { field: 'CN_NAME', title: lang.item.name, width: 150 },
            { field: 'CN_XH', title: lang.item.xh, align: "center", width: 80 },
            { field: 'CN_GG', title: lang.item.gg, align: "center", width: 80 },
            { field: 'CN_WEIGHT', title: lang.item.weight, align: "center", width: 60 },
            { field: 'CN_UNIT', title: lang.item.unit, align: "center", width: 40 },
            { field: 'CN_DESC', title: lang.item.desc, width: 200 }
        ]],
        loadFilter: loadFilter
    });

    dlg.dialog({
        title: '添加选装件',
        closed: true,
        width: 800,
        height: 500,
        modal: true,
        footer: '#dlgOptionalItemsFooter'
    })
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

function dlgQuery() {
    var data = $("#dlgQueryFrm").serializeJSON();
    dgItems.datagrid("load", data);
}
function openDialog() {
    dlg.dialog('open');
    dlg.dialog('center');
    $("#dlgQueryFrm").form("clear");
    dgItems.datagrid("clearSelections");
    dgItems.datagrid("reload");
}
function itemOptionalMapDel() {
    var items = dg.datagrid("getSelections");
    if (items == null || items.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var hlinkids = "";
    var itemnames = "";
    for (var i = 0, len = items.length; i < len; i++){
        var item = items[i];
        hlinkids = hlinkids + item["CN_HLINK_ID"] + ",";
        itemnames = itemnames + item["CN_OPTIONAL_ITEM_CODE"] + "，";
    }
    hlinkids = hlinkids.substr(0, hlinkids.length - 1);
    itemnames = "<br/>【" + itemnames.substr(0, itemnames.length - 1) + "】";
    $.messager.confirm('确认', '是否确认删除选中的对应关系？' + itemnames, function (r) {
        if (r) {
            postData(URL_MAPREMOVE, { hlinkids: hlinkids }, function (result) {
                if (result.success) {
                    dgItems.datagrid("clearSelections");
                    dgItems.datagrid("reload");
                    dg.datagrid("reload");
                }
                if (result.msg) {
                    InfoWin(result.msg)
                }
            })
        }
    });
}
function itemOptionalMapAdd() {
    //设置选中的物料为选装件
    //先获取选中的物料
    var items = dgItems.datagrid("getSelections");
    //判断是否选中了物料
    if (items.length == 0) {
        //未选中物料时弹出警告，程序停止执行
        AlertWin(lang.item.noSelections);
        return false;
    }
    //获取选中物料的ID
    var itemid = $("#itemid").val();
    var ids = [];
    for (var i = 0, len = items.length; i < len; i++) {
        var item = items[i];
        ids.push(item["CN_ID"]);
    }
    postData(URL_MAPADD, { itemid: itemid, itemids: ids.toString() }, function (result) {
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

function openOptionalItemTab() {
    //设置选中的选装件关系
    var item = dg.datagrid("getSelected");
    if (item == null) {
        AlertWin(lang.item.noSelections);
        return false;
    }
}
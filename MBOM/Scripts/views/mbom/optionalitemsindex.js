var URL_PAGELIST = "/Item/OptionalItemPageList"
var URL_PAGELIST2 = "/Item/NoOptionalItemPageList"
var URL_SETOPTIONALITEMS = "/Item/SetOptionalItems"
var URL_OPTIONALITEM_DEL = "/Item/DeleteOptionalItem"
var URL_OPTIONALITEMSETINDEX = "/MBOM/OptionalItemSetIndex"
var dg = $("#dgOptionalItems");
var dgItems = $("#dgItems");
var dlg = $("#dlgOptionalItems");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "hlinkid",
        toolbar: '#toolbar',
        columns: [[
            { field: 'code', title: lang.item.code, width: 120 },
            { field: 'itemcode', title: lang.item.itemcode, width: 120 },
            { field: 'name', title: lang.item.name, width: 150 },
            { field: 'xh', title: lang.item.xh, align: "center", width: 80 },
            { field: 'gg', title: lang.item.gg, align: "center", width: 80 },
            { field: 'weight', title: lang.item.weight, align: "center", width: 60 },
            { field: 'unit', title: lang.item.unit, align: "center", width: 40 },
            { field: 'desc', title: lang.item.desc, width: 200 }
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
        idField: "id",
        toolbar: '#dlgToolbar',
        columns: [[
            { field: 'id', checkbox: true },
            { field: 'code', title: lang.item.code, width: 120 },
            { field: 'itemcode', title: lang.item.itemcode, width: 120 },
            { field: 'name', title: lang.item.name, width: 150 },
            { field: 'xh', title: lang.item.xh, align: "center", width: 80 },
            { field: 'gg', title: lang.item.gg, align: "center", width: 80 },
            { field: 'weight', title: lang.item.weight, align: "center", width: 60 },
            { field: 'unit', title: lang.item.unit, align: "center", width: 40 },
            { field: 'desc', title: lang.item.desc, width: 200 }
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

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("load", data);
}

function dlgQuery() {
    var data = $("#dlgQueryFrm").serializeJSON();
    dgItems.datagrid("load", data);
}
function openDialog() {
    //打开设置选装件的窗口
    dlg.dialog('open');
    dlg.dialog('center');
    $("#dlgQueryFrm").form("clear");
    dgItems.datagrid("clearSelections");
    dgItems.datagrid("reload");
}
function delOptionalItem() {
    //删除选中的选装件
    var item = dg.datagrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    $.messager.confirm('确认', '是否确认删除此选装件？', function (r) {
        if (r) {
            postData(URL_OPTIONALITEM_DEL, { id  : item["hlinkid"] }, function (result) {
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
function setOptionalItems() {
    //设置选中的物料为选装件
    //先获取选中的物料
    var items = dgItems.datagrid("getSelections");
    //判断是否选中了物料
    if (items.length == 0) {
        //未选中物料时弹出警告，程序停止执行
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    //获取选中物料的ID
    var ids = [];
    for (var i = 0, len = items.length; i < len; i++) {
        var item = items[i];
        ids.push(item["id"]);
    }
    postData(URL_SETOPTIONALITEMS, { itemids: ids.toString() }, function (result) {
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
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var itemid = item["id"];
    var title = item["itemcode"] + " 选装信息";
    window.parent.openTab(title, URL_OPTIONALITEMSETINDEX + "?itemid=" + itemid);
}
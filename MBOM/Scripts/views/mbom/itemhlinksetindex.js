var URL_PAGELIST = "/Item/SearchByTypePageList"
var URL_CONFIRM = "/Item/ItemTypeTrans"
var dg = $("#dg");
var dlg = $("#dlg");
var queryParams = {
    typenames: ["采购件", "自制件"]
};
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
        columns: [[
            { field: 'code', title: lang.item.code, width: 120 },
            { field: 'itemcode', title: lang.item.itemcode, width: 120 },
            { field: 'name', title: lang.item.name, width: 150 },
            { field: 'xh', title: lang.item.xh, align: "center", width: 80 },
            { field: 'gg', title: lang.item.gg, align: "center", width: 80 },
            { field: 'weight', title: lang.item.weight, align: "center", width: 60 },
            { field: 'unit', title: lang.item.unit, align: "center", width: 40 },
            { field: 'typename', title: lang.item.type, align: "center", width: 80 }
        ]],
        loadFilter: loadFilter
    });

    dlg.dialog({
        title: '物料分类标识设置',
        closed: true,
        width: 800,
        height: 500,
        modal: true,
        footer: '#dlgFooter'
    })
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("load", $.extend({}, queryParams, data));
}

function setItemType() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var dist = "";
    if ($.trim(item["typename"]) == "采购件") {
        dist = "自制件";
    }
    if ($.trim(item["typename"]) == "自制件") {
        dist = "采购件";
    }
    $.messager.confirm('提示', '是否将物料【'+item["itemcode"]+'】的类型修改为【'+dist+'】？', function (r) {
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
}
/// ---------------
/// 下方代码暂时不用
/// ---------------
function openDialog() {
    //打开设置选装件的窗口
    dlg.dialog('open');
    dlg.dialog('center');
    $("#dlgQueryFrm").form("clear");
    dg.datagrid("clearSelections");
    dg.datagrid("reload");
}

function confirm() {
    //设置选中的物料为选装件
    //先获取选中的物料
    var item = dg.datagrid("getSelected");
    //判断是否选中了物料
    if (item.length == 0) {
        //未选中物料时弹出警告，程序停止执行
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    //获取选中物料的ID
    var ids = [];
    for (var i in item) {
        var item = item[i];
        ids.push(item["id"]);
    }
    postData(URL_SETOPTIONALitem, { itemids: ids.toString() }, function (result) {
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

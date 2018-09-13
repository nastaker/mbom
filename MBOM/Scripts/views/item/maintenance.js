var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_MAINTENANCELIST = "/Item/MaintenancePageList"
var URL_EDITITEM = "/Item/Edit"
var URL_DELETEITEM = "/Item/Delete"
var URL_ITEMUNITLIST = "/Item/UnitList"
var URL_ITEMTYPELIST = "/Item/TypeList"

var dg = $("#dgItems");
var winItem = $("#winItem");
var frmItem = $("#frmItem");
var cboUnit = $("#cboItemUnit");
var cboType = $("#cboItemType");
$(function () {
    dg.datagrid({
        height: "100%",
        fitColumns: true,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 20 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 20 },
            { field: "CN_NAME", title: "物料名称", width: 20 },
            { field: "CN_UNIT", title: "单位", align: "center", width: 5 },
            { field: "CN_WEIGHT", title: "重量", align: "center", width: 5 },
            {
                field: "Desc", title: "类型", align: "center", width: 15,
                formatter: function (value, row, index) {
                    var str = "";
                    if (row["自制件"]) {
                        str += "," + row["自制件"];
                    }
                    if (row["采购件"]) {
                        str += "," + row["采购件"];
                    }
                    if (row["MBOM合件"]) {
                        str += "," + row["MBOM合件"];
                    }
                    return str.substr(1, str.length);
                }
            },
            {
                field: "CN_DT_CREATE", title: "发布日期", align: "center", width: 15,
                formatter: function (value, row, index) {
                    if (value && $.trim(value) != "") {
                        return ToJavaScriptDate(value);
                    }
                    return value;
                }
            }
        ]],
        loadFilter: loadFilter
    });

    frmItem.form({
        url: URL_EDITITEM,
        onSubmit: function (param) {
            if (!frmItem.form("validate")) {
                return false;
            }
            $.messager.progress({ title: lang.infoTitle, msg: lang.loading });
        },
        success: function (data) {
            $.messager.progress('close');
            winItem.window({
                closed: true
            });
            dg.datagrid("clearSelections");
            var result = eval('(' + data + ')');
            if (!result) {
                AlertWin(data);
                return;
            }
            if (result.success) {
                dg.datagrid("reload");
            }
            if (result.msg) {
                InfoWin(result.msg);
            }
        }
    });

    cboUnit.combobox({
        url: URL_ITEMUNITLIST,
        valueField: 'CN_NAME',
        textField: 'CN_NAME',
        editable: false,
        required: true,
        loadFilter: loadFilter
    });

    cboType.combobox({
        url: URL_ITEMTYPELIST,
        valueField: 'CN_ID',
        textField: 'CN_NAME',
        editable: false,
        required: true,
        multiple: true,
        loadFilter: function (result) {
            if (result.success) {
                for (var i = 0, len = result.data.length; i < len; i++) {
                    result.data[i]["CN_NAME"] = $.trim(result.data[i]["CN_NAME"]);
                }
                return result.data;
            } else {
                InfoWin(result.msg);
                return [];
            }
        }
    });

    winItem.window({
        closed: true,
        minimizable: false,
        collapsible: false,
        modal: true,
        width: 450,
        height: 350,
        footer: "#winFooter"
    });

    var opts = dg.datagrid("options");
    opts.url = URL_MAINTENANCELIST;
});

function itemAdd() {
    $("#txtItemCode").textbox({
        readonly: false
    });
    winItem.window({
        title: "添加物料",
        closed: false
    });
    frmItem.form("clear");
    winItem.window("center");
}

function itemEdit() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择需要修改的物料");
        return;
    }
    if (item["CN_IS_TOERP"] == "1") {
        AlertWin("物料已发布，无法修改");
        return;
    }
    $("#txtItemCode").textbox({
        readonly: true
    });
    cboType.combobox("clear");
    var mbomtypeid = item["MBOM合件ID"];
    var purchaseid = item["采购件ID"];
    var selfmadeid = item["自制件ID"];
    if (mbomtypeid) {
        cboType.combobox("select", mbomtypeid)
    }
    if (purchaseid) {
        cboType.combobox("select", purchaseid)
    }
    if (selfmadeid) {
        cboType.combobox("select", selfmadeid)
    }
    frmItem.form("load", item);
    winItem.window({
        title: "编辑物料",
        closed: false
    });
    winItem.window("center");
}

function itemDelete() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择要删除的物料");
        return;
    }
    if (item["CN_IS_TOERP"] == "1") {
        AlertWin("物料已发布，无法删除");
        return;
    }
    var itemcode = $.trim(item["CN_ITEM_CODE"]);
    var itemid = item["CN_ID"];
    $.messager.confirm("提示", "确实要删除自定义物料["+itemcode+"]吗？", function (r) {
        if (r) {
            dg.datagrid("clearSelections");
            postData(URL_DELETEITEM, { id: itemid }, function (result) {
                if (result.success) {
                    dg.datagrid("reload");
                }
                if (result.msg) {
                    AlertWin(result.msg);
                }
            });
        }
    });
}

function query() {
    var param = $("#queryFrm").serializeJSON();
    dg.datagrid("load", param);
}

function showDetail() {
    var item = dg.datagrid("getSelected");
    if (!item) { return; }
    var title = "物料详情" + item.CN_ITEM_CODE + " " + item.CN_NAME;
    var prod_itemcode = item.CN_ITEM_CODE
    window.parent.openTab(title, URL_ITEMDETAIL + "?prod_itemcode=" + prod_itemcode);
}
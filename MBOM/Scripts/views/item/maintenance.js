var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_MAINTENANCELIST = "/Item/MaintenancePageList"
var URL_EDITITEM = "/Item/Edit"
var URL_DELETEITEM = "/Item/Delete"
var URL_ITEMUNITLIST = "/Item/UnitList"
var URL_ITEMTYPELIST = "/Item/TypeList"
var URL_PRODUCTLIST = "/Item/ProductLineList"

var dg = $("#dgItems");
var vWinItem = $("#winItem");
var vFrmItem = $("#frmItem");
var vItemCode = $("#txtItemCode");
var vItemUnit = $("#cboItemUnit");
var vItemType = $("#cboItemType");
var vSearchItemType = $("#cboSearchItemType");
var vProductLine = $("#cboProductLine")
var dataProductLine = null;
var dataItemTypeList = null;
var dataFilteredProductLine = null;
$(function () {
    var loadSuccess = true;
    postDataSync(URL_PRODUCTLIST, null, function (result) {
        if (!result.success) {
            loadSuccess = false;
            AlertWin(result.msg);
        }
        dataProductLine = result.data;
    });
    if (!loadSuccess) {
        return;
    }
    postDataSync(URL_ITEMTYPELIST, {
        names: ["自制件", "采购件", "MBOM合件"]
    }, function (result) {
        if (!result.success) {
            loadSuccess = false;
            AlertWin(result.msg);
        }
        dataItemTypeList = result.data;
    });
    if (!loadSuccess) {
        return;
    }
    dg.datagrid({
        height: "100%",
        striped: true,
        pageSize: 20,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 150 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 150 },
            { field: "CN_NAME", title: "物料名称", width: 150 },
            { field: "CN_UNIT", title: "单位", align: "center", width: 60 },
            { field: "CN_WEIGHT", title: "重量", align: "center", width: 60 },
            { field: "CN_PRODUCTLINECODE", title: "产品线", align: "center", width: 60 },
            {
                field: "CN_IS_PDM", title: "是否PDM", align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value) {
                        return "PDM";
                    }
                    return "";
                }
            },
            { field: "CN_TYPENAMES", title: "类型", width: 200 },
            {
                field: "CN_IS_TOERP", title: "发布状态", align: "center", width: 80,
                formatter: function (value, row, index) {
                    switch (value) {
                        case 0:
                            return "未发布";
                        case 1:
                            return "发布中";
                        case 2:
                            return "已发布";
                        default:
                            return "";
                    }
                }
            },
            {
                field: "CN_DT_TOERP", title: "发布日期", align: "center", width: 90,
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

    vFrmItem.form({
        url: URL_EDITITEM,
        onSubmit: function (param) {
            if (!vFrmItem.form("validate")) {
                return false;
            }
            param.CN_TYPEIDS = vItemType.combobox("getValues").toString();
            Loading();
        },
        success: function (data) {
            Loaded();
            vWinItem.window({
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

    vItemCode.textbox({
        width: 185,
        required: true,
        onChange: function (newValue, oldValue) {
            dataFilteredProductLine = [];
            vProductLine.combobox('clear')
            var opts = vProductLine.combobox('options');
            for (var i = 0, j = dataProductLine.length; i < j; i++) {
                var pl = dataProductLine[i];
                if (newValue.toUpperCase().indexOf(pl[opts.groupField]) > - 1) {
                    dataFilteredProductLine.push(pl);
                }
            }
            if (newValue.toUpperCase().indexOf("U") > -1) {
                for (var i = 0, j = dataFilteredProductLine.length; i < j; i++) {
                    var fpll = dataFilteredProductLine[i];
                    if (fpll[opts.groupField].indexOf("U") > -1) {
                        dataFilteredProductLine = [];
                        dataFilteredProductLine.push(fpll);
                        break;
                    }
                }
            }
            if (dataFilteredProductLine.length == 0) {
                for (var i = 0, j = dataProductLine.length; i < j; i++) {
                    var pll = dataProductLine[i];
                    if (pll[opts.groupField].indexOf("ZZZ") > -1) {
                        dataFilteredProductLine.push(pll);
                    }
                }
            }
            vProductLine.combobox('loadData', dataFilteredProductLine);
        }
    })

    vItemUnit.combobox({
        url: URL_ITEMUNITLIST,
        valueField: 'CN_NAME',
        textField: 'CN_NAME',
        editable: false,
        required: true,
        width: 185,
        loadFilter: loadFilter
    });

    vSearchItemType.combobox({
        data: dataItemTypeList,
        width: 90,
        panelWidth: 90,
        valueField: 'CN_ID',
        textField: 'CN_NAME',
        editable: false,
        multiple: false,
        loadFilter: function (originData) {
            var data = JSON.parse(JSON.stringify(originData));
            for (var i = 0, j = data.length; i < j; i++) {
                data[i]["CN_ID"] = $.trim(data[i]["CN_NAME"]);
            }
            data.unshift({
                'CN_ID': '',
                'CN_NAME': '-请选择-'
            }, {
                'CN_ID': '无',
                'CN_NAME': '无类型'
            });
            return data;
        }
    });

    vItemType.combobox({
        data: dataItemTypeList,
        width: 185,
        panelWidth: 185,
        valueField: 'CN_ID',
        textField: 'CN_NAME',
        groupField: 'group',
        validType: 'itemType',
        editable: false,
        required: true,
        multiple: true,
        onSelect: function (record) {
            var cbo = $(this);
            var selections = cbo.combobox('getValues');
            if ((record["CN_ID"] == 2 || record["CN_ID"] == 3)) {
                if (selections.indexOf('2') > -1) {
                    setTimeout(function () {
                        cbo.combobox('unselect', 2);
                    }, 0);
                } else if (selections.indexOf('3') > -1) {
                    setTimeout(function () {
                        cbo.combobox('unselect', 3);
                    }, 0);
                }
            }
        },
        loadFilter: function (originData) {
            var data = JSON.parse(JSON.stringify(originData));
            for (var i = 0, len = data.length; i < len; i++) {
                data[i]["CN_NAME"] = $.trim(data[i]["CN_NAME"]);
                data[i]["group"] = "必选项";
                if (data[i]["CN_ID"] > 3) {
                    data[i]["group"] = "可选项";
                }
            }
            return data;
        }
    });

    vProductLine.combobox({
        data: dataProductLine,
        width: 185,
        panelWidth: 185,
        required: true,
        editable: false,
        valueField: 'CN_ID',
        textField: 'CN_NAME',
        groupField: 'CN_CODE',
        groupPosition: 'sticky',
        onSelect: function (record) {
            $("#hiddenProductLineCode").val(record["CN_NUMBER"]);
        },
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return (row[opts.textField].indexOf(q) > -1 || row['CN_NUMBER'].toString().indexOf(q) > -1);
        },
        formatter: function (data) {
            return '<span style="font-weight:bold">' + data["CN_NAME"] + '</span><br/>' +
                '<span>' + data["CN_NUMBER"] + '</span>';
        },
        onLoadSuccess: function () {
            if (!dataProductLine) {
                dataProductLine = $(this).combobox("getData");
            }
        }
    });
    
    vWinItem.window({
        closed: true,
        minimizable: false,
        maximizable: false,
        collapsible: false,
        resizable: false,
        modal: true,
        width: 400,
        height: 330,
        footer: "#winFooter"
    });

    var opts = dg.datagrid("options");
    opts.url = URL_MAINTENANCELIST;
});

function itemAdd() {
    readonlyForm(false);
    vWinItem.window({
        title: "添加物料",
        closed: false
    });
    vFrmItem.form("clear");
    vWinItem.window("center");
}

function itemEdit() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择需要修改的物料");
        return;
    }
    vFrmItem.form("clear");

    //判断是否为PDM，若为PDM则不允许修改除分类外所有属性，若不为PDM，则可以修改除ITEMCODE、NAME外的属性
    if (item["CN_IS_PDM"]) {
        readonlyForm(true);
        $("#cboItemType").combobox('readonly', false);
        $("#cboItemType").combobox('enableValidation');
    } else {
        readonlyForm(false);
        $("#txtItemCode").textbox('readonly', true);
    }

    var typeids = item["CN_TYPEIDS"];
    var prodlinenumber = $.trim(item["CN_PRODUCTLINECODE"])
    if (typeids) {
        vItemType.combobox("setValues", typeids.split(','))
    }
    var prodlineId;
    for (var i = 0, j = dataProductLine.length; i < j; i++) {
        var prodline = dataProductLine[i];
        if (prodline["CN_NUMBER"] == prodlinenumber) {
            prodlineId = prodline["CN_ID"];
        }
    }
    if (prodlineId) {
        setTimeout(function () {
            vProductLine.combobox("setValue", prodlineId);
        }, 0);
    }
    vFrmItem.form("load", item);
    vWinItem.window({
        title: "编辑物料",
        closed: false
    });
    vWinItem.window("center");
}

function readonlyForm(readonly) {
    $("#txtItemCode").textbox('readonly', readonly);
    $("#txtItemName").textbox('readonly', readonly);
    $("#txtItemWeight").textbox('readonly', readonly);
    $("#cboItemUnit").combobox('readonly', readonly);
    $("#cboProductLine").combobox('readonly', readonly);
    $("#cboItemType").combobox('readonly', readonly);

    if (readonly) {
        $("#txtItemName").textbox('disableValidation');
        $("#txtItemWeight").textbox('disableValidation');
        $("#cboItemUnit").combobox('disableValidation');
        $("#cboProductLine").combobox('disableValidation');
        $("#cboItemType").combobox('disableValidation');
    } else {
        $("#txtItemName").textbox('enableValidation');
        $("#txtItemWeight").textbox('enableValidation');
        $("#cboItemUnit").combobox('enableValidation');
        $("#cboProductLine").combobox('enableValidation');
        $("#cboItemType").combobox('enableValidation');
    }
}

function itemDelete() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择要删除的物料");
        return;
    }
    if (item["CN_IS_TOERP"] != 0) {
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
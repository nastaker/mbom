var checkedRowIds = { count: 0 };
var list = [];
var discreteList = [];
var parentId = undefined;
var gradientStep = 6;
var checkedCss = "datagrid-row-checked-custom";
var refreshCooldown = 5;
var tgTrPreId = "#datagrid-row-r1-2-";
var tgviTrPreId = "#datagrid-row-r2-2-";

var checkOnSelect = false;
var canRefresh = true;
var isDeduction = false;

//列表信息获取
var URL_ENTER = "/MBOM/Maintenance";
var URL_ITEMTREE = "/MBOM/List";
var URL_DISCRETE_LIST = "/MBOM/DiscreteList";
//虚件操作
var URL_VIRTUAL_ITEM_SET = "/MBOM/VirtualItemSet";
var URL_VIRTUAL_ITEM_DROP = "/MBOM/VirtualItemDrop";
var URL_VIRTUAL_ITEM_LINK = "/MBOM/VirtualItemLink";
var URL_VIRTUAL_ITEM_UNLINK = "/MBOM/VirtualItemUnlink";
//合件操作
var URL_COMPOSITE_ITEM_SET = "/MBOM/CompositeItemSet";
var URL_COMPOSITE_ITEM_DROP = "/MBOM/CompositeItemDrop";
var URL_COMPOSITE_ITEM_LINK = "/MBOM/CompositeItemLink";
var URL_COMPOSITE_ITEM_UNLINK = "/MBOM/CompositeItemUnlink";
var URL_EDITCOMBINENAME = "/MBOM/EditCombineName";
//物料
var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_ITEMPAGELIST = "/Item/MaintenancePageList"
var URL_ITEM_LINK = "/MBOM/ItemLink"
var URL_ITEM_UNLINK = "/MBOM/ItemUnlink"
var URL_ITEM_EDITQUANTITY = "/MBOM/ItemEditQuantity"

var tg = $("#treegrid");
var tgvi = $("#tgvi");
var dgItem = $("#dgItem");

var COLS = {
    Level: "Level",
    ParentId: "ParentId",
    Id: "Id",
    ItemId: "ItemId",
    Name: "Name",
    Code: "Code",
    ItemCode: "ItemCode",
    Guid: "Guid",
    Quantity: "Quantity",
    Unit: "Unit",
    IsBorrow: "IsBorrow",
    IsAssembly: "IsAssembly",
    Type: "Type",
    Status: "Status",
    Order: "Order"
}

var treegridOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    checkbox: function (row) {
        if (row[COLS.Type] == "产品" || row[COLS.Type] == "VP") {
            return false;
        }
        return true;
    },
    cascadeCheck: false,
    idField: COLS.Id,
    treeField: COLS.ItemCode,
    toolbar: '#toolbar',
    rowStyler: function (row) {
        var cls = "";
        switch (row[COLS.Type]) {
            case "V":
                cls = "main-virtual";
                break;
            case "C":
                cls = "main-combine";
                break;
            case "U":
                cls = "main-user";
                break;
        }
        return { class: cls };
    },
    columns: [[
        { field: COLS.Level, title: lang.mbom.level, width: "35",align:"center" },
        { field: COLS.ItemCode, title: lang.mbom.itemCode, width: "250" },
        { field: COLS.Name, title: lang.mbom.itemName, width: "150" },
        { field: COLS.Quantity, title: lang.mbom.quantity, width: "50",align:"right" },
        { field: COLS.Unit, title: lang.mbom.unit, align: "center", width: "35" },
        {
            field: COLS.IsBorrow, title: "借用", align: "center", width: "35",
            formatter: function (value, row, index) {
                if (value) {
                    return  "借用";
                }
            }
        },
        { field: COLS.Order, title: lang.menu.order, width: "50" }
    ]],
    onSelect: function (row) {
        if (!checkOnSelect) { return; }
        var itemid = row[treegridOption.idField];
        var node = tg.treegrid("find", itemid);
        var checked = node.checked;
        if (!checked) {
            tg.treegrid("checkNode", itemid);
        } else {
            tg.treegrid("uncheckNode", itemid);
        }
    },
    onBeforeCheckNode: function (row, checked) {
        var itemcode = row[COLS.ItemCode];
        var id = row[treegridOption.idField];
        var rowid = tgTrPreId + id;
        var parent = tg.treegrid("getParent", id);
        //判断是否和之前选择的属于同一直接父级
        var children = tg.treegrid("getChildren", id);
        if (parentId !== undefined && parent[treegridOption.idField] !== parentId) {
            clearChecked();
        }
        if (checked) {
            parentId = parent[treegridOption.idField];
            checkedRowIds.count++;
            checkedRowIds[itemcode] = rowid;
            $(checkedRowIds[itemcode]).addClass(checkedCss);
        } else {
            $(checkedRowIds[itemcode]).removeClass(checkedCss);
            checkedRowIds.count--;
            if (checkedRowIds.count == 0) {
                parentId = undefined;
            }
            delete checkedRowIds[itemcode];
        }
    }
};

var tgviOption = {
    title: lang.mbom.titleDiscreteList,
    height: "100%",
    border: false,
    rownumbers: true,
    cascadeCheck: false,
    idField: COLS.Id,
    treeField: COLS.ItemCode,
    onSelect: function (row) {
        if (row[COLS.Type] == "V" || row[COLS.Type] == "C") {
            var id = row[COLS.ParentId];
            var item = tg.treegrid("find", id);
            if (item != null) {
                tg.treegrid("select", id);
                $(tgTrPreId + id).parents("div.datagrid-body").scrollTo(tgTrPreId + id);
            }
        } else {
            tg.treegrid("select");
        }
    },
    rowStyler: function (row) {
        var cls;
        if (row[COLS.Status] == "_VL") {
            cls = "discreteLinked";
        }
        return { class: cls };
    },
    toolbar: '#dlgDiscreteToolbar',
    columns: [[
        { field: COLS.ItemCode, title: lang.mbom.itemCode, width: "170" },
        { field: COLS.Name, title: lang.mbom.itemName, width: "140" },
        { field: COLS.Quantity, title: lang.mbom.quantity, align: "center", width: "35" },
        { field: COLS.Unit, title: lang.mbom.unit, align: "center", width: "35" }
    ]]
};

var dgItemOption = {
    url: URL_ITEMPAGELIST,
    height: "100%",
    singleSelect: true,
    rownumbers: true,
    border: false,
    pagination: true,
    idField: "CN_ID",
    toolbar: "dlgItemToolbar",
    footer: "dlgItemFooter",
    loadFilter: loadFilter,
    columns: [[
        { field: "CN_ITEM_CODE", title: "物料编码", width: 130 },
        { field: "CN_NAME", title: "物料名称", width: 130 },
        { field: "CN_UNIT", title: "单位", align: "center", width: 40 },
        { field: "CN_WEIGHT", title: "重量", align: "center", width: 40 },
        {
            field: "Desc", title: "类型", align: "center", width: 120,
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
            field: "CN_DT_CREATE", title: "发布日期", align: "center", width: 80,
            formatter: function (value, row, index) {
                if (value && $.trim(value) != "") {
                    return ToJavaScriptDate(value);
                }
                return value;
            }
        }
    ]],
    onLoadSuccess: function (data) {
        if (data.rows.length > 0) {
            dgItem.datagrid("selectRow", 0);
        }
    }
}

$(function () {
    InitPage();
});
/*

//初始化界面

*/
function InitPage() {
    tg.treegrid(treegridOption);
    tgvi.treegrid(tgviOption);
    dgItem.datagrid(dgItemOption);
    initDialogs();
    initEvents();
    reloadAllTables();
}

//过滤离散件
function filterDiscrete(value) {
    if (!value) {
        tgvi.treegrid("loadData", discreteList);
        return;
    }
    var filterList = [];
    for (var i = 0, len = discreteList.length; i < len; i++) {
        var discrete = discreteList[i];
        var filters = value.split(",");
        for (var j = 0, lenj = filters.length; j < lenj; j++) {
            var filter = filters[j];
            if (discrete[COLS.Type] == filter) {
                filterList.push(discrete);
            }
        }
    }
    tgvi.treegrid("loadData", filterList);
}

//重新加载表格
function reloadTable(options) {
    var defaults = {
        treegrid: tg,
        url: URL_ITEMTREE,
        loadSuccess: function(data){
            var val = $("#cboDiscreteFilter").combobox("getValue");
            filterDiscrete(val);
        }
    }
    options = $.extend(defaults, options);

    var treegrid = options.treegrid;
    var url = options.url;

    clearChecked();
    treegrid.treegrid("loading");
    postData(url, params, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        if (!result.success) {
            return false;
        }
        list = [];
        discreteList = [];
        buildTree({
            items: result.data,
            list: list
        });
        getDiscreteList(list[0].children);
        treegrid.treegrid("loadData", list);
        treegrid.treegrid("loaded");
        options.loadSuccess(list);
    });
}
/**
///
/// 物料操作 - 按钮事件 Events
///
*/
function initDialogs() {

    $("#dlgItem").dialog({
        title: "选择需要引用的自定义物料",
        width: 600,
        height: 400,
        modal: true,
        closed: true,
        toolbar: '#dlgItemToolbar',
        footer: '#dlgItemFooter'
    });
    
    $("#dlgItemQuantity").dialog({
        width: 400,
        height: 200,
        modal: true,
        closed: true,
        footer: '#dlgItemQuantityFooter'
    });

    $("#dlgCreateCombineItem").dialog({
        width: 400,
        height: 300,
        modal: true,
        closed: true,
        footer: "#dlgCreateCombineItemFooter"
    });
}

function initEvents() {
    $("#cboCombineItemType").change(function () {
        var itemcode = $("#dlgCreateCombineItem").dialog("options").itemcode;
        var parentitemcode = $("#dlgCreateCombineItem").dialog("options").parentitemcode;
        if (itemcode == null) { return; }
        var type = $(this).val();
        if (type == "K" || type == "L") {
            $("#txtCombineItemName").html(itemcode.replace(/(K|K[0-9])?P1\s*$/, "KP1"));
        } else if (type == "B") {
            $("#txtCombineItemName").html(parentitemcode.replace(/(K|K[0-9])?P1\s*$/, "KP1"));
        } else if (type == "H") {
            $("#txtCombineItemName").html(itemcode.replace(/(K|K[0-9])?P1\s*$/, "K<sub>N</sub>P1"));
        }
    })
}
//添加物料
function itemEditQuant() {
    var item = tg.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    if (item[COLS.Level] == 0) {
        AlertWin(lang.mbom.noSelectRoot);
        return;
    }
    var itemcode = $.trim(item[COLS.ItemCode])
    var quantity = $.trim(item[COLS.Quantity])
    $("#dlgItemQuantity").dialog({
        closed: false,
        title: "修改引用物料数量[" + itemcode + "]"
    });
    $("#dlgItemQuantity").dialog("center");
    $("#txtEditLinkQuantity").textbox("setValue", quantity);
}

function itemEditQuantityConfirm() {
    var item = tg.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelectMain);
        return;
    }
    var guid = item[COLS.Guid];
    if (!guid) {
        AlertWin(lang.mbom.noSelectRoot);
        return;
    }
    if (!$("#txtEditLinkQuantity").textbox("isValid")) {
        return;
    }
    var quantity = parseFloat($("#txtEditLinkQuantity").textbox("getValue"));
    if (typeof (quantity) != "number") {
        AlertWin("请填写数字，小数点精确到后四位");
        return;
    }
    $("#dlgItemQuantity").dialog("close");
    postData(URL_ITEM_EDITQUANTITY, {
        guid: guid,
        quantity: quantity
    }, function (result) {
        if (result.success) {
            reloadTable();
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}
function openLinkDialog(title) {
    $("#dlgItem").dialog({
        closed: false,
        title: title
    });
    $("#txtItemLinkQuantity").textbox("clear");
    dgItem.datagrid("clearSelections");
}
//引用物料
function itemLink() {
    var pitem = tg.treegrid("getSelected");
    if (!pitem) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    var pitemcode = $.trim(pitem[COLS.ItemCode]);
    var title = "将引用至[" + pitemcode + "]下";
    if (!pitem[COLS.IsAssembly]) {
        $.messager.confirm("提示", "物料["+pitemcode+"]并非组件，确定要在此物料下引用吗？", function (r) {
            if (r) {
                openLinkDialog(title);
            }
        });
    } else {
        openLinkDialog(title);
    }
}

function itemLinkConfirm() {
    var txtQuant = $("#txtItemLinkQuantity");
    if (!txtQuant.textbox("isValid")) {
        return;
    }
    //获取当前产品代号
    //获取父级LINK、父级的链路
    var pitem = tg.treegrid("getSelected");
    var item = dgItem.datagrid("getSelected");
    if (item == null) {
        AlertWin("请选择要引用的物料");
        return;
    }
    var parentcode = pitem[COLS.Code];
    //获取引用的ITEMID
    var code = item["CN_CODE"];
    //获取引用数量
    var quantity = txtQuant.textbox("getValue");
    //提交
    dgItem.datagrid("clearSelections");
    $("#dlgItem").dialog("close");
    postData(URL_ITEM_LINK, {
        prodcode: params.code,
        parentcode: parentcode,
        code: code,
        quantity: quantity
    }, function (result) {
        if (result.success) {
            reloadAllTables();
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}

//取消引用物料
function itemUnlink() {
    var item = tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    var guids = "";
    var itemcode = "";
    if (item == null && items.length == 0) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    if (items.length == 0) {
        items.push(item);
    }
    for (var i = 0, j = items.length; i < j; i++) {
        itemcode = itemcode + items[i][COLS.ItemCode] + ",";
        guids = guids + items[i][COLS.Guid] + ",";
    }
    itemcode = itemcode.substr(0, itemcode.length - 1);
    guids = guids.substr(0, guids.length - 1);
    $.messager.confirm("提示", "您确认要删除物料"+ itemcode +"的引用吗？", function (r) {
        if (r) {
            postData(URL_ITEM_UNLINK, {
                prodcode: params.code,
                guids: guids
            }, function (result) {
                if (result.success) {
                    reloadAllTables();
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            })
        }
    });
}

function queryItem() {
    var param = $("#queryFrm").serializeJSON();
    dgItem.datagrid("load", param);
}

function showItemDetail() {
    var item = dgItem.datagrid("getSelected");
    if (item == null) { return; }
    var title = "物料详情" + item.CN_ITEM_CODE + " " + item.CN_NAME;
    var code = item.CN_CODE
    window.parent.openTab(title, URL_ITEMDETAIL + "?code=" + code);
}
/**
//
//虚件操作 - 按钮事件 Events
//
*/
//设为虚件
function virtualItemSet() {
    //将当前选中项视为选中的
    var item = tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    if (items.length == 0 && item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }else if (items.length > 1) {
        AlertWin(lang.mbom.noMultiSelect);
        return false;
    }
    if (items.length == 1) {
        item = items[0];
    }
    if (item[COLS.Guid] == null) {
        AlertWin(lang.mbom.noSelectRoot);
        return false;
    }
    if (item[COLS.Type] != "") {
        AlertWin(lang.mbom.notSelectHaveType);
        return false;
    }
    if (item.children.length == 0) {
        AlertWin(lang.mbom.haveToSelectParent);
        return false;
    }
    var itemcode = $.trim(item[COLS.ItemCode]);
    var guid = item[COLS.Guid];
    $.messager.confirm("提示", "您将设置“&lt;" + itemcode + "&gt;”为虚件，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_SET, {
                prodcode: params.code,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
}
//删除虚件
function virtualItemDrop() {
    var item = tgvi.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    if (item[COLS.Type] != "V") {
        AlertWin(lang.mbom.notSelectVirtualRoot);
        return false;
    }
    var itemcode = $.trim(item[COLS.ItemCode]);
    var guid = item[COLS.Guid];
    $.messager.confirm("提示", "您将删除虚件“&lt;" + itemcode + "&gt;”，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_DROP, {
                prodcode: params.code,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
}

//引用虚件
function virtualItemLink() {
    //是否选中主表，且选中的主表有效
    var item = tg.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    //右侧是否选中的有效
    var citem = tgvi.treegrid("getSelected");
    if (!citem) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    //右侧是否选中根节点
    if (citem[COLS.Type] != "V") {
        AlertWin(lang.mbom.notSelectDiscreteVirtual);
        return false;
    }
    //左右是否为父子
    if (citem[COLS.ParentId].indexOf(item[COLS.Id]) < 0) {
        AlertWin(lang.mbom.notChild);
        return false;
    }
    //判断是否为子级
    var p_itemcode = $.trim(item[COLS.ItemCode]);
    var c_itemcode = $.trim(citem[COLS.ItemCode]);
    var parentcode = item[COLS.Code];
    var guid = citem[COLS.Guid];
    if (citem[COLS.Type] != "V") {
        AlertWin(lang.mbom.notSelectDiscreteVirtual);
        return false;
    }
    $.messager.confirm("提示", "您将在“" + p_itemcode + "”下引用虚件“" + c_itemcode + "”，请您确认！", function (r) {
        if (r) {
            postData(URL_VIRTUAL_ITEM_LINK, {
                prodcode: params.code,
                parentcode: parentcode,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
}
//取消引用虚件
function virtualItemUnlink() {
    //判断是否选中离散区的虚件
    var item = tgvi.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (item[COLS.Type] != "V") {
        AlertWin(lang.mbom.notSelectVirtual);
        return false;
    }
    var itemcode = $.trim(item[COLS.ItemCode]);
    var guid = item[COLS.Guid]
    $.messager.confirm("提示", "您将取消虚件“&lt;" + itemcode + "&gt;”的引用，请您确认！", function (r) {
        if (r) {
            postData(URL_VIRTUAL_ITEM_UNLINK, {
                prodcode: params.code,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            })
        }
    });
}
/**
//
//合件操作 - 按钮事件 Events
//
*/
//新建合件
function compositeItemSet() {
    //纯显示用变量
    var itemcode = undefined;
    var parentitemcode = undefined;
    var childrenItemCodeStr = "";
    var childrenItemNameStr = "";
    //业务用变量
    var guids = "";
    var item = tg.treegrid("getSelected")
    var items = tg.treegrid("getCheckedNodes");
    $("#cboCombineItemType").find("option[value=-]").prop("selected", "selected").change();
    if (item == null && items.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return;
    }
    if (items.length > 1) {
        itemcode = tg.treegrid("getParent", items[0][COLS.Id])[COLS.ItemCode];
        for (var i = 0, len = items.length; i < len; i++) {
            var item = items[i];
            guids = item[COLS.Guid] + "," + guids;
            childrenItemCodeStr = childrenItemCodeStr + item[COLS.ItemCode] + "<br/>";
            childrenItemNameStr = childrenItemNameStr + item[COLS.Name] + "<br/>";
        }
        guids = guids.substring(0, guids.length - 1);
        $("#dlgCreateCombineItem").dialog({
            title: "新建合件" + itemcode,
            itemcode: itemcode,
            data: {
                guids: guids
            },
            closed: false
        });
        $("#cboCombineItemType").find("option[value=B]").hide();
        $("#cboCombineItemType").find("option[value=K]").hide();
        $("#cboCombineItemType").find("option[value=H]").show();
        $("#cboCombineItemType").find("option[value=L]").show();
        $("#cboCombineItemType").find("option[value=H]").prop("selected", "selected").change();
        $("#txtCombineItemChildrenItemCode").html(childrenItemCodeStr);
        $("#txtCombineItemChildrenItemName").html(childrenItemNameStr);
    } else {
        if (items.length == 1) {
            item = items[0];
        }
        if (item[COLS.Type] == "产品") {
            //选择的是根节点，不允许做合件
            AlertWin(lang.mbom.noSelectRoot);
            return;
        }
        itemcode = $.trim(item[COLS.ItemCode])
        parentitemcode = $.trim(tg.treegrid("getParent", item[COLS.Id])[COLS.ItemCode]);
        guids = item[COLS.Guid];
        $("#dlgCreateCombineItem").dialog({
            title: "新建合件" + itemcode,
            itemcode: itemcode,
            parentitemcode: parentitemcode,
            data: {
                guids: guids
            },
            closed: false
        });
        $("#cboCombineItemType").find("option[value=H]").hide();
        $("#cboCombineItemType").find("option[value=L]").hide();
        $("#cboCombineItemType").find("option[value=B]").show();
        $("#cboCombineItemType").find("option[value=K]").show();
        $("#cboCombineItemType").find("option[value=K]").prop("selected", "selected").change();
        $("#txtCombineItemChildrenItemCode").html(itemcode);
        $("#txtCombineItemChildrenItemName").html(item[COLS.Name]);
    }

}
//设置合件
function dlgCreateCombineItemConfirm() {
    var data = $("#dlgCreateCombineItem").dialog("options").data;
    if (!data) {
        AlertWin("参数获取失败，请联系管理员");
        return false;
    }
    //获取cboCombineItemCode
    var type = $("#cboCombineItemType").val();
    data.type = type;
    data.prodcode = params.code;
    //选中有效，传入data参数
    $("#dlgCreateCombineItem").dialog("close");
    postData(URL_COMPOSITE_ITEM_SET, data,
        function (result) {
            if (result.msg) {
                InfoWin(result.msg);
            }
            if (result.success) {
                reloadAllTables(true);
            }
        });
}
//删除合件
function compositeItemDrop() {
    var item = tgvi.treegrid("getSelected") || tg.treegrid("getSelected")
    var items = tg.treegrid("getCheckedNodes");
    if (item == null && items.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.noMultiSelect);
        return false;
    }
    if (items.length == 1) {
        item = items[0];
    }
    var itemcode = $.trim(item[COLS.ItemCode]);
    var guid = item[COLS.Guid];
    if (item[COLS.Type] != "C") {
        AlertWin(lang.mbom.notSelectDiscreteComposite)
        return false;
    }
    $.messager.confirm("提示", "您将删除合件“&lt;" + itemcode + "&gt;”，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_COMPOSITE_ITEM_DROP, {
                prodcode: params.code,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
} 
//引用合件
function compositeItemLink() {
    var item = tg.treegrid("getSelected");
    var items = tgvi.treegrid("getCheckedNodes");

    var citem = tgvi.treegrid("getSelected");
    //判断是否选中主表，且选中的主表有效
    if (item == null && items.length == 0) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.noMultiSelect);
        return false;
    }
    if (items.length == 1) {
        item = items[0];
    }
    //判断右侧是否选中的有效
    if (citem == null) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    var p_itemcode = $.trim(item[COLS.ItemCode])
    var c_itemcode = $.trim(citem[COLS.ItemCode]);
    var parentcode = item[COLS.Code];
    var guid = citem[COLS.Guid];
    if (citem[COLS.Type] != "C") {
        AlertWin(lang.mbom.notSelectDiscreteComposite);
        return false;
    }
    $.messager.confirm("提示", "您将在“" + p_itemcode + "”下引用合件“" + c_itemcode + "”，请您确认！", function (r) {
        if (r) {
            postData(URL_COMPOSITE_ITEM_LINK, {
                prodcode: params.code,
                parentcode: parentcode,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
}
//取消合件引用
function compositeItemUnlink() {
    //判断是否选中主表，且选中的主表有效
    var item = tgvi.treegrid("getSelected") || tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    if (item == null && items.length == 0) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.noMultiSelect);
        return false;
    }
    if (items.length == 1) {
        item = items[0];
    }
    var itemcode = $.trim(item[COLS.ItemCode]);
    var guid = $.trim(item[COLS.Guid]);
    if (item[COLS.Type] != "C") {
        AlertWin(lang.mbom.notSelectComposite);
        return false;
    }
    $.messager.confirm("提示", "您将取消合件“" + itemcode + "”的引用，请您确认！", function (r) {
        if (r) {
            postData(URL_COMPOSITE_ITEM_UNLINK, {
                prodcode: params.code,
                guid: guid
            }, function (result) {
                if (result.msg) {
                    InfoWin(result.msg);
                }
                if (result.success) {
                    reloadAllTables(true);
                }
            });
        }
    });
}

//editCombineName 修改合件名称
function editCombineName() {
    var data = tgvi.treegrid("getSelected");
    if (!data) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (data[COLS.Type] != "C") {
        AlertWin(lang.mbom.notSelectComposite);
        return false;
    }
    var guid = data[COLS.Guid];
    var m = $.messager.prompt({
        title: lang.mbom.editCombineName,
        msg: lang.mbom.inputNewCombineName,
        fn: function (r) {
            if (!r) {
                return;
            }
            postData(URL_EDITCOMBINENAME, { guid: guid, name: r },
                function (result) {
                    if (result.success) {
                        reloadDiscrete();
                    } else {
                        AlertWin(result.msg);
                    }
                }
            );
        }
    });
    m.find('.messager-input').val($.trim(data[COLS.Name]));
}

//清空所有的选中
function clearChecked() {
    tg.treegrid("clearChecked");
    tg.treegrid("unselectAll");
    tgvi.treegrid("unselectAll");
    for (var i in checkedRowIds) {
        var rowid = checkedRowIds[i];
        $(rowid).removeClass(checkedCss);
    }
    checkedRowIds = { count: 0 };
    parentId = undefined;
}
//重新加载所有表格
function reloadAllTables(force) {
    if (!canRefresh && !force) {
        InfoWin(lang.mbom.refreshCooldown)
        return false;
    }
    canRefresh = false;
    setTimeout(function () {
        canRefresh = true;
    }, refreshCooldown * 1000)

    reloadTable();
}
//切换-选中行同时选中节点
function toggleCheckStateOnSelect() {
    checkOnSelect = !checkOnSelect;
    $("#btnToggleCheckState").toggleClass("bg-success");
}
function checkAllChildren() {
    var item = tg.treegrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelectMain);
        return;
    }
    clearChecked();
    var children = item.children;
    for (var i = 0, len = children.length; i < len; i++) {
        var id = children[i][COLS.Id];
        tg.treegrid("checkNode", id);
    }
}
//通用方法

//构造树
function buildTree(options) {
    var settings = {
        items: [],
        list: [],
        pval: null,
        pid: COLS.ParentId,
        id: COLS.Id,
        children: "children"
    };

    $.extend(settings, options);

    var items = settings.items;
    var list = settings.list;
    var id = settings.id;
    var pid = settings.pid;
    var pval = settings.pval;
    var children = settings.children;

    var count = items.length;

    if (count == 0) {
        return false;
    }
    for (var i = 0; i < count; i++) {
        var item = items[i];
        if (item[pid] == pval || item[COLS.Level] == 0) {
            switch (item[COLS.Type]) {
                case "V":
                    item.iconCls = "icon-virtual";
                    break;
                case "C":
                    item.iconCls = "icon-combine";
                    break;
            }
            list.push(item);
            remove(items, i);
            i--;
            count--;
        }
    }

    for (var i = 0, len = list.length; i < len; i++) {
        var item = list[i];
        item[children] = item[children] ? item[children] : [];
        buildTree($.extend(settings, { items: items, list: item[children], pval: item[id] }));
    }
}

function getDiscreteList(l) {
    for (var i = 0, j = l.length; i < j; i++) {
        var item = l[i];
        if (item == null) {
            continue;
        }
        if (item[COLS.Status] == "Y" ||
            item[COLS.Status] == "V" ||
            item[COLS.Status] == "C") {
            getDiscreteList(item.children);
            continue;
        }
        discreteList.push(l.splice(i, 1)[0]);
        i--;
    }
}
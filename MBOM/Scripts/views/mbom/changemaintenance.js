var checkedRowIds = { count: 0 };
var parentId = undefined;
var checkedCss = "datagrid-row-checked-custom";
var refreshCooldown = 5;
var tgTrPreId = "#datagrid-row-r1-2-";
var dgChangesTrPreId = "#datagrid-row-r2-2-";

var checkOnSelect = false;
var canRefresh = true;

//列表信息获取
var URL_ENTER = "/MBOM/Maintenance";
var URL_ITEMTREE = "/MBOM/List";
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
var dgChanges = $("#dgChanges");

var treegridOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    idField: "ID",
    treeField: "ITEM_CODE",
    toolbar: '#toolbar',
    rowStyler: function (row) {
        switch (row["MBOMTYPE"]) {
            case "VP":
                return { class: "parent-virtual" };
                break;
            case "V":
                return { class: "main-virtual" };
                break;
            case "C":
                return { class: "main-combine" };
                break;
        }
    },
    columns: [[
        { field: 'LEVEL', title: lang.mbom.level, width: "35",align:"center" },
        { field: 'ITEM_CODE', title: lang.mbom.itemCode, width: "220" },
        { field: 'NAME', title: lang.mbom.itemName, width: "150" },
        { field: 'QUANTITY', title: lang.mbom.quantity, width: "50",align:"right" },
        { field: 'UNIT', title: lang.mbom.unit, align: "center", width: "35" },
        {
            field: 'ISBORROW', title: "借用", align: "center", width: "35",
            formatter: function (value, row, index) {
                if (value) {
                    return  "借用";
                }
            }
        }
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
        var itemid = row[treegridOption.idField];
        var rowid = tgTrPreId + itemid;
        if (checked) {
            clearMainSelected();
            var canCheck = canCheckNode(itemid);
            if (!canCheck) { return canCheck; }
            checkedRowIds.count++;
            checkedRowIds[itemid] = rowid;
            $(checkedRowIds[itemid]).addClass(checkedCss);
        } else {
            $(checkedRowIds[itemid]).removeClass(checkedCss);
            checkedRowIds.count--;
            if (checkedRowIds.count == 0) {
                parentId = undefined;
            }
            delete checkedRowIds[itemid];
        }
    }
};

var dgDeductionOption = {
    height: "100%",
    singleSelect: true,
    rownumbers: true,
    border: false,
    fitColumns: true,
    idField: '',
    columns: [[
    ]]
}

$(function () {
    postData(URL_ENTER, params, function (result) {
        if (result.msg) {
            InfoWin(result.msg)
        }
        if (result.success) {
            InitPage();
        }
    });
});
/*

//初始化界面

*/
function InitPage() {
    tg.treegrid(treegridOption);
    dgItem.datagrid(dgItemOption);
    dgDeduction.datagrid(dgDeductionOption);
    initDialogs();
    initEvents();
    reloadAllTables();
}

//重新加载右侧表格
//过滤离散件
//清空离散区表的关联样式
//是否可以选中节点
function canCheckNode(itemid) {
    var parent = tg.treegrid("getParent", itemid);
    if (!parent) {
        InfoWin(lang.mbom.noSelectRoot);
        return false;
    }
    //判断是否和之前选择的属于同一直接父级
    var children = tg.treegrid("getChildren", itemid);
    if (parentId !== undefined && parent[treegridOption.idField] !== parentId) {
        return false;
    }
    parentId = parent[treegridOption.idField];
    return true;
}

//重新加载表格
function reloadTable(options) {
    var defaults = {
        treegrid: tg,
        url: URL_ITEMTREE,
        loadSuccess: function(data){

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

        var list = [];
        buildTree({
            items: result.data,
            list: list,
            pid: "PARENTID",
            id: "ID"
        });
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

    $("#dlgDeduction").dialog({
        width: 600,
        height: 400,
        modal: true,
        closed: true,
        toolbar: '#dlgDeductionToolbar',
        footer: '#dlgDeductionFooter'
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
        if (!itemcode) { return; }
        var type = $(this).val();
        if (type == "K") {
            $("#txtCombineItemName").html(itemcode.replace(/(K|K[0-9])?P1\s*$/, "KP1"));
        } else {
            $("#txtCombineItemName").html(itemcode.replace(/(K|K[0-9])?P1\s*$/, "K<sub>N</sub>P1"));
        }
    })
}
//添加物料
function itemEditQuant() {
    var item = tg.treegrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    if (item["LEVEL"] == 0) {
        AlertWin(lang.mbom.noSelectRoot);
        return;
    }
    var itemcode = $.trim(item["ITEM_CODE"])
    var quantity = $.trim(item["QUANTITY"])
    $("#dlgItemQuantity").dialog({
        closed: false,
        title: "修改引用物料数量[" + itemcode + "]"
    });
    $("#dlgItemQuantity").dialog("center");
    $("#txtEditLinkQuantity").textbox("setValue", quantity);
}

function itemEditQuantityConfirm() {
    var item = tg.treegrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return;
    }
    var hlinkid = item["HLINK_ID"];
    if (!hlinkid) {
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
        hlinkid: hlinkid,
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
    $("#dlgItem").dialog("center");
}
//引用物料
function itemLink() {
    var pitem = tg.treegrid("getSelected");
    if (!pitem) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    var pitemcode = $.trim(pitem["ITEM_CODE"]);
    var title = "将引用至[" + pitemcode + "]下";
    if (!pitem["IS_ASSEMBLY"]) {
        $.messager.confirm("提示", "物料并非组件，确定要在此物料下引用吗？", function (r) {
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
    //获取父级ID、父级的链路
    var pitem = tg.treegrid("getSelected");
    var item = dgItem.datagrid("getSelected");
    var pid = pitem["ITEMID"];
    var plink = pitem["LINK"];
    //获取引用的ITEMID
    var itemid = item["CN_ID"];
    //获取引用数量
    var quantity = txtQuant.textbox("getValue");
    //提交
    $("#dlgItem").dialog("close");
    postData(URL_ITEM_LINK, {
        pid: pid,
        plink: plink,
        itemid: itemid,
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
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    var hlinkid = item["HLINK_ID"];
    var itemcode = $.trim(item["ITEM_CODE"]);
    $.messager.confirm("提示", "您确认要删除物料"+ itemcode +"的引用吗？此操作仅可用于自定义添加物料", function (r) {
        if (r) {
            postData(URL_ITEM_UNLINK, { hlinkid: hlinkid }, function (result) {
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
    if (!item) { return; }
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
    if (!item) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    if (item["MBOMTYPE"]) {
        AlertWin(lang.mbom.notSelectHaveType);
        return false;
    }
    if (!item["ITEMID"] || !item["BOM_ID"]) {
        AlertWin(lang.mbom.noSelectRoot);
        return false;
    }
    if (item.children.length == 0) {
        AlertWin(lang.mbom.haveToSelectParent);
        return false;
    }
    var itemcode = $.trim(item["ITEM_CODE"]);
    $.messager.confirm("提示", "您将设置“&lt;" + itemcode + "&gt;”为虚件，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_SET, {
                bomid: item["BOM_ID"],
                itemid: item["ITEMID"],
                show: 0
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
//设为虚件，且引用时显示在列表中
function virtualItemSetShow() {
    //将当前选中项视为选中的
    var item = tg.treegrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    if (item["MBOMTYPE"]) {
        AlertWin(lang.mbom.notSelectHaveType);
        return false;
    }
    if (!item["ITEMID"] || !item["BOM_ID"]) {
        AlertWin(lang.mbom.noSelectRoot);
        return false;
    }
    if (item.children.length == 0) {
        AlertWin(lang.mbom.haveToSelectParent);
        return false;
    }
    var itemcode = $.trim(item["ITEM_CODE"]);
    $.messager.confirm("提示", "您将设置“&lt;" + itemcode + "&gt;”为虚件，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_SET, {
                bomid: item["BOM_ID"],
                itemid: item["ITEMID"],
                show: 1
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
function clearMainSelected() {
    tg.treegrid("unselectAll");
}

function clearMainChecked() {
    tg.treegrid("clearChecked");
    for (var i in checkedRowIds) {
        var rowid = checkedRowIds[i];
        $(rowid).removeClass(checkedCss);
    }
    checkedRowIds = { count: 0 };
    parentId = undefined;
}
//清空所有的选中
function clearChecked() {
    clearMainChecked();
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
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return;
    }
    clearMainChecked();
    var children = item.children;
    for (var i in children) {
        var id = children[i]["ID"];
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
        pid: "pid",
        id: "id",
        isroot: "ISROOT",
        children: "children",
        type: null
    };

    $.extend(settings, options);

    var items = settings.items;
    var list = settings.list;
    var id = settings.id;
    var pid = settings.pid;
    var pval = settings.pval;
    var isroot = settings.isroot;
    var type = settings.type;
    var children = settings.children;

    var count = items.length;

    if (count == 0) {
        return false;
    }

    for (var i = 0; i < count; i++) {
        var item = items[i];
        if (item[pid] == pval || item[isroot]) {
            if (item["MBOMTYPE"] == null) {
                if (type == "V" || type == "C") {
                    item["MBOMTYPE"] = type;
                }
            }
            switch (item["MBOMTYPE"]) {
                case "V":
                case "VP":
                case "VC":
                    item.iconCls = "icon-virtual";
                    break;
                case "C":
                case "CC":
                    item.iconCls = "icon-combine";
                    break;
            }
            list.push(item);
            remove(items, i);
            i--;
            count--;
        }
    }

    for (var i in list) {
        var item = list[i];
        item[children] = item[children] ? item[children] : [];
        buildTree($.extend(settings, { items: items, list: item[children], pval: item[id], type: item["MBOMTYPE"]}));
    }
}
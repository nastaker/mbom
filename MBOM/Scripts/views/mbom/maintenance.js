var checkedRowIds = { count: 0 };
var discreteList = [];
var parentId = undefined;
var gradientStep = 6;
var gradient = new gradientColor('#11ff55', '#ffff00', gradientStep);
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
//
var URL_ITEMPROCESSINFO = "/Item/ProductProcessInfo";
var URL_ITEMDEDUCTIONSET = "/MBOM/ItemDeductionSet";
//物料
var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_ITEMPAGELIST = "/Item/MaintenancePageList"
var URL_ITEM_LINK = "/MBOM/ItemLink"
var URL_ITEM_UNLINK = "/MBOM/ItemUnlink"
var URL_ITEM_EDITQUANTITY = "/MBOM/ItemEditQuantity"


var tg = $("#treegrid");
var tgvi = $("#tgvi");
var dgDeduction = $("#dgDeduction");
var dgItem = $("#dgItem");

var treegridOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    checkbox: function (row) {
        if (row["MBOMTYPE"] == "产品" || row["MBOMTYPE"] == "VP") {
            return false;
        }
        return true;
    },
    cascadeCheck: false,
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
        { field: 'ITEM_CODE', title: lang.mbom.itemCode, width: "250" },
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
        if (row == null) { return; }
        //选中时判断tgvi中是否拥有此项子级，若有则标红
        clearTgviClass();
        //循环tgvi的根判断
        var link = row["LINK"];
        var plink = row["PARENT_LINK"]
        var ptype = row["MBOMTYPE"];
        var pid = row[treegridOption.idField];
        var croots = tgvi.treegrid("getRoots");
        var lastId = undefined;
        var isFirst = true;
        for (var i in croots) {
            var croot = croots[i];
            var crootId = croot[tgviOption.idField];
            var classId = tgviTrPreId + crootId;
            var c_plink = croot["PARENT_LINK"];
            var c_mbomtype = croot["MBOMTYPE"];
            if (c_plink === link) {
                if (isFirst) {
                    lastId = classId;
                    isFirst = false;
                }
                if (c_mbomtype == "V") {
                    $(classId).addClass("discrete-direct-virtual");
                } else if (c_mbomtype == "C") {
                    $(classId).addClass("discrete-combine");
                }
            } else if (ptype == "C" && pid != crootId && plink === c_plink && c_mbomtype == "C") {
                $(classId).addClass("discrete-combine");
            } else if (c_plink.indexOf(link) == 0 && c_mbomtype == "V") {
                if (isFirst) {
                    lastId = classId;
                    isFirst = false;
                }
                $(classId).addClass("discrete-sub-virtual");
            }
        }
        if (!isFirst) {
            $(lastId).parents("div.datagrid-body").scrollTo(lastId);
        }



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

var tgviOption = {
    title: lang.mbom.titleDiscreteList,
    height: "100%",
    border: false,
    cascadeCheck: false,
    lines: true,
    idField: "ID",
    treeField: "ITEM_CODE",
    checkbox: function (row) {
        if (row["ISROOT"]) {
            return true;
        }
        return false;
    },
    onBeforeSelect: function(row){
        if (row["ISROOT"]) {
            var id = row["PARENTID"];
            var item = tg.treegrid("find", id);
            if (item != null) {
                clearTgviClass();
                tg.treegrid("select", id);
                $(tgTrPreId + id).parents("div.datagrid-body").scrollTo(tgTrPreId + id);
            }
            return true;
        }
        return false;
    },
    onBeforeCheckNode: function (row, checked) {
        var tg = $(this);
        if (checked) {
            tg.treegrid("clearChecked");
        }
    },
    rowStyler: function (row) {
        if (row["ISLINKED"]) {
            return { class: "discreteLinked" };
        }
    },
    toolbar: '#dlgDiscreteToolbar',
    columns: [[
        { field: 'ITEM_CODE', title: lang.mbom.itemCode, width: "170" },
        { field: 'NAME', title: lang.mbom.itemName, width: "140" },
        { field: 'QUANTITY', title: lang.mbom.quantity, align: "center", width: "35" },
        { field: 'UNIT', title: lang.mbom.unit, align: "center", width: "35" }
    ]]
};

var dgDeductionOption = {
    height: "100%",
    singleSelect: true,
    rownumbers: true,
    border: false,
    fitColumns: true,
    idField: 'HLINKID',
    columns: [[
        { field: 'GX_CODE', title: lang.processFlow.gxCode },
        { field: 'GX_NAME', title: lang.processFlow.gxName },
        { field: 'GXNR', title: lang.processFlow.gxnr }
    ]]
}

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
    tgvi.treegrid(tgviOption);
    dgItem.datagrid(dgItemOption);
    dgDeduction.datagrid(dgDeductionOption);
    initDialogs();
    initEvents();
    reloadAllTables();
}

//重新加载右侧表格
function reloadDiscrete() {
    reloadTable({
        treegrid: tgvi,
        url: URL_DISCRETE_LIST,
        loadSuccess: function(data){
            discreteList = data;
            var val = $("#cboDiscreteFilter").combobox("getValue");
            filterDiscrete(val);
        }
    });
}
//过滤离散件
function filterDiscrete(value) {
    if (!value) {
        tgvi.treegrid("loadData", discreteList);
        return;
    }
    var filterList = [];
    for (var i in discreteList) {
        var discrete = discreteList[i];
        if (discrete["MBOMTYPE"] == value) {
            filterList.push(discrete);
        }
    }
    tgvi.treegrid("loadData", filterList);
}

//清空离散区表的关联样式
function clearTgviClass() {
    var croots = tgvi.treegrid("getRoots");
    for (var i in croots) {
        var croot = croots[i];
        var crootId = croot[tgviOption.idField];
        $(tgviTrPreId + crootId).removeClass("datagrid-row-selected discrete-direct-virtual discrete-sub-virtual discrete-combine");
    }
}

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
        var parentitemcode = $("#dlgCreateCombineItem").dialog("options").parentitemcode;
        if (!itemcode) { return; }
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
    if (item["MBOMTYPE"]) {
        AlertWin(lang.mbom.notSelectHaveType);
        return false;
    }
    if (item.children.length == 0) {
        AlertWin(lang.mbom.haveToSelectParent);
        return false;
    }
    if (!item["ITEMID"] || !item["BOM_ID"]) {
        AlertWin(lang.mbom.noSelectRoot);
        return false;
    }
    var itemcode = $.trim(item["ITEM_CODE"]);
    $.messager.confirm("提示", "您将设置“&lt;" + itemcode + "&gt;”为虚件，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_SET, {
                code: params.code,
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
    var item = tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    if (items.length == 0 && item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    } else if (items.length > 1) {
        AlertWin(lang.mbom.noMultiSelect);
        return false;
    }
    if (items.length == 1) {
        item = items[0];
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
                code: params.code,
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
//删除虚件
function virtualItemDrop() {
    var items = tgvi.treegrid("getCheckedNodes");
    if (items.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.notSelectSingleDiscrete)
        return false;
    }
    var item = items[0];
    var itemcode = $.trim(item["ITEM_CODE"]);
    $.messager.confirm("提示", "您将删除虚件“&lt;" + itemcode + "&gt;”，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_VIRTUAL_ITEM_DROP, {
                code: params.code,
                bomid: item["BOM_ID"],
                itemid: item["ITEMID"]
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
    //判断是否选中主表，且选中的主表有效
    var item = tg.treegrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    //判断右侧是否选中的有效
    var citems = tgvi.treegrid("getCheckedNodes");
    if (citems.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }else if (citems.length > 1) {
        AlertWin(lang.mbom.notSelectSingleDiscrete);
        return false;
    }
    var citem = citems[0];
    var plink = item["LINK"];
    var cplink = citem["PARENT_LINK"];
    var link = citem["LINK"];
    var parentitemid = item["ITEMID"];
    var itemid = citem["ITEMID"];
    var type = citem["MBOMTYPE"];
    var p_itemcode = $.trim(item["ITEM_CODE"]);
    var c_itemcode = $.trim(citem["ITEM_CODE"]);
    if (type != "V") {
        AlertWin(lang.mbom.notSelectDiscreteVirtual);
        return false;
    }
    if (cplink.indexOf(plink) != 0 && plink != cplink) {
        AlertWin(lang.mbom.notChild);
        return false;
    }
    $.messager.confirm("提示", "您将在“" + p_itemcode + "”下引用虚件“" + c_itemcode + "”，请您确认！", function (r) {
        if (r) {
            postData(URL_VIRTUAL_ITEM_LINK, {
                code: params.code,
                parentitemid: parentitemid,
                itemid: itemid,
                parentlink: plink,
                link: link
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
    var items = tgvi.treegrid("getCheckedNodes");
    if (items.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.notSelectSingleDiscrete)
        return false;
    }
    var item = items[0];
    var itemid = item["ITEMID"];
    var link = item["LINK"];
    var itemcode = $.trim(item["ITEM_CODE"]);
    if (item["MBOMTYPE"] != "V") {
        AlertWin(lang.mbom.notSelectVirtual);
        return false;
    }
    $.messager.confirm("提示", "您将取消虚件“&lt;" + itemcode + "&gt;”的引用，请您确认！", function (r) {
        if (r) {
            postData(URL_VIRTUAL_ITEM_UNLINK, {
                code: params.code,
                itemid: itemid,
                link: link
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
    //获取选中的件的ITEMID、BOMID，若多个用逗号[,]拼接其ITEMID，但BOMID仅能是一个，若多个的情况取消操作
    var bomid = undefined;
    var itemcode = undefined;
    var parentitemcode = undefined;
    var itemids = "";
    var childrenItemCodeStr = "";
    var childrenItemNameStr = "";
    var item = tg.treegrid("getSelected")
    var items = tg.treegrid("getCheckedNodes");
    var link;
    $("#cboCombineItemType").find("option[value=-]").prop("selected", "selected").change();
    if (!item && items.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return;
    }
    if (items.length > 1) {
        for (var i in items) {
            var item = items[i];
            if (!itemcode) {
                itemcode = tg.treegrid("getParent", item["ID"])["ITEM_CODE"];
            }
            if (!bomid) {
                bomid = item["BOM_ID"]
            } else if (bomid != item["BOM_ID"]) {
                AlertWin(lang.mbom.notSelectSameParent);
                return;
            }
            itemids = item["ITEMID"] + "," + itemids;
            childrenItemCodeStr = childrenItemCodeStr + item["ITEM_CODE"] + "<br/>";
            childrenItemNameStr = childrenItemNameStr + item["NAME"] + "<br/>";
        }
        itemids = itemids.substring(0, itemids.length - 1);
        link = items[0]["PARENT_LINK"];
        $("#dlgCreateCombineItem").dialog({
            title: "新建合件" + itemcode,
            itemcode: itemcode,
            data: {
                bomid: bomid,
                link: link,
                itemids: itemids
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
        if (item["MBOMTYPE"] == "产品") {
            //选择的是根节点，不允许做合件
            AlertWin(lang.mbom.noSelectRoot);
            return;
        }
        itemcode = $.trim(item["ITEM_CODE"])
        parentitemcode = $.trim(tg.treegrid("getParent", item["ID"])["ITEM_CODE"]);
        bomid = item["BOM_ID"];
        itemids = item["ITEMID"];
        link = item["PARENT_LINK"];
        $("#dlgCreateCombineItem").dialog({
            title: "新建合件" + itemcode,
            itemcode: itemcode,
            parentitemcode: parentitemcode,
            data: {
                code: params.code,
                bomid: bomid,
                link: link,
                itemids: itemids
            },
            closed: false
        });
        $("#cboCombineItemType").find("option[value=H]").hide();
        $("#cboCombineItemType").find("option[value=L]").hide();
        $("#cboCombineItemType").find("option[value=B]").show();
        $("#cboCombineItemType").find("option[value=K]").show();
        $("#cboCombineItemType").find("option[value=K]").prop("selected", "selected").change();
        $("#txtCombineItemChildrenItemCode").html(itemcode);
        $("#txtCombineItemChildrenItemName").html(item["NAME"]);
    }

}
function dlgCreateCombineItemConfirm() {
    var data = $("#dlgCreateCombineItem").dialog("options").data;
    if (!data) {
        AlertWin("参数获取失败，请联系管理员");
        return false;
    }
    //获取cboCombineItemCode
    var type = $("#cboCombineItemType").val();
    data.type = type;
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
    var items = tgvi.treegrid("getCheckedNodes");
    if (items.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    }
    if (items.length > 1) {
        AlertWin(lang.mbom.notSelectSingleDiscrete)
        return false;
    }
    var item = items[0];
    var itemcode = $.trim(item["ITEM_CODE"]);
    if (item["MBOMTYPE"] != "C") {
        AlertWin(lang.mbom.notSelectDiscreteComposite)
        return false;
    }
    $.messager.confirm("提示", "您将删除合件“&lt;" + itemcode + "&gt;”，请您确认！", function (r) {
        if (r) {
            //选中有效，传入bomid和itemid
            postData(URL_COMPOSITE_ITEM_DROP, {
                code: params.code,
                bomid: item["BOM_ID"],
                itemid: item["ITEMID"]
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
    //判断是否选中主表，且选中的主表有效
    var item = tg.treegrid("getSelected");
    if (!item) {
        AlertWin(lang.mbom.notSelectMain);
        return false;
    }
    //判断右侧是否选中的有效
    var citems = tgvi.treegrid("getCheckedNodes");
    if (citems.length == 0) {
        AlertWin(lang.mbom.notSelectDiscrete);
        return false;
    } else if (citems.length > 1) {
        AlertWin(lang.mbom.notSelectSingleDiscrete);
        return false;
    }
    var citem = citems[0];
    var plink = item["MBOMTYPE"] == "C" ? item["PARENT_LINK"] : item["LINK"];
    var cplink = citem["PARENT_LINK"];
    var link = citem["LINK"];
    var parentitemid = item["ITEMID"];
    var itemid = citem["ITEMID"];
    var type = citem["MBOMTYPE"];
    var p_code = $.trim(item["CODE"]);
    var p_itemcode = $.trim(item["ITEM_CODE"])
    var c_itemcode = $.trim(citem["ITEM_CODE"]);
    if (p_code == c_itemcode) {
        AlertWin(lang.mbom.notSelectSame);
        return false;
    }
    if (type != "C") {
        AlertWin(lang.mbom.notSelectDiscreteComposite);
        return false;
    }
    if (plink != cplink) {
        AlertWin(lang.mbom.notChild);
        return false;
    }
    $.messager.confirm("提示", "您将在“" + p_itemcode + "”下引用合件“" + c_itemcode + "”，请您确认！", function (r) {
        if (r) {
            postData(URL_COMPOSITE_ITEM_LINK, {
                code: params.code,
                parentitemid: parentitemid,
                itemid: itemid,
                parentlink: plink,
                link: link
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
    var item = tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    if (!item && items.length == 0) {
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
    var type = item["MBOMTYPE"];
    var itemid = item["ITEMID"];
    var itemcode = $.trim(item["ITEM_CODE"]);
    var code = $.trim(item["CODE"]);
    var link = item["LINK"];
    var bomid = item["BOM_ID"];
    if (type != "C") {
        AlertWin(lang.mbom.notSelectComposite);
        return false;
    }
    $.messager.confirm("提示", "您将取消合件“" + itemcode + "”的引用，请您确认！", function (r) {
        if (r) {
            postData(URL_COMPOSITE_ITEM_UNLINK, {
                code: params.code,
                itemid: itemid,
                bomid: bomid,
                link: link
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
//虚件下引用合件
function linkC2V() {
    //判断右侧是否选中的有效
    var items = tgvi.treegrid("getCheckedNodes");
    //必选中两项才可操作
    if (items.length != 2) {
        AlertWin(lang.mbom.notSelectDiscreteTwoItems);
        return false;
    }
    //必选中一合件一虚件才可操作
    var vitem,citem 
    for (var i in items) {
        var item = items[i];
        if (item["MBOMTYPE"] == "C") {
            vitem = item;
        } else {
            citem = item;
        }
    }
    if (!vitem || !citem) {
        AlertWin(lang.mbom.notSelectDiscreteVCItems);
        return false;
    }
    //
    var v_itemcode = $.trim(vitem["ITEM_CODE"])
    var c_itemcode = $.trim(citem["ITEM_CODE"])
    var parentitemid = vitem["ITEMID"]
    var itemid = citem["ITEMID"]
    var plink = vitem["LINK"]
    var link = citem["LINK"]
    $.messager.confirm("提示", "您将在虚件“" + v_itemcode + "”下引用合件“" + c_itemcode + "”，请您确认！", function (r) {
        if (r) {
            postData(URL_COMPOSITE_ITEM_LINK, {
                parentitemid: parentitemid,
                itemid: itemid,
                parentlink: plink,
                link: link
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
    if (data["MBOMTYPE"] != "C") {
        AlertWin(lang.mbom.notSelectComposite);
        return false;
    }
    var itemid = data["ITEMID"];
    var m = $.messager.prompt({
        title: lang.mbom.editCombineName,
        msg: lang.mbom.inputNewCombineName,
        fn: function (r) {
            if (!r) {
                return;
            }
            postData(URL_EDITCOMBINENAME, { itemid: itemid, name: r },
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
    m.find('.messager-input').val($.trim(data["NAME"]));
}

//打开扣料窗口
function deductionDialog() {
    //若当前未选中任何件则不允许启动扣料
    var item = tg.treegrid("getSelected");
    var items = tg.treegrid("getCheckedNodes");
    if (!item && items.length == 0) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    if (item && item["children"].length > 0) {
        items = [];
        for (var i in item["children"]) {
            items.push(item["children"][i]);
        }
    } else if (item) {
        items = [];
        items.push(item);
    }
    //获取当前选中的ITEMHLINKID
    var bomhlinkids = "";
    var parent = undefined;
    var pcode = undefined;
    var itemcodes = "";
    if (items.length > 0) {
        for (var i in items) {
            item = items[i];
            if (!parent) {
                parent = tg.treegrid("getParent", item[treegridOption.idField]);
            }
            itemcodes = $.trim(item["ITEM_CODE"]) + "," + itemcodes;
            bomhlinkids = item["HLINK_ID"] + "," + bomhlinkids;
        }
        bomhlinkids = bomhlinkids.substring(0, bomhlinkids.length - 1);
        itemcodes = itemcodes.substring(0, itemcodes.length - 1);
    }
    //获取父级的CODE，获取到其工序；若父级为合件则继续找到上级，直到父级不是合件
    while (parent["MBOMTYPE"] == "C") {
        parent = tg.treegrid("getParent", parent[treegridOption.idField]);
        if (parent == null) {
            AlertWin(lang.mbom.getItemFailed);
            return false;
        }
    }
    pcode = parent["CODE"];
    pitemcode = $.trim(parent["ITEM_CODE"]);
    $("#dlgDeductionToolbar").html("设置扣料：" + itemcodes);
    $("#dlgDeduction").dialog({
        closed: false,
        title: pitemcode + " " + lang.processFlow.processInfo
    })
    dgDeduction.datagrid("clearSelections");
    dgDeduction.datagrid("loading");
    postData(URL_ITEMPROCESSINFO, { code: pcode }, function (result) {
        if (result.success) {
            if (!result.data || result.data.length == 0) {
                AlertWin(lang.mbom.notHaveProcessInfo);
            }
            dgDeduction.datagrid("loadData", result.data);
            dgDeduction.datagrid("options").bomhlinkids = bomhlinkids;
            dgDeduction.datagrid("loaded");
        }
    });
    $("#dlgDeduction").dialog("center");
}

function deductionSet() {
    var item = dgDeduction.datagrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var bomhids = dgDeduction.datagrid("options").bomhlinkids;
    var pvhid = item["HLINK_ID"];
    postData(URL_ITEMDEDUCTIONSET, { bomhids: bomhids, pvhid: pvhid }, function (result) {
        if (result.success) {
            InfoWin(lang.mbom.itemDeductionSetSuccess);
            reloadTable();
        } else {
            InfoWin(result.msg);
        }
        $("#dlgDeduction").dialog("close");
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
function clearDiscreteChecked() {
    tgvi.treegrid("clearChecked");
}
//清空所有的选中
function clearChecked() {
    clearMainChecked();
    clearDiscreteChecked();
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
    reloadDiscrete();
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
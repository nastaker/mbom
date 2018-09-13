var list = [];
var refreshCooldown = 5;
var canRefresh = true;

//列表
var URL_ITEMTREE = "/MBOM/ItemList";
//物料
var URL_ITEMDETAIL = "/Item/ItemDetailIndex"
var URL_ITEMPAGELIST = "/Item/MaintenancePageList"
var URL_ITEM_LINK = "/MBOM/ItemLink"
var URL_ITEM_UNLINK = "/MBOM/ItemUnlink"
var URL_ITEM_PUBLISH = "/MBOM/ItemPublish"

var tg = $("#treegrid");
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
    IsToErp: "IsToErp",
    IsAssembly: "IsAssembly",
    Type: "Type",
    Status: "Status",
    Order: "Order"
}
//物料树配置信息
var treegridOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    cascadeCheck: false,
    idField: COLS.Id,
    treeField: COLS.ItemCode,
    toolbar: '#toolbar',
    rowStyler: function (row) {
        switch (row[COLS.IsToErp]) {
            case 0:
            case 1:
                return { style: 'font-weight:bold' };
        }
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
                    return "借用";
                }
            }
        },
        {
            field: COLS.IsToErp, title: "发布状态", align: "center", width: "55",
            formatter: function (value, row, index) {
                switch (value) {
                    case 0:
                        return "未发布";
                    case 1:
                        return "发布中";
                    case 2:
                        return "已发布";
                }
            }
        },
        { field: COLS.Order, title: lang.menu.order, width: "50" }
    ]]
};
//物料表格配置信息
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
        { field: "CN_ITEM_CODE", title: lang.item.itemcode, width: 130 },
        { field: "CN_NAME", title: lang.item.name, width: 130 },
        { field: "CN_UNIT", title: lang.item.unit, align: "center", width: 40 },
        { field: "CN_WEIGHT", title: lang.item.weight, align: "center", width: 40 },
        {
            field: "Desc", title: lang.item.type, align: "center", width: 120,
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
            field: "CN_DT_CREATE", title: lang.item.create, align: "center", width: 80,
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
//初始化界面
$(function () {
    tg.treegrid(treegridOption);
    dgItem.datagrid(dgItemOption);
    initDialogs();
    reloadTable();
});
//重新加载表格
function reloadTable() {
    var treegrid = tg;
    var url = URL_ITEMTREE;

    treegrid.treegrid("loading");
    postData(url, params, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        if (!result.success) {
            return false;
        }
        list = [];
        buildTree({
            items: result.data,
            list: list
        });
        treegrid.treegrid("loadData", list);
        treegrid.treegrid("loaded");
    });
}
//初始化弹框
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
    var pitemcode = params.itemcode;
    var title = "将引用至[" + pitemcode + "]下";
    openLinkDialog(title);
}
//确认引用物料
function itemLinkConfirm() {
    var txtQuant = $("#txtItemLinkQuantity");
    if (!txtQuant.textbox("isValid")) {
        return;
    }
    //获取当前产品代号
    //获取父级LINK、父级的链路
    var item = dgItem.datagrid("getSelected");
    if (item == null) {
        AlertWin("请选择要引用的物料");
        return;
    }
    var itemcode_parent = params.itemcode;
    //获取引用的ITEMID
    var itemcode = item["CN_ITEM_CODE"];
    //如果引用件和被引用件相同，不允许
    if (itemcode == itemcode_parent) {
        AlertWin("引用物料与被引用物料相同，将导致循环引用！");
        return;
    }
    //获取引用数量
    var quantity = txtQuant.textbox("getValue");
    //提交
    dgItem.datagrid("clearSelections");
    $("#dlgItem").dialog("close");
    postData(URL_ITEM_LINK, {
        prod_itemcode: "",
        itemcode_parent: itemcode_parent,
        itemcode: itemcode,
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
//取消引用物料
function itemUnlink() {
    var item = tg.treegrid("getSelected");
    if (item[COLS.Level] == 0) {
        AlertWin(lang.mbom.noSelectRoot);
        return false;
    }
    if (item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var itemcode = item[COLS.ItemCode];
    var guid = item[COLS.Guid];
    $.messager.confirm("提示", "您确认要删除物料"+ itemcode +"的引用吗？", function (r) {
        if (r) {
            postData(URL_ITEM_UNLINK, {
                prod_itemcode: "",
                guids: guid
            }, function (result) {
                if (result.success) {
                    reloadTable();
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            })
        }
    });
}
//
function publish() {
    //发布物料
    var itemcode = params.itemcode
    $.messager.confirm("提示", "您确认要发布[" + itemcode + "]吗？", function (r) {
        if (r) {
            postData(URL_ITEM_PUBLISH, {
                itemcode: itemcode
            }, function (result) {
                if (result.success) {
                    reloadTable();
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            })
        }
    });
}
//查询物料
function queryItem() {
    var param = $("#queryFrm").serializeJSON();
    dgItem.datagrid("load", param);
}
//显示物料详情
function showItemDetail() {
    var item = dgItem.datagrid("getSelected");
    if (item == null) { return; }
    var title = "物料详情" + item.CN_ITEM_CODE + " " + item.CN_NAME;
    var prod_itemcode = item.CN_ITEM_CODE
    window.parent.openTab(title, URL_ITEMDETAIL + "?prod_itemcode=" + prod_itemcode);
}
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
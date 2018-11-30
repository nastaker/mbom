var URL_ITEMTREE = "/MBOM/List";
var URL_PROCESS_VER = "/ProductionLine/ProcessVerList";
var URL_PAGELIST_PRODUCT = "/ProductionLine/ProductPageList";
var URL_PAGELIST_PROCESS = "/ProductionLine/ProcessPageList";

var URL_PRODUCT_VER_ADD = "/ProductionLine/AddProductVer";
var URL_MBOM_PROCESS_SET = "/ProductionLine/SetMbomProcess";

var guid_ver;
var list = [];
var tg = $("#treegrid");
var dg = $("#datagrid");
var dgProduct = $("#dgProduct");
var dgProductProcess = $("#dgProductProcess");
var dgProcess = $("#dgProcess");
var winCreateProcessVer = $("#winCreateProcessVer");
var winProcess = $("#winProcess");
var isProcessLoaded = false;

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
    Type: "Type",
    Status: "Status",
    Assembled: "Assembled",
    Feeded: "Feeded",
    IsEmpty: "IsEmpty",
    Order: "Order"
}

var datagridOption = {
    url: URL_PROCESS_VER,
    queryParams: params,
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    title: "产品工序版本列表",
    idField: "CN_GUID",
    toolbar: '#dgToolbar',
    onSelect: function (index, row) {
        if (list.length > 0) {
            return;
        }
        reloadTable()
    },
    columns: [[
        { field: "CN_NAME", title: "版本名称", width: "80", align: "center" },
        {
            field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 80,
            formatter: function (value, row, index) {
                if (value) {
                    return ToJavaScriptDate(value);
                }
            }
        },
        { field: "CN_DESC", title: "版本说明", width: "100" }
    ]],
    loadFilter: loadFilter
}

var dgProductOption = {
    height: "100%",
    striped: true,
    rownumbers: true,
    singleSelect: true,
    border: false,
    idField: "CN_GUID",
    title: "生产线产品列表",
    columns: [[
        { field: "CN_PRODUCT_ITEMCODE", title: "产品物料编码", width: 120 },
        {
            field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 90,
            formatter: function (value, row, index) {
                if (value) {
                    return ToJavaScriptDate(value);
                }
            }
        }
    ]],
    onSelect: function (index, row) {
        dgProductProcess.datagrid("clearSelections");
        dgProductProcess.datagrid("load", { CN_GUID_LINE_PRODUCT: row["CN_GUID"] });
    },
    loadFilter: loadFilter
}

var dgProcessOption = {
    height: "100%",
    striped: true,
    rownumbers: true,
    singleSelect: true,
    border: false,
    idField: "CN_ID",
    title: "生产线产品工序列表",
    columns: [[
        { field: "CN_PROCESS_ORDER", title: "工序编号", width: 90 },
        { field: "CN_PROCESS_NAME", title: "工序名称", width: 120 },
        { field: "CN_PROCESS_INFO", title: "工序详细内容描述", width: 250 }
    ]],
    loadFilter: loadFilter
}

var treegridOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    idField: COLS.Id,
    treeField: COLS.ItemCode,
    title: "产品工序设置详细信息",
    toolbar: '#tgToolbar',
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
            case "VP":
                cls = "main-virtual-parent";
                break;
            case "VPC":
                cls = "main-virtual-parent-child";
                break;
        }
        return { class: cls };
    },
    columns: [[
        { field: COLS.Level, title: lang.mbom.level, width: "35", align: "center" },
        { field: COLS.ItemCode, title: lang.mbom.itemCode, width: "250" },
        { field: COLS.Name, title: lang.mbom.itemName, width: "150" },
        { field: COLS.Quantity, title: lang.mbom.quantity, width: "50", align: "right" },
        { field: COLS.Unit, title: lang.mbom.unit, align: "center", width: "35" },
        {
            field: COLS.IsBorrow, title: "借用", align: "center", width: "35",
            formatter: function (value, row, index) {
                if (value) {
                    return "借用";
                }
            }
        },
        { field: COLS.Assembled, title: "做成工序", width: "90" },
        { field: COLS.Feeded, title: "投料工序", width: "90" },
        {
            field: COLS.IsEmpty, title: "不需投料", width: "90",
            formatter: function (value, row, index) {
                if (value) {
                    return "Y";
                }
                return "";
            }
        },
        { field: COLS.Order, title: lang.menu.order, width: "50" }
    ]]
};

function InitPage() {
    dg.datagrid(datagridOption);
    tg.treegrid(treegridOption);
    dgProduct.datagrid(dgProductOption);
    dgProductProcess.datagrid(dgProcessOption);
    dgProcess.datagrid(dgProcessOption);

    winCreateProcessVer.window({
        width: 800,
        height: 600,
        closed: true,
        collapsible: false,
        minimizable: false,
        modal: true,
        title: "创建新版本",
        footer: "#winFooter"
    })

    winProcess.window({
        width: 550,
        height: 500,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        resizable: false,
        closed: true,
        modal: true,
        title: "设置工序",
        footer: "#winProcessFooter"
    })

    dgProduct.datagrid("options").url = URL_PAGELIST_PRODUCT;
    dgProductProcess.datagrid("options").url = URL_PAGELIST_PROCESS;
    dgProcess.datagrid("options").url = URL_PAGELIST_PROCESS;

    dgProduct.datagrid("load", { CN_PRODUCT_ITEMCODE: params.prod_itemcode });
}

//
function isCreatedVer() {
    var isCreated = true;
    postDataSync(URL_GET_PROCESS_VER, params, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        if (!result.success) {
            isCreated = false;
        } else {
            guid_ver = result.data.CN_GUID;
        }
    });
    return isCreated;
}

//点击创建版本按钮
function addProductVer() {
    winCreateProcessVer.window("open");
    dgProduct.datagrid("clearSelections");
    dgProductProcess.datagrid("loadData", { "success": true, "data": { "rows": [], "total": 0 } });
}
function confirmAddProductVer() {
    var prod = dgProduct.datagrid("getSelected");
    if (!prod) {
        InfoWin("必须选择一个生产线产品才可以创建版本。<br/>如果左侧列表为空，请前往【生产线及工序管理】模块创建生产线产品。");
        return;
    }
    if (!$("#txtName").textbox('isValid')){
        return;
    }
    var name = $("#txtName").textbox("getText");
    var desc = $("#txtDesc").textbox("getText");
    postData(URL_PRODUCT_VER_ADD, {
        CN_GUID_LINE_PRODUCT: prod["CN_GUID"],
        CN_PRODUCT_ITEMCODE: params.prod_itemcode,
        CN_NAME: name,
        CN_DESC: desc
    }, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        dg.datagrid("reload");
    })
    winCreateProcessVer.window("close")
}
//点击设置工序按钮
function setProcess() {
    var prodver = dg.datagrid("getSelected");
    var mbom = tg.treegrid("getSelected");
    if (!prodver) {
        AlertWin("请选择产品工序【版本】。")
        return;
    }
    if (!mbom) {
        AlertWin("请选择要设置工序的【物料】。");
        return;
    }
    if (mbom[COLS.Level] == 0) {
        AlertWin("不能选择产品级做操作。");
        return;
    }
    if (!isProcessLoaded) {
        isProcessLoaded = true;
        dgProcess.datagrid("load", { CN_GUID_LINE_PRODUCT: prodver.CN_GUID_LINE_PRODUCT });
    }
    winProcess.window("open");
}
function setNoProcess() {
    var prodver = dg.datagrid("getSelected");
    var mbom = tg.treegrid("getSelected");
    if (!prodver) {
        AlertWin("请选择产品工序【版本】。")
        return;
    }
    if (!mbom) {
        AlertWin("请选择要设置工序的【物料】。");
        return;
    }
    if (mbom[COLS.Level] == 0) {
        AlertWin("不能选择产品级做操作。");
        return;
    }
    postData(URL_MBOM_PROCESS_SET, {
        guid_ver: prodver["CN_GUID"],
        guid_mbom: mbom[COLS.Guid],
        guid_process: "",
        type: 2
    }, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        if (result.success) {
            reloadTable();
        }
    })
}
//
function confirmSetMbomProcess() {
    var prodver = dg.datagrid("getSelected");
    var mbom = tg.treegrid("getSelected");
    var process = dgProcess.datagrid("getSelected");
    if (!prodver) {
        AlertWin("请选择产品工序【版本】。")
        return;
    }
    if (!mbom) {
        AlertWin("请选择要设置工序的【物料】。");
        return;
    }
    if (!process) {
        AlertWin("请选择要设置的【工序】。");
        return;
    }
    if (mbom[COLS.Level] == 0) {
        AlertWin("不能选择产品级做操作。");
        return;
    }
    var type = $("input[name='rdoProcessType']:checked").val();
    postData(URL_MBOM_PROCESS_SET, {
        guid_ver: prodver["CN_GUID"],
        guid_mbom: mbom[COLS.Guid],
        guid_process: process["CN_GUID"],
        type: type
    }, function (result) {
        if (result.msg && result.msg != "") {
            InfoWin(result.msg);
        }
        if (result.success) {
            reloadTable();
        }
        winProcess.window("close");
    })
}

//重新加载表格
function reloadTable() {
    tg.treegrid("loading");
    postData(URL_ITEMTREE, params, function (result) {
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
        tg.treegrid("loadData", list);
        tg.treegrid("loaded");
    });
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
            switch (item[COLS.Type]) {
                case "V":
                    item.iconCls = "icon-virtual";
                    break;
                case "C":
                    item.iconCls = "icon-combine";
                    break;
                case "U":
                    item.iconCls = "icon-number2";
                    break;
                case "VP":
                case "VPC":
                    item.iconCls = "icon-link";
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

$(function () {
    InitPage();
});
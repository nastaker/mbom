var URL_PAGELIST = "/ProductionLine/PageList";
var URL_PAGELIST_PROD = "/MBOM/ProductBillboardsPageList"
var URL_PAGELIST_SUB = "/ProductionLine/ProductPageList";
var URL_PAGELIST_PROCESS = "/ProductionLine/ProcessPageList";

var URL_PROD_ADD = "/ProductionLine/AddProduct";
var URL_PROD_REMOVE = "/ProductionLine/RemoveProduct";
var URL_PROCESS_ADD = "/ProductionLine/AddProcess";
var URL_PROCESS_REMOVE = "/ProductionLine/RemoveProcess";

var dgLines;
var dgLineProducts;
var dgProd;
var dgProductProcess;
var dlgProcess;
$(function () {
    dgLines = $("#dgLines");
    dgLineProducts = $("#dgLineProducts");
    dgProductProcess = $("#dgProcess");
    dgProd = $("#dgProducts");
    dlgProcess = $("#dlgProcess");

    dgLines.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_GUID",
        title: "生产线列表",
        columns: [[
            { field: 'CN_LINE_NAME', title: "生产线名称", width: 95 },
            { field: 'CN_LINE_CODE', title: "生产线代码", width: 95 }
        ]],
        onSelect: function (index, row) {
            dgLineProducts.datagrid("clearSelections");
            dgLineProducts.datagrid("load", { CN_GUID_LINE: row["CN_GUID"] });
            dgProductProcess.datagrid("clearSelections");
            dgProductProcess.datagrid("loadData", { "success": true, "data": { "rows": [], "total": 0 } });
        },
        loadFilter: loadFilter
    });

    dgLineProducts.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_GUID",
        toolbar: '#dgToolbar',
        title: "生产线产品列表",
        columns: [[
            { field: "CN_PRODUCT_ITEMCODE", title: "产品物料编码", width: 120 },
            {
                field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                } },
            {
                field: "CN_DT_EFFECTIVE", title: "生效日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                } },
            {
                field: "CN_DT_EXPIRY", title: "过期日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                } },
            { field: "CN_CREATE_NAME", title: "创建人", align: "center", width: 90 }
        ]],
        onSelect: function (index, row) {
            dgProductProcess.datagrid("clearSelections");
            dgProductProcess.datagrid("load", { CN_GUID_LINE_PRODUCT: row["CN_GUID"] });
        },
        loadFilter: loadFilter
    });

    dgProductProcess.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#dgProcessToolbar',
        title: "生产线产品工序列表",
        columns: [[
            { field: "CN_PROCESS_ORDER", title: "工序编号", width: 90 },
            { field: "CN_PROCESS_NAME", title: "工序名称", width: 120 },
            { field: "CN_PROCESS_INFO", title: "工序详细内容描述", width: 250 },
            {
                field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: "CN_DT_EFFECTIVE", title: "生效日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            {
                field: "CN_DT_EXPIRY", title: "过期日期", align: "center", width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            { field: "CN_CREATE_NAME", title: "创建人", align: "center", width: 90 }
        ]],
        loadFilter: loadFilter
    });

    dgProd.datagrid({
        url: URL_PAGELIST_PROD,
        height: "100%",
        striped: true,
        rownumbers: true,
        pagination: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#dgProductstoolbar',
        columns: [[
            { checkbox: true, field: "CN_ID" },
            { field: "CN_CODE", title: "代号", width: 120 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 120 },
            { field: "CN_NAME", title: "物料名称", width: 220 },
            { field: "CN_STATUS", title: "状态", align: "center", width: 90 }
        ]],
        loadFilter: function (result) {
            if (result.success) {
                return result.data;
            } else {
                return [];
            }
        }
    });

    dlgProcess.dialog({
        footer: "#dlgProcessFooter",
        width: 380,
        height: 400,
        closed: true,
        modal: true
    });

    dgLineProducts.datagrid("options").url = URL_PAGELIST_SUB;
    dgProductProcess.datagrid("options").url = URL_PAGELIST_PROCESS;
});

function query() {
    var param = $("#queryFrm").serializeJSON();
    dgProd.datagrid("load", param);
}

function productinfo() {
    var prod = dgProd.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var prod_itemcode = prod.CN_ITEM_CODE;
    var title = "产品详情" + prod.CN_ITEM_CODE + " " + prod.CN_NAME;
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?prod_itemcode=" + prod_itemcode);
}

function addProduct() {
    var line = dgLines.datagrid("getSelected");
    if (line == null) {
        AlertWin("请先选择一个生产线")
        return;
    }
    dgProd.datagrid("clearSelections");
    $("#win").dialog({
        title: "选择产品放入[" + line["CN_LINE_NAME"] + "]",
        width: 650,
        height: 450,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: "icon-save",
            handler: function () {
                var prods = dgProd.datagrid("getSelections");
                if (prods == null || prods.length == 0) {
                    AlertWin(lang.mbom.notSelect)
                    return false;
                }
                var itemcodes = [];
                for (var i = 0, len = prods.length; i < len; i++) {
                    var prod = prods[i];
                    itemcodes.push(prod["CN_ITEM_CODE"]);
                }
                postData(URL_PROD_ADD, { guid_line: line["CN_GUID"], itemcodes: itemcodes }, function (result) {
                    dgProd.datagrid("clearSelections");
                    if (result.success) {
                        InfoWin("保存成功")
                        dgLineProducts.datagrid("reload");
                    } else {
                        AlertWin(result.msg);
                    }
                    $("#win").dialog("close");
                })
            }
        }, {
            text: '关闭',
            iconCls: "icon-cancel",
            handler: function () {
                $("#win").dialog("close");
            }
        }]
    });
    $("#win").dialog("center");
}

function removeProduct() {
    var line = dgLines.datagrid("getSelected");
    if (line == null) {
        AlertWin("请先选择一个生产线")
        return;
    }
    var data = dgLineProducts.datagrid("getSelections");
    if (data.length == 0) {
        return;
    }
    var guids = [];
    for (var i = 0, len = data.length; i < len; i++) {
        var prod = data[i];
        guids.push(prod["CN_GUID"]);
    }
    $.messager.confirm("提示", "确定要删除生产线关联的产品吗？", function (r) {
        if (r) {
            postData(URL_PROD_REMOVE, { guids: guids }, function (result) {
                if (result.success) {
                    dgLineProducts.datagrid("clearSelections");
                    dgLineProducts.datagrid("reload");
                } else {
                    AlertWin(result.msg);
                }
            });
        }
    });
}

function addProcess() {
    var prod = dgLineProducts.datagrid("getSelected");
    if (!prod) {
        AlertWin("请先选择一个产品")
        return;
    }
    $("#frmProcess").form("reset");
    $("#frmProcess").form("load", { CN_GUID_LINE_PRODUCT: prod["CN_GUID"] });
    dlgProcess.dialog({
        title: "添加工序",
        closed: false
    });
    dlgProcess.dialog("center");
}
function confirmAddProcess() {
    var data = $("#frmProcess").serializeJSON();
    postData(URL_PROCESS_ADD, data, function (result) {
        if (result.success) {
            dlgProcess.dialog("close");
            dgProductProcess.datagrid("clearSelections");
            dgProductProcess.datagrid("reload");
        } else {
            AlertWin(result.msg);
        }
    });
}
function removeProcess() {
    var process = dgProductProcess.datagrid("getSelected");
    if (!process) {
        AlertWin("请选择要删除的工序")
        return;
    }
    postData(URL_PROCESS_REMOVE, { CN_ID: process["CN_ID"] }, function (result) {
        if (result.success) {
            dgProductProcess.datagrid("clearSelections");
            dgProductProcess.datagrid("reload");
        } else {
            AlertWin(result.msg);
        }
    });
    
}
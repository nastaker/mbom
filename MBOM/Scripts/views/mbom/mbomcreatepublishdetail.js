var URL_PAGELIST = "/MBOM/CreatePublishDetailList";
var URL_ITEMPAGELIST = "/Item/PageList";
var URL_BOMPAGELIST = "/MBOM/BomPageList";
var URL_PARAPAGELIST = "/MBOM/BomHlinkList";
var URL_ADDCHILD = "/MBOM/BomHlinkChildAdd";
var URL_ADDBOMHLINK = "/MBOM/BomHlinkAdd";
var URL_DELCHILD = "/MBOM/DisableBomHlink";
var URL_APPLYCHANGE = "/MBOM/ApplyBomChange";
var URL_PUBLISH = "/MBOM/Publish";
var dg = $("#dg");
var dgSelect = $("#dgSelect");
var dgSelectPara = $("#dgSelectPara");
var dgSelectBom = $("#dgSelectBom");
var cboPublishFilter = $("#cboPublishFilter");
var cboImpactDoFilter = $("#cboImpactDoFilter");
var cboChangeSignFilter = $("#cboChangeSignFilter");
var win = $("#win");
var winPara = $("#winPara");
var winBom = $("#winBom");
var winApply = $("#winApply");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        singleSelect: true, 
        rownumbers: true,
        border: false,
        idField: "hlinkid",
        toolbar: '#toolbar',
        queryParams: {
            itemcode: $("#itemcode").val(),
        },
        rowStyler: function (index, row) {
            if (row.changesign == "Y" || row.changesign == "N") {
                return {
                    style: 'color:red'
                };
            }
        },
        columns: [[
            { field: 'hlinkid', checkbox: true },
            { field: 'pitemcode', title: lang.pbom.baseprod, width: 150 },
            { field: 'itemcode', title: lang.pbom.itemcode, width: 150 },
            { field: 'name', title: lang.pbom.itemname, width: 200 },
            { field: 'quantity', title: lang.pbom.quantity, align:"center", width: 60 },
            { field: 'source', title: lang.cp.source, align: "center", width: 80 },
            {
                field: 'istoerp', title: lang.item.istoerp, align: "center", width: 60,
                formatter: function (value, row, index) {
                    if (value == 0) {
                        return "未发布";
                    } else if(value == 2){
                        return "已发布";
                    }
                }
            },
            {
                field: 'changesign', title: lang.cp.changesign, align: "center", width: 80,
                formatter: function (value, row, index) {
                    if ($.trim(value) == '') {
                        return "未变更";
                    } else if (value == 'Y') {
                        return "新增加";
                    } else if (value == 'N') {
                        return "已取消";
                    }
                }
            },
            {
                field: 'impactdo', title: lang.cp.impactdo, align: "center", width: 80,
                formatter: function (value, row, index) {
                    return value;
                }
            },
            { field: 'bywhat', title: lang.cp.bywhat, width: 300 }
        ]],
        loadFilter: loadFilter
    });

    dgSelect.datagrid({
        url: URL_ITEMPAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        pagination: true,
        singleSelect: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#dgSelectToolbar',
        columns: [[
            { field: 'CN_ID', checkbox: true },
            { field: 'CN_CODE', title: lang.item.code, width: 120 },
            { field: 'CN_ITEM_CODE', title: lang.item.itemcode, width: 120 },
            { field: 'CN_NAME', title: lang.item.name, width: 160 },
            { field: 'CN_DESC', title: lang.item.desc, width: 250 }
        ]],
        loadFilter: loadFilter
    });

    dgSelectPara.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_HLINK_ID",
        columns: [[
            { field: 'CN_HLINK_ID', checkbox: true },
            { field: 'CN_S_CODE', title: lang.item.code, width: 120 },
            { field: 'CN_DISPLAYNAME', title: lang.item.displayname, width: 200 },
            { field: 'CN_DESC', title: lang.item.desc, width: 200 }
        ]],
        loadFilter: loadFilter
    });

    dgSelectBom.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        pagination: true,
        singleSelect: true,
        border: false,
        idField: "id",
        toolbar: '#dgSelectBomToolbar',
        columns: [[
            { field: 'id', checkbox: true },
            { field: 'code', title: lang.item.code, width: 120 },
            { field: 'item_code', title: lang.item.itemcode, width: 120 },
            { field: 'name', title: lang.item.name, width: 160 }
        ]],
        loadFilter: loadFilter,
        onLoadSuccess: function (data) {
            if (data.total > 0) {
                $(this).datagrid("selectRow", 0);
            }
        }
    });

    var commonComboBoxOptions = {
        width: 80,
        editable: false,
        panelHeight:'auto',
        onChange: function (newValue, oldValue) {
            var data = $("#queryFrm").serializeJSON();
            dg.datagrid("load", data);
        }
    };
    cboPublishFilter.combobox(commonComboBoxOptions);
    cboChangeSignFilter.combobox(commonComboBoxOptions);
    cboImpactDoFilter.combobox(commonComboBoxOptions);


    win.window({
        footer: "#winFooter",
        closed: true,
        height: 400,
        width: 800,
        collapsible: false,
        minimizable: false,
        shadow: false,
        modal: true,
        border: false
    });

    winPara.window({
        footer: "#winParaFooter",
        closed: true,
        height: 400,
        width: 600,
        collapsible: false,
        minimizable: false,
        shadow: false,
        modal: true,
        border: false
    })

    winBom.window({
        footer: "#winBomFooter",
        closed: true,
        height: 400,
        width: 600,
        collapsible: false,
        minimizable: false,
        shadow: false,
        modal: true,
        border: false
    })

    winApply.window({
        footer: "#winApplyFooter",
        closed: true,
        height: 200,
        width: 400,
        collapsible: false,
        minimizable: false,
        shadow: false,
        modal: true,
        border: false
    })
});

var timeout = 5;
var refreshable = true;
function reloadTable() {
    if (!refreshable) {
        return;
    }
    refreshable = false;
    setTimeout(function () {
        refreshable = true;
    }, timeout * 1000);
    dg.datagrid("reload");
}

function addItem() {
    $("#querySelectFrm").form("reset");
    $("#bywhat").textbox("reset");
    $("#quantity").textbox("reset");
    dgSelect.datagrid("clearSelections");
    win.window({
        title: "添加新物料",
        closed: false
    });
    win.window("center");
}

function disableItem() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择要操作的物料。");
        return;
    }
    if (item["istoerp"] == 2) {
        AlertWin("物料已发布，无法删除。");
        return;
    }
    if ($.trim(item["type"]) != "MBOM_P") {
        AlertWin("物料非新添加的物料，无法进行删除。");
        return;
    }
    $.messager.confirm('提示', '是否确实要删除此物料？', function (r) {
        if (r) {
            postData(URL_DELCHILD, {
                hlinkid: item["hlinkid"]
            }, function (result) {
                if (result.success) {
                    win.window("close");
                    dg.datagrid("reload");
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            })
        }
    });
}

function applyItem() {
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择要操作的物料。");
        return;
    }
    if (item["istoerp"] == 2) {
        AlertWin("物料已发布，无法应用变更。");
        return;
    }
    if (item["impactdo"] == 'Y') {
        AlertWin("物料已应用变更，无需重复操作。");
        return;
    }
    $.messager.confirm('提示', '是否应用变更？', function (r) {
        if (r) {
            winApply.window({
                title: "输入变更理由",
                closed: false
            });
            winApply.window("center");
        }
    });
}

function winApplyConfirm() {
    if (!$("#applyreason").textbox("isValid")) {
        InfoWin("请填写变更理由");
        $("#bywhat").textbox("isValid")
        return false;
    }
    var item = dg.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择需要应用变更的物料。");
        return;
    }
    postData(URL_APPLYCHANGE, {
        hlinkid: item["hlinkid"],
        bywhat: $("#applyreason").textbox("getText")
    }, function (result) {
        if (result.success) {
            win.window("close");
            dg.datagrid("reload");
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}

function winConfirm() {
    var item = dgSelect.datagrid("getSelected");
    if (!item) {
        AlertWin("请选择需要添加的物料。");
        return;
    }
    if (!$("#quantity").textbox("isValid")) {
        InfoWin("请填写引用数量");
        return false;
    }
    if (!$("#bywhat").textbox("isValid")) {
        InfoWin("请填写变更理由");
        return false;
    }
    var selectedItemid = item["CN_ID"];
    var records = dg.datagrid("getData").rows;
    var ismultiple = false;
    for (var i = 0, len = records.length; i < len; i++) {
        if (records[i]["itemid"] == selectedItemid) {
            ismultiple = true;
            break;
        }       
    }
    var confirm = false;
    if (ismultiple) {
        $.messager.confirm('提示', '选择的物料已经在列表中，是否确认重复添加？', function (r) {
            if (r) {
                openBomWin();
            }
        });
    } else {
        //打开新的窗口
        openBomWin();
    }
}


function openBomWin() {
    $.messager.confirm("提示", "是否选择对应关系？", function (r) {
        if (r) {
            //打开新的窗口
            dgSelectBom.datagrid("clearSelections");
            dgSelectBom.datagrid("options").url = URL_BOMPAGELIST;
            dgSelectBom.datagrid("load", { item_code: $("#itemcode").val() })
            $("#querySelectBomFrm").form("reset")
            winBom.window({
                title: "选择组件",
                closed: false
            });
            winBom.window("center");
            //默认查询
        } else {
            addBomHlink();
        }
    });
}

function addBomHlink() {
    var item = dgSelect.datagrid("getSelected");
    var bywhat = $("#bywhat").textbox("getText");
    var quantity = $("#quantity").textbox("getText");
    if (!item) {
        AlertWin("未获取到选择的物料，请联系管理员。");
        return false;
    }
    postData(URL_ADDBOMHLINK, {
        parentitemcode: $("#itemcode").val(),
        itemid: item["CN_ID"],
        bywhat: bywhat,
        quantity: quantity
    }, function (result) {
        if (result.success) {
            win.window("close");
            dg.datagrid("reload");
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}

function winParaConfirm() {
    //需要添加的物料
    var item = dgSelect.datagrid("getSelected");
    //变更依据
    var bywhat = $("#bywhat").textbox("getText");
    //引用数量
    var quantity = $("#quantity").textbox("getText");
    //选中的关联物料
    var itemp = dgSelectPara.datagrid("getSelected");
    if (!item) {
        //
        AlertWin("未获取到选择的物料，请联系管理员！");
        winPara.window("close");
        return false;
    }
    if (!itemp) {
        AlertWin("请选择关联物料");
        return false;
    }
    postData(URL_ADDCHILD, {
        parentitemcode: $("#itemcode").val(),
        itemid: item["CN_ID"],
        hlinkid: itemp["CN_HLINK_ID"],
        bywhat: bywhat,
        quantity: quantity
    }, function (result) {
        if (result.success) {
            win.window("close");
            winPara.window("close");
            dg.datagrid("reload");
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}

function dgSelectQuery() {
    var data = $("#querySelectFrm").serializeJSON();
    dgSelect.datagrid("load", data);
    dgSelect.datagrid("clearSelections");
}

function dgSelectBomQuery() {
    var data = $("#querySelectBomFrm").serializeJSON();
    dgSelectBom.datagrid("load", data);
    dgSelectBom.datagrid("clearSelections");
}

function winBomConfirm() {
    //确认选择的组件
    var item = dgSelectBom.datagrid("getSelected");
    if (!item) {
        AlertWin("请选中一个组件后再点击确认");
        return false;
    }
    //关闭原窗口
    winBom.window("close");
    //打开对应组件下的BOM_HLINK数据
    winPara.window({
        title: "选择关联物料",
        closed: false,
    });
    winPara.window("center");
    dgSelectPara.datagrid("options").url = URL_PARAPAGELIST;
    dgSelectPara.datagrid("load", { itemcode: item["item_code"] });
}



function mbomPublish() {
    $.messager.confirm("提示", "是否确认发布？", function (r) {
        if (r) {
            postData(URL_PUBLISH, {
                itemcode: $("#itemcode").val()
            }, function (result) {
                if (result.success) {
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            });
        }
    });
}
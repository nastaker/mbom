var URL_PAGELIST = "/ProductionLine/PageList";
var URL_EDIT = "/ProductionLine/Edit";
var URL_MAINTENANCEPAGE = "/MBOM/ComponentMaintenanceIndex";
var dg = $("#dg");
var dlgEdit = $("#dlgEdit");
var vFrm = $("#frmProductionLine");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "PRODUCT_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: 'CN_LINE_NAME', title: "生产线名称", width: 200 },
            { field: 'CN_LINE_CODE', title: "生产线代码", width: 120 },
            { field: 'CN_LINE_INFO', title: "生产线信息", width: 250 },
            {
                field: 'CN_DT_EFFECTIVE', title: "生效日期", align: "center", width: 85,
                formatter: function (value, row, index) {
                    if (value) {
                        row["CN_DT_EFFECTIVE"] = ToJavaScriptDate(value);
                        return row["CN_DT_EFFECTIVE"];
                    }
                }
            },
            {
                field: 'CN_DT_EXPIRY', title: "失效日期", align: "center", width: 85,
                formatter: function (value, row, index) {
                    if (value) {
                        row["CN_DT_EXPIRY"] = ToJavaScriptDate(value);
                        return row["CN_DT_EXPIRY"];
                    }
                }
            },
            {
                field: 'CN_DT_CREATE', title: lang.item.create, align: "center", width: 85,
                formatter: function (value, row, index) {
                    if (value) {
                        row["CN_DT_CREATE"] = ToJavaScriptDate(value);
                        return row["CN_DT_CREATE"];
                    }
                }
            },
            { field: 'CN_CREATE_NAME', title: "创建人", width: 100 }
        ]],
        loadFilter: loadFilter
    });

    vFrm.form({
        url: URL_EDIT,
        onSubmit: function (param) {
            if (!vFrm.form("validate")) {
                return false;
            }
            Loading();
        },
        success: function (data) {
            Loaded();
            dlgEdit.dialog({
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

    dlgEdit.dialog({
        footer: "#dlgEditFooter",
        width: 380,
        height: 400,
        closed: true,
        modal: true
    });
});

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("clearSelections");
    dg.datagrid("load", data);
}

function create() {
    vFrm.form("reset");
    dlgEdit.dialog({
        title: '新建生产线',
        closed: false
    })
}

function update() {
    var data = dg.datagrid("getSelected");
    vFrm.form("load", data);
    dlgEdit.dialog({
        title: '修改生产线',
        closed: false
    })
}

function dialogConfirm() {
    vFrm.submit();
}

function maintenance() {
    var item = dg.datagrid("getSelected");
    if (item === null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var param = "?itemcode=" + item.CN_ITEM_CODE;
    var title = $.trim(item.CN_NAME) + "【" + $.trim(item.CN_ITEM_CODE) + "】";
    window.parent.openTab(title, URL_MAINTENANCEPAGE + param);
}
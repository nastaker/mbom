var URL_PAGELIST = "/MBOM/ItemWithProcessPageList"
var URL_PROCESSVER = "/Item/ItemProcessVer"
var URL_PROCESS = "/Item/ItemProcessByVerId"
var dg = $("#dgProducts");
var winProcess = $("#winProcessInfo");
var dgProcessVer = $("#dgProcessVer");
var dgProcess = $("#dgProcess");

var arrProcess = [];
$(function () {
    dg.datagrid({
        height: "100%", 
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 120 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 120 },
            { field: "CN_NAME", title: "物料名称", width: 220 },
            {
                field: "PDM_RELEASE_DATE", title: "PDM发布日期", align: "center", width: 80,
                formatter: function (value, row, index) {
                    return ToJavaScriptDate(value);
                }
            },
            {
                field: "TOERP_DATE", title: "ERP发布日期", align: "center", width: 80,
                formatter: function (value, row, index) {
                    return ToJavaScriptDate(value);
                }
            }
        ]],
        loadFilter: loadFilter
    });

    winProcess.window({
        collapsible: false,
        minimizable: false, 
        closed: true,
        width: 600,
        height: 400,
        modal: true
    });

    dgProcessVer.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "id",
        columns: [[
            { field: "ver", title: "版本", width: "50%" },
            { field: "sys_status", title: "是否启用", width: "50%" }
        ]],
        loadFilter: loadFilter,
        onLoadSuccess: function (result) {
            if (result && result.rows.length > 0) {
                dgProcessVer.datagrid("selectRow", 0);
            }
        },
        onSelect: function (index, row) {
            var id = row["id"];
            dgProcess.datagrid("load", { id: id });
        }
    });

    dgProcess.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "HLINK_ID",
        columns: [[
            { field: "GX_CODE", title: "工序", width: "15%" },
            { field: "GX_NAME", title: "名称", width: "15%" },
            { field: "GXNR", title: "备注", width: "70%" }
        ]],
        loadFilter: loadFilter
    });



    var opts = dg.datagrid("options");
    var opts2 = dgProcessVer.datagrid("options");
    var opts3 = dgProcess.datagrid("options");
    opts.url = URL_PAGELIST;
    opts2.url = URL_PROCESSVER;
    opts3.url = URL_PROCESS;
});

function query() {
    var param = $("#queryFrm").serializeJSON();
    dg.datagrid("load", param);
}

function openProcessWindow() {
    var data = dg.datagrid("getSelected");
    if (!data) {
        AlertWin(lang.mbom.notSelect);
        return;
    }
    var prod_itemcode = data["CN_ITEM_CODE"];
    if (prod_itemcode) {
        winProcess.window("open");
        winProcess.window("setTitle", prod_itemcode);
        dgProcessVer.datagrid("load", { prod_itemcode: prod_itemcode });
    }
}
var URL_MBOMVERLIST = "/MBOM/ProductMbomVerList"
var URL_MBOMINFO = "/Maintenance/MBOMIndex"

var dg = $("#dgMBomVer");
$(function () {
    dg.datagrid({
        url: URL_MBOMVERLIST,
        queryParams: params,
        title:"版本信息",
        height: "100%",
        fitColumns: true,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_ID",
        columns: [[
            { field: 'CN_CODE', title: lang.productCode, width: 35 },
            { field: 'CN_ITEM_CODE', title: lang.mbom.itemCode, width: 35 },
            { field: 'CN_VER', title: lang.mbomVer, width: 20, align: "center" },
            { field: 'CN_SYS_STATUS', title: lang.status, width: 10, align: "center" }
        ]],
        onLoadSuccess: function (data) {
            dg.datagrid("selectRow", 0);
        },
        onSelect: function(index, row){
            var code = row["CN_CODE"];
            $('#pnl').panel('open').panel('refresh', URL_MBOMINFO + "?code=" + code);
        },
        loadFilter: loadFilter
    });
});
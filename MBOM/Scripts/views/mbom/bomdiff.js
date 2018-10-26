var URL_ITEMDETAIL = "/MBOM/BomDiffDetailIndex"
var URL_MAIN = "/MBOM/BomPageList"
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        height: "100%",
        pageSize: 30,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "CN_ITEM_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: "CN_CODE", title: "代号", width: 220 },
            { field: "CN_ITEM_CODE", title: "物料编码", width: 220 },
            { field: "CN_NAME", title: "物料名称", width: 250 }
        ]],
        loadFilter: loadFilter
    });
    var opts = dg.datagrid("options");
    opts.url = URL_MAIN;
});

function query() {
    var param = $("#queryFrm").serializeJSON();
    dg.datagrid("load", param);
}

function showDetail() {
    var item = dg.datagrid("getSelected");
    if (!item) { return; }
    var title = "【BOM差异查看】【" + item.CN_ITEM_CODE + "】" + item.CN_NAME;
    window.parent.openTab(title, URL_ITEMDETAIL + "?itemcode=" + item.CN_ITEM_CODE);
}
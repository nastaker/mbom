var URL_ITEMDETAIL = "/MBOM/BomDiffDetailIndex"
var URL_MAIN = "/MBOM/BomPageList"
var dg = $("#dgProducts");
$(function () {
    dg.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        pagination: true,
        border: false,
        idField: "id",
        toolbar: '#toolbar',
        columns: [[
            { field: "code", title: "代号", width: 220 },
            { field: "item_code", title: "物料编码", width: 220 },
            { field: "name", title: "物料名称", width: 250 }
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
    var title = "物料详情" + item.item_code + " " + item.name;
    window.parent.openTab(title, URL_ITEMDETAIL + "?bomid=" + item.id);
}
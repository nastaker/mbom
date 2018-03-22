var URL_PAGELIST = "/MBOM/PBOMChangeItemPageList";
var URL_ITEMCHANGEINDEX = "/MBOM/ItemChangeDetailIndex";
var dg = $("#dg");
$(function () {
    dg.datagrid({
        url: URL_PAGELIST,
        height: "100%",
        striped: true,
        singleSelect: true,
        rownumbers: true,
        pagination: true,
        border: false,
        idField: "CN_ITEM_CODE",
        toolbar: '#toolbar',
        columns: [[
            { field: 'CN_ITEM_CODE', title: lang.pbom.itemcode, width: 150 },
            { field: 'CN_NAME', title: lang.pbom.itemname, width: 150 },
            { field: 'CN_PRODUCT_BASE', title: lang.pbom.baseprod, width: 150 },
            { field: 'CN_DESC', title: lang.pbom.desc, width: 300 }
        ]],
        loadFilter: loadFilter
    }); 
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

function query() {
    var data = $("#queryFrm").serializeJSON();
    dg.datagrid("load", data);
}

function openOptionalItemTab() {
    //设置选中的选装件关系
    var item = dg.datagrid("getSelected");
    if (item == null) {
        AlertWin(lang.mbom.notSelect);
        return false;
    }
    var itemcode = item["CN_ITEM_CODE"];
    var title = itemcode + " PBOM物料变更明细";
    window.parent.openTab(title, URL_ITEMCHANGEINDEX + "?itemcode=" + itemcode);
} 
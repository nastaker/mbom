var URL_LIST = "/MBOM/ItemChangeDetailList";
var dg = $("#dg");
$(function () {
    dg.datagrid({
        url: URL_LIST,
        height: "100%",
        striped: true,
        singleSelect: true,
        rownumbers: true,
        border: false,
        queryParams: {
            itemcode: $("#itemcode").val()
        },
        columns: [[
            { field: 'itemcode', title: lang.pbom.prodcode, width: 150 },
            { field: 'displayname', title: lang.pbom.displayname, width: 200 },
            { field: 'quantity', title: lang.pbom.quantity, width: 60 },
            { field: 'ver', title: lang.pbom.ver, width: 80 },
            {
                field: 'dtver', title: lang.pbom.dtver, width: 90,
                formatter: function (value, row, index) {
                    if (value) {
                        return ToJavaScriptDate(value);
                    }
                }
            },
            { field: 'status', title: lang.pbom.status, width: 80 },
            { field: 'isimplement', title: lang.pbom.isimplement, width: 100 },
            { field: 'notice', title: lang.pbom.notice, width: 100 }
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
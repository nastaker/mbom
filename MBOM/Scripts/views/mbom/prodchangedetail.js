var URL_LIST = "/MBOM/ProductChangeDetailList";
var dg = $("#dg");
$(function () {
    dg.datagrid({
        url: URL_LIST,
        height: "100%",
        striped: true,
        singleSelect: true,
        rownumbers: true,
        border: false,
        idField: "itemcode",
        toolbar: '#toolbar',
        queryParams: {
            prodcode: $("#prodcode").val()
        },
        columns: [[
            { field: 'itemcode', title: lang.pbom.prodcode, width: 120 },
            { field: 'name', title: lang.pbom.name, width: 150 },
            { field: 'effect', title: lang.pbom.effect, width: 80 },
            { field: 'displayname', title: lang.pbom.displayname, width: 150 },
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
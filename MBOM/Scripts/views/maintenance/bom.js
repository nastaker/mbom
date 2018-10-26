var COLS = {
    Level: "Level",
    ParentId: "ParentId",
    Id: "Id",
    ItemId: "ItemId",
    Name: "Name",
    Code: "Code",
    ItemCode: "ItemCode",
    Guid: "Guid",
    Quantity: "Quantity",
    Unit: "Unit",
    IsBorrow: "IsBorrow",
    IsAssembly: "IsAssembly",
    Type: "Type",
    Status: "Status",
    Order: "Order"
}

var dgOption = {
    url: URL_PRODUCT_BOMVER_LIST,
    toolbar: "#dgBomVerToolbar",
    queryParams: param,
    height: "100%",
    rownumbers: true,
    singleSelect: true,
    border: false,
    idField: 'DateTimeCreate',
    loadFilter: loadFilter,
    columns: [[
        { field: 'Ver', title: bomVer, width: 80 },
        { field: 'DateTimeCreate', title: bomDtVer, width: 80 },
    ]],
    onSelect: function (index, row) {
        $("#txtEfDate").textbox('setText', row["DateTimeCreate"]);
        viewByDate();
    },
    onLoadSuccess: function (data) {
        rows = data.rows;
    }
}

var pbomColumns = [[
    { field: 'ItemCodeParent', title: lang.item.itemcode_parent, width: 120 },
    { field: 'ItemCode', title: lang.item.itemcode, width: 200 },
    { field: 'Name', title: lang.item.name, width: 100 },
    { field: 'Quantity', title: lang.item.quantity, width: 50 },
    { field: 'Unit', title: lang.item.unit, width: 40, align: "center" },
    {
        field: 'DtPbomEf', title: lang.item.dtpbomef, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value > '2000-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    {
        field: 'DtPbomEx', title: lang.item.dtpbomex, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value < '2100-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    { field: 'PbomVerCode', title: lang.item.pbomvercode, width: 120 },
    { field: 'PbomVer', title: lang.item.pbomver, width: 70 },
    { field: 'Desc', title: lang.pbom.desc, width: 200 }
]]

var mbomColumns = [[
    { field: 'ItemCodeParent', title: lang.item.itemcode_parent, width: 120 },
    { field: 'ItemCode', title: lang.item.itemcode, width: 200 },
    { field: 'Name', title: lang.item.name, width: 100 },
    { field: 'Quantity', title: lang.item.quantity, width: 50 },
    { field: 'Unit', title: lang.item.unit, width: 40, align: "center" },
    {
        field: 'DtPbomEf', title: lang.item.dtpbomef, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value > '2000-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    {
        field: 'DtPbomEx', title: lang.item.dtpbomex, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value < '2100-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    {
        field: 'DtMbomEf', title: lang.item.dtmbomef, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value > '2000-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    {
        field: 'DtMbomEx', title: lang.item.dtmbomex, width: 90, align: "center",
        styler: function (value, row, index) {
            if (value < '2100-01-01') {
                return 'color:red;font-weight:bold;';
            }
        }
    },
    { field: 'Desc', title: lang.pbom.desc, width: 200 }
]]

var tgOption = {
    height: "100%",
    border: false,
    rownumbers: true,
    lines: true,
    singleSelect: true,
    idField: 'Id',
    treeField: COLS.ItemCode,
    columns: (ISPBOM ? pbomColumns : mbomColumns)
}

var tree = $("#treeItems");
var dgBomVer = $("#dgBomVer");
var tgItems = $("#tgItems");
var cachedData = [];
var list = [];
var rows;
$(function () {
    dgBomVer.datagrid(dgOption);
    tgItems.treegrid(tgOption);
});

function viewByDate(forceRefresh) {
    if (!$("#txtEfDate").textbox('isValid')) {
        return;
    }
    var date = $("#txtEfDate").textbox('getText');
    if (cachedData[date] && !forceRefresh) {
        Loading();
        tgItems.treegrid("loadData", cachedData[date]);
        Loaded(100);
        return;
    }
    postData(URL_ITEMTREE, {
        prod_itemcode: param.prod_itemcode,
        date: date
    }, function (result) {
        if (result.success) {
            var data = result.data;
            if (data.length == 0) {
                data.push({ Id: "P", ItemCode: "如有疑问请联系管理员", Name: "无下级物料" });
            } else {
                list = [];
                buildTree({
                    items: data,
                    list: list
                });
                cachedData[date] = list;
                var isSearched = false;
                for (var i = 0, j = rows.length; i < j; i++) {
                    if (rows[i].DateTimeCreate == date) {
                        isSearched = true;
                        break;
                    }
                }
                if (!isSearched) {
                    rows.push({ Ver: "用户搜索", Desc: "截至日期前的数据", DateTimeCreate: date });
                    dgBomVer.datagrid("loadData", { success: true, data: rows });
                    tgItems.treegrid("loadData", list);
                }
            }
            dgBomVer.datagrid("selectRecord", date);
        }
    });
}


function buildTree(options) {
    var settings = {
        items: [],
        list: [],
        pval: null,
        pid: COLS.ParentId,
        id: COLS.Id,
        children: "children"
    };

    $.extend(settings, options);

    var items = settings.items;
    var list = settings.list;
    var id = settings.id;
    var pid = settings.pid;
    var pval = settings.pval;
    var children = settings.children;

    var count = items.length;

    if (count == 0) {
        return false;
    }
    for (var i = 0; i < count; i++) {
        var item = items[i];
        if (item[pid] == pval || item[COLS.Level] == 0) {
            switch (item[COLS.Type]) {
                case "V":
                    item.iconCls = "icon-virtual";
                    break;
                case "C":
                    item.iconCls = "icon-combine";
                    break;
                case "U":
                    item.iconCls = "icon-number2";
                    break;
                case "VP":
                case "VPC":
                    item.iconCls = "icon-link";
                    break;
            }
            list.push(item);
            remove(items, i);
            i--;
            count--;
        }
    }

    for (var i = 0, len = list.length; i < len; i++) {
        var item = list[i];
        item[children] = item[children] ? item[children] : [];
        buildTree($.extend(settings, { items: items, list: item[children], pval: item[id] }));
    }
}
$(function () {
    var dgItems = $("#dgItems");
    var columns = [[
        { field: 'CODE', title: lang.saleSet.productCode },
        { field: 'NAME', title: lang.saleSet.productName },
        { field: '部件', title: lang.bomlist.child, align: "center", width: 15 },
        { field: '自制件', title: lang.bomlist.selfmade, align: "center", width: 15 },
        { field: '标准件', title: lang.bomlist.standard, align: "center", width: 15 },
        { field: '采购件', title: lang.bomlist.purchase, align: "center", width: 15 },
        { field: '销售件', title: lang.bomlist.sell, align: "center", width: 15 },
        { field: '工艺件', title: lang.bomlist.process, align: "center", width: 15 },
        { field: '包装件', title: lang.bomlist.pack, align: "center", width: 15 },
        { field: '原材料', title: lang.bomlist.origin, align: "center", width: 15 },
        { field: '借用', title: lang.bomlist.borrow, align: "center", width: 15 }
    ]];
    //本页面基础datagrid属性
    var commonOptions = {
        height: "100%",
        rownumbers: true,
        border: false,
        fitColumns: true,
        idField: 'ITEMID'
    }
    //私有datagrid属性
    var itemsOptions = {
        url: URL_ITEMLIST,
        title: lang.maintenance.bomList,
        columns: columns,
        queryParams: $.extend({}, param, {}),
        loadFilter: function (result) {
            if (!result.success) {
                AlertWin(result.msg != null ? result.msg : lang.bomlist.dataGetFailed)
                return [];
            }
            return result.data;
        }
    };
    //继承基础属性
    //设置datagrid
    dgItems.datagrid($.extend({}, commonOptions, itemsOptions));
})

$(function () {
    var dgItems = $("#dgItems");
    var columns = [[
        { field: 'CODE', title: lang.selfmade.code },
        { field: 'ITEM_CODE', title: lang.selfmade.itemcode },
        { field: 'NAME', title: lang.selfmade.name },
        { field: 'PDATE', title: lang.selfmade.date },
        { field: 'ISBORROWSTR', title: lang.selfmade.borrow, align:"center" }
        //{ field: '', title: lang.selfmade.remarks }
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
        url: URL_ITEMCATELIST,
        title: lang.maintenance.purchaseList,
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

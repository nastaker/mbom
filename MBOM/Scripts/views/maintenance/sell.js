var URL_SELL_LIST = "/Item/SellListByVer"
var URL_PRODUCT_VER = "/Item/ProductVerList";
var dgItem = $("#dgItem");
var dgVer = $("#dgVer");
var verColumns = [[
    { field: 'CN_NAME', title: "版本名称", width: 60 },
    {
        field: 'CN_DT_CREATE', title: "版本日期", width: 80,
        formatter: function (value, row, index) {
            if (value) {
                return ToJavaScriptDate(value);
            }
            return "";
        }
    }
]];
var columns = [[
    { field: 'CODE', title: lang.selfmade.code },
    { field: 'ITEM_CODE', title: lang.selfmade.itemcode },
    { field: 'NAME', title: lang.selfmade.name },
    { field: 'PDATE', title: lang.selfmade.date },
    { field: 'ISBORROWSTR', title: lang.selfmade.borrow, align: "center" },
    { field: 'CUS_CODE', title: lang.saleSet.customerCode },
    { field: 'CUS_NAME', title: lang.saleSet.customerName },
    { field: 'CUS_ITEMCODE', title: lang.saleSet.customerItemCode },
    { field: 'CUS_ITEMNAME', title: lang.saleSet.customerItemName },
    { field: 'CUS_SHIPPINGADDR', title: lang.saleSet.shippingAddr },
    //{ field: '', title: lang.selfmade.remarks }
]];
//本页面基础datagrid属性
var commonOptions = {
    height: "100%",
    singleSelect: true,
    rownumbers: true,
    border: false
}
var verOptions = {
    url: URL_PRODUCT_VER,
    title: "版本列表",
    idField: 'CN_GUID',
    queryParams: $.extend({}, param, {}),
    columns: verColumns,
    onSelect: function (index, row) {
        dgItem.datagrid("load", {
            prod_itemcode: param.prod_itemcode,
            guid: row.CN_GUID
        })
    },
    loadFilter: loadFilter
}
//私有datagrid属性
var itemsOptions = {
    title: lang.maintenance.sellList,
    idField: 'ITEMID',
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
$(function () {
    //继承基础属性
    //设置datagrid
    dgItem.datagrid($.extend({}, commonOptions, itemsOptions));
    dgVer.datagrid($.extend({}, commonOptions, verOptions));
    var opt = dgItem.datagrid("options")
    opt.url = URL_SELL_LIST;
})

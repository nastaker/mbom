var URL_PROCESSINFO = "/Item/ProductProcessList";
var URL_ITEMPROCESSINFO = "/Item/ProductProcessInfo";

var dgItems = $("#dgItems");
var dgItemProcess = $("#dgItemProcess");
$(function () {
    var columns = [[
        { field: 'CODE', title: lang.saleSet.productCode },
        { field: 'NAME', title: lang.saleSet.productName }
    ]];
    var dgItemProcessColumns = [[
        { field: 'GX_CODE', title: lang.processFlow.gxCode },
        { field: 'GX_NAME', title: lang.processFlow.gxName },
    ]];
    //本页面基础datagrid属性
    var commonOptions = {
        height: "100%",
        singleSelect: true,
        rownumbers: true,
        border: false,
        fitColumns: true,
        idField: 'ITEMID'
    }
    //私有datagrid属性
    var itemsOptions = {
        url: URL_PROCESSINFO,
        title: lang.processFlow.itemList,
        columns: columns,
        onSelect: onSelect,
        queryParams: param,
        loadFilter: loadFilter,
        onLoadSuccess: onLoadSuccess
    };
    var saleItemsOptions = {
        title: lang.processFlow.processInfo,
        columns: dgItemProcessColumns,
        loadFilter: loadFilter,
        onLoadSuccess: onProcessInfoLoadSuccess
    }
    //继承基础属性
    //设置datagrid
    dgItems.datagrid($.extend({}, commonOptions, itemsOptions));
    dgItemProcess.datagrid($.extend({}, commonOptions, saleItemsOptions));
    dgItemProcess.datagrid("options").url = URL_ITEMPROCESSINFO;
});

var selectIndex = undefined;

function onSelect(index, row) {
    if (selectIndex == index) { return false; }
    selectIndex = index;
    var dgItemProcess = $("#dgItemProcess");
    dgItemProcess.datagrid("loading");
    if (row.processFlow && typeof (row.processFlow) == "object") {
        dgItemProcess.datagrid("loadData", row.processFlow);
        dgItemProcess.datagrid("loaded");
    } else {
        dgItemProcess.datagrid("reload", { code: row.CODE });
    }
}

function onLoadSuccess(data) {
    //加载成功后选中第一行
    if (data.total == 0) { return false; }
    var _this = $(this);
    _this.datagrid("selectRow", 0);
}

function onProcessInfoLoadSuccess(data) {
    if (data.total == 0) { return false; }
    dgItems.datagrid("getSelected").processFlow = {
        "success": true, "msg": null, "data": data.rows
    };
}
$(function () {
    var dgItems = $("#dgItems");
    var dgItemProcess = $("#dgItemProcess");
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
        title: lang.processFlow.itemList,
        columns: columns,
        onSelect: onSelect,
        onLoadSuccess: onLoadSuccess
    };
    var saleItemsOptions = {
        title: lang.processFlow.processInfo,
        columns: dgItemProcessColumns
    }
    //继承基础属性
    //设置datagrid
    dgItems.datagrid($.extend({}, commonOptions, itemsOptions));
    dgItemProcess.datagrid($.extend({}, commonOptions, saleItemsOptions));
    //
    //获取SALE_SET
    postData(URL_PROCESSINFO, param, function (result) {
        if (!result.success) {
            AlertWin(result.msg != null ? result.msg : lang.processFlow.loadFailed)
        }
        dgItems.datagrid({
            data: result.data
        });
    });
});

var selectIndex = undefined;

function onSelect(index, row) {
    if (selectIndex == index) { return false; }
    selectIndex = index;
    var dgItemProcess = $("#dgItemProcess");
    dgItemProcess.datagrid("loading");
    if (row.processFlow && typeof (row.processFlow) == "object" & row.processFlow.length > 0) {
        setTimeout(function () {
            dgItemProcess.datagrid({
                data: row.processFlow
            });
        }, 100);
    } else {
        postData(URL_ITEMPROCESSINFO, { code: row.CODE }, function (result) {
            if (result.success) {
                row.processFlow = result.data;
                dgItemProcess.datagrid({
                    data: row.processFlow
                });
            }
        });
    }
}

function onLoadSuccess(data) {
    //加载成功后选中第一行
    if (data.length == 0) { return false; }
    var _this = $(this);
    _this.datagrid("selectRow", 0);
}

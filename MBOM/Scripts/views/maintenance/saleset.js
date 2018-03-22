$(function () {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    var columns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width:50 },
        { field: 'NAME', title: lang.saleSet.productName, width:50 }
    ]];
    var dgSaleItemsColumns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width: 40 },
        { field: 'NAME', title: lang.saleSet.productName, width: 40 },
        {
            field: 'F_QUANTITY', title: lang.saleSet.saleWeight, width: 12,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'numberbox',
                options: { precision: 4, required: true }
            }
        },
        { field: 'UNIT', title: lang.saleSet.productUnit, width: 8, align:"center" }
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
        title: lang.maintenance.itemList,
        toolbar: '#dgItemsToolbar',
        columns: columns
    };
    var saleItemsOptions = {
        title: lang.maintenance.sellList,
        toolbar: '#dgSaleItemsToolbar',
        columns: dgSaleItemsColumns,
        onClickCell: onClickCell
    }
    //继承基础属性
    //设置datagrid
    dgItems.datagrid($.extend({}, commonOptions, itemsOptions));
    dgSaleItems.datagrid($.extend({}, commonOptions, saleItemsOptions));
    //
    //获取SALE_SET
    postData(URL_SALESETLIST, param, function (result) {
        //saleset 
        // 为 0 时，表示未设置为销售件
        // 为 1 时，表示已设置为销售件
        var i = result.data.length - 1;
        var tmpData = [];
        while (i >= 0) {
            var data = result.data[i];
            if (data.SALESET == 1) {
                tmpData.push(data);
                remove(result.data, i);
            }
            i--;
        }
        dgItems.datagrid({
            data: result.data
        });
        dgSaleItems.datagrid({
            data: tmpData
        });
    });
})


var editIndex = undefined;
var removeList = [];
var editList = [];

function validateSaleSet(dg) {
    endEditing(dg);
    var items = dg.datagrid('getChanges');
    var isValid = true;
    for(var i in items){
        var item = items[i];
        if(!item.F_QUANTITY){
            isValid = false;
            break;
        }
    }
    return isValid;
}

function endEditing(dg) {
    if (editIndex == undefined) { return true }
    if (dg.datagrid('validateRow', editIndex)) {
        dg.datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}

function onClickCell(index, field) {
    var editCellField = 'F_QUANTITY';
    var dg = $(this);
    if (editIndex != index) {
        if (endEditing(dg)) {
            dg.datagrid('selectRow', index)
              .datagrid('beginEdit', index);
            var ed = dg.datagrid('getEditor', { index: index, field: editCellField });
            if (ed) {
                ($(ed.target).data('textbox') ? $(ed.target).textbox('textbox') : $(ed.target)).select();
            }
            editIndex = index;
        } else {
            setTimeout(function () {
                dg.datagrid('selectRow', editIndex);
            }, 0);
        }
    }
}

function btnDgItemsCancelSelections() {
    var dgItems = $("#dgItems");
    dgItems.datagrid("clearSelections");
}

function moveItemsFromDatagrid2Datagrid(dgFrom, dgTo, callback) {
    var items = dgFrom.datagrid("getSelections");
    var i = items.length - 1;
    while (i >= 0) {
        var item = items[i];
        if (typeof (callback) == 'function') {
            callback(item);
        }
        dgTo.datagrid('appendRow', item);
        var rowIndex = dgFrom.datagrid('getRowIndex', item.ITEMID);
        dgFrom.datagrid('deleteRow', rowIndex);
        i--;
    }
}

function btnDgItemsSetSaleItems() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    moveItemsFromDatagrid2Datagrid(dgItems, dgSaleItems, function (item) {
        if (item.ITEM_HLINK_ID) {
            removeByValue(removeList, item.ITEM_HLINK_ID);
        }
    });
}

function btnDgSaleItemsCancelSelections() {
    var dgSaleItems = $("#dgSaleItems");
    dgSaleItems.datagrid("clearSelections");
}

function btnDgSaleItemsSetItems() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    moveItemsFromDatagrid2Datagrid(dgSaleItems, dgItems, function (item) {
        editIndex = undefined;
        if (item.ITEM_HLINK_ID) {
            removeList.push(item.ITEM_HLINK_ID);
        }
    });
}

function btnDgSaleItemsEndEditing() {
    var dgSaleItems = $("#dgSaleItems");
    endEditing(dgSaleItems);
}

function btnRejectChanges() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    dgSaleItems.datagrid('rejectChanges');
    dgItems.datagrid('rejectChanges');
    editList = [];
    removeList = [];
}

function btnSaveChanges() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    if (!validateSaleSet(dgSaleItems)) {
        AlertWin(lang.saleSet.notValid);
        return false;
    }
    //可能在变更列表中会存在被删除的，此时不加入到editList中
    var items = dgSaleItems.datagrid('getChanges');
    if (items.length == 0) {
        AlertWin(lang.saleSet.noChanges);
        return false;
    }
    for (var i = items.length - 1; i >= 0; i--) {
        var item = items[i];
        //被变更的有itemhlid且不存在于removeList中才被添加到editList，并且从addList中删除
        if (item.ITEM_HLINK_ID) {
            if (!exists(removeList, item.ITEM_HLINK_ID)) {
                editList.push(item);
            }
            remove(items, i);
        }
    }

    postData(URL_SAVESALESETLIST, $.extend({}, param, {
        addList: items,
        editList: editList,
        removeList: removeList
    }), function (result) {
        editList = [];
        removeList = [];
        if (result.success) {
            dgItems.datagrid('acceptChanges');
            dgSaleItems.datagrid('acceptChanges');
            InfoWin(result.msg != null ? result.msg : lang.saleSet.saleSetSuccess);
        } else {
            InfoWin(result.msg != null ? result.msg : lang.saleSet.saleSetFailed);
        }
    });
}
var URL_SALESETLIST = "/Item/SaleSetList";
var URL_SAVESALESETLIST = "/Item/SaveSaleSetList";
var URL_SHIPPINGADDRLIST = "/Item/GetShippingAddr";

var code = $("#code").val();
var dgItems = $("#dgItems");
var dgSaleItems = $("#dgSaleItems");
var shippingAddrData;
var param = { code: code };
$(function () {

    postDataSync(URL_SHIPPINGADDRLIST, {}, function (result) {
        if (result.success) {
            shippingAddrData = result.data;
        }
        if (result.msg) {
            AlertWin(result.msg);
        }
    });

    var columns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width:50 },
        { field: 'NAME', title: lang.saleSet.productName, width:50 }
    ]];
    var dgSaleItemsColumns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width: 150 },
        { field: 'NAME', title: lang.saleSet.productName, width: 150 },
        {
            field: 'F_QUANTITY', title: lang.saleSet.saleWeight, width: 80,
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
        {
            field: 'SHIPPINGADDR', title: lang.saleSet.shippingAddr, width: 150,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'combobox',
                options: {
                    editable: false,
                    valueField: 'CN_NAME',
                    textField: 'CN_NAME',
                    data: shippingAddrData,
                    required: true
                }
            }
        },
        { field: 'UNIT', title: lang.saleSet.productUnit, width: 50, align:"center" }
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
    moveItemsFromDatagrid2Datagrid(dgItems, dgSaleItems);
}

function btnDgSaleItemsCancelSelections() {
    var dgSaleItems = $("#dgSaleItems");
    dgSaleItems.datagrid("clearSelections");
}

function btnDgSaleItemsSetItems() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    moveItemsFromDatagrid2Datagrid(dgSaleItems, dgItems);
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
}

function btnSaveChanges() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    if (!validateSaleSet(dgSaleItems)) {
        AlertWin(lang.saleSet.notValid);
        return false;
    }
    var items = [];
    var inserted = dgSaleItems.datagrid('getChanges', 'inserted');
    var updated = dgSaleItems.datagrid('getChanges', 'updated');
    var deleted = dgSaleItems.datagrid('getChanges', 'deleted');
    pushInItems(items, inserted, "C");
    pushInItems(items, updated, "U");
    pushInItems(items, deleted, "D");
    if (items.length == 0) {
        AlertWin(lang.saleSet.noChanges);
        return false;
    }
    postData(URL_SAVESALESETLIST, $.extend({}, param, {
        list: items
    }), function (result) {
        if (result.success) {
            dgItems.datagrid('acceptChanges');
            dgSaleItems.datagrid('acceptChanges');
            InfoWin(result.msg != null ? result.msg : lang.saleSet.saleSetSuccess);
        } else {
            InfoWin(result.msg != null ? result.msg : lang.saleSet.saleSetFailed);
        }
    });
}

function pushInItems(list, inlist, type) {
    for (var i in inlist) {
        var item = inlist[i];
        list.push({
            itemid: item["ITEMID"],
            f_quantity: item["F_QUANTITY"],
            shippingaddr: item["SHIPPINGADDR"],
            type: type
        });
    }
}
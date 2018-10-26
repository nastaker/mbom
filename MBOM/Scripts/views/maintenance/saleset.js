var URL_SELLLIST = "/Item/SellList";
var URL_SAVESALESETLIST = "/Item/SaveSaleSetList";
var URL_SHIPPINGADDRLIST = "/Item/GetShippingAddr";
var URL_CUSTOMERCODENAME = "/Item/GetCustomerCodeName";

var prod_itemcode = $("#prod_itemcode").val();
var dgItems = $("#dgItems");
var dgSaleItems = $("#dgSaleItems");
var shippingAddrData;
var customerCodeNames;
var param = { prod_itemcode: prod_itemcode };
$(function () {
    var isError = false;
    postDataSync(URL_SHIPPINGADDRLIST, {}, function (result) {
        if (result.success) {
            shippingAddrData = result.data;
        }
        if (result.msg) {
            AlertWin(result.msg);
            isError = true;
        }
    });

    postDataSync(URL_CUSTOMERCODENAME, {}, function (result) {
        if (result.success) {
            customerCodeNames = result.data;
        }
        if (result.msg) {
            AlertWin(result.msg);
            isError = true;
        }
    });

    if (isError) {
        return;
    }

    var columns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width:"50%" },
        { field: 'NAME', title: lang.saleSet.productName, width:"50%" }
    ]];
    var dgSaleItemsColumns = [[
        { field: 'CODE', title: lang.saleSet.productCode, width: 120 },
        { field: 'NAME', title: lang.saleSet.productName, width: 170 },
        {
            field: 'CUSTOMER_ID', title: lang.saleSet.customerName, width: 200,
            formatter: function (value, row, index) {
                return row["CUSTOMERNAME"];
            },
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'combobox',
                options: {
                    data: customerCodeNames,
                    valueField: 'CN_ID',
                    textField: 'CN_NAME',
                    panelWidth: 200,
                    formatter: function (data) {
                        return '<span style="font-weight:bold">' + data["CN_NAME"] + '</span><br/>' +
                            '<span style="color:#888">' + data["CN_CODE"] + '</span>';
                    }
                }
            }
        },
        {
            field: 'CUSTOMERITEMCODE', title: lang.saleSet.customerItemCode, width: 120,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'textbox',
                options: { validType: 'maxlength[100]', tipPosition: "top" }
            }
        },
        {
            field: 'CUSTOMERITEMNAME', title: lang.saleSet.customerItemName, width: 170,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'textbox',
                options: { validType: 'maxlength[100]', tipPosition: "top" }
            }
        },
        {
            field: 'SHIPPINGADDR', title: lang.saleSet.shippingAddr, width: 70,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'combobox',
                options: {
                    valueField: 'CN_NAME',
                    textField: 'CN_NAME',
                    data: shippingAddrData
                }
            }
        },
        {
            field: 'F_QUANTITY', title: lang.saleSet.saleWeight, width: 70,
            styler: function (value, row, index) {
                if (!value) {
                    return 'background-color:#ffee00;color:red;';
                }
            },
            editor: {
                type: 'numberbox',
                options: { precision: 4 }
            }
        },
        { field: 'UNIT', title: lang.saleSet.productUnit, width: 40, align:"center" }
    ]];
    //本页面基础datagrid属性
    var commonOptions = {
        height: "100%",
        rownumbers: true,
        border: false,
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
        onClickCell: onClickCell,
        onEndEdit: onEndEdit
    }
    //继承基础属性
    //设置datagrid
    dgItems.datagrid($.extend({}, commonOptions, itemsOptions));
    dgSaleItems.datagrid($.extend({}, commonOptions, saleItemsOptions));
    //
    //获取SALE_SET
    postData(URL_SELLLIST, param, function (result) {
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
    for (var i = 0, len = items.length; i < len; i++){
        var item = items[i];
        if(!item.F_QUANTITY){
            isValid = false;
            break;
        }
        if (!item.CUSTOMERITEMNAME || $.trim(item.CUSTOMERITEMNAME).length == 0) {
            isValid = false;
            break;
        }
        if (!item.CUSTOMERITEMCODE || $.trim(item.CUSTOMERITEMCODE).length == 0) {
            isValid = false;
            break;
        }
        if (!item.SHIPPINGADDR || $.trim(item.SHIPPINGADDR).length == 0) {
            isValid = false;
            break;
        }
        if (!item.CUSTOMER_ID || item.CUSTOMER_ID == 0) {
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

function onEndEdit(index, row, changes) {
    if (changes["CUSTOMER_ID"]) {
        var item = customerCodeNames.get(changes["CUSTOMER_ID"], "CN_ID");
        if (item) {
            row["CUSTOMERNAME"] = item.CN_NAME;
        } else {
            row["CUSTOMER_ID"] = null;
            row["CUSTOMERNAME"] = null;
        }
    }
    if (changes["SHIPPINGADDR"]) {
        var sad = shippingAddrData.get(changes["SHIPPINGADDR"], "CN_NAME");
        if (!sad) {
            row["SHIPPINGADDR"] = null;
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
    return endEditing(dgSaleItems);
}

function btnRejectChanges() {
    var dgItems = $("#dgItems");
    var dgSaleItems = $("#dgSaleItems");
    dgSaleItems.datagrid('rejectChanges');
    dgItems.datagrid('rejectChanges');
}

function btnSaveChanges() {
    if (!btnDgSaleItemsEndEditing()) {
        AlertWin("请填写完成所有必须的信息");
        return;
    }
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
    for (var i = 0, len = inlist.length; i < len; i++) {
        var item = inlist[i];
        list.push({
            itemid: item["ITEMID"],
            f_quantity: item["F_QUANTITY"],
            customer_id: item["CUSTOMER_ID"],
            shippingaddr: item["SHIPPINGADDR"],
            customeritemcode: item["CUSTOMERITEMCODE"],
            customeritemname: item["CUSTOMERITEMNAME"],
            type: type
        });
    }
}
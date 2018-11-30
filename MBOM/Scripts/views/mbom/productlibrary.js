var zTreeSetting = {
    view: {
        addHoverDom: addHoverDom,
        removeHoverDom: removeHoverDom,
        selectedMulti: false
    },
    edit: {
        enable: true,
        editNameSelectAll: true,
        showRemoveBtn: showRemoveBtn,
        showRenameBtn: true,
        renameTitle: "重命名节点",
        removeTitle: "删除该节点"
    },
    callback: {
        onClick: zTreeOnClick,
        beforeDrag: beforeDrag,
        beforeEditName: beforeEditName,
        beforeRemove: beforeRemove,
        beforeRename: beforeRename,
        onRemove: onRemove,
        onRename: onRename
    },
    data: {
        simpleData: {
            enable: true,
            idKey: "id",
            parentidKey: "parentid",
            rootparentid: null
        },
        key: {
            name: "name",
            children: "children",
            url: "xUrl"
        }
    }
};
var processing = false;

var URL_LINKLIST = "/UserProductLibrary/LinkList"
var URL_PAGELIST = "/MBOM/ProductBillboardsPageList"
var URL_PRODUCTDETAIL = "/MBOM/ProductDetailIndex";
var URL_ITEMTREE = "/UserProductLibrary/List";
var URL_ADDNODE = "/UserProductLibrary/Add"
var URL_RENAMENODE = "/UserProductLibrary/Rename"
var URL_DELNODE = "/UserProductLibrary/Delete"
var URLPROD_ADD = "/UserProductLibrary/LinkAdd"
var URL_PRODUCTDEL = "/UserProductLibrary/LinkDelete"

var dgProd = $("#dgProducts");
var dg = $("#dgList");
var dgProdConfig = {
    url: URL_PAGELIST,
    height: "100%",
    striped: true,
    rownumbers: true,
    pagination: true,
    border: false,
    idField: "CN_ID",
    toolbar: '#dgProductstoolbar',
    columns: [[
        { checkbox: true, field: "CN_ID" },
        { field: "CN_CODE", title: "代号", width: 120 },
        { field: "CN_ITEM_CODE", title: "物料编码", width: 120 },
        { field: "CN_NAME", title: "物料名称", width: 220 },
        { field: "CN_STATUS", title: "状态", align: "center", width: 90 }
    ]],
    loadFilter: function (result) {
        if (result.success) {
            return result.data;
        } else {
            return [];
        }
    }
};
var dgConfig = {
    height: "100%",
    striped: true,
    rownumbers: true,
    pagination: true,
    border: false,
    toolbar: '#toolbar',
    idField: "ID",
    columns: [[
        { checkbox: true, field: "n" },
        { field: "ItemCode", title: "物料编码", width: 200 },
        { field: "ProductCode", title: "产品编号", width: 200 },
        { field: "ProductName", title: "产品名称", width: 300 },
        {
            title: "操作", field: "ID", width: 60,
            formatter: function (value, row, index) {
                return "<a href='javascript:;' onclick='showProductInfo(\"" + row["ItemCode"] + "\",\"" + row["ProductName"] + "\")'>查看详情</a>"
            }
        }
    ]],
    loadFilter: function (result) {
        if (result.success) {
            return result.data;
        } else {
            return [];
        }
    }
};
$(function () {
    var tree = $("#tree");

    postData(URL_ITEMTREE, {}, function (result) {
        if (result.success) {
            var data = result.data;
            $.fn.zTree.init(tree, zTreeSetting, data);
            var treeObj = $.fn.zTree.getZTreeObj("tree");
            var nodes = treeObj.getNodes();
            if (nodes.length > 0) {
                currSelectNode = nodes[0];
                dg.datagrid("load", { id: nodes[0]['id'] });
                treeObj.selectNode(nodes[0]);
            }
            treeObj.expandAll(true);
        }
    });
    dgProd.datagrid(dgProdConfig);
    dg.datagrid(dgConfig);
    dg.datagrid("options").url = URL_LINKLIST;
});
function showRemoveBtn(treeId, treeNode) {
    return treeNode.parentTId !== null;
}
function beforeDrag(treeId, treeNodes) {
    return false;
}
function beforeEditName(treeId, treeNode) {
}
function onRemove(e, treeId, treeNode) {
}
function beforeRemove(treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("tree");
    zTree.selectNode(treeNode);
    if (!confirm("是否确认删除节点[" + treeNode.name + "]！")) {
        return false;
    }
    processing = true;
    var success = false;
    postDataSync(URL_DELNODE, { id: treeNode.id }, function (result) {
        if (result.success) {
            success = true;
        } else {
            AlertWin(result.msg);
        }
        processing = false;
    });
    return success;
}
function onRename(e, treeId, treeNode, isCancel) {
}
function beforeRename(treeId, treeNode, newName, isCancel) {
    if (newName.length == 0 || $.trim(newName).length == 0) {
        setTimeout(function () {
            var zTree = $.fn.zTree.getZTreeObj("tree");
            zTree.cancelEditName();
        }, 0);
        return false;
    }
    var success = true;
    if (!isCancel && $.trim(newName) !== $.trim(treeNode.name)) {
        processing = true;
        postDataSync(URL_RENAMENODE, {
            id: treeNode.id,
            parentid: treeNode.parentid,
            name: newName
        }, function (result) {
            processing = false;
            if (result.success) {
            } else {
                success = false;
                var zTree = $.fn.zTree.getZTreeObj("tree");
                zTree.cancelEditName();
                AlertWin(result.msg);
            }
        });
    }
    return success;
}
function addHoverDom(treeId, treeNode) {
    if (processing) { return false; }
    var sObj = $("#" + treeNode.tId + "_span");
    if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
    var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
        + "' title='添加子节点' onfocus='this.blur();'></span>";
    sObj.after(addStr);
    var btn = $("#addBtn_" + treeNode.tId);
    if (btn) btn.bind("click", function () {
        var zTree = $.fn.zTree.getZTreeObj("tree");
        processing = true;
        postData(URL_ADDNODE, { parentid: treeNode.id, name: "新节点" }, function (result) {
            if (result.success) {
                zTree.addNodes(treeNode, result.data);
            } else {
                AlertWin(result.msg);
            }
            processing = false;
        })
        return false;
    });
};
function removeHoverDom(treeId, treeNode) {
    $("#addBtn_" + treeNode.tId+",#addlibBtn_" + treeNode.tId).unbind().remove();
};

var currSelectNode = undefined;

function zTreeOnClick(event, treeId, treeNode) {
    if (currSelectNode != treeNode) {
        currSelectNode = treeNode;
    } else {
        return;
    }
    dg.datagrid("load", { id: treeNode.id });
};

function query() {
    var param = $("#queryFrm").serializeJSON();
    dgProd.datagrid("load", param);
}

function productinfo() {
    var prod = dgProd.datagrid("getSelected");
    if (prod == null) {
        AlertWin(lang.mbom.notSelect)
        return false;
    }
    var prod_itemcode = prod.CN_ITEM_CODE;
    var title = "产品详情" + prod.CN_ITEM_CODE + " " + prod.CN_NAME;
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?prod_itemcode=" + prod_itemcode);
}

function showProductInfo(itemcode, name) {
    var title = "产品详情" + itemcode + " " + name;
    window.parent.openTab(title, URL_PRODUCTDETAIL + "?prod_itemcode=" + itemcode);
}

function addProduct() {
    var treeNode = currSelectNode;
    if (treeNode == null) {
        AlertWin("请先选择一个节点")
        return;
    }
    dgProd.datagrid("clearSelections");
    $("#win").dialog({
        title: "选择产品放入[" + treeNode.name + "]",
        width: 650,
        height: 450,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: "icon-save",
            handler: function () {
                var prods = dgProd.datagrid("getSelections");
                if (prods == null || prods.length == 0) {
                    AlertWin(lang.mbom.notSelect)
                    return false;
                }
                var ids = "";
                for (var i = 0, len = prods.length; i < len; i++) {
                    var prod = prods[i];
                    ids = prod["CN_ID"] + "," + ids;
                }
                ids = ids.substring(0, ids.length - 1);
                postData(URLPROD_ADD, { libid: treeNode.id, ids: ids }, function (result) {
                    dgProd.datagrid("clearSelections");
                    if (result.success) {
                        InfoWin("保存成功")
                        dg.datagrid("load", { id: treeNode.id });
                    } else {
                        AlertWin(result.msg);
                    }
                    $("#win").dialog("close");
                })
            }
        }, {
            text: '关闭',
            iconCls: "icon-cancel",
            handler: function () {
                $("#win").dialog("close");
            }
        }]
    });
    $("#win").dialog("center");
}

function deleteProduct() {
    var data = dg.datagrid("getSelections");
    if (data.length == 0) {
        return;
    }
    var ids = [];
    for (var i = 0, len = data.length; i < len; i++) {
        var prod = data[i];
        ids.push(prod["ID"]);
    }
    postData(URL_PRODUCTDEL, { ids: ids }, function (result) {
        if (result.success) {
            dg.datagrid("clearSelections");
            dg.datagrid("load");
        } else {
            AlertWin(result.msg);
        }
    });
}
var obj = {
    id: "Id",
    pid: "ParentId",
    code: "Code",
    itemcode: "ItemCode",
    name: "Name",
    quantity: "Quantity"
}

var zTreeSetting = {
    view: {
        expandSpeed: 0,
        showIcon: false,
        txtSelectedEnable: true,
        addDiyDom: addDiyDom,
        fontCss: getFont,
        dblClickExpand: function (treeId, treeNode) {
            return treeNode.level > 0;
        }
    },
    data: {
        simpleData:{
            enable: true,
            idKey: obj.id,
            pIdKey: obj.pid,
            rootPId: null
        },
        key: {
            name: getName,
            children: "children",
            url: "xUrl"
        }
    }
};

$(function () {
    var tree = $("#treeItems");

    $.post(URL_ITEMTREE, param, function (result) {
        if (result.success) {
            var data = result.data;
            if (data.length == 0) {
                data.push({ Code: "", ItemCode: "", Name: "无下级物料", Quantity: "如有疑问请联系管理员" });
            }
            $.fn.zTree.init(tree, zTreeSetting, data);
            var treeObj = $.fn.zTree.getZTreeObj("treeItems");
            treeObj.expandAll(true);
        }
    });
});

function getFont(treeId, node) {
    if (node["Type"] == "C") {
        return { color: "blue" };
    }
    else if (node["Type"] == "V") {
        return { color: "red" };
    }
    else {
        return {};
    }
}

var showCode = false;
function addDiyDom(treeId, treeNode) {
    if (treeNode["Level"] === 0) {
        var aObj = $("#" + treeNode.tId + "_a");
        var id = treeNode[obj.id];
        var name = treeNode[obj.name];
        if ($("#diyBtn_" + id).length > 0) return;
        var editStr = "<span class='button' style='display:inline-block;width:18px;height:18px;vertical-align:middle;' id='diyBtn_" + id + "' title='切换数据显示' onfocus='this.blur();'></span>";
        aObj.after(editStr);
        var btn = $("#diyBtn_" + id);
        if (btn) {
            btn.css({
                margin: 0,
                background: "url(../Content/ztree/zTreeStyle/img/diy/9.png) no-repeat scroll 0 0 transparent",
                verticalAlign: "middle"
            });
            btn.bind("click", swithDisplay);
        }
    }
}

function getName(node) {
    if (showCode) {
        return node[obj.code] + " "+ node[obj.name] + " (" + node[obj.quantity] + ")";
    } else {
        return node[obj.itemcode] + " " + node[obj.name] + " (" + node[obj.quantity] + ")";
    }
}

function swithDisplay() {
    showCode = !showCode;
    var treeObj = $.fn.zTree.getZTreeObj("treeItems");
    treeObj.refresh();
}



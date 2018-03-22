var zTreeSetting = {
    data: {
        simpleData:{
            enable: true,
            idKey: "ID",
            pIdKey: "PARENTID",
            rootPId: null
        },
        key: {
            name: getName,
            children: "children",
            url: "xUrl"
        }
    },
    view: {
        addDiyDom: addDiyDom
    }
};

$(function () {
    var tree = $("#treeItems");

    postData(URL_ITEMTREE, param, function (result) {
        if (result.success) {
            var data = result.data;
            if (data.length == 0) {
                data.push({ PARENTID: null, CODE: "", ITEM_CODE: "", NAME: "本产品不含有PBOM", QUANTITY: "如有疑问请联系管理员" });
            }
            $.fn.zTree.init(tree, zTreeSetting, data);
            var treeObj = $.fn.zTree.getZTreeObj("treeItems");
            treeObj.expandAll(true);
        }
    });
});

var showCode = true;
function addDiyDom(treeId, treeNode) {
    if (treeNode["LEVEL"] === 0) {
        var aObj = $("#" + treeNode.tId + "_a");
        var id = treeNode["ID"];
        var name = treeNode["NAME"];
        if ($("#diyBtn_" + id).length > 0) return;
        var editStr = "<span id='diyBtn_space_" + id + "' >&nbsp;</span><span class='button' id='diyBtn_" + id + "' title='切换数据显示' onfocus='this.blur();'></span>";
        aObj.append(editStr);
        var btn = $("#diyBtn_" + id);
        if (btn) {
            btn.css({
                margin: 0,
                background: "url(../Content/ztree/zTreeStyle/img/diy/9.png) no-repeat scroll 0 0 transparent",
                verticalAlign: "top"
            });
            btn.bind("click", swithDisplay);
        }
    }
}

function getName(node) {
    if (showCode) {
        return node["CODE"] + node["NAME"] + "(" + node["QUANTITY"] + ")";
    } else {
        return node["ITEM_CODE"] + node["NAME"] + "(" + node["QUANTITY"] + ")";
    }
}

function swithDisplay() {
    showCode = !showCode;
    var treeObj = $.fn.zTree.getZTreeObj("treeItems");
    treeObj.refresh();
}



var zTreeSetting = {
    data: {
        simpleData:{
            enable: true,
            idKey: "ID",
            pIdKey: "PARENTID",
            rootPId: null
        },
        key: {
            name: function (node) {
                return node["ITEM_CODE"] + node["NAME"] + "(" + node["QUANTITY"] + ")";
            },
            children: "children",
            url: "xUrl"
        }
    }
};

$(function () {
    var tree = $("#treeItems");

    postData(URL_ITEMTREE, params, function (result) {
        if (result.success) {
            var data = result.data;
            if (data.length == 0) {
                data.push({ PARENTID: null, ITEM_CODE: "", NAME: "本产品不含有MBOM", QUANTITY: "如有疑问请联系管理员" });
            }
            $.fn.zTree.init(tree, zTreeSetting, data);
            var treeObj = $.fn.zTree.getZTreeObj("treeItems");
            treeObj.expandAll(true);
        }
    });
});
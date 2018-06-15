var zTreeSetting = {
    async: {
        enable: true,
        autoParam: ["ITEMID"],
        url: "/Item/ItemParentList",
        dataFilter: function (treeId, parentNode, childNodes) {
            console.log(childNodes);
            if (!childNodes.success) {
                return { code: "", name: childNodes.msg };
            }
            if (!childNodes.data || childNodes.data.length == 0) {
                parentNode.isParent = false;
                return null;
            }
            for (var i = 0, len = childNodes.data.length; i < len; i++) {
                var item = childNodes.data[i];
                item.isParent = true;
            }
            return childNodes.data;
        }
    },
    data: {
        key: {
            name: function(node){
                return node["CODE"] + node["NAME"];
            }
        }
    }
};

$(function () {
    var tree = $("#treeItems");
    if (data.length == 0) {
        data.push({ code: "", name: "本产品无父级使用（如有疑问请联系管理员）" });
    }
    $.fn.zTree.init(tree, zTreeSetting, data);
});
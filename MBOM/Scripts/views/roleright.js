var zTreeSetting = {
    data: {
        key: {
            name: "Name",
            children: "Children",
            url: "xUrl"
        }
    },
    check: {
        enable: true
    },
    callback: {
        onClick: function (event, treeId, treeNode, clickFlag) {
            //treeNode代表节点数据，可参照ztree中的api
            var ztreeobj = this.getZTreeObj(treeId)
            var checkState = treeNode.checked;
            ztreeobj.checkNode(treeNode, null, true);
        },
        beforeCollapse: function () {
            return false;
        }

    }
};

function CheckHaveRights(nodes, rights)
{
    for (var i in nodes) {
        var node = nodes[i];
        node.checked = false;
        for (var j in rights) {
            var right = rights[j];
            if (node.ID == right.RightId) {
                node.checked = true;
                break;
            }
        }
        if (node.Children && node.Children.length > 0) {
            CheckHaveRights(node.Children, rights);
        }
    }
}

$(function () {
    var currRowIndex = -1;
    var dlRoles = $("#dlRoles");
    var tree = $("ul#treeMenus");
    var ztreeobj = null;
    dlRoles.datalist({
        height: "100%",
        valueField: 'ID',
        textField: 'RoleName',
        border: false,
        onLoadSuccess: function (data) {
            if (data.rows && data.rows.length > 0) {
                dlRoles.datalist("selectRow", 0);
            }
        },
        onSelect: function (index, row) {
            if (currRowIndex == index) { return false;}
            currRowIndex = index;
            //获取当前角色的权限
            var rights = row.RoleRights;
            var zNodes = tree.data("data-treedata");
            CheckHaveRights(zNodes, rights);
            //删除之前的zTree
            if (ztreeobj) {
                ztreeobj.destroy();
            }
            //初始化zTree
            ztreeobj = $.fn.zTree.init(tree, zTreeSetting, zNodes);
            ztreeobj.expandAll(true);
            //
        },
        loadFilter: loadFilter
    });

    $("#btnSave").click(function () {
        var role = dlRoles.datalist("getSelected");
        var roleId = role.ID;
        var menuIds = [];
        var nodes = ztreeobj.getCheckedNodes();
        for (var i in nodes) {
            var node = nodes[i];
            menuIds.push(node.ID);
        }
        postData("/RoleRight/Edit", { roleId: roleId, menuIds: menuIds }, function (data) {
            if (data.success) {
                for (var i in menuIds) {
                    role.RoleRights.push({ RightId: menuIds[i] });
                }
                InfoWin(data.msg);
            } else {
                AlertWin(data.msg);
            }
        })
    });

    $("#btnReset").click(function () {
        var role = dlRoles.datalist("getSelected");
        //获取当前角色的权限
        var rights = role.RoleRights;
        var zNodes = tree.data("data-treedata");
        CheckHaveRights(zNodes, rights);
        //删除之前的zTree
        if (ztreeobj) {
            ztreeobj.destroy();
        }
        //初始化zTree
        ztreeobj = $.fn.zTree.init(tree, zTreeSetting, zNodes);
        ztreeobj.expandAll(true);
    });

    postData("/Menu/List", {}, function (result) {
        if (!result.success) {
            InfoWin(result.msg);
        } else {
            tree.data("data-treedata", result.data);
            dlRoles.datalist({
                url: "/RoleRight/List"
            });
        }
    });
});
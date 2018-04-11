var URL_LIST = "/Group/List";
var URL_USERS = "/Group/Users";
var URL_USERLIST = "/UserRole/OrganizationTree";
var URL_GROUPUSERADD = "/Group/UserAdd";
var URL_GROUPUSERDEL = "/Group/UserDel";

var dgGroup = $("#dgGroup");
var dgGroupUser = $("#dgGroupUser");
var treeUser = $("#treeUser");
var dlgUser = $("#dlgUser");
var loading = false;
$(function () {
    dgGroupUser.datagrid({
        height: "100%",
        striped: true,
        rownumbers: true,
        border: false,
        idField: "CN_ID",
        columns: [[
            { field: "CN_ID", checkbox: true },
            { field: "CN_USERNAME", title: "用户名", width: 100  },
            {
                field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value && $.trim(value) != "") {
                        return ToJavaScriptDate(value);
                    }
                    return value;
                }
            },
            { field: "CN_CREATE_NAME", title: "创建人", width: 100 },
            { field: "CN_CREATE_LOGIN", title: "创建人登录名", width: 100 }
        ]],
        loadFilter: loadFilter
    })

    dgGroup.datagrid({
        url: URL_LIST,
        height: "100%",
        striped: true,
        rownumbers: true,
        singleSelect: true,
        border: false,
        idField: "CN_ID",
        toolbar: '#dgGroupToolbar',
        onSelect: function (index, row) {
            if (!row["users"]) {
                postData(URL_USERS, { groupid: row["CN_ID"] }, function (result) {
                    if (result.success) {
                        dgGroupUser.datagrid("loadData", result);
                        row["users"] = result;
                    }
                });
            } else {
                dgGroupUser.datagrid("loadData", row["users"]);
            }
        },
        columns: [[
            { field: "CN_NAME", title: "域", width: 120 },
            {
                field: "CN_SYS_STATUS", title: "是否启用", align: "center", width: 70,
                formatter: function (value, row, index) {
                    return value;
                }
            },
            {
                field: "CN_DT_CREATE", title: "创建日期", align: "center", width: 80,
                formatter: function (value, row, index) {
                    if (value && $.trim(value) != "") {
                        return ToJavaScriptDate(value);
                    }
                    return value;
                }
            },
            { field: "CN_DESC", title: "备注", width: 200 },
            { field: "CN_CREATE_NAME", title: "创建人", width: 100 },
            { field: "CN_CREATE_LOGIN", title: "创建人登录名", width: 100 }
        ]],
        loadFilter: loadFilter,
        onLoadSuccess: function (result) {
            if (result && result.rows && result.rows.length > 0) {
                dgGroup.datagrid("selectRow", 0);
            }
        }
    });

    treeUser.tree({
        url: URL_USERLIST,
        lines: true,
        checkbox: true,
        formatter: function (node) {
            node.text = node.name;
            return node.text;
        },
        onBeforeSelect: function (node) {
            if (loading) {
                return false;
            }
        },
        onClick: function (node) {
            if (node.type == "部门") {
                return false;
            }
            if (loading) {
                return false;
            }
        },
        loadFilter: loadFilter
    });




    dlgUser.dialog({
        title: "添加域用户",
        closed: true,
        minimizable: false,
        collapsible: false,
        modal: true,
        width: 450,
        height: 350,
        toolbar: "#dlgUserToolbar",
        footer: "#dlgUserFooter"
    })
});

function openAddUserDialog() {
    //如果不选择域，不能打开添加用户的窗口
    var group = dgGroup.datagrid("getSelected");
    if (!group) {
        AlertWin("请选中一个域。");
        return false;
    }
    //清空所有选择和过滤
    clearTree();
    dlgUser.dialog({
        closed: false
    });
    dlgUser.dialog("center");
}

function searchByUsername() {
    var username = $("#txtUsername").textbox("getText");
    treeUser.tree("doFilter", username);
}

function dlgUserConfirm() {
    //确认选择的用户，如果没有选中用户，不允许确认
    var users = treeUser.tree('getChecked');	
    if (!users || users.length == 0) {
        AlertWin("请选择要添加的用户");
        return false;
    }
    //确认选中的域，如果没有选中，不允许确认
    var group = dgGroup.datagrid("getSelected");
    if (!group) {
        AlertWin("请选中一个域。");
        return false;
    }
    //获取所有用户
    var users2add = [];
    for (var i = 0; i < users.length; i++) {
        var user = users[i];
        if (user.type == "人员") {
            users2add.push({ id: user.id, name: user.name });
        }
    }
    if (users2add.length == 0) {
        AlertWin("您选择的是部门，请选择有效用户。");
        return false;
    }
    //提交数据
    postData(URL_GROUPUSERADD, {
        groupid: group["CN_ID"],
        users: users2add
    }, function (result) {
        dlgUser.dialog("close");
        if (result.success) {
            dgGroup.datagrid("reload");
        }
        if (result.msg) {
            InfoWin(result.msg);
        }
    })
}

function clearTree() {
    treeUser.tree("doFilter", "");
    treeUser.tree("expandAll");
    var users = treeUser.tree('getChecked', ['checked', 'indeterminate']);
    for (var i = 0; i < users.length; i++) {
        var user = users[i];
        if (user.type == "部门") {
            treeUser.tree("uncheck", user.target);
        }
    }
    $("#txtUsername").textbox("clear");
}

function delUserFromGroup() {
    var users = dgGroupUser.datagrid("getSelections");
    if (!users || users.length == 0) {
        AlertWin("请选中需要删除的用户。");
        return false;
    }
    $.messager.confirm("提示", "是否确认从域中删除选中的用户？", function (r) {
        if (r) {
            var groupuserids = [];
            for (var i = 0; i < users.length; i++) {
                var user = users[i];
                groupuserids.push(user["CN_ID"]);
            }
            postData(URL_GROUPUSERDEL, {
                groupuserids: groupuserids
            }, function (result) {
                if (result.success) {
                    refreshGroupUser();
                }
                if (result.msg) {
                    InfoWin(result.msg);
                }
            });
        }
    });
}

function refreshGroupUser() {
    var group = dgGroup.datagrid("getSelected");
    if (!group) {
        AlertWin("请选中一个域。");
        return false;
    }
    dgGroupUser.datagrid("clearSelections");
    postData(URL_USERS, { groupid: group["CN_ID"] }, function (result) {
        if (result.success) {
            dgGroupUser.datagrid("loadData", result);
            group["users"] = result;
        }
    });
}

function refreshGroup() {
    dgGroupUser.datagrid("loadData", { success: true, data: [] });
    dgGroup.datagrid("clearSelections");
    dgGroup.datagrid("reload");
}
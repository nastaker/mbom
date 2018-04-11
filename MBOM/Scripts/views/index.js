var zTreeSetting = {
    data: {
        key: {
            name: "name",
            children: "children",
            url: "xUrl"
        }
    },
    callback: {
        onClick: function (event, treeId, treeNode, clickFlag) {  //treeNode代表节点数据，可参照ztree中的api  
            var title = treeNode.name;
            var url = treeNode.url;
            if (treeNode.click == false) {   //click为false不可以进行点击  
                return;
            }
            openTab(title, url);
        }
    }
};

var URL_MENULIST = "/Menu/UserMenuList";
var URL_LOGIN = "/User/Login";

$(function () {
    if (window.parent != window && window.parent.openTab != null) {
        window.parent.location.href = "";
        return;
    }
    postData(URL_MENULIST, {}, function (result) {
        if (!result.success) {
            AlertWin(result.msg, function () {
                window.location.href = URL_LOGIN;
            });
            return;
        }
        var data = result.data;
        for(var i = 0; i < data.length; i++)
        {
            var menu = data[i];
            var ul = $("<ul>").attr({ id: "tree" + menu.id }).addClass("myMenu");

            for (var j = 0; menu.children && j < menu.children.length; j++) {
                var item = menu.children[j];
                var li = $("<li>").addClass("menuitem").appendTo(ul);
                var a = $("<a href=\"javascript:;\">").appendTo(li);
                var iconcls = item.iconcls != undefined ? item.iconcls : "list";
                var icon = $("<i>").addClass("myIcon fa fa-" + iconcls);
                a.append(icon).append(item.name).data("m", item);
                a.click(function (e) {
                    var _this = $(this);
                    var _m = _this.data("m");
                    var title = _m.name;
                    var url = _m.url;

                    openTab(title, url);
                    $("li.menuitem").removeClass("active");
                    _this.parent().addClass("active");
                });
            }

            $("#accordion").accordion('add', {
                title: menu.name,
                content: ul,
                selected: (i==0)
            });
        }
    });

    var tab = $("#Mytab");
    var menu = $("#TabMenu");
    menu.menu({
        onClick: function (item) {
            var _this = $(this);
            var targetTab = _this.data("targetTab");
            var index = targetTab.index;
            var selectedTab = tab.tabs("getTab", index);
            switch (item.name) {
                case "refresh":
                    var src = $(selectedTab).find("iframe").attr("src");
                    $(selectedTab).find("iframe").attr("src", src);
                    break;
                case "close":
                    if (!canClose(selectedTab)) { return false;}
                    tab.tabs('close', index);
                    break;
                case "closeOthers":
                    var tabs = tab.tabs('tabs');
                    for (var i = tabs.length - 1; i >= 0; i--) {
                        if (!canClose(tabs[i])) { continue; }
                        var curridx = tab.tabs('getTabIndex', tabs[i]);
                        if (index == curridx) {
                            continue;
                        }
                        tab.tabs('close', curridx);
                    }
                    break;
                case "closeAll":
                    var tabs = tab.tabs('tabs');
                    for (var i = tabs.length - 1; i >= 0; i--) {
                        if (!canClose(tabs[i])) { continue; }
                        tab.tabs('close', i);
                    }
                    break;
                case "closeLeft":
                    var tabs = tab.tabs('tabs');
                    var count = tabs.length;
                    for (var i = index - 1; i >= 0; i--) {
                        if (!canClose(tabs[i])) { continue; }
                        tab.tabs('close', i);
                    }
                    break;
                case "closeRight":
                    var tabs = tab.tabs('tabs');
                    var count = tabs.length;
                    for (var i = count - 1; i > index; i--) {
                        if (!canClose(tabs[i])) { continue; }
                        tab.tabs('close', i);
                    }
                    break;
            }
        }
    });

    tab.tabs({
        onContextMenu: function (e, title, index) {
            e.stopPropagation();
            e.preventDefault();
            tab.tabs('select', title);
            menu.data("targetTab", { title: title, index: index });
            //显示快捷菜单
            if (index > 0) {
                menu.menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        }
    })
});
var wins = [];

function canClose(panel) {
    return $(panel).panel('options').closable;
}

function openTab(title, url) {
    if (!url || url == "") {
        url = "about:blank";
    }
    if ($('#Mytab').tabs('exists', title)) {
        $('#Mytab').tabs('select', title);
    } else {
        $('#Mytab').tabs('add', {
            title: title,
            content: '<div style="width:100%;height:100%;overflow:hidden;">'
                + '<iframe src="'
                + url
                + '" scrolling="auto" style="width:100%;height:100%;border:0;" ></iframe></div>', // 可以局部刷新tab选项卡  
            closable: true //提供关闭选项卡的按钮                                    
        });
    }
}

function openWin(title, url) {
    var win = wins[title] || $("<div>");
    if(!wins[title]){
        wins[title] = win;
        $(win).window({
            title: title,
            content: '<div style="width:100%;height:100%;overflow:hidden;">'
                + '<iframe src="'
                + url
                + '" scrolling="auto" style="width:100%;height:100%;border:0;" ></iframe></div>', // 可以局部刷新tab选项卡  
            width: "80%",
            height: "80%",
            modal: true,
            shadow: false,
            collapsible: false,
            draggable: false,
            resizable: false
        });
    } else {
        $(win).window("open");
    }
}


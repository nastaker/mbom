String.prototype.concatUrlParam = function (param) {
    return this.indexOf('?') > 0 ? this.concat("&", $.param(param)) : this.concat("?", $.param(param));
}

$(function () {
    var ul = $("#tree");
    var tab = $("#Mytab");
    var tabMenu = $("#TabMenu");

    for (var i = 0, len = menuData.length; i < len; i++) {
        var menu = menuData[i];
        var li = $("<li>").appendTo(ul);
        var icon = $("<i>").addClass("myIcon fa fa-" + menu.iconCls);
        li.addClass("list-" + i);
        li.append(icon).append(menu.text).data("m", $.extend(menu, { id: i }));
        li.click(function (e) {
            var _this = $(this);
            var _m = _this.data("m");
            var title = _m.text;
            var url = _m.url.concatUrlParam(params);

            openTab(title, url, {
                id: _m.id
            });
        });
        if (i == 0) {
            li.addClass("active");
            var url = menu.url.concatUrlParam(params);
            openTab(menu.text, null, {
                id: i,
                href: url,
                closable: false,
                tools: [{
                    iconCls: 'icon-mini-refresh',
                    handler: function () {
                        var selectedTab = tab.tabs("getTab", 0);
                        selectedTab.panel('refresh', url);
                    }
                }]
            });
        }
    }
    tabMenu.menu({
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
                    if (!canClose(selectedTab)) { return false; }
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
            e.preventDefault();
            e.stopPropagation();
            tab.tabs('select', title);
            tabMenu.data("targetTab", { title: title, index: index });
            //显示快捷菜单
            if (index > 0) {
                tabMenu.menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        },
        onSelect: function (title, index) {
            var selectedTab = $(tab).tabs('getSelected');
            var opts = $(selectedTab).panel("options");
            if (!opts.id) {
                $(".menuitem").removeClass("active");
                return;
            }
            $(".list-" + opts.id).siblings().removeClass("active");
            $(".list-" + opts.id).addClass("active");
        }
    });
});

function canClose(panel) {
    return $(panel).panel('options').closable;
}

function openTab(title, url, param) {
    if ($('#Mytab').tabs('exists', title)) {
        $('#Mytab').tabs('select', title);
    } else {
        var div = $("<div>").css({ width: "100%", height: "100%", overflow: "hidden" });
        if (url) {
            $.messager.progress({
                title: lang.progressTitle,
                msg: lang.loading
            });
            $("<iframe onload=onIframeLoad(this)>")
                .css({ width: "100%", height: "100%", border: 0 })
                .attr({ src: url })
                .appendTo(div);
        }
        var options = {
            title: title,
            content: div, // 可以局部刷新tab选项卡  
            closable: true
        };
        if (param) {
            $.extend(options, param);
        }
        $('#Mytab').tabs('add', options);
    }
}

function onIframeLoad(_iframe) {
    $.messager.progress("close");
}
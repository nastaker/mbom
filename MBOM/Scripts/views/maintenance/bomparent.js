
$(function () {
    $('#tree').jstree({
        'core': {
            "themes": {
                "responsive": false
            },
            "check_callback": true,
            'data': function (obj, callback) {
                if (obj.id === "#") {
                    callback([{ text: data["CODE"] + " " + data["NAME"], CODE: data["CODE"], children: true }]);
                    return;
                }
                var param = obj.original;
                var arr = [];
                $.post("/Item/FindParent", {
                    code: param["CODE"]
                }, function (result) {
                    var items = result.data;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        arr.push({
                            "text": item["CODE"] + " " + item["NAME"],
                            "CODE": item["CODE"],
                            "NAME": item["NAME"],
                            children: true
                        });
                    }
                    callback(arr);
                });
            }
        },
        "types": {
            "default": {
                "icon": "fa fa-gear icon-state-warning icon-lg"
            }
        },
        "plugins": ["types"]
    }); 
});
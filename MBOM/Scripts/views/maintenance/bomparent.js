
$(function () {
    $('#tree').jstree({
        'core': {
            "themes": {
                "responsive": false
            },
            "check_callback": true,
            'data': function (obj, callback) {
                if (obj.id === "#") {
                    callback([{ text: data["ITEMCODE"] + " " + data["NAME"], ITEMCODE: data["ITEMCODE"], children: true }]);
                    return;
                }
                var param = obj.original;
                var arr = [];
                $.post("/Item/FindParent", {
                    prod_itemcode: param["ITEMCODE"]
                }, function (result) {
                    var items = result.data;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        console.log(item)
                        arr.push({
                            "text": item["ITEMCODE"] + " " + item["NAME"],
                            "ITEMCODE": item["ITEMCODE"],
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
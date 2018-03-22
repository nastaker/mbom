function AlertWin(msg, callback) {
    $.messager.alert(lang.alertTitle, msg, 'error', callback);
}

function InfoWin(msg, callback) {
    $.messager.alert(lang.infoTitle, msg, 'info', callback);
}

function postData(url, data, callback) {
    $.messager.progress({ title: lang.infoTitle, msg: lang.loading });
    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        data: data,
        success: function (data) {
            $.messager.progress('close');
            callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.progress('close');
            AlertWin(XMLHttpRequest.responseJSON.msg);
        }
    })
}

function postDataSync(url, data, callback) {
    $.ajax({
        url: url,
        type: "post",
        dataType: "json",
        async: false,
        data: data,
        success: function (data) {
            callback(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            AlertWin(XMLHttpRequest.responseJSON.msg);
        }
    })
}

function ToJavaScriptDate(value, fmt) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    if (!fmt) {
        fmt = "yyyy-MM-dd";
    }
    return new Date(parseFloat(results[1])).format(fmt);
}

function loadFilter(result) {
    if (result.success) {
        return result.data;
    } else {
        InfoWin(result.msg);
        return [];
    }
}

function remove(arr, i) {
    arr.some(function (item, index) {
        if (index == i) {
            arr.splice(index, 1);
            return true;
        }
    })
}

function removeByValue(arr, val) {
    arr.some(function (item, index) {
        if (item === val) {
            arr.splice(index, 1);
            return true;
        }
    })
}

function exists(arr, val) {
    var isExists = false;
    for (var i in arr) {
        if (arr[i] == val) {
            isExists = true;
            break;
        }
    }
    return isExists;
}

$.extend($.fn.validatebox.defaults.rules, {
    number: {
        validator: function (value, param) {
            var regexNum = /^[0-9]*(.[0-9]{1,4})?$/
            return regexNum.test(value);
        },
        message: '请输入数字，且小数点后不超过4位'
    }
});

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,  //月份 
        "d+": this.getDate(),       //日 
        "h+": this.getHours(),      //小时 
        "m+": this.getMinutes(),    //分 
        "s+": this.getSeconds(),    //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3),    //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

function isJsonString(str) {  
    try {  
        if (typeof JSON.parse(str) == "object") {  
            return true;  
        }
    } catch(e) { 
    }  
    return false;  
} 
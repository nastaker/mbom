var URL_APPLYCHANGES = "/ProductChange/ApplyChanges";
var URL_APPLYVIRTUALCHANGE = "/ProductChange/ApplyVirtualChange";

var dlgApplyReason = $("#dlgApplyReason");
var txtApplyReason = $("#txtApplyReason");
$(function () {
    dlgApplyReason.dialog({
        title: "输入变更理由",
        width: 400,
        height: 240,
        modal: true,
        closed: true,
        footer: '#dlgApplyReasonFooter'
    });
    txtApplyReason.textbox({
        width: "100%",
        height: "165px",
        tipPosition: "bottom",
        cls: "textbox-noborder",
        required: true,
        multiline: true
    });
});

function dlgApplyReasonOpen() {
    txtApplyReason.textbox("clear");
    dlgApplyReason.dialog({
        closed: false
    });
    dlgApplyReason.dialog("center");
}

//应用所有变更
function dlgApplyChanges() {
    //获取输入的理由
    var isValid = txtApplyReason.textbox("isValid");
    if (!isValid) {
        AlertWin("请输入变更理由");
        return;
    }
    var reason = txtApplyReason.textbox("getText");
    postData(URL_APPLYCHANGES, {
        code: params.code,
        reason: reason
    }, function (result) {
        dlgApplyReason.dialog("close");
        if (result.success) {
            if (result.msg) {
                InfoWin(result.msg)
            }
            reloadAllTables(true);
        } else {
            if (result.msg) {
                AlertWin(result.msg)
            }
        }
    });
}
//应用虚件变更
function applyVirtualChange() {
    //获取输入的理由
    var isValid = txtApplyReason.textbox("isValid");
    if (!isValid) {
        AlertWin("请输入变更理由");
        return;
    }
    var reason = txtApplyReason.textbox("getText");
    postData(URL_APPLYVIRTUALCHANGE, {
        code: params.code,
        reason: reason
    }, function (result) {
        dlgApplyReason.dialog("close");
        if (result.success) {
            if (result.msg) {
                InfoWin(result.msg)
            }
            reloadAllTables(true);
        } else {
            if (result.msg) {
                AlertWin(result.msg)
            }
        }
    });
} 
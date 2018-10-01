$(function () {

    //保存
    $("#btnSave").click(function () {
        OnSave();
    })
    $("#abtnSave").click(function () {
        OnSave();
    })
    //返回
    $("#abtnBack").click(function () {
        historyGo();
    })
    //返回
    $("#btnBack").click(function () {
        historyGo();
    })
})

var jeasyuiFun = {
    topCenter: function (title, msg) {
        $.messager.show({
            title: title,
            msg: msg,
            showType: 'slide',
            style: {
                right: '',
                top: document.body.scrollTop + document.documentElement.scrollTop,
                bottom: ''
            }
        });
    }
}
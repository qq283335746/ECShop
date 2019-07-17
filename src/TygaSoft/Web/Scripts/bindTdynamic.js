$(function () {
    var $_bindT = $("#bindT");
    if (($_bindT != undefined) && ($_bindT.length > 0)) {
        var trList = $_bindT.find("tr");
        var $_thRow = trList.filter(":first");
        var $_tdRow = trList.filter(":not(:first)");
        $_tdRow.filter(":even").addClass("evenStyle");
        $_tdRow.hover(function () {
            $(this).css('backgroundColor', '#C9E7E9');
        }, function () {
            $(this).css('backgroundColor', '');
        })
        $_tdRow.click(function () {
            $(this).addClass("clickStyle").siblings().removeClass("clickStyle");
        })
        $_thRow.click(function () {
            trList.each(function () {
                $(this).removeClass("clickStyle");
            })
        })
        var cbList = $_tdRow.find("[name$=cbItem]");
        $("#cbAll").click(function () {
            cbList.attr("checked", this.checked);
        })

        $("[name$=cbItem]").click(function () {
            $('#cbAll').attr('checked', cbList.length == cbList.filter(':checked').length);
        })

        //规定一定行，超出则将表格设定为自适应，否则将表格设定为固定样式
        $_th = $_thRow.find("th");
        if ($_th.length <= 9) {
            $_bindT.removeAttr("style");
            $_th.each(function () {
                $(this).removeAttr("style");
            })
        }

        //将数据绑定表格置于可拖动、拉伸效果
        $_th.each(function () {
            $(this).resizable();
        })
        $_bindT.draggable();
    }
})
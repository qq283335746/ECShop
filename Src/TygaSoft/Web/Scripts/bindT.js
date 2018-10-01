$(function () {
    var bindT = $("#bindT");
    if ((bindT != undefined) && (bindT.length > 0)) {
        var trList = bindT.find("tr");
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
        cbList.attr("checked",$("#cbAll").attr("checked"));
        $("#cbAll").click(function () {
            cbList.attr("checked", this.checked);
        })

        $("[name$=cbItem]").click(function () {
            $('#cbAll').attr('checked', cbList.length == cbList.filter(':checked').length);
        })
    }
})

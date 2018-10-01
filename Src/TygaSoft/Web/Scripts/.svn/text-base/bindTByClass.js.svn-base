$(function () {
    if ($(".bindT").length > 0) {
        var $_thRow = $(".bindT>thead>tr:first");
        var trList = $(".bindT>tbody>tr");
        trList.filter(":even").addClass("evenStyle");
        trList.hover(function () {
            $(this).css('backgroundColor', '#C9E7E9');
        }, function () {
            $(this).css('backgroundColor', '');
        })
        trList.click(function () {
            $(this).addClass("clickStyle").siblings().removeClass("clickStyle");
        })
        $_thRow.click(function () {
            trList.each(function () {
                $(this).removeClass("clickStyle");
            })
        })

        $("#cbAll").click(function () {
            $('.bindT').find("[name$=cbItem]").attr("checked", this.checked);
        })

        $("[name$=cbItem]").click(function () {
            var cbList = $('.bindT').find("[name$=cbItem]");
            $('#cbAll').attr('checked', cbList.length == cbList.filter(':checked').length);
        })
    }
})
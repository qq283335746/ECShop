$(function () {
    var txtStartPlace = $("[id$=txtStartPlace]");
    var sStartPlace = $.trim(txtStartPlace.val());
    var sStartPlaceArr = sStartPlace.split("-");

    $("#s-area-s").dialog({ autoOpen: false });
    txtStartPlace.click(function () {
        var isSOpen = $("#s-area-s").dialog("isOpen");
        if (!isSOpen) {
            $("#s-area-s").dialog("open");
        }
    })

    //发货地开始

    var div_li = $("#s-area-s>ul>li");
    div_li.click(function () {
        $(this).addClass("selected")
        .siblings().removeClass("selected");
        var index = div_li.index(this);
        $("#s-area-s>div")
        .eq(index).show()
        .siblings("div").hide();
    }).hover(function () {
        $(this).addClass("hover");
    }, function () {
        $(this).removeClass("hover");
    })

    var url = $("#hProvinceCityUrl").val();

    $.ajax({
        type: "GET",
        url: url,
        async: false,
        data: { pcity: sStartPlace },
        success: function (msg) {
            $("#s-area-s>div:eq(0)").html($(msg).filter("[id=highUse]").html());
            $("#s-area-s>div:eq(1)").html($(msg).filter("[id=province]").html());
            $("#s-area-s>div:eq(2)").html($(msg).filter("[id=city]").html());
            $("#s-area-s>div:eq(3)").html($(msg).filter("[id=county]").html());
        }
    })

    var aList = $("#s-area-s").find("a");
    if (aList.length == 0) return false;
    if (sStartPlaceArr.length > 0) {
        aList.each(function () {
            for (var i = 0; i < sStartPlaceArr.length; i++) {
                if ($.trim($(this).text()) == $.trim(sStartPlaceArr[i])) {
                    $(this).addClass("current");
                }
            }
        })
    }

    aList.live("click", function () {
        var currentObj = $(this);
        txtStartPlace.val(currentObj.text());
        var currentId = currentObj.attr("code");
        currentObj.addClass("current").parent().siblings().children().removeClass("current");
        var currentIndex = currentObj.parent().parent().parent().index();

        if (currentIndex == 1) {
            var currentIdArr = currentId.split(",");
            var aProvince = $("#s-area-s>div:eq(1)").find("a");
            aProvince.each(function () {
                for (var i = 0; i < currentIdArr.length; i++) {
                    if ($.trim($(this).attr("code")) == $.trim(currentIdArr[i])) {
                        txtStartPlace.val($(this).text() + "-" + txtStartPlace.val());
                        $(this).addClass("current").parent().siblings("li").children("a").removeClass("current");
                    }
                }

            })

            $.ajax({
                type: "GET",
                url: url,
                async: false,
                data: { pcity: $("[id$=txtStartPlace]").val() },
                success: function (msg) {
                    $("#s-area-s>div:eq(2)").html($(msg).filter("[id=city]").html());
                    $("#s-area-s>div:eq(3)").html($(msg).filter("[id=county]").html());
                }
            })

            $("#s-area-s>ul>li:eq(3)").addClass("selected").siblings().removeClass("selected");
            $("#s-area-s>div:eq(3)").show().siblings("div").hide();

            $("#s-area-s>div:eq(2)").find("a").each(function () {
                if ($.trim($(this).text()) == $.trim(currentObj.text())) {
                    $(this).addClass("current").parent().siblings("li").children("a").removeClass("current");
                }
            })
        }
        else if (currentIndex == 4) {
            SetStartPlace();
            $("#s-area-s").dialog("close");
        }
        else if (currentIndex == 2) {
            $("#s-area-s>div:eq(2)").html("");
            $("#s-area-s>div:eq(3)").html("");
            $("#s-area-s>div:eq(0)").find("a").filter(":contains('" + currentObj.text() + "')");

            SetStartPlace();
            $.ajax({
                type: "GET",
                url: url,
                async: false,
                data: { pcity: $("[id$=txtStartPlace]").val() },
                success: function (msg) {
                    $("#s-area-s>div:eq(2)").html($(msg).filter("[id=city]").html());
                }
            })

            $("#s-area-s>ul>li:eq(2)").addClass("selected").siblings().removeClass("selected");
            $("#s-area-s>div:eq(2)").show().siblings("div").hide();
        }
        else if (currentIndex == 3) {
            SetStartPlace();
            $.ajax({
                type: "GET",
                url: url,
                async: false,
                data: { pcity: $("[id$=txtStartPlace]").val() },
                success: function (msg) {
                    $("#s-area-s>div:eq(3)").html($(msg).filter("[id=county]").html());
                }
            })
            $("#s-area-s>ul>li:eq(3)").addClass("selected").siblings().removeClass("selected");
            $("#s-area-s>div:eq(3)").show().siblings("div").hide();
        }

        return false;
    })

    //发货地结束
})

function SetStartPlace() {
    var startPlace = "";
    var aPcc = $("#s-area-s>div:gt(0)").find("a");
    var n = 0;
    aPcc.each(function () {
        if ($(this).hasClass("current")) {
            n++;
            if (n == 1) {
                startPlace += $(this).text();
            }
            else {
                startPlace += "-" + $(this).text();
            }
        }
    })
    $("[id$=txtStartPlace]").val(startPlace);
}

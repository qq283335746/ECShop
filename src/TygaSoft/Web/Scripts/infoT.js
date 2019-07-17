
$(function () {
    var $_infoT = $("#infoT");
    var $_trList = $_infoT.find("tr");
    $_trList.filter(":odd:not(:last)").addClass("evenStyle");
    $_trList.filter(":last").css("border-bottom-style", "none");
    var $_tdList = $_trList.find("td:not([colspan])");
    $_tdList.each(function (i) {
        if (i % 4 == 2) $(this).addClass("taf");
    })
    
})
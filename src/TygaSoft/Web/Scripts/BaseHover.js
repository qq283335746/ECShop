$(function () {
    hoverById("category");
    hoverSubItem();
})


function hoverById(id) {
    var obj = document.getElementById(id);
    $(obj).hover(function () {
        $(this).addClass("hover");
    }, function () {
        $(this).removeClass("hover");
    })
}

function hoverByObj(obj) {
    obj.hover(function () {
        $(this).addClass("hover");
    }, function () {
        $(this).removeClass("hover");
    })
}

function hoverSubItem() {
    $("#category .item").hover(function () {
        $(this).addClass("hover").siblings().removeClass("hover");
    }, function () {
        $(this).removeClass("hover");
    })
}
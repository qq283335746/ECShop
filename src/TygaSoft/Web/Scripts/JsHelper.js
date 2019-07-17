//加入收藏夹
function addFavorite() {
    if (document.all) {
        window.external.addFavorite('http://www.tyga.com/', '天涯孤岸电子商务');
    }
    else if (window.sidebar) {
        window.sidebar.addPanel('天涯孤岸电子商务', 'http://www.tyga.com/', "");
    }
    else {
        alert("您的浏览器不支持，请手动添加。");
    }
    //return false;
}
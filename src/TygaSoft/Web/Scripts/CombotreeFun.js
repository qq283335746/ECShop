var CombotreeFun = {
    init: function () {
        CombotreeFun.load("");
    },
    obj: null,
    url: '',
    isCollapseAll: true,
    load: function () {
        $.ajax({
            type: "POST",
            url: CombotreeFun.url,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                CombotreeFun.obj.combotree({
                    data: jsonData,
                    panelHeight: 'auto',
                    onLoadSuccess: function (node, data) {
                        if (CombotreeFun.isCollapseAll) {
                            var t = CombotreeFun.obj.combotree('tree');
                            var rootNode = t.tree('getRoot');
                            if (rootNode) {
                                var nodes = t.tree('getChildren', rootNode.target);
                                var nodesLen = nodes.length;
                                if (nodesLen > 0) {
                                    for (var i = 0; i < nodesLen; i++) {
                                        t.tree('collapseAll', nodes[i].target);
                                    }
                                }
                            }
                        }
                    }
                });
            }
        })
    },
    load: function (v) {
        $.ajax({
            type: "POST",
            url: CombotreeFun.url,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                CombotreeFun.obj.combotree({
                    data: jsonData,
                    panelHeight: 'auto',
                    onLoadSuccess: function (node, data) {
                        var t = CombotreeFun.obj.combotree('tree');
                        if (CombotreeFun.isCollapseAll) {
                            var rootNode = t.tree('getRoot');
                            if (rootNode) {
                                var nodes = t.tree('getChildren', rootNode.target);
                                var nodesLen = nodes.length;
                                if (nodesLen > 0) {
                                    for (var i = 0; i < nodesLen; i++) {
                                        t.tree('collapseAll', nodes[i].target);
                                    }
                                }
                            }
                        }
                        if (v != undefined && v.length > 0) {
                            var node = t.tree('find', v);
                            if (node) {
                                CombotreeFun.obj.combotree('setValue', node.id);
                            }
                        }
                        else {
                            if (jsonData != null && jsonData.length > 0) {
                                CombotreeFun.obj.combotree('setValue', jsonData[0].id);
                            }
                        }
                    }
                });
            }
        })
    }
}
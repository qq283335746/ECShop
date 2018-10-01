var treeFun = {
    init: function (h) {
        treeFun.load(h);
    },
    url: '',
    load: function (h) {
        var obj = $(document.getElementById(h));
        $.ajax({
            type: "POST",
            url: "/ScriptServices/AdminService.asmx/GetTreeJson",
            contentType: "application/json; charset=utf-8",
            data: "{}",
            success: function (json) {
                var jsonData = (new Function("", "return " + json.d))();
                console.log("aaa-" + jsonData);
                obj.tree({
                    data: jsonData,
                    animate: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $(this).tree('select', node.target);
                        $('#mm').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    }
                })
            }
        })
    },
    add: function (h) {
        var obj = $(document.getElementById(h));
        var node = obj.tree('getSelected');
        if (node) {
            $('#dlgAdd').dialog({
                title: '新建',
                closed: false,
                cache: false,
                modal: true,
                href: $(this).attr("href"),
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-ok',
                    handler: function () { treeFun.save(); }
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () { $('#dlgAdd').window('close'); }
                }],
                onLoad: function () {
                    OnAddDlgLoad();
                }
            });
        }
    },
    edit: function (h) {
        var obj = $(document.getElementById(h));
        var node = obj.tree('getSelected');
        if (node) {
            $('#dlgAdd').dialog({
                title: '修改',
                closed: false,
                cache: false,
                modal: true,
                href: $(this).attr("href"),
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-ok',
                    handler: function () { treeFun.save(); }
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () { $('#dlgAdd').window('close'); }
                }],
                onLoad: function () {
                    onEditDlgLoad();
                }
            });
        }
    },
    save: function () {
        OnDlgSave();
    },
    del: function () {
        OnDel();
    }
}
$(function () {
    $.extend($.fn.validatebox.defaults.rules, {
        checkPsw: {
            validator: function (value, param) {
                var reg = /(([0-9]+)|([a-zA-Z]+)){6,30}/;
                return reg.test($(param[0]).val());
            },
            message: '密码正确格式由数字或字母组成的字符串，且最小6位，最大30位'
        }
    });
    $.extend($.fn.validatebox.defaults.rules, {
        cfmPsw: {
            validator: function (value, param) {
                return value == $(param[0]).val();
            },
            message: '前后输入密码不一致'
        }
    });
    $.extend($.fn.validatebox.defaults.rules, {
        checkMobile: {
            validator: function (value, param) {
                var reg = /^1[3|4|5|8][0-9]\d{4,8}$/;
                return reg.test($(param[0]).val());
            },
            message: '请输入正确的手机号'
        }
    });
    $.extend($.fn.validatebox.defaults.rules, {
        checkTelephone: {
            validator: function (value, param) {
                var reg = /(\(\d{3,4}\)|\d{3,4}-|\s)?\d{8}/;
                return reg.test($(param[0]).val());
            },
            message: '请输入正确的固定电话号码'
        }
    });
    $.extend($.fn.validatebox.defaults.rules, {
        checkInt: {
            validator: function (value, param) {
                var reg = /^(\d+)$/;
                return reg.test($(param[0]).val());
            },
            message: '请输入正确的整数'
        }
    });

})

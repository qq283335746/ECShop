<%@ Page Title="新建/修改收货地址" Language="C#" MasterPageFile="~/Users/Users.Master" AutoEventWireup="true" CodeBehind="AddAddress.aspx.cs" Inherits="TygaSoft.Web.Users.AddAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="../Styles/ProviceCity.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="收货地址" style="width:465px;">
  <div class="row mt10">
     <span class="fl rl">收货人：</span>
     <div class="fl"><input type="text" runat="server" id="txtReceiver" name="txtReceiver" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" /></div>
     <div class="clr"></div>
  </div>
  <div class="row mt10">
     <span class="fl rl">所在地区：</span>
     <div class="fl">
       <input type="text" runat="server" id="txtProvinceCity" readonly="readonly" name="txtProvinceCity" class="easyui-validatebox" data-options="required:true,missingMessage:'必选项'" />
       <div id="dlgArea" style="padding:10px;"></div>
     </div>
     <div class="clr"></div>
  </div>
  <div class="row mt10">
     <span class="fl rl">详细地址：</span>
     <div class="fl"><input type="text" runat="server" id="txtAddress" name="txtAddress" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" style=" width:285px;" /></div>
     <div class="clr"></div>
  </div>
  <div class="row mt10">
     <span class="fl rl">手机：</span>
     <div class="fl"><input type="text" runat="server" id="txtMobilephone" name="txtMobilephone" class="easyui-validatebox" data-options="missingMessage:'手机或固定电话必填一项'" validType="checkMobile['#cphMain_txtMobilephone']" /></div>
     <div class="clr"></div>
  </div>
  <div class="row mt10">
     <span class="fl rl">固定电话：</span>
     <div class="fl"><input type="text" runat="server" id="txtTelephone" name="txtTelephone" class="easyui-validatebox" validType="checkTelephone['#cphMain_txtTelephone']" /></div>
     <div class="clr"></div>
  </div>
  <div class="row mtb10">
     <span class="fl rl">电子邮箱：</span>
     <div class="fl"><input type="text" runat="server" id="txtEmail" name="txtEmail" class="easyui-validatebox" data-options="required:true,validType:'email',missingMessage:'必填项',invalidMessage:'请输入正确的电子邮箱格式'" /></div>
     <div class="clr"></div>
  </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" />
<input type="hidden" runat="server" id="hOp" value="" />
<input type="hidden" runat="server" id="hBackToN" value="1" />

<script type="text/javascript" src="../Scripts/Jeasyui.js"></script>
<script src="../Scripts/EasyuiExtend.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        $('#dlgArea').dialog({
            title: '省市区选择',
            width: 335,
            height: 350,
            href:"/Templates/HtmlPage/AreaTabs.htm",
            closed: true
        });
        $("[id$=txtProvinceCity]").click(function () {
            $('#dlgArea').window('open');
            
        })
    })

    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    function OnSave() {
        var isValid = $('#form1').form('validate');
        if (isValid) {
            if (($.trim($("[id$=txtMobilephone]").val()).length == 0) && ($.trim($("[id$=txtTelephone]").val()).length == 0)) {
                alert("手机号或固定电话必填写一项");
                return false;
            }
            $("[id$=hOp]").val("commit");
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }
    }

    
</script>

</asp:Content>

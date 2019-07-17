<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddSearchKeyword.aspx.cs" Inherits="TygaSoft.Web.Admin.AboutSite.AddSearchKeyword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div class="easyui-panel" title="填写信息" style="width:400px;">
  <div class="row mt10">
    <span class="fl rl">关键字名称：</span>
    <div class="fl">
      <input type="text" runat="server" id="txtSearchName" name="txtSearchName" class="easyui-validatebox" data-options="required:true,missingMessage:'必填项'" />
    </div>
    <div class="clr"></div>
  </div>
    <div class="row mt10">
    <span class="fl rl">累计次数：</span>
    <div class="fl">
      <input type="text" runat="server" clientidmode="Static" id="txtTotalCount" name="txtTotalCount" class="easyui-validatebox" validType="checkInt['#txtTotalCount']" />
    </div>
    <div class="clr"></div>
    </div>
    <div class="row mtb10">
      <span class="fl rl">数据个数：</span>
      <div class="fl">
        <input type="text" runat="server" clientidmode="Static" id="txtDataCount" name="txtDataCount" class="easyui-validatebox" validType="checkInt['#txtDataCount']" />
      </div>
      <div class="clr"></div>
    </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" CommandName="lbtnsave" />
<script type="text/javascript" src="../../Scripts/Jeasyui.js"></script>
<script src="../../Scripts/EasyuiExtend.js" type="text/javascript"></script>

<input type="hidden" runat="server" id="hBackToN" value="1" />
<script type="text/javascript">
    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {
        var isValid = $('#form1').form('validate');
        if (isValid) {
            __doPostBack('ctl00$cphMain$lbtnPostBack', '');
        }
    }
</script>

</asp:Content>

<%@ Page Title="新建/修改产品信息" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="TygaSoft.Web.Admin.Product.AddProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link href="/Jquery/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
  <link href="/Jquery/plugins/uploadify/css/uploadify.css" rel="stylesheet" type="text/css" />
  <link href="/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
  <link href="/kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" /> 
  <script type="text/javascript" src="/kindeditor/kindeditor.js"></script>
  <script type="text/javascript" src="/kindeditor/lang/zh_CN.js"></script>
  <script type="text/javascript" src="/kindeditor/plugins/code/prettify.js"></script>
  <script type="text/javascript" src="/Jquery/plugins/uploadify/scripts/jquery.uploadify.min.js"></script>
  
  <style type="text/css">
      .uploadify{ float:left; width:100px; height:80px; margin-right:10px;}
      .uploadify-queue{ float:left; width:580px; max-height:300px; overflow:auto;}
      #cAttr { list-style-type: none; margin: 0; padding: 0; width:465px;}
	  #cAttr li { margin: 0 3px 3px 3px; padding: 0.2em; padding-left: 1.5em;}
	  #cAttr li span { position: absolute; margin-left: -1.3em; }
  </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="product-accordion" class="easyui-accordion">
    <div title="产品基本信息" data-options="iconCls:'icon-edit'">
      <div class="row mt10">
          <span class="fl rl"><b class="cr">*</b>所属分类：</span>
          <div class="fl">
              <asp:DropDownList ID="ddlCategory" runat="server" />
              <span id="error-Category" class="cr"></span>
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mt10">
          <span class="fl rl"><b class="cr">*</b>产品名称：</span>
          <div class="fl">
              <input type="text" runat="server" id="txtProductName" class="txtl" />
              <span id="error-ProductName" class="cr"></span>
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mt10">
          <span class="fl rl">副标题：</span>
          <div class="fl">
              <input type="text" runat="server" id="txtSubtitle" class="txtl" />
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mt10">
          <span class="fl rl"><b class="cr">*</b>价格：</span>
          <div class="fl">
              ￥<input type="text" runat="server" id="txtPrice" class="txt" style="width:100px;" />
                <span id="error-Price" class="cr"></span>
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mt10">
          <span class="fl rl">产品图片：</span>
          <div class="fl">
               <input type="file" id="fileUp" />    
          </div>
           
          <div class="clr"></div>
      </div>
      <div class="row mt10">
          <span class="fl rl">&nbsp;</span>
          <div class="fl">
                <div class="mb10">图片最佳宽高：800像素*800像素</div>
                <a href="javascript:$('#fileUp').uploadify('upload', '*');" class="easyui-linkbutton">上传</a>
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mtb10">
          <span class="fl rl">&nbsp;</span>
          <div class="fl">
            <ul>
              <li class="fl">
                  <div class="easyui-panel" title="已上传文件" style="width:580px; max-height:250px; min-height:110px; overflow:auto; padding:0 10px 10px 10px;" >
                    <ul id="showUpload"></ul>
                  </div>
              </li>
              <li class="fl ml10">
                  <div class="easyui-panel" title="商品主图片" style="padding:0 10px 10px 10px; overflow:hidden;">
                      <div class="mt10" id="productImgMain"><img src="" alt="" width="100" height="100" /></div>
                  </div>
              </li>
            </ul>
          </div>
          <div class="clr"></div>
      </div>

    </div>
    <div title="产品属性" data-options="iconCls:'icon-edit'">
      <div class="row mt10">
         <span class="fl rl">商品编号：</span>
         <div class="fl"><input type="text" runat="server" id="txtPNum" class="txt" style="width:100px;" /></div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl"><b class="cr">*</b>库存：</span>
         <div class="fl">
             <input type="text" runat="server" id="txtStockNum" class="txt" style="width:100px;" />
             <span id="error-StockNum" class="cr"></span>
         </div>
         <div class="clr"></div>
      </div>
      <div class="row mt10">
         <span class="fl rl">市场价：</span>
         <div class="fl">
             ￥<input type="text" runat="server" id="txtMarketPrice" class="txt" style="width:100px;" />
             <span id="error-MarketPrice" class="cr"></span>
         </div>
         <div class="clr"></div>
      </div>
      <div class="row mtb10">
         <span class="fl rl"><b class="cr">*</b>支付方式：</span>
         <div class="fl">
             <input type="text" runat="server" id="txtPayOptions" class="txt" />
             <span id="error-PayOptions" class="cr"></span>
         </div>
         <div class="clr"></div>
      </div>
    </div>
    <div title="产品档案" data-options="iconCls:'icon-edit'">
      <div class="row mt10">
          <span class="fl rl">自定义属性：</span>
          <div class="fl" id="cAttrLoad">
              
          </div>
          <div class="clr"></div>
      </div>
      <div class="row mtb10">
          <span class="fl rl">产品描述：</span>
          <div class="fl">
              <textarea id="editor1" cols="100" rows="8" style="width:800px;height:400px;visibility:hidden;"></textarea>
          </div>
          <div class="clr"></div>
      </div>

    </div>
    
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command"/>
<input type="hidden" runat="server" id="hOp" value="commit" />
<input type="hidden" runat="server" id="hCustomAttrs" value="" />
<input type="hidden" runat="server" id="hBackToN" value="1" />
<input type="hidden" runat="server" id="hEditor1" value="" />
<input type="hidden" runat="server" id="hUploadify" value="" />
<input type="hidden" runat="server" id="hPImagMain" />
    
<script type="text/javascript" src="/Scripts/Jeasyui.js"></script>
<script type="text/javascript" src="/Scripts/UploadifyFun.js"></script>
<script type="text/javascript">
    var editor1;
    KindEditor.ready(function (K) {
        editor1 = K.create('#editor1', {
            cssPath: '/kindeditor/plugins/code/prettify.css',
            uploadJson: '/Handlers/KindeditorFilesUpload.ashx',
            fileManagerJson: '/Handlers/KindeditorFiles.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                K.ctrl(document, 13, function () {
                    //                        self.sync();
                    //                        K('form[name=form1]')[0].submit();
                });
                K.ctrl(self.edit.doc, 13, function () {
                    //                        self.sync();
                    //                        K('form[name=form1]')[0].submit();
                });
            }
        });
        prettyPrint();

    });
    $(function () {
        //加载自定义属性的html
        $("#cAttrLoad").load("/Templates/HtmlPage/UserAttrs.htm", function () {
            var sCAttrs = $("[id$=hCustomAttrs]").val();
            SetAttrs(sCAttrs);
        });

        var hEditor1 = $("[id$=hEditor1]");
        if (hEditor1.val().length > 0) {
            $("#editor1").html(decodeURIComponent(hEditor1.val()));
        }

        //上传
        OnUploadify("fileUp");
        var sPImagMain = $("[id$=hPImagMain]").val();
        $("#productImgMain").find("img").attr("src", sPImagMain);
        var hUploadify = $.trim($("[id$=hUploadify]").val());
        if (hUploadify.length > 0) {
            var imgArr = hUploadify.split(",");
            var imgArrLen = imgArr.length;
            for (var i = 0; i < imgArrLen; i++) {
                var li = "";
                if (sPImagMain == imgArr[i]) {
                    li = "<li class='fl mr10 mt10 bd'><img src='" + imgArr[i] + "' alt='' width='100' height='100' onclick='OnUploadifyImg(this)' />";
                }
                else {
                    li = "<li class='fl mr10 mt10'><img src='" + imgArr[i] + "' alt='' width='100' height='100' onclick='OnUploadifyImg(this)' />";
                }
                li += "<div class='mt10 tc'> <a class='mt10' href=\"javascript:;\" onclick=\"OnImgRemove(this)\">删除</a></div></li>";
                $("#showUpload").append(li);
            }
        }

    })

    function OnQueueComplete() {
        if ($("#showUpload").find("[class=bd]").length == 0) {
            var firstItem = $("#showUpload>li:first");
            firstItem.addClass("bd").siblings().removeClass("bd");
            var src = firstItem.find("img").attr("src");
            $("#productImgMain").find("img").attr("src", src);
            $("[id$=hPImagMain]").val(src);
        }
    }
    function OnUploadSuccess(h) {
        var li = "<li class='fl mr10 mt10'><img src='" + h.replace("~", "") + "' alt='' width='100' height='100' onclick='OnUploadifyImg(this)' />";
        li += "<div class='mt10 tc'> <a class='mt10' href=\"javascript:;\" onclick=\"OnImgRemove(this)\">删除</a></div></li>";
        $("#showUpload").append(li);
    }
    function OnUploadifyImg(h) {
        $(h).parent().addClass("bd").siblings().removeClass("bd");
        var sSrc = $(h).attr("src");
        $("#productImgMain").find("img").attr("src", sSrc);
        $("[id$=hPImagMain]").val(sSrc);
    }
    function OnImgRemove(h) {
        $.messager.confirm('温馨提醒', '确定要删除吗？', function (r) {
            if (r) {
                var sSrc = $(h).parent().parent().find("img").attr("src");
                var imgMain = $("#productImgMain").find("img");
                var hPImagMain = $("[id$=hPImagMain]");
                if ($.trim(sSrc) == imgMain.attr("src")) {
                    $("#productImgMain").html("<img src=\"\" alt=\"\" width=\"100\" height=\"100\" />");
                    if ($.trim(sSrc) == $.trim(hPImagMain.val())) {
                        hPImagMain.val("");
                    }
                }
                $(h).parent().parent().remove();
            }
        });
    }
    function historyGo() {
        var n = parseInt($("[id$=hBackToN]").val());
        history.go(-n);
    }

    //保存事件
    function OnSave() {
        var ddlCategoryIndex = $("[id$=ddlCategory]").find("option:selected").index();
        if (ddlCategoryIndex < 1) {
            $("#error-Category").text("所属分类为必选项");
            return false;
        }
        else {
            $("#error-Category").text("");
        }
        var txtProductName = $("[id$=txtProductName]");
        if ($.trim(txtProductName.val()).length == 0) {
            $('#product-accordion').accordion('select', 0);
            txtProductName.focus();
            $("#error-ProductName").text("产品名称为必填项");
            return false;
        }
        else {
            $("#error-ProductName").text("");
        }
        var txtPrice = $("[id$=txtPrice]");
        if ($.trim(txtPrice.val()).length == 0) {
            $('#product-accordion').accordion('select', 0);
            txtPrice.focus();
            $("#error-Price").text("价格为必填项");
            return false;
        }
        else {
            $("#error-Price").text("");
        }
        var reg = /^(\d+)$|^(\d+)\.{0,1}(\d+)$/;
        if (!reg.test(txtPrice.val())) {
            $("#error-Price").text("价格正确格式为：整数或浮点数");
            return false;
        }
        else {
            $("#error-Price").text("");
        }
        var txtStockNum = $("[id$=txtStockNum]");
        if ($.trim(txtStockNum.val()).length == 0) {
            $('#product-accordion').accordion('select', 1);
            txtStockNum.focus();
            $("#error-StockNum").text("库存为必填项");
            return false;
        }
        else {
            $("#error-StockNum").text("");
        }
        var regInt = /^(\d+)$/;
        if (!regInt.test(txtStockNum.val())) {
            $("#error-StockNum").text("库存正确格式为：整数");
            return false;
        }
        else {
            $("#error-StockNum").text("");
        }
        var txtPayOptions = $("[id$=txtPayOptions]");
        if ($.trim(txtPayOptions.val()).length == 0) {
            $('#product-accordion').accordion('select', 1);
            txtPayOptions.focus();
            $("#error-PayOptions").text("支付方式为必填项");
            return false;
        }
        else {
            $("#error-PayOptions").text("");
        }
        var txtMarketPrice = $("[id$=txtMarketPrice]");
        if ($.trim(txtMarketPrice.val()).length > 0) {
            if (!reg.test(txtMarketPrice.val())) {
                $('#product-accordion').accordion('select', 1);
                txtMarketPrice.focus();
                $("#error-MarketPrice").text("市场价正确格式为：整数或浮点数");
                return false;
            }
            else {
                $("#error-MarketPrice").text("");
            }
        }

        var cAttrAppend = GetAttrs();

        var sImgMain = $.trim($("#productImgMain").find("img").attr("src"));
        var imgs = $("#showUpload").find("img");
        var urls = "";
        imgs.each(function () {
            urls += $(this).attr("src") + ",";
        })
        if (urls.length > 0) {
            if (sImgMain.length == 0) {
                $.messager.alert('温馨提醒', '请设置商品主图片', 'error');
                return false;
            }

            $("[id$=hPImagMain]").val(sImgMain);
            $("[id$=hUploadify]").val(urls);
        }

        $("[id$=hCustomAttrs]").val(cAttrAppend);
        $("[id$=hEditor1]").val(encodeURIComponent(editor1.html()));
        $("[id$=hOp]").val("commit");

        __doPostBack('ctl00$cphMain$lbtnPostBack', '');
    }
 
</script>

</asp:Content>

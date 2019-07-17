<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddOrderSucceed.aspx.cs" Inherits="TygaSoft.Web.Users.Order.AddOrderSucceed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<asp:Literal runat="server" ID="ltrTheme"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

<div id="pAddOrderSucceed" style="padding:20px;">
  <div class="main">
      <div class="m m3" id="qpay">
		<div class="mc" style="padding-left:20px;">
           <asp:Literal runat="server" ID="ltrSucceed">
           </asp:Literal>
			
		</div>
	</div>
      <asp:Literal runat="server" ID="ltrBank">
      <div id="bankBox">
        <div>
            <strong>网银支付</strong> 需开通网银
          </div>
        <ul id="bankList">
              <li>
                <input type="radio" class="radio" name="rB" value="icbc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="工商银行" src="../../Images/Bank/icbc_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="ccb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国建设银行" src="../../Images/Bank/ccb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="cmbc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="招商银行" src="../../Images/Bank/cmb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="bcm" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="交通银行" src="../../Images/Bank/bcom_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="abc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国农业银行" src="../../Images/Bank/abc_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="boc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国银行" src="../../Images/Bank/boc_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="ceb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国光大银行" src="../../Images/Bank/ceb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="gdb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="广发银行" src="../../Images/Bank/gdb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="cmbc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国民生银行" src="../../Images/Bank/cmbc_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="hxb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="华夏银行" src="../../Images/Bank/hxb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="cib" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="兴业银行" src="../../Images/Bank/cib_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="psbc" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中国邮政储蓄银行" src="../../Images/Bank/post_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="citic" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="中信银行" src="../../Images/Bank/citic_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="spdb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="浦发银行" src="../../Images/Bank/spdb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="sdb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="深圳发展银行" src="../../Images/Bank/sdb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="bob" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="北京银行" src="../../Images/Bank/bob_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="cbhb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="渤海银行" src="../../Images/Bank/cbhb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="pab" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="平安银行" src="../../Images/Bank/pab_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="njcb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="南京银行" src="../../Images/Bank/njcb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              <li>
                <input type="radio" class="radio" name="rB" value="hzb" />
                <div class="bankImg">
                  <label>
					<a href="javascript:void(0)">
                      <img width="125" height="28" alt="杭州银行" src="../../Images/Bank/hzb_1301.png" />
                    </a>
				  </label>
                </div>
              </li>
              
          </ul>

        <div class="btns">
		  <a href="javascript:void(0)" id="abtnPay" class="btn-surepay" onclick="OnPay()">确认支付方式</a>
	    </div>
   </div>
    </asp:Literal>
  </div>
</div>

<asp:LinkButton runat="server" ID="lbtnPostBack" OnCommand="btn_Command" />
<input type="hidden" id="hOp" runat="server" value="saveAddress" />

<script type="text/javascript">
    function OnPay() {
        var bankName = $("#bankList input[type=radio]:checked").val();
        if (bankName == undefined) {
            alert("请选择网银");
            return false;
        }
        $("[id$=hOp]").val("OnPay");
        __doPostBack('ctl00$cphMain$lbtnPostBack', '');
    }
</script>

</asp:Content>

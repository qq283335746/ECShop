using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.CustomProviders;
using System.Web.UI.HtmlControls;

namespace TygaSoft.Web.Shares.ShoppingCart
{
    public partial class ListCart : System.Web.UI.Page
    {
        CustomProfileCommon profile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebHelper.PageHelper.LoadHeaderForShare(Page, ltrTheme);

                Bind();
            }
        }

        private void Bind()
        {
            if (profile == null) profile = new CustomProfileCommon();
            rpData.DataSource = profile.ShoppingCart.CartItems;
            rpData.DataBind();
        }

        protected void rpData_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            string commName = e.CommandName;
            switch (commName)
            {
                case "delBatch":
                    DelBatch();
                    break;
                case "del":
                    Del(e.CommandArgument.ToString());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 按钮OnCommand事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Command(object sender, CommandEventArgs e)
        {
            string commName = e.CommandName;
            switch (commName)
            {
                case "lbtnTopay":
                    OnTopay();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 去结算事件
        /// </summary>
        private void OnTopay()
        {
            //当前部门分配给用户
            RepeaterItemCollection ric = rpData.Items;
            if (ric.Count > 0)
            {
                bool hasQtyChange = false;
                if (profile == null) profile = new CustomProfileCommon();
                ICollection<Model.CartItemInfo> list = profile.ShoppingCart.CartItems;
                foreach (RepeaterItem item in ric)
                {
                    HtmlInputCheckBox cb = item.FindControl("cbItem") as HtmlInputCheckBox;
                    HtmlInputText txtQuantity = item.FindControl("txtQuantity") as HtmlInputText;
                    if (cb != null)
                    {
                        string productId = cb.Value;
                        if (cb.Checked)
                        {
                            if (txtQuantity != null)
                            {
                                int quantity = 1;
                                int.TryParse(txtQuantity.Value.Trim(), out quantity);
                                if (quantity < 1) quantity = 1;
                                if (quantity > 1)
                                {
                                    foreach (Model.CartItemInfo model in list)
                                    {
                                        if (model.ProductId == productId)
                                        {
                                            profile.ShoppingCart.SetQuantity(productId, quantity);

                                            hasQtyChange = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            profile.ShoppingCart.Remove(productId);
                            hasQtyChange = true;
                        }
                    }
                }

                if (hasQtyChange)
                {
                    profile.Save();
                }

                Response.Redirect("../../Users/Order/AddOrder.aspx", true);
            }
        }

        private void DelBatch()
        {
            if (profile == null) profile = new CustomProfileCommon();
            bool isNoCheck = true;

            RepeaterItemCollection rows = rpData.Items;
            //当前部门分配给用户
            foreach (RepeaterItem item in rows)
            {
                //找到CheckBox
                HtmlInputCheckBox cb = item.FindControl("cbItem") as HtmlInputCheckBox;
                if (cb != null && cb.Checked)
                {
                    if (isNoCheck) isNoCheck = false;
                    profile.ShoppingCart.Remove(cb.Value);
                }
            }
            if (isNoCheck)
            {
                WebHelper.MessageBox.Messager(this.Page, Page.Controls[0], "请至少选中一行进行操作！","操作错误","error");
                return;
            }

            profile.Save();
            Bind();
        }

        private void Del(string productId)
        {
            if (profile == null) profile = new CustomProfileCommon();
            profile.ShoppingCart.Remove(productId);
            profile.Save();
            Bind();
        }

        protected string GetCount()
        {
            if (profile == null) profile = new CustomProfileCommon();
            return profile.ShoppingCart.Count.ToString();
        }

        protected string GetTotalPrice()
        {
            if (profile == null) profile = new CustomProfileCommon();
            return profile.ShoppingCart.TotalPrice.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Users
{
    public partial class ListAddress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ////动态加载css和script
                //WebHelper.PageHelper.LoadHeaderForUsers(Page, ltrTheme);

                Bind();
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void Bind()
        {
            CustomProfileCommon profile = new CustomProfileCommon();
            rpData.DataSource = profile.UserAddress.GetList();
            rpData.DataBind();
        }

        /// <summary>
        /// 按钮OnCommand事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Command(object sender, CommandEventArgs e)
        {
            string commName = hOp.Value.Trim();
            switch (commName)
            {
                case "del":
                    OnDelete();
                    break;
                case "setDefault":
                    OnSetDefault();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void OnDelete()
        {
            string sItemsAppend = hV.Value.Trim();
            if (string.IsNullOrEmpty(sItemsAppend))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请勾选一行或多行数据再进行操作","操作错误","error");
                return;
            }
            string[] items = sItemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool hasChanged = false;
            CustomProfileCommon profile = new CustomProfileCommon();
            foreach (string item in items)
            {
                profile.UserAddress.Remove(Guid.Parse(item));
                hasChanged = true;
            }
            if (hasChanged)
            {
                profile.Save();
                Bind();
            }
        }

        /// <summary>
        /// 设为默认
        /// </summary>
        private void OnSetDefault()
        {
            string nId = hV.Value.Trim();
            if (!string.IsNullOrEmpty(nId))
            {
                CustomProfileCommon profile = new CustomProfileCommon();
                List<Model.UserAddressInfo> list = profile.UserAddress.GetList();
                foreach (Model.UserAddressInfo item in list)
                {
                    if (item.NumberID.ToString() == nId)
                    {
                        item.IsDefault = true;
                    }
                    else
                    {
                        item.IsDefault = false;
                    }
                }
                profile.Save();

                Bind();

                WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功");
            }
        }
    }
}
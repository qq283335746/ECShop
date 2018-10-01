using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;

namespace TygaSoft.Web.Admin.UserProfile
{
    public partial class ListProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ////动态加载css和script
                //WebHelper.PageHelper.LoadHeaderForAdmin(Page, ltrTheme);

                //数据绑定
                Bind();
            }
        }

        private void Bind()
        {
            ////查询条件
            //GetSearchItem();

            int totalCount = 0;

            rpData.DataSource = ProfileManager.GetAllProfiles(ProfileAuthenticationOption.Anonymous, AspNetPager1.CurrentPageIndex - 1, AspNetPager1.PageSize, out totalCount);
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {

        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            Bind();
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
                case "OnSearch":
                    OnSearch();
                    break;
                case "OnDel":
                    OnDelete();
                    break;
                default:
                    break;
            }
        }

        private void OnSearch()
        {
            bool isAnon = true;
            if (!cbIsAnon.Checked) isAnon = false;
            string sDtime = txtDueDate.Value.Trim();
            DateTime dtime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(sDtime))
            {
                sDtime = sDtime + " 23:59:59";
                if (!DateTime.TryParse(sDtime, out dtime))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "截止日期输入日期格式不正确，请检查", "操作错误", "error");
                    return;
                }
            }

            ProfileAuthenticationOption po = ProfileAuthenticationOption.Anonymous;
            if (!isAnon) po = ProfileAuthenticationOption.Authenticated;
            int totalCount = 0;
            ProfileInfoCollection profiles = null;
            if (dtime != DateTime.MinValue)
            {
                profiles = ProfileManager.GetAllInactiveProfiles(po, dtime, AspNetPager1.CurrentPageIndex - 1, AspNetPager1.PageSize, out totalCount);
            }
            else
            {
                profiles = ProfileManager.GetAllProfiles(po, AspNetPager1.CurrentPageIndex - 1, AspNetPager1.PageSize, out totalCount);
            }
            rpData.DataSource = profiles;
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        private void OnDelete()
        {
            string sAppend = hV.Value.Trim();
            if (!string.IsNullOrEmpty(sAppend))
            {
                string[] userNames = sAppend.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
                if (userNames.Length > 0)
                {
                    ProfileManager.DeleteProfiles(userNames);
                    OnSearch();
                }
            }
        }
    }
}
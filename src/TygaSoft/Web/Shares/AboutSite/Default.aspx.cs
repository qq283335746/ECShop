using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace TygaSoft.Web.Shares.AboutSite
{
    public partial class Default : System.Web.UI.Page
    {
        string nId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            {
                nId = HttpUtility.UrlDecode(Request.QueryString["nId"]);
            }
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            if (!Page.IsPostBack)
            {
                BLL.ContentDetail bll = new BLL.ContentDetail();
                Model.ContentDetailInfo model = bll.GetModel(nId);
                if (model != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("<div id='cTitle' class='tc'>{0}</div>", model.Title, model.ContentTypeID);
                    sb.AppendFormat("<div id='cContent' class='mt10'>{0}</div>", model.ContentText);

                    ltrContent.Text = sb.ToString();
                }
                else
                {
                    ltrContent.Text = "<div id='cTitle' class='tc'></div><div id='cContent' class='mt10'></div>";
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.IO;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.WebHelper;

namespace TygaSoft.Web.Admin.Product
{
    public partial class ListProduct : System.Web.UI.Page
    {
        string sqlWhere;
        ParamsHelper parms;
        BLL.Product bll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //数据绑定
                Bind();
            }
        }

        private void Bind()
        {
            //查询条件
            GetSearchItem();

            int totalCount = 0;
            if (bll == null) bll = new BLL.Product();

            rpData.DataSource = bll.GetDataSet(AspNetPager1.CurrentPageIndex, AspNetPager1.PageSize, out totalCount, sqlWhere, parms == null ? null : parms.ToArray()); ;
            rpData.DataBind();
            AspNetPager1.RecordCount = totalCount;
        }

        /// <summary>
        /// 获取列表查询条件项,并构建查询参数集
        /// </summary>
        private void GetSearchItem()
        {
            parms = new ParamsHelper();

            string sProductName = txtProductName.Value.Trim();
            if (!string.IsNullOrEmpty(sProductName))
            {
                sqlWhere += "and ProductName like @ProductName ";
                SqlParameter parm = new SqlParameter("@ProductName", SqlDbType.NVarChar, 256);
                parm.Value = "%" + sProductName + "%";
                parms = new ParamsHelper();
                parms.Add(parm);
            }
        }

        protected void rpData_ItemCommand(object sender, RepeaterCommandEventArgs e)
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
                case "reload":
                    Bind();
                    break;
                case "del":
                    OnDelete();
                    break;
                case "search":
                    OnSearch();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        private void OnSearch()
        {
            Bind();
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        private void OnDelete()
        {
            string itemsAppend = hV.Value.Trim();
            if (string.IsNullOrEmpty(itemsAppend))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请至少勾选一行进行操作","操作错误","error");
                return;
            }

            if (bll == null) bll = new BLL.Product();
            string[] itemsAppendArr = itemsAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string pIdAppend = "";
            List<string> list = new List<string>();
            foreach (string item in itemsAppendArr)
            {
                list.Add(item);
                pIdAppend += string.Format("'{0}',",item);
            }

            pIdAppend = pIdAppend.Trim(',');
            IList<ProductInfo> imagesList = bll.GetImagesListInProductIds(pIdAppend);

            UploadFilesHelper ufh = new UploadFilesHelper();
            string errorMsg = "";
            try
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
                options.Timeout = TimeSpan.FromSeconds(90);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,options))
                {
                    if(bll.DeleteBatch(list))
                    {
                        if (imagesList != null)
                        {
                            foreach (ProductInfo model in imagesList)
                            {
                                ufh.DeleteProductImage(model.ImagesUrl);

                                var sLImages = model.LImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in sLImages)
                                {
                                    ufh.DeleteProductImage(item);
                                }
                                var sMImages = model.MImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in sMImages)
                                {
                                    ufh.DeleteProductImage(item);
                                }
                                var sSImages = model.SImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in sSImages)
                                {
                                    ufh.DeleteProductImage(item);
                                }
                            }
                        }
                    }

                    //提交事务
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
            }

            if(!string.IsNullOrEmpty(errorMsg))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, errorMsg,"系统异常提示");
                return;
            }

            WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "操作成功");
            Bind();
        }
    }
}
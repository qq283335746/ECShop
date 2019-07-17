using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TygaSoft.CustomProviders;

namespace TygaSoft.Web.Admin.Product
{
    public partial class AddProduct : System.Web.UI.Page
    {
        BLL.Product bll;
        BLL.Category cBll;
        string nId;
        object userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            {
                nId = HttpUtility.UrlDecode(Request.QueryString["nId"]);
            }

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        ///创建验证的票据
                        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        string[] datas = userData.Split(',');
                        if (datas.Length >= 0) userId = datas[0];
                    }
                }
            }

            if (!Page.IsPostBack)
            {
                //绑定分类
                InitCategoryList(ddlCategory);

                Bind();
            }
        }

        private void Bind()
        {
            if (!string.IsNullOrEmpty(nId))
            {
                if(bll == null) bll = new BLL.Product();
                Model.ProductInfo model = bll.GetModel(nId);
                if (model != null)
                {
                    ListItem li = null;
                    li = ddlCategory.Items.FindByValue(model.CategoryId);
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                    txtProductName.Value = model.ProductName;
                    txtSubtitle.Value = model.Subtitle;
                    txtPrice.Value = model.ProductPrice.ToString();
                    txtPNum.Value = model.PNum;
                    txtStockNum.Value = model.StockNum.ToString();
                    txtMarketPrice.Value = model.MarketPrice.ToString();
                    txtPayOptions.Value = model.PayOptions;
                    hCustomAttrs.Value = model.CustomAttrs;
                    hEditor1.Value = model.Descr;
                    //商品图片
                    hPImagMain.Value = model.MainImage;
                    hUploadify.Value = model.ImagesAppend;

                    ViewState["ProductModel"] = model;
                }
            }
        }

        /// <summary>
        /// 绑定父级分类
        /// </summary>
        /// <param name="listControl"></param>
        private void InitCategoryList(ListControl listControl)
        {
            if (listControl.Items.Count > 0) listControl.Items.Clear();

            listControl.Items.Add(new ListItem("请选择", "0"));

            Dictionary<string, string> dic = null;
            if (cBll == null) cBll = new BLL.Category();

            dic = cBll.GetKeyValueByParentName("所有分类");

            if (dic != null && dic.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    ListItem li = null;
                    li = ddlCategory.Items.FindByValue(kvp.Key);
                    if (li == null)
                    {
                        string name = kvp.Value;
                        string value = kvp.Key;
                        listControl.Items.Add(new ListItem(name, value));
                        CreateSubNode(listControl, value, name);
                    }
                }
            }
        }

        /// <summary>
        /// 绑定子级分类
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="parentValue"></param>
        /// <param name="parentName"></param>
        private void CreateSubNode(ListControl listControl, string parentValue, string parentName)
        {
            if (cBll == null) cBll = new BLL.Category();
            Dictionary<string, string> dic = cBll.GetKeyValueByParentId(parentValue);

            if (dic != null)
            {
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    string name = parentName + "-/-" + kvp.Value;
                    string value = kvp.Key;
                    listControl.Items.Add(new ListItem(name, value));
                    CreateSubNode(listControl, value, name);
                }
            }
        }

        /// <summary>
        /// 按钮OnCommand事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Command(object sender, CommandEventArgs e)
        {
            hBackToN.Value = (int.Parse(hBackToN.Value.Trim()) + 1).ToString();
            string commName = hOp.Value.Trim();
            switch (commName)
            {
                case "commit":
                    OnCommit();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void OnCommit()
        {
            #region 获取输入并验证

            string sCategoryId = string.Empty;
            if(ddlCategory.SelectedIndex > 0)
            {
                sCategoryId = ddlCategory.SelectedValue;
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "所属分类为必选项");
                return;
            }
            string sProductName = txtProductName.Value.Trim();
            string sSubtitle = txtSubtitle.Value.Trim();
            string sProductPrice = txtPrice.Value.Trim();
            decimal productPrice = 0;
            if(!decimal.TryParse(sProductPrice,out productPrice))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "价格正确格式为：整数或浮点数");
                return;
            }
            
            DateTime dtNow = DateTime.Now;
            string sPNum = txtPNum.Value.Trim();
            string sStockNum = txtStockNum.Value.Trim();
            int stockNum = 0;
            if (!int.TryParse(sStockNum, out stockNum))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "库存正确格式为：整数");
                return;
            }
            string sMarketPrice = txtMarketPrice.Value.Trim();
            decimal marketPrice = 0;
            if (!decimal.TryParse(sMarketPrice, out marketPrice))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "市场价正确格式为：整数或浮点数");
                return;
            }
            string sPayOptions = txtPayOptions.Value.Trim();
            string sCustomAttrs = hCustomAttrs.Value.Trim().Trim('|');
            string sDescr = HttpUtility.HtmlDecode(hEditor1.Value);

            string sHUploadify = hUploadify.Value.Trim().Trim(',');
            string sImgmain = hPImagMain.Value.Trim();

            #endregion

            if (bll == null) bll = new BLL.Product();
            Model.ProductInfo model = new Model.ProductInfo();
            
            string sNewImgmain = sImgmain;
            string sImagesAppend = string.Empty;  //原始图片
            string sMainImage = string.Empty;     //原始商品主图片 只一个
            string sImagesUrl = string.Empty;     //产品图片 只一个
            string sLImagesUrl = string.Empty;    //产品大图
            string sMImagesUrl = string.Empty;    //产品中图
            string sSImagesUrl = string.Empty;    //产品小图
            string htmlAppend = "";
            string errorMsg = string.Empty;

            if (!string.IsNullOrEmpty(nId))
            {
                #region 修改商品时，对商品图片的操作

                if (ViewState["ProductModel"] != null)
                {
                    WebHelper.UploadFilesHelper ufh = new WebHelper.UploadFilesHelper();
                    model = (Model.ProductInfo)ViewState["ProductModel"];
                    sMainImage = model.MainImage;
                    sImagesUrl = model.ImagesUrl;
                    sImagesAppend = model.ImagesAppend;
                    sLImagesUrl = model.LImagesUrl;
                    sMImagesUrl = model.MImagesUrl;
                    sSImagesUrl = model.SImagesUrl;

                    List<string> imagesAppends = sImagesAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    List<string> lImages = sLImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    List<string> mImages = sMImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    List<string> sImages = sSImagesUrl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    List<string> newImagesAppends = sHUploadify.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

                    //取交集
                    List<string> A = newImagesAppends.Intersect(imagesAppends).ToList<string>();
                    //取差集
                    List<string> B = newImagesAppends.Except(imagesAppends).ToList<string>();
                    //取差集
                    List<string> C = imagesAppends.Except(newImagesAppends).ToList<string>();

                    try
                    {

                        if (sImgmain != sMainImage)
                        {
                            if (string.IsNullOrEmpty(sImgmain))
                            {
                                sMainImage = string.Empty;
                                sImagesUrl = string.Empty;
                            }
                            else if (sImgmain.IndexOf("Product") > -1)
                            {
                                sMainImage = sImgmain;
                                sImagesUrl = ufh.GetProductImgMain(sImgmain);
                            }
                            else
                            {
                                string pItemUrl = ufh.FromTempToProduct("~" + sImgmain);
                                htmlAppend += pItemUrl + ",";
                                sNewImgmain = pItemUrl;
                                imagesAppends.Add(pItemUrl);
                                string[] itemthumbnailImages = ufh.GetProductThumbnailImages(pItemUrl);
                                lImages.Add(itemthumbnailImages[1]);
                                mImages.Add(itemthumbnailImages[2]);
                                sImages.Add(itemthumbnailImages[3]);
                            }
                        }

                        foreach (string item in B)
                        {
                            if (item.Trim() != sImgmain)
                            {
                                string pItemUrl = ufh.FromTempToProduct("~" + item);
                                imagesAppends.Add(pItemUrl);
                                string[] itemthumbnailImages = ufh.GetProductThumbnailImages(pItemUrl);
                                lImages.Add(itemthumbnailImages[1]);
                                mImages.Add(itemthumbnailImages[2]);
                                sImages.Add(itemthumbnailImages[3]);
                            }
                        }
                        foreach (string item in C)
                        {
                            imagesAppends.Remove(item);
                            string fileName = VirtualPathUtility.GetFileName(item);
                            string sExtension = VirtualPathUtility.GetExtension(item);
                            string dirName = VirtualPathUtility.GetDirectory(item);
                            string parentPath = dirName + fileName.Replace(sExtension, "") + "/";
                            lImages.Remove(lImages.Find(delegate(string m) { return VirtualPathUtility.GetDirectory(m) == parentPath; }));
                            mImages.Remove(lImages.Find(delegate(string m) { return VirtualPathUtility.GetDirectory(m) == parentPath; }));
                            sImages.Remove(lImages.Find(delegate(string m) { return VirtualPathUtility.GetDirectory(m) == parentPath; }));

                            ufh.DeleteProductImage("~" + item);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                    }

                    if (B.Count > 0 || C.Count > 0)
                    {
                        sImagesAppend = "";
                        sLImagesUrl = "";
                        sMImagesUrl = "";
                        sSImagesUrl = "";
                        foreach (string item in imagesAppends)
                        {
                            sImagesAppend += item + ",";
                            htmlAppend += item + ",";
                        }
                        foreach (string item in lImages)
                        {
                            sLImagesUrl += item + ",";
                        }
                        foreach (string item in mImages)
                        {
                            sMImagesUrl += item + ",";
                        }
                        foreach (string item in sImages)
                        {
                            sSImagesUrl += item + ",";
                        }
                        sImagesAppend = sImagesAppend.Trim(',');
                        sLImagesUrl = sLImagesUrl.Trim(',');
                        sMImagesUrl = sMImagesUrl.Trim(',');
                        sSImagesUrl = sSImagesUrl.Trim(',');
                    }

                }
                #endregion
            }
            else
            {
                #region 新增商品时，对商品图片操作

                if (!string.IsNullOrEmpty(sHUploadify))
                {
                    WebHelper.UploadFilesHelper ufh = new WebHelper.UploadFilesHelper();
                    string[] items = sHUploadify.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    try
                    {
                        sNewImgmain = ufh.FromTempToProduct("~" + sNewImgmain);
                        string[] thumbnailImages = ufh.GetProductThumbnailImages(sNewImgmain);
                        sMainImage = sNewImgmain;
                        sImagesUrl = thumbnailImages[0];
                        sLImagesUrl += thumbnailImages[1] + ",";
                        sMImagesUrl += thumbnailImages[2] + ",";
                        sSImagesUrl += thumbnailImages[3] + ",";
                        htmlAppend += sNewImgmain + ",";
                        sImagesAppend += sNewImgmain + ",";

                        foreach (string item in items)
                        {
                            if (item.Trim() != sImgmain)
                            {
                                string pItemUrl = ufh.FromTempToProduct("~" + item);
                                sImagesAppend += pItemUrl + ",";
                                htmlAppend += pItemUrl + ",";
                                string[] itemthumbnailImages = ufh.GetProductThumbnailImages(pItemUrl);
                                sLImagesUrl += itemthumbnailImages[1] + ",";
                                sMImagesUrl += itemthumbnailImages[2] + ",";
                                sSImagesUrl += itemthumbnailImages[3] + ",";
                            }
                        }

                        sImagesAppend = sImagesAppend.Trim(',');
                        htmlAppend = htmlAppend.Trim(',');
                        sLImagesUrl = sLImagesUrl.Trim(',');
                        sMImagesUrl = sMImagesUrl.Trim(',');
                        sSImagesUrl = sSImagesUrl.Trim(',');
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                    }
                }

                #endregion
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, errorMsg,"系统异常提醒");
                return;
            }
            hUploadify.Value = htmlAppend;
            hPImagMain.Value = sNewImgmain;

            model.CategoryId = sCategoryId;
            model.ProductName = sProductName;
            model.Subtitle = sSubtitle;
            model.ProductPrice = productPrice;
            model.ImagesUrl = sImagesUrl;
            model.CreateDate = dtNow;
            model.PNum = sPNum;
            model.StockNum = stockNum;
            model.ImagesAppend = sImagesAppend;
            model.MainImage = sMainImage;
            model.LImagesUrl = sLImagesUrl;
            model.MImagesUrl = sMImagesUrl;
            model.SImagesUrl = sSImagesUrl;
            model.MarketPrice = marketPrice;
            model.PayOptions = sPayOptions;
            model.CustomAttrs = sCustomAttrs;
            model.Descr = sDescr;

            int result = -1;
            if (!string.IsNullOrEmpty(nId))
            {
                model.ProductId = nId;
                result = bll.Update(model);
            }
            else
            {
                model.UserId = userId;
                result = bll.Insert(model);
            }

            if (result == 110)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "已存在相同记录！");
                return;
            }

            if (result > 0)
            {
                WebHelper.MessageBox.MessagerShow(this.Page, lbtnPostBack, "提交成功！", "ListProduct.aspx");
            }
            else
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "提交失败,系统异常！","系统提示");
            }
        }
    }
}
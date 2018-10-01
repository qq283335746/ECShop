using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.CustomProviders;
using System.Text.RegularExpressions;

namespace TygaSoft.Web.Users
{
    public partial class AddAddress : System.Web.UI.Page
    {
        Guid nId;
        CustomProfileCommon profile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["nId"]))
            {
                if (!Guid.TryParse(Request.QueryString["nId"], out nId))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "非法操作，已终止执行","操作错误","error");
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                ////动态加载css和script
                //WebHelper.PageHelper.LoadHeaderForUsers(Page, ltrTheme);

                Bind();
            }
        }

        private void Bind()
        {
            if (!nId.Equals(Guid.Empty))
            {
                if (profile == null) profile = new CustomProfileCommon();
                Model.UserAddressInfo model = profile.UserAddress.GetModel(nId);
                if (model != null)
                {
                    txtReceiver.Value = model.Receiver;
                    txtProvinceCity.Value = model.ProvinceCity;
                    txtAddress.Value = model.Address;
                    txtMobilephone.Value = model.Mobilephone;
                    txtTelephone.Value = model.Telephone;
                    txtEmail.Value = model.Email;
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

        private void OnCommit()
        {
            bool hasSucceed = false;
            if (profile == null) profile = new CustomProfileCommon();
            if (!nId.Equals(Guid.Empty))
            {
                List<Model.UserAddressInfo> list = profile.UserAddress.GetList();
                Model.UserAddressInfo updateModel = list.Find(delegate(Model.UserAddressInfo m) { return m.NumberID == nId; });
                if (updateModel != null)
                {
                    if (!OnCheckForm(ref updateModel))
                    {
                        return;
                    }
                    profile.UserAddress.Update(updateModel);
                    hasSucceed = true;
                }
            }
            else
            {
                if (profile.UserAddress.Count >= 5)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "当前地址数量已达上限（5个），若要继续添加新地址，请先删除部分收货地址。","操作错误","error");
                    return;
                }
                List<Model.UserAddressInfo> list = profile.UserAddress.GetList();

                Model.UserAddressInfo model = new Model.UserAddressInfo();
                if (!OnCheckForm(ref model))
                {
                    return;
                }
                Model.UserAddressInfo model2 = list.Find(delegate(Model.UserAddressInfo m)
                {
                    return (m.Receiver == model.Receiver && m.ProvinceCity == model.ProvinceCity && m.Address == model.Address && m.Mobilephone == model.Mobilephone && m.Telephone == model.Telephone && m.Email == model.Email);
                });
                if (model2 != null)
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "已存在相同记录，请检查");
                    return;
                }

                model.NumberID = Guid.NewGuid();

                profile.UserAddress.Insert(model);
                hasSucceed = true;
            }

            if (hasSucceed)
            {
                profile.Save();
                WebHelper.MessageBox.Show(this.Page, lbtnPostBack, "操作成功", "ListAddress.aspx");
            }
        }

        private bool OnCheckForm(ref Model.UserAddressInfo model)
        {
            #region 获取输入并验证
            string sReceiver = txtReceiver.Value.Trim();
            if (string.IsNullOrEmpty(sReceiver))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请您填写收货人姓名","操作错误","error");
                return false;
            }
            string sProvinceCity = txtProvinceCity.Value.Trim();
            if (string.IsNullOrEmpty(sProvinceCity))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请选择所在地区", "操作错误", "error");
                return false;
            }
            string[] pArr = sProvinceCity.Split('-');
            if (pArr.Length != 3)
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请正确选择所在地区中的省市区", "操作错误", "error");
                return false;
            }
            string sAddress = txtAddress.Value.Trim();
            if (string.IsNullOrEmpty(sAddress))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请填写详细地址", "操作错误", "error");
                return false;
            }
            string sMobilephone = txtMobilephone.Value.Trim();
            string sTelephone = txtTelephone.Value.Trim();
            if (string.IsNullOrEmpty(sMobilephone) && string.IsNullOrEmpty(sTelephone))
            {
                WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "手机或固定电话至少填写一个", "操作错误", "error");
                return false;
            }

            Regex r = null;
            if (!string.IsNullOrEmpty(sMobilephone))
            {
                r = new Regex(@"^1[3|4|5|8][0-9]\d{4,8}$");
                if (!r.IsMatch(sMobilephone))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请输入正确的手机号码格式", "操作错误", "error");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(sTelephone))
            {
                r = new Regex(@"(\(\d{3,4}\)|\d{3,4}-|\s)?\d{8}");
                if (!r.IsMatch(sTelephone))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请输入正确的电话号码格式", "操作错误", "error");
                    return false;
                }
            }

            string sEmail = txtEmail.Value.Trim();
            if (!string.IsNullOrEmpty(sEmail))
            {
                r = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                if (!r.IsMatch(sEmail))
                {
                    WebHelper.MessageBox.Messager(this.Page, lbtnPostBack, "请输入正确的电子邮箱格式", "操作错误", "error");
                    return false;
                }
            }

            model.Receiver = sReceiver;
            model.ProvinceCity = sProvinceCity;
            model.Address = sAddress;
            model.Mobilephone = sMobilephone;
            model.Telephone = sTelephone;
            model.Email = sEmail;
            model.IsDefault = false;
            model.LastUpdatedDate = DateTime.Now;

            #endregion

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMS.Inner.Coffee.Service.Model;
using System.Collections.Specialized;
using XMS.Core.Web;
using System.Web;
using XMS.Core;

namespace Business
{
    public class BasePage : System.Web.UI.Page
    {
        //public BasePage() { }

        public BasePage(string toUrl) { ToUrl = toUrl; }


        #region "变量"
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string ToUrl = string.Empty;
             
        private string _openId = string.Empty;
        private string _name = string.Empty;
        private string _weixinAccount = string.Empty;
        private string _platformUserToken = string.Empty;
        private string _platformUserId = string.Empty;
        private string _phoneNumber = string.Empty;
        private MemberWeiXinDTO _objMemberWeiXinDTO = null;
        public MemberWeiXinDTO CurrentMemberWeiXinDTO
        {
            get
            {
                if (_objMemberWeiXinDTO == null)
                {
                    _objMemberWeiXinDTO = GetMemberWeiXin();
                }
                return _objMemberWeiXinDTO;
            }

        }
        #endregion

        /// <summary>
        /// 页面初始化前
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            HttpCookie objCookie = HttpContext.Current.GetCookie(Constants.cookieName_UserInfo);
            if (objCookie != null && objCookie.Values != null && !string.IsNullOrWhiteSpace(objCookie.Values["oi"]))
            {
                NameValueCollection nvc = objCookie.Values;
                _openId = nvc["oi"].DoTrim();
                _name = nvc["name"].DoTrim();
                _weixinAccount = nvc["wa"].DoTrim();
                _platformUserToken = nvc["pfut"].DoTrim();
                _platformUserId = nvc["pfui"].DoTrim();
                _phoneNumber = nvc["pn"].DoTrim();
            }
            else
            {
                WCFClient.LoggerService.Info("获取微信OpenId 开始");
                string code = Request["code"];
                string state = Request["state"];
                if (string.IsNullOrEmpty(code))
                {
                    if (string.IsNullOrWhiteSpace(ToUrl))
                        return;
                    string url = WXHelper.GetAuthorizeCodeUrl(true, ToUrl);
                    WCFClient.LoggerService.Info("获取微信OpenId URl" + url);
                    Response.Redirect(url);
                }
                else
                {
                    GetOpenIdResponse ResponseWithOpenId = WXHelper.GetOpenidInfoFromCode(code);
                    WCFClient.LoggerService.Info("获取微信OpenId code" + code);
                    WCFClient.LoggerService.Info("获取微信OpenId state" + state);
                    if (ResponseWithOpenId != null)
                    {
                        try
                        {
                            _openId = ResponseWithOpenId.openid;
                            _name = ResponseWithOpenId.openid;
                            _weixinAccount = string.Empty;
                            _platformUserToken = string.Empty;
                            _platformUserId = string.Empty;
                            _phoneNumber = string.Empty;

                            WCFClient.LoggerService.Info("获取微信OpenId OpenId" + _openId);

                            NameValueCollection nvc = new NameValueCollection();
                            nvc["oi"] = _openId;
                            nvc["name"] = _name;
                            nvc["wa"] = _weixinAccount;
                            nvc["pfut"] = _platformUserToken;
                            nvc["pfui"] = _platformUserId;
                            nvc["pn"] = _phoneNumber;
                            HttpContext.Current.AddCookie(Constants.cookieName_UserInfo, nvc, null, HttpContext.Current.Request.Url.Host);
                        }
                        catch (Exception ex)
                        {
                            Container.LogService.Error(ex.ToString());
                        }
                    }
                }
                WCFClient.LoggerService.Info("获取微信OpenId 结束");
            }
        }

        /// <summary>
        /// 获取用户微信信息
        /// </summary>
        /// <returns></returns>
        private MemberWeiXinDTO GetMemberWeiXin()
        {
            //_openId = "oJd-wt4zAf-m9a-dsNWlUwTGdZFQ";
            //_openId = "o9j1twZEAPDUCCRIwKa7Wgp6bDXE";

            if (string.IsNullOrWhiteSpace(_openId))
                return null;

            XMS.Core.ReturnValue<QueryResultCMemberWeiXinDTO> resultMemberWeiXin = WCFClient.CoffeeService.GetMemberWeiXins(null, null, _openId, null, null, 1, 1);
            if (resultMemberWeiXin.Code != 200)
            {
                WCFClient.LoggerService.Error(string.Format("获取微信用户错误。微信ID：{0} 错误信息：{1}", _openId, resultMemberWeiXin.RawMessage));
                return null;
            }

            MemberWeiXinDTO memberWeiXinDTO = null;
            if (resultMemberWeiXin.Value != null && resultMemberWeiXin.Value.Items != null && resultMemberWeiXin.Value.Items.Length > 0)
            {
                memberWeiXinDTO = new MemberWeiXinDTO()
                {
                    Id = resultMemberWeiXin.Value.Items[0].id,
                    MemberUUID = resultMemberWeiXin.Value.Items[0].memberUUID,
                    Name = resultMemberWeiXin.Value.Items[0].name,
                    PhoneNumber = resultMemberWeiXin.Value.Items[0].phoneNumber,
                    PlatformUserId = resultMemberWeiXin.Value.Items[0].platformUserId,
                    PlatformUserToken = resultMemberWeiXin.Value.Items[0].platformUserToken,
                    WeixinAccount = resultMemberWeiXin.Value.Items[0].weixinAccount,
                    WeiXinOpenId = resultMemberWeiXin.Value.Items[0].weiXinOpenId
                };

                return memberWeiXinDTO;
            }

            CMemberWeiXinDTO CMemberWeiXinDTO = new XMS.Inner.Coffee.Service.Model.CMemberWeiXinDTO()
            {
                CreateName = _name,
                CreateTime = DateTime.Now,
                name = _name,
                weiXinOpenId = _openId,
                weixinAccount = _weixinAccount,
                isDelete = false,
                platformUserToken = _platformUserToken,
                platformUserId = _platformUserId,
                memberUUID = System.Guid.NewGuid().ToString(),
                phoneNumber = _phoneNumber,
                UpdateName = _name,
                UpdateTime = DateTime.Now,
            };

            XMS.Core.ReturnValue<int> resultAddMemberWeiXin = WCFClient.CoffeeService.AddOrUpdateMemberWeiXin(CMemberWeiXinDTO, _name);
            if (resultAddMemberWeiXin.Code != 200)
            {
                WCFClient.LoggerService.Error(string.Format("添加微信用户错误。微信ID：{0} 错误信息：{1}", _openId, resultAddMemberWeiXin.RawMessage));
                return null;
            }

            memberWeiXinDTO = new MemberWeiXinDTO()
            {
                Id = resultAddMemberWeiXin.Value,
                WeiXinOpenId = CMemberWeiXinDTO.weiXinOpenId,
                WeixinAccount = CMemberWeiXinDTO.weixinAccount,
                PlatformUserToken = CMemberWeiXinDTO.platformUserToken,
                PlatformUserId = CMemberWeiXinDTO.platformUserId,
                PhoneNumber = CMemberWeiXinDTO.phoneNumber,
                MemberUUID = CMemberWeiXinDTO.memberUUID,
                Name = CMemberWeiXinDTO.name
            };

            return memberWeiXinDTO;
        }
    }

    [Serializable]
    public class MemberWeiXinDTO 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// UUID
        /// </summary>
        public string MemberUUID { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string WeiXinOpenId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 平台会员Id
        /// </summary>
        public string PlatformUserId { get; set; }

        /// <summary>
        /// 平台会员Token
        /// </summary>
        public string PlatformUserToken { get; set; }

        /// <summary>
        /// 平台微信账号
        /// </summary>
        public string WeixinAccount { get; set; }

    }
}

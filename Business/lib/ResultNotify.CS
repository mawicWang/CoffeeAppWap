using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.IO;
using System.Text;
using System.Xml;
using XMS.Inner.Coffee.Service.Model;

namespace Business.lib
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        public ResultNotify(Page page):base(page)
        {
        }

        public override void ProcessNotify()
        {
            string orderUUID = page.Request["OrderUUID"];
            string payType = page.Request["PayType"];
            WxPayData notifyData = GetNotifyData();
            WxPayData res = new WxPayData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                WCFClient.LoggerService.Error(this.GetType().ToString(), "The Pay result is error : " + notifyData.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            ////查询订单，判断订单真实性
            //if (!QueryOrder(transaction_id))
            //{
            //    //若订单查询失败，则立即返回结果给微信支付后台
            //    WxPayData res = new WxPayData();
            //    res.SetValue("return_code", "FAIL");
            //    res.SetValue("return_msg", "订单查询失败");
            //    WCFClient.LoggerService.Error(this.GetType().ToString(), "Order query failure : " + notifyData.ToXml());
            //    page.Response.Write(res.ToXml());
            //    page.Response.End();
            //}
            ////查询订单成功
            //else
            //{
            string outTradeNo = notifyData.GetValue("out_trade_no").ToString();

            Dictionary<string, string> dicData = new Dictionary<string, string>();
            dicData = InitAndCheckXML(notifyData);
            dicData["xml"] = notifyData.ToXml();
            ProcessPayNotifyRequestDto processPayNotifyRequest = new ProcessPayNotifyRequestDto()
            {
                PayMode = int.Parse(payType),
                PayOrderId = outTradeNo,
                ProductId = AppSettingHelper.ProductId,
                SaleChannelApplicationId = AppSettingHelper.SaleChannelApplicationId,
                Parameters = dicData
            };
            XMS.Core.ReturnValue<PayNotifyResult> payNotifyResult = WCFClient.CoffeeService.ProcessPayNotify(processPayNotifyRequest);
            if (payNotifyResult.Code != 200 || payNotifyResult.Value.HasError == true)
            {
                WCFClient.LoggerService.Error(this.GetType().ToString(), string.Format("提交支付状态错误，详细信息：{0},微信返回数据：{1}", payNotifyResult.RawMessage, notifyData.ToXml()));
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "提交支付状态错误");
                page.Response.Write(res.ToXml());
                page.Response.End();
                return;
            }

            //返回对象成功
            if ("SUCCESS".Equals(notifyData.GetValue("return_code").ToString()) && "SUCCESS".Equals(notifyData.GetValue("result_code").ToString()) && payNotifyResult.Value.HasError == false)
            {
                WCFClient.LoggerService.Error(this.GetType().ToString(), string.Format("此订单已支付。商户订单ID：{0}", outTradeNo));
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "此订单已支付");
                page.Response.Write(res.ToXml());
                return;
            }
            else
            {
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中未交易成功");
                WCFClient.LoggerService.Info(this.GetType().ToString(), "支付结果中未交易成功 : " + notifyData.ToXml());
            }

            page.Response.Write(res.ToXml());
            page.Response.End();
        }

        private Dictionary<string, string> InitAndCheckXML(WxPayData notifyData)
        {
            notifyData.GetValue("transaction_id").ToString();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (notifyData.IsSet("appid"))
                dic["appid"] = notifyData.GetValue("appid").ToString();
            if (notifyData.IsSet("openid"))
                dic["openid"] = notifyData.GetValue("openid").ToString();
            if (notifyData.IsSet("time_end"))
                dic["timestamp"] = notifyData.GetValue("time_end").ToString();
            if (notifyData.IsSet("nonce_str"))
                dic["noncestr"] = notifyData.GetValue("nonce_str").ToString();
            if (notifyData.IsSet("sign"))
                dic["appsignature"] = notifyData.GetValue("sign").ToString();
            if (notifyData.IsSet("is_subscribe"))
                dic["issubscribe"] = notifyData.GetValue("is_subscribe").ToString();
            return dic;
        }
    }
}
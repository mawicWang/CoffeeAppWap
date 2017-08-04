using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using XMS.Inner.Coffee.Service.Model;

public partial class OrderDetail : BasePage
{
    public OrderDetail()
        : base(string.Format("http://{0}/AppWapCoffee/OrderDetail", AppSettingHelper.DomainName))
    { }

    protected COrderDTO orderInfo = null;
    protected CRestaurantDTO restaurant = null;
    protected string tel = string.Empty;
    protected string distributionTel = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return;
            }

            int orderId = int.Parse(Request["id"]);

            XMS.Core.ReturnValue<COrderDTO> resultQueryResult = WCFClient.CoffeeService.GetOrderInfo(orderId, CurrentMemberWeiXinDTO.Id);
            if (resultQueryResult.Code != 200 || resultQueryResult.Value == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultQueryResult.RawMessage));
                return;
            }

            orderInfo = resultQueryResult.Value;
            distributionTel = orderInfo.distributionMobile;
            XMS.Core.ReturnValue<QueryResultCRestaurantDTO> restResult = WCFClient.CoffeeService.GetRestaurantDTOByCondition(new string[] { orderInfo.resUUID }, null, null,
                null, null, null, null, 1, 1, true, null);
            if (restResult.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(restResult.RawMessage));
            }

            if (restResult.Value != null && restResult.Value.Items != null && restResult.Value.Items.Length > 0)
            {
                restaurant = restResult.Value.Items[0];
                if (!string.IsNullOrWhiteSpace(restaurant.contactNumber))
                {
                    string[] tels = restaurant.contactNumber.Split(new char[] { ';' });
                    tel = tels[0];
                }
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = resultQueryResult.Value;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("获取订单列表错误,详细情况:{0}", ex.Message));
        }
    }
}
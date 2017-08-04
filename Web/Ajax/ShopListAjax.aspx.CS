using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using XMS.Inner.Coffee.Service.Model;

public partial class Ajax_ShopListAjax : BasePage
{
    public Ajax_ShopListAjax()
        : base(string.Format("http://{0}/AppWapCoffee/ShopList", AppSettingHelper.DomainName))
    { }

    protected void Page_Load(object sender, EventArgs e)
    {
        string memthod = Request["method"] + "";
        switch (memthod)
        {
            //获取用户最后下单地址
            case "GetLastAddress":
                Response.Write(GetLastAddress());
                break;
            //获取餐厅列表（配送范围内）
            case "GetShopList":
                Response.Write(GetShopList());
                break;
            //获取获取餐厅列表（全部餐厅）
            case "GetShopListContainOutRange":
                Response.Write(GetShopListContainOutRange());
                break;
            //默认
            default:
                break;
        }
    }


    /// <summary>
    /// 获取用户最后下单地址
    /// </summary>
    /// <returns></returns>
    private string GetLastAddress()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        if (CurrentMemberWeiXinDTO == null)
        {
            messageInfo.Message = "未获取到微信用户信息";
            WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        XMS.Core.ReturnValue<CMemberAddressDTO> addressResult = WCFClient.CoffeeService.GetLastMemberAddress(CurrentMemberWeiXinDTO.Id);
        if (addressResult.Code != 200)
        {
            messageInfo.Message = "网络异常稍后再试";
            WCFClient.LoggerService.Error(string.Format(addressResult.RawMessage));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        messageInfo.Status = 0;
        messageInfo.Message = "success";
        messageInfo.Data = addressResult.Value;
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 获取餐厅列表（配送范围内）
    /// </summary>
    /// <returns></returns>
    private string GetShopList()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        if (CurrentMemberWeiXinDTO == null)
        {
            messageInfo.Message = "未获取到微信用户信息";
            WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        decimal lan = decimal.Parse(Request["latitude"]);
        decimal lat = decimal.Parse(Request["longitude"]);

        XMS.Core.ReturnValue<QueryResultCRestaurantDTO> resultRestaurant = WCFClient.CoffeeService.GetRestaurantDTOByCondition(null, null, null, null, lan, lat, null, 1, 1000, false, new int[] { 1 });
        if (resultRestaurant.Code != 200 || resultRestaurant.Value == null)
        {
            messageInfo.Message = "网络异常，获取餐厅列表错误";
            WCFClient.LoggerService.Error(string.Format("获取餐厅列表错误，详细：{0}", resultRestaurant.RawMessage));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        if (resultRestaurant.Value != null && resultRestaurant.Value.Items != null && resultRestaurant.Value.Items.Length > 0)
        {
            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = resultRestaurant.Value.Items;
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo); ;
        }

        messageInfo.Status = 0;
        messageInfo.Message = "success";
        messageInfo.Data = resultRestaurant.Value.Items;
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    private string GetShopListContainOutRange()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        if (CurrentMemberWeiXinDTO == null)
        {
            messageInfo.Message = "未获取到微信用户信息";
            WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        decimal lan = decimal.Parse(Request["latitude"]);
        decimal lat = decimal.Parse(Request["longitude"]);

        //int startIndex = int.Parse(Request["startIndex"]);
        //int pageSize = int.Parse(Request["pageSize"]);

        XMS.Core.ReturnValue<QueryResultCRestaurantDTO> resultRestaurant = WCFClient.CoffeeService.GetRestaurantDTOByCondition(null, null, null, null, lan, lat, null, 1, 1000, true, new int[] { 1 });
        if (resultRestaurant.Code != 200 || resultRestaurant.Value == null)
        {
            messageInfo.Message = "网络异常，获取餐厅列表错误";
            WCFClient.LoggerService.Error(string.Format("获取餐厅列表错误，详细：{0}", resultRestaurant.RawMessage));
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        }

        if (resultRestaurant.Value != null && resultRestaurant.Value.Items != null && resultRestaurant.Value.Items.Length > 0)
        {
            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = resultRestaurant.Value.Items;
            return XMS.Core.Json.JsonSerializer.Serialize(messageInfo); ;
        }

        messageInfo.Status = 0;
        messageInfo.Message = "success";
        messageInfo.Data = resultRestaurant.Value.Items;
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }
}
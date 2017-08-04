using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using XMS.Inner.Coffee.Service.Model;
using XMS.Core;
using XMS.Core.Json;
public partial class Ajax_AddRessListAjax : BasePage
{
    // Methods
    public Ajax_AddRessListAjax()
        : base(string.Format("http://{0}/AppWapCoffee/AddRessList", AppSettingHelper.DomainName))
    {
    }

    private string GetAddressList()
    {
        MessageInfo info = new MessageInfo
        {
            Status = 1
        };
        if (base.CurrentMemberWeiXinDTO == null)
        {
            info.Message = "未获取到微信用户信息";
            WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息", new object[0]));
            return JsonSerializer.Serialize(info);
        }
        try
        {
            int? shopId = null;
            if (!string.IsNullOrWhiteSpace(Request["shopId"]))
                shopId = int.Parse(Request["shopId"]);
           XMS.Core.ReturnValue<CMemberAddressDTO[]> memberAddressResult =  WCFClient.CoffeeService.GetAllMemberAddress(null, null, CurrentMemberWeiXinDTO.MemberUUID, null, null, null, null, null, shopId);

            //ReturnValue<QueryResultCMemberAddressDTO> value2 = WCFClient.CoffeeService.GetMemberAddress(null, base.CurrentMemberWeiXinDTO.MemberUUID, null, null, null, null, null, 1, 0x3e8);
           if ((memberAddressResult == null) || (memberAddressResult.Code != 200))
            {
                info.Message = "获取客户地址错误";
                WCFClient.LoggerService.Error(string.Format("获取客户地址错误,详细情况:{0}", memberAddressResult.RawMessage));
                return JsonSerializer.Serialize(info);
            }
            info.Status = 0;
            info.Message = "success";
            info.Data = memberAddressResult.Value;
        }
        catch (Exception exception)
        {
            info.Status = 1;
            info.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("获取客户地址错误,详细情况:{0}", exception.Message));
        }
        return JsonSerializer.Serialize(info);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string str2 = base.Request["method"] ?? "";
        XMS.Core.Container.LogService.Error("测试" + str2);
        switch (str2)
        {
            case "GetAddressList":
                base.Response.Write(this.GetAddressList());
                break;
            case "SubmitAddress":
                base.Response.Write(this.SubmitAddress());
                break;
            case "DeleteAddress":
                base.Response.Write(this.DeleteOrder());
                break;
            default:
                break;
        }
        //if (str2 != null)
        //{
        //    if (!(str2 == "GetAddressList"))
        //    {
        //        if (!(str2 == "SubmitAddress"))
        //        {
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        base.Response.Write(this.GetAddressList());
        //        return;
        //    }
        //    base.Response.Write(this.SubmitAddress());
        //}

    }


    /// <summary>
    /// 删除订单
    /// </summary>
    /// <returns></returns>
    private string DeleteOrder()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            int addressId = int.Parse(Request["Id"]);
            XMS.Core.ReturnValue<bool> resultGood = WCFClient.CoffeeService.DeleteMemberAddress(addressId, CurrentMemberWeiXinDTO.Id);
            if (resultGood.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultGood.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = resultGood.Value;

        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("删除用户地址错误,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }


    private string SubmitAddress()
    {
        MessageInfo info = new MessageInfo
        {
            Status = 1
        };
        if (base.CurrentMemberWeiXinDTO == null)
        {
            info.Message = "未获取到微信用户信息";
            WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息", new object[0]));
            return JsonSerializer.Serialize(info);
        }
        try
        {
            string str = base.Request["name"];
            int num = 0;
            try
            {
                num = int.Parse(base.Request["id"]);
            }
            catch
            {
                num = 0;
            }
            string str2 = base.Request["sex"];
            string str3 = base.Request["telephone"];
            string str4 = base.Request["addres"];
            decimal num2 = decimal.Parse(base.Request["latitude"]);
            decimal num3 = decimal.Parse(base.Request["longitude"]);
            string str5 = base.Request["houseNumber"];
            string str6 = base.Request["label"];
            AddMemberAddressDTO dto = new AddMemberAddressDTO
            {
                address = str4,
                cityId = "上海",
                houseNumber = str5,
                label = str6,
                latitude = new decimal?(num2),
                longitude = new decimal?(num3),
                memberUUID = base.CurrentMemberWeiXinDTO.MemberUUID,
                name = str,
                telephone = str3,
                id = num,
                sex = str2
            };
            ReturnValue<int> value2 = WCFClient.CoffeeService.AddOrUpdateMemberAddress(dto, "MemberUUID");
            if (((value2 == null) || (value2.Code != 200)) || (value2.Value <= 0))
            {
                info.Message = "添加客户地址错误";
                WCFClient.LoggerService.Error(string.Format("添加客户地址错误,详细情况:{0}", value2.RawMessage));
                return JsonSerializer.Serialize(info);
            }
            return this.GetAddressList();
        }
        catch (Exception exception)
        {
            info.Status = 1;
            info.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("添加客户地址错误,详细情况:{0}", exception.Message));
        }
        return JsonSerializer.Serialize(info);

    }
}

 

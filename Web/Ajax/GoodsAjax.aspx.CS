using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using XMS.Inner.Coffee.Service.Model;
using XMS.Core;

public partial class Ajax_GoodsAjax : BasePage
{
    public Ajax_GoodsAjax()
        : base(string.Format("http://{0}/AppWapCoffee/Goods", AppSettingHelper.DomainName))
    { }

    protected void Page_Load(object sender, EventArgs e)
    {
        string memthod = Request["method"] + "";
        switch (memthod)
        {
            //获取餐厅商品分类
            case "GetGoodsClassify":
                Response.Write(GetGoodsClassify());
                break;
            //根据商品分类获取商品
            case "GetGoodsByClassify":
                Response.Write(GetGoodsByClassify());
                break;
            //checkRange(检查范围)
            case "CheckRange":
                Response.Write(CheckRange());
                break;
            //getCoupon（获取优惠信息）
            case "GetCoupon":
                Response.Write(GetCoupon());
                break;
            //getGoodsList（根据餐厅获取商品）
            case "GetGoodsList":
                Response.Write(GetGoodsList());
                break;
            //getOrderList(获取订单集合)
            case "GetOrderList":
                Response.Write(GetOrderList());
                break;
            //submitOrder(提交订单)
            case "SubmitOrder":
                Response.Write(SubmitOrder());
                break;
            //检查订单支付状态
            case "CheckOrderPayStatus":
                Response.Write(CheckOrderPayStatus());
                break;
            //确认此用户是否需要手机验证
            case "IsRequiredVerification":
                Response.Write(IsRequiredVerification());
                break;
            //生成验证码
            case "CreateCode":
                Response.Write(CreateCode());
                break;
            //验证验证码
            case "CheckCode":
                Response.Write(CheckCode());
                break;
            //继续支付
            case "ContinuePay":
                Response.Write(ContinuePay());
                break;
            case "GetEffectivePeriod":
                //获取餐厅有效时间段
                Response.Write(GetEffectivePeriod());
                break;
            //删除订单
            case "DeleteOrder":
                Response.Write(DeleteOrder());
                break;
            //默认
            default:
                break;
        }
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
            int orderId = int.Parse(Request["orderId"]);
            XMS.Core.ReturnValue<bool> resultGood = WCFClient.CoffeeService.DeleteOrder(orderId, CurrentMemberWeiXinDTO.Id);
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
            WCFClient.LoggerService.Error(string.Format("根据餐厅过去商品类别,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 获取有效的营业时间段
    /// </summary>
    /// <returns></returns>
    private string GetEffectivePeriod()
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
            int resId = int.Parse(Request["shopId"]);
            XMS.Core.ReturnValue<EffectivePeriodDTO[]> resultGood = WCFClient.CoffeeService.GetEffectivePeriod(resId);
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
            WCFClient.LoggerService.Error(string.Format("根据餐厅过去商品类别,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    private string ContinuePay()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1, Data = false };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            int orderId = int.Parse(Request["orderId"]);
            XMS.Core.ReturnValue<AddOrderResultDTO> getOrderResult = WCFClient.CoffeeService.GetPayInfo(orderId, CurrentMemberWeiXinDTO.Id);
            if (getOrderResult.Code == 1000)
            {
                messageInfo.Message = getOrderResult.Message;
                WCFClient.LoggerService.Error(string.Format(getOrderResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (getOrderResult.Code != 200 || getOrderResult.Value == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(getOrderResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (getOrderResult.Value.order == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format("获取订单失败"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (getOrderResult.Value.payInfo == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format("获取支付信息失败"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            SubmitOrderResult submitOrderResult = new SubmitOrderResult()
            {
                id = getOrderResult.Value.order.id,
                payInfo = getOrderResult.Value.payInfo.payUrl
            };

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = submitOrderResult;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("获取验证码错误,详细情况:{0}", ex.Message));
        }

        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 验证验证码
    /// </summary>
    /// <returns></returns>
    private string CheckCode()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1, Data = false };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            string code = Request["code"];
            XMS.Core.ReturnValue<bool> resultCommodityInfo = WCFClient.CoffeeService.CheckCode(CurrentMemberWeiXinDTO.Id, code);
            if (resultCommodityInfo.Code == 1000)
            {
                messageInfo.Message = resultCommodityInfo.Message;
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (resultCommodityInfo.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (resultCommodityInfo.Value == false)
            {
                messageInfo.Message = "验证码无效";
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = true;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("验证验证码错误,详细情况:{0}", ex.Message));
        }

        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <returns></returns>
    private string CreateCode()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1, Data = false };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            string phoneNumber = Request["phoneNumber"];
            XMS.Core.ReturnValue<int> resultCommodityInfo = WCFClient.CoffeeService.CreateVerificationCode(CurrentMemberWeiXinDTO.Id, phoneNumber);
            if (resultCommodityInfo.Code == 1000)
            {
                messageInfo.Message = resultCommodityInfo.Message;
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (resultCommodityInfo.Code != 200 || resultCommodityInfo.Value <= 0)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = true;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("生成验证码错误,详细情况:{0}", ex.Message));
        }

        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 确认此用户是否需要手机验证
    /// </summary>
    /// <returns></returns>
    private string IsRequiredVerification()
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

              XMS.Core.ReturnValue<QueryResultCMemberWeiXinDTO> Queryresult = WCFClient.CoffeeService.GetMemberWeiXins(CurrentMemberWeiXinDTO.Id, null, null, null, null, 1, 1);
              if (Queryresult.Code == 1000)
              {
                  messageInfo.Message = Queryresult.Message;
                  WCFClient.LoggerService.Error(string.Format(Queryresult.RawMessage));
                  return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
              }

              if (Queryresult.Code != 200)
              {
                  messageInfo.Message = "网络异常，稍后重试";
                  WCFClient.LoggerService.Error(string.Format(Queryresult.RawMessage));
                  return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
              }

              if (Queryresult.Value == null || Queryresult.Value.Items == null || Queryresult.Value.Items.Length == 0)
              {
                  messageInfo.Message = "未获取到微信用户信息";
                  WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                  return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
              }
              messageInfo.Status = 0;
              messageInfo.Message = "success";
              messageInfo.Data = false;
              if (string.IsNullOrWhiteSpace(Queryresult.Value.Items[0].phoneNumber))
                  messageInfo.Data = true;
          }
          catch (Exception ex)
          {
              messageInfo.Status = 1;
              messageInfo.Message = "网络异常，稍后重试";
              WCFClient.LoggerService.Error(string.Format("判断用户是否需要验证,详细情况:{0}", ex.Message));
          }

          return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    private string GetGoodsByClassify()
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
            int id = int.Parse(Request["id"]);
            XMS.Core.ReturnValue<CommodityInfoDTO[]> resultCommodityInfo = WCFClient.CoffeeService.GetGoodsByClassify(id);
            if (resultCommodityInfo.Code == 1000)
            {
                messageInfo.Message = resultCommodityInfo.Message;
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (resultCommodityInfo.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultCommodityInfo.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = resultCommodityInfo.Value;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("根据商品类别获取商品,详细情况:{0}", ex.Message));
        }
      
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 获取支付商品类别
    /// </summary>
    /// <returns></returns>
    private string GetGoodsClassify()
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
            int resId = int.Parse(Request["shopId"]);
            XMS.Core.ReturnValue<GoodsClassifyDTO[]> resultGood = WCFClient.CoffeeService.GetGoodsClassifyDTO(resId);
            if (resultGood.Code == 1000)
            {
                messageInfo.Message = resultGood.Message;
                WCFClient.LoggerService.Error(string.Format(resultGood.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
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
            WCFClient.LoggerService.Error(string.Format("根据餐厅过去商品类别,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 检查订单支付状态
    /// </summary>
    /// <returns></returns>
    private string CheckOrderPayStatus()
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

            int orderId = int.Parse(Request["id"]);

            XMS.Core.ReturnValue<COrderPO[]> orderResult = WCFClient.CoffeeService.GetAllOrders(orderId, null, null, CurrentMemberWeiXinDTO.MemberUUID, null, null, null, null, null, null, null, null, null, null, null);
            if (orderResult.Code == 1000)
            {
                messageInfo.Message = orderResult.Message;
                WCFClient.LoggerService.Error(string.Format(orderResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            if (orderResult.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(orderResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            if (orderResult.Value == null || orderResult.Value.Length == 0)
            {
                messageInfo.Message = "此订单不存在";
                WCFClient.LoggerService.Error(string.Format("此订单不存在"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = orderResult.Value[0].PayStatus == 1 || orderResult.Value[0].PayStatus == 2 ? true : false;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("根据餐厅过去商品类别,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// submitOrder(提交订单)
    /// </summary>
    /// <returns></returns>
    private string SubmitOrder()
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

            string submitOrderInfo = Request["orderInfo"];
            if (string.IsNullOrEmpty(submitOrderInfo))
            {
                messageInfo.Message = "网络异常，稍后再试";
                WCFClient.LoggerService.Error(string.Format("未获取到提交的订单信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            //bool outOfTime = Request["outOfTime"] == "0" ? false : true;

            XMS.Core.Container.LogService.Info("orderInfo:" + submitOrderInfo);

            SubmitOrderDTO submitOrderDTO = XMS.Core.Json.JsonSerializer.Deserialize<SubmitOrderDTO>(submitOrderInfo);
            if (submitOrderDTO == null)
            {
                messageInfo.Message = "网络异常，稍后再试";
                WCFClient.LoggerService.Error(string.Format("未获取到提交的订单信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            DateTime? deliveryMaxTimeDate = null;
            DateTime? deliveryMinTimeDate = null;
            if (submitOrderDTO.deliveryMaxTime > 0)
                deliveryMaxTimeDate = submitOrderDTO.deliveryMaxTime.MilliSecondsFrom1970ToDateTime();
            if (submitOrderDTO.deliveryMinTime > 0)
                deliveryMinTimeDate = submitOrderDTO.deliveryMinTime.MilliSecondsFrom1970ToDateTime();

            List<AddOrderDTO> listOrder = new List<AddOrderDTO>();
            submitOrderDTO.commodity.ForEach(item => {
                AddOrderDTO addOrder = new AddOrderDTO()
                {
                    cityId = "上海",
                    memberId = CurrentMemberWeiXinDTO.Id,
                    commodityId = item.commodityId,
                    couponId = submitOrderDTO.couponId.HasValue ? submitOrderDTO.couponId.Value : 0,
                    deliveryMaxTime = deliveryMaxTimeDate,
                    deliveryMinTime = deliveryMinTimeDate,
                    memberAddressId = submitOrderDTO.addrssId,
                    remark = submitOrderDTO.remark,
                    serverFee = submitOrderDTO.serverFee,
                    skuList = item.skuList == null ? null : item.skuList.ToArray(),
                    resId = submitOrderDTO.resId,
                    quantity = item.quantity,
                    deliveryType = submitOrderDTO.deliveryType,
                    isOutOfTime = !submitOrderDTO.outOfTime.HasValue ? false : (submitOrderDTO.outOfTime.Value == 0 ? false : true)
                };
                listOrder.Add(addOrder);
            });
            XMS.Core.ReturnValue<AddOrderResultDTO> addOrderResult = WCFClient.CoffeeService.AddOrder(listOrder.ToArray());
            if (addOrderResult.Code != 200 || addOrderResult.Value == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(addOrderResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (addOrderResult.Value.order == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format("提交订单失败"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            if (addOrderResult.Value.payInfo == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format("获取支付信息失败"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            SubmitOrderResult submitOrderResult = new SubmitOrderResult()
            {
                id = addOrderResult.Value.order.id,
                payInfo = addOrderResult.Value.payInfo.payUrl
            };

            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = submitOrderResult;
        }
        catch(Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("提交订单错误,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// getOrderList(获取订单集合)
    /// </summary>
    /// <returns></returns>
    private string GetOrderList()
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

            int startIndex = int.Parse(Request["startIndex"]);
            int pageSize = int.Parse(Request["pageSize"]);

            XMS.Core.ReturnValue<QueryResultCOrderPO> resultQueryResult = WCFClient.CoffeeService.GetOrders(null, null, null, CurrentMemberWeiXinDTO.MemberUUID, null, null, null, null, null, null, null, null, null, null, null, startIndex, pageSize);
            if (resultQueryResult.Code != 200 || resultQueryResult.Value == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(resultQueryResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
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
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// getGoodsList（根据餐厅获取商品）
    /// </summary>
    /// <returns></returns>
    private string GetGoodsList()
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
            int resId = int.Parse(Request["shopId"]);
            XMS.Core.ReturnValue<CommodityCategoryInfoDTO[]> CommodityCategoryInfoResult = WCFClient.CoffeeService.GetCommodityCategory(resId);
            if (CommodityCategoryInfoResult.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(CommodityCategoryInfoResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = CommodityCategoryInfoResult.Value;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("根据餐厅获取商品,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 检查范围
    /// </summary>
    /// <returns></returns>
    private string CheckRange()
    {
        MessageInfo messageInfo = new MessageInfo() { Status = 1, Data = false };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            decimal lan = decimal.Parse(Request["longitude"]);
            decimal lat = decimal.Parse(Request["latitude"]);
            XMS.Core.ReturnValue<QueryResultCRestaurantDTO> restResult = WCFClient.CoffeeService.GetRestaurantDTOByCondition(null, null, null, null, lan, lat, null, 1, 1, false, new int[] { 1});
            if (restResult.Code != 200 || restResult.Value == null)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(restResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            if (restResult.Value.Items == null || restResult.Value.Items.Length == 0)
            {
                messageInfo.Status = 0;
                messageInfo.Message = "success";
                messageInfo.Data = false;
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = true;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("检查是否在配送范围内,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }

    /// <summary>
    /// 获取优惠卷信息
    /// </summary>
    /// <returns></returns>
    private string GetCoupon()
    {
        //MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        //try
        //{
        //    if (CurrentMemberWeiXinDTO == null)
        //    {
        //        messageInfo.Message = "未获取到微信用户信息";
        //        WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
        //        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        //    }

        //    int resId = int.Parse(Request["shopId"]);
        //    XMS.Core.ReturnValue<CCouponDTO[]> CouponResult = WCFClient.CoffeeService.GetNoUseCouponDTOById(resId, CurrentMemberWeiXinDTO.Id);
        //    if (CouponResult.Code != 200)
        //    {
        //        messageInfo.Message = "网络异常稍后再试";
        //        WCFClient.LoggerService.Error(string.Format(CouponResult.RawMessage));
        //        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
        //    }
        //    messageInfo.Status = 0;
        //    messageInfo.Message = "success";
        //    messageInfo.Data = CouponResult.Value;
        //}
        //catch (Exception ex)
        //{
        //    messageInfo.Status = 1;
        //    messageInfo.Message = "网络异常，稍后重试";
        //    WCFClient.LoggerService.Error(string.Format("获取优惠卷,详细情况:{0}", ex.Message));
        //}
        //return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);

        MessageInfo messageInfo = new MessageInfo() { Status = 1 };
        try
        {
            if (CurrentMemberWeiXinDTO == null)
            {
                messageInfo.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            string submitOrderInfo = Request["orderInfo"];
            if (string.IsNullOrEmpty(submitOrderInfo))
            {
                messageInfo.Message = "网络异常，稍后再试";
                WCFClient.LoggerService.Error(string.Format("未获取到提交的订单信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            XMS.Core.Container.LogService.Info("orderInfo:" + submitOrderInfo);

            Business.GetAvailableCouponDTO getAvailableCoupon = XMS.Core.Json.JsonSerializer.Deserialize<Business.GetAvailableCouponDTO>(submitOrderInfo);
            if (getAvailableCoupon == null)
            {
                messageInfo.Message = "网络异常，稍后再试";
                WCFClient.LoggerService.Error(string.Format("未获取到提交的订单信息"));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }

            XMS.Inner.Coffee.Service.Model.GetAvailableCouponDTO availableCoupon = new XMS.Inner.Coffee.Service.Model.GetAvailableCouponDTO()
            {
                resId = getAvailableCoupon.resId,
                deliveryType = getAvailableCoupon.deliveryType,
                deliveryMaxTime = DateTime.Parse("1970-01-01 08:00").AddMilliseconds(getAvailableCoupon.deliveryMaxTime),
                deliveryMinTime = DateTime.Parse("1970-01-01 08:00").AddMilliseconds(getAvailableCoupon.deliveryMinTime),
            };

            List<XMS.Inner.Coffee.Service.Model.Commodity> list = new List<XMS.Inner.Coffee.Service.Model.Commodity>();
            if (getAvailableCoupon.commodity != null && getAvailableCoupon.commodity.Count > 0)
            {
                getAvailableCoupon.commodity.ForEach(x => {
                    list.Add(new XMS.Inner.Coffee.Service.Model.Commodity
                    {
                        commodityId = x.commodityId,
                        quantity = x.quantity,
                        skuList = x.skuList == null ? null : x.skuList.ToArray()
                    });
                });
            }
            availableCoupon.commodity = list.ToArray();

            XMS.Core.ReturnValue<CCouponDTO[]> couponResult = WCFClient.CoffeeService.GetAvailableCouponDTO(availableCoupon, CurrentMemberWeiXinDTO.Id);
            if (couponResult.Code != 200)
            {
                messageInfo.Message = "网络异常稍后再试";
                WCFClient.LoggerService.Error(string.Format(couponResult.RawMessage));
                return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
            }
            messageInfo.Status = 0;
            messageInfo.Message = "success";
            messageInfo.Data = couponResult.Value;
        }
        catch (Exception ex)
        {
            messageInfo.Status = 1;
            messageInfo.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("获取优惠卷,详细情况:{0}", ex.Message));
        }
        return XMS.Core.Json.JsonSerializer.Serialize(messageInfo);
    }
}
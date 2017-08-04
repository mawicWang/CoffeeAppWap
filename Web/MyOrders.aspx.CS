using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using XMS.Inner.Coffee.Service.Model;
using XMS.Core;
public partial class MyOrders : BasePage
{
    // Fields
    protected List<OrderInfo> listOrderInfo;

    // Methods
    public MyOrders()
        : base(string.Format("http://{0}/AppWapCoffee/MyOrders", AppSettingHelper.DomainName))
    {
        this.listOrderInfo = new List<OrderInfo>();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        MessageInfo info = new MessageInfo
        {
            Status = 1
        };
        try
        {
            if (base.CurrentMemberWeiXinDTO == null)
            {
                info.Message = "未获取到微信用户信息";
                WCFClient.LoggerService.Error(string.Format("未获取到微信用户信息", new object[0]));
            }
            else
            {
                XMS.Core.ReturnValue<QueryResultCOrderPO> value2 = WCFClient.CoffeeService.GetOrders(null, null, null, base.CurrentMemberWeiXinDTO.MemberUUID, null, null, null, null, null, null, null, null, null, null, null, 1, 0x3e8);
                if (((value2.Code != 200) || (value2.Value == null)) || ((value2.Value.Items == null) || (value2.Value.Items.Length == 0)))
                {
                    info.Message = "网络异常稍后再试";
                    WCFClient.LoggerService.Error(string.Format(value2.RawMessage, new object[0]));
                }
                else
                {
                    List<string> list = (from x in value2.Value.Items select x.ResUUID).Distinct<string>().ToList<string>();
                    ReturnValue<QueryResultCRestaurantDTO> value3 = WCFClient.CoffeeService.GetRestaurantDTOByCondition(list.ToArray(), null, null, null, null, null, null, 1, list.Count, true, new int[] { 1 });
                    if (value3.Code != 200)
                    {
                        info.Message = "网络异常稍后再试";
                        WCFClient.LoggerService.Error(string.Format(value3.RawMessage, new object[0]));
                    }
                    List<CRestaurantDTO> source = new List<CRestaurantDTO>();
                    if (((value3.Value != null) && (value3.Value.Items != null)) && (value3.Value.Items.Length > 0))
                    {
                        source.AddRange(value3.Value.Items);
                    }
                    COrderPO[] items = value2.Value.Items;
                    for (int i = 0; i < items.Length; i++)
                    {
                        Func<CRestaurantDTO, bool> predicate = null;
                        COrderPO item = items[i];
                        if (predicate == null)
                        {
                            predicate = x => x.resUUID == item.ResUUID;
                        }
                        CRestaurantDTO tdto = source.FirstOrDefault<CRestaurantDTO>(predicate);
                        OrderInfo info2 = new OrderInfo
                        {
                            ResAddress = (tdto != null) ? tdto.address : string.Empty,
                            CityId = (tdto != null) ? tdto.cityId : string.Empty,
                            DeliveryAddress = item.DeliveryAddress,
                            DeliveryType = (item.DeliveryType == 1) ? "门店自取" : "配送",
                            ResImgUrl = (tdto != null) ? tdto.imgUrl : string.Empty,
                            MemberName = item.MemberName,
                            ResName = (tdto != null) ? tdto.name : string.Empty,
                            OrderMomey = item.OrderMomey,
                            OrderState = item.OrderState,
                            OrderTime = item.OrderTime,
                            PayDateTime = item.PayDateTime,
                            PayMomey = item.PayMomey,
                            PayStatus = item.PayStatus,
                            PayType = item.PayType,
                            ResUUID = item.ResUUID,
                            ServerFee = item.ServerFee,
                            Telephone = item.Telephone,
                            Id = item.Id
                        };
                        this.listOrderInfo.Add(info2);
                    }
                    info.Status = 0;
                    info.Message = "success";
                    info.Data = value2.Value;
                }
            }
        }
        catch (Exception exception)
        {
            info.Status = 1;
            info.Message = "网络异常，稍后重试";
            WCFClient.LoggerService.Error(string.Format("获取订单列表错误,详细情况:{0}", exception.Message));
        }

    }

    // Nested Types
    [Serializable]
    public class OrderInfo
    {
        // Methods

        // Properties
        public string CityId { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryType { get; set; }
        public int Id { get; set; }
        public string MemberName { get; set; }
        public decimal OrderMomey { get; set; }
        public int OrderState { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? PayDateTime { get; set; }
        public decimal? PayMomey { get; set; }
        public int? PayStatus { get; set; }
        public int? PayType { get; set; }
        public string ResAddress { get; set; }
        public string ResImgUrl { get; set; }
        public string ResName { get; set; }
        public string ResUUID { get; set; }
        public decimal ServerFee { get; set; }
        public string Telephone { get; set; }
    }
}

 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.lib;

public partial class PayWeiXinNotify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string orderUUID = Request["OrderUUID"];
        WCFClient.LoggerService.Info("微信回调成功（OrderUUID）：" + (string.IsNullOrWhiteSpace(orderUUID) ? string.Empty : orderUUID));

        lock (LockObject.Instance)
        {
            ResultNotify resultNotify = new ResultNotify(this);
            resultNotify.ProcessNotify();
        }
    }
}
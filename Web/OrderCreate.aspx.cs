using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;

public partial class OrderCreate : BasePage
{
    public OrderCreate()
        : base(string.Format("http://{0}/AppWapCoffee/OrderCreate", AppSettingHelper.DomainName))
    { }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
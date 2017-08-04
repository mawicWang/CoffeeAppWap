using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;

public partial class Goods : BasePage
{
    public Goods()
        : base(string.Format("http://{0}/AppWapCoffee/Goods", AppSettingHelper.DomainName))
    { }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerRelationshipModule
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var data = (List<dynamic>)((dynamic)e.Item.DataItem).ChildMenu;
            var repeater2 = (Repeater)e.Item.FindControl("rptSubMenu");
            repeater2.DataSource = data;
            repeater2.DataBind();
        }
    }
}
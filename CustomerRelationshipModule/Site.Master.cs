using Data.ORMHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerRelationshipModule
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public static string currentUserId { get; set; }
        public static string currentUserType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ArrayList menulist = new ArrayList();
                int id = 0;

                var siteMapNodes = SiteMap.Provider.RootNode.ChildNodes;
                foreach (SiteMapNode node in siteMapNodes)
                {
                    if (node["module"] == "*")
                    {
                        var mainMenu = new { id = "menuitem-" + id++, url = node.Url, title = node.Title, icon = node.ResourceKey, hasChuild = false, ChildMenu = new List<dynamic>() };
                        menulist.Add(mainMenu);
                    }

                    if (!string.IsNullOrEmpty(currentUserId))
                    {
                        if (currentUserType == "Employee")
                        {
                            if (new EmployeeHelper().IsAdmin(currentUserId) && node["module"].Contains("Admin"))
                            {
                                var mainMenu = new { id = "menuitem-" + id++, url = node.Url, title = node.Title, icon = node.ResourceKey, hasChuild = false, ChildMenu = new List<dynamic>() };
                                menulist.Add(mainMenu);
                            }
                            else if (node["module"].Contains("Employee"))
                            {
                                var mainMenu = new { id = "menuitem-" + id++, url = node.Url, title = node.Title, icon = node.ResourceKey, hasChuild = false, ChildMenu = new List<dynamic>() };
                                menulist.Add(mainMenu);
                            }
                        }
                        else if (node["module"].Contains("Customer"))
                        {
                            var mainMenu = new { id = "menuitem-" + id++, url = node.Url, title = node.Title, icon = node.ResourceKey, hasChuild = false, ChildMenu = new List<dynamic>() };
                            menulist.Add(mainMenu);
                        }
                    }
                }

                menu.DataSource = menulist;
                menu.DataBind();
            }
        }

        protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //var data = (List<dynamic>)((dynamic)e.Item.DataItem).ChildMenu;
            //var repeater2 = (Repeater)e.Item.FindControl("menu");
            //repeater2.DataSource = data;
            //repeater2.DataBind();
        }
    }
}
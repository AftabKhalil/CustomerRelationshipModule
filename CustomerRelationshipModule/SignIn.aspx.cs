using CustomerRelationshipModule.Models;
using Data.ORMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerRelationshipModule
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetSystemId()
        {
            var emailId = HttpContext.Current.Request.Params["emailId"];
            var password = HttpContext.Current.Request.Params["password"];
            var isCustomer = HttpContext.Current.Request.Params["isCustomer"];

            var result = new Response<Models.Employee>();
            try
            {
                var systemId = "";

                if (isCustomer.ToLower() == "false")
                    systemId = new EmployeeHelper().GetSystemId(emailId, password);

                if (string.IsNullOrEmpty(systemId))
                {
                    result.isSuccess = false;
                }
                else
                {
                    result.isSuccess = true;
                    result.data = new Models.Employee()
                    {
                        systemId = systemId
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

    }
}
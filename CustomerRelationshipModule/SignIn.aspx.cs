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

            var result = new Response<Object>();
            try
            {
                var currentUserId = "";
                var currentUserType = "";

                if (isCustomer.ToLower() == "false")
                {
                    currentUserId = new EmployeeHelper().GetSystemId(emailId, password);
                    CustomerRelationshipModule.Site.currentUserId = currentUserId;

                    var isAdmin = new EmployeeHelper().IsAdmin(currentUserId);
                    currentUserType = isAdmin ? "Admin" : "Employee";
                    CustomerRelationshipModule.Site.currentUserType = currentUserType;
                }
                else
                {
                    currentUserId = new CustomerHelper().GetSystemId(emailId, password);
                    CustomerRelationshipModule.Site.currentUserId = currentUserId;
                    currentUserType = "Customer";
                    CustomerRelationshipModule.Site.currentUserType = currentUserType;
                }

                if (string.IsNullOrEmpty(currentUserId))
                {
                    result.isSuccess = false;
                }
                else
                {
                    result.isSuccess = true;
                    result.data = new
                    {
                        currentUserId = currentUserId,
                        currentUserType = currentUserType,
                    };
                }
            }
            catch (Exception ex)
            {
                //need to escape the "\" in your string (turning it into a double-"\"), otherwise it will become a newline in the JSON source, not the JSON data
                var errorMessage = ex.Message.Replace("\n", "\\n").Replace("\r", "\\r");
                result.isSuccess = false;
                result.error = errorMessage;
            }
            return result;
        }

    }
}
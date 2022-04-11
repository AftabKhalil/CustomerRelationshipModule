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
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetName
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetName()
        {
            var result = new Models.AjaxResponse<string>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];

                if (currentUserTpe == "Admin" || currentUserTpe == "Employee")
                {
                    var employee = new EmployeeHelper().GetEmployee(currentUserId);
                    if (employee == null)
                    {
                        throw new Exception("No emplyee with this user Id exists in system");
                    }
                    else
                    {
                        result.data = employee.name;
                    }
                }
                else if (currentUserTpe == "Customer")
                {
                    var customer = new CustomerHelper().GetCustomer(currentUserId);
                    if (customer == null)
                    {
                        throw new Exception("No emplyee with this user Id exists in system");
                    }
                    else
                    {
                        result.data = customer.name;
                    }
                }
                else
                {
                    throw new Exception("User type not supported");
                }
                result.isSuccess = true;
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
        #endregion
    }
}
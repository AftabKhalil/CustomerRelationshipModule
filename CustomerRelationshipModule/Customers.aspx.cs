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
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region GetCustomers
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetCustomers()
        {
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new DataTableResponse<Models.Customer>(draw);
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only Admin user can view this page");
                }

                var customer = new CustomerHelper().GetCustomers();
                List<Models.Customer> converted = customer.ConvertAll(x => new Models.Customer()
                {
                    id = x.id,

                    name = x.name,
                    contactNo = x.contact_no,
                    emailId = x.email_id,

                    systemId = x.system_id,
                });

                result.data = converted;
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

        #region DeleteCustomer
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object DeleteCustomer()
        {
            var result = new Models.AjaxResponse<string>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var customerId = HttpContext.Current.Request.Params["customerId"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin user can delete employees");
                }

                new CustomerHelper().DeleteCustomer(customerId);
                result.isSuccess = true;
                result.data = $"customer with system id {customerId} is deleted.";
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
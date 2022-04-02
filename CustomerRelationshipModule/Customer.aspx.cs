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
    public partial class Customer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region GetCustomer
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetCustomer()
        {
            var result = new Models.AjaxResponse<Models.Customer>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var customerId = HttpContext.Current.Request.Params["customerId"];

                if (currentUserTpe == "Employee")
                {
                    var customer = new CustomerHelper().GetCustomer(customerId);
                    if (customer == null)
                    {
                        throw new Exception("No customer with this user Id exists in system");
                    }

                    if (new EmployeeHelper().IsAdmin(currentUserId) || customer.system_id == currentUserId)
                    {
                        result.data = new Models.Customer()
                        {
                            id = customer.id,
                            contactNo = customer.contact_no,
                            emailId = customer.email_id,

                            name = customer.name,
                            password = customer.password,
                            
                            systemId = customer.system_id,
                        };
                    }
                    else
                    {
                        throw new Exception("Only admin or same user can view this page");
                    }

                }
                else if (currentUserTpe == "Customer")
                {
                    throw new Exception("Only admin user can view this page");
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

        #region Save
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object Save()
        {
            var result = new Models.AjaxResponse<string>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];

                var name = HttpContext.Current.Request.Params["name"];
                var email = HttpContext.Current.Request.Params["email"];
                var contactNo = HttpContext.Current.Request.Params["contactNo"];
                var password = HttpContext.Current.Request.Params["password"];

                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Name is invalid");
                }
                if (string.IsNullOrEmpty(email) || !email.Contains("@") || email.Length < 3)
                {
                    throw new Exception("Email is invalid");
                }
                if (!int.TryParse(contactNo, out int resultcontactNo))
                {
                    throw new Exception("Contactno is invalid");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception("password is invalid");
                }


                var mode = HttpContext.Current.Request.Params["mode"];
                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only Customer user can add/edit new employess");
                }

                if (mode == "UPDATE")
                {
                    var customerId = HttpContext.Current.Request.Params["customerId"];
                    var temp = new CustomerHelper().GetCustomerByEmail(email);
                    if (temp != null && temp.system_id != customerId)
                    {
                        throw new Exception($"Customer with same email: {email} alreay exists, cant update email");
                    }
                    var customer = new CustomerHelper().Update(customerId, name, contactNo, email);
                    result.data = $"Customer updated with system Id: {customer.system_id}";
                }
                else
                {
                    if (new CustomerHelper().GetCustomerByEmail(email) != null)
                    {
                        throw new Exception($"Employee with same email: {email} alreay exists.");
                    }

                    var customer = new CustomerHelper().Add(name, contactNo, email, password);
                    result.data = $"New emplyee added with system Id: {customer.system_id}";
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

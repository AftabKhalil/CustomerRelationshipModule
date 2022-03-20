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
    public partial class Employee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetEmployee
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetEmployee()
        {
            var result = new Models.AjaxResponse<Models.Employee>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var employeeId = HttpContext.Current.Request.Params["employeeId"];

                if (currentUserTpe == "Employee")
                {
                    var employee = new EmployeeHelper().GetEmployee(employeeId);
                    if (employee == null)
                    {
                        throw new Exception("No emplyee with this user Id exists in system");
                    }

                    if (new EmployeeHelper().IsAdmin(currentUserId) || employee.system_id == currentUserId)
                    {
                        result.data = new Models.Employee()
                        {
                            id = employee.id,
                            contactNo = employee.contact_no,
                            emailId = employee.email_id,
                            expirence = employee.previous_expirence_in_months,
                            name = employee.name,
                            password = employee.password,
                            position = employee.position.ToString(),
                            salary = employee.salary,
                            systemId = employee.system_id,
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
                var expirence = int.Parse(HttpContext.Current.Request.Params["expirence"]);
                var position = int.Parse(HttpContext.Current.Request.Params["position"]);
                var salary = int.Parse(HttpContext.Current.Request.Params["salary"]);
                var password = HttpContext.Current.Request.Params["password"];

                var mode = HttpContext.Current.Request.Params["mode"];
                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin user can add/edit new employess");
                }

                if (mode == "UPDATE")
                {
                    var employeeId = HttpContext.Current.Request.Params["employeeId"];
                    var temp = new EmployeeHelper().GetEmployeeByEmail(email);
                    if (temp != null && temp.system_id != employeeId)
                    {
                        throw new Exception($"Employee with same email: {email} alreay exists, cant update email");
                    }
                    var employee = new EmployeeHelper().Update(employeeId, name, position, salary, expirence, contactNo, email, password);
                    result.data = $"Emplyee updated with system Id: {employee.system_id}";
                }
                else
                {
                    if (new EmployeeHelper().GetEmployeeByEmail(email) != null)
                    {
                        throw new Exception($"Employee with same email: {email} alreay exists.");
                    }

                    var employee = new EmployeeHelper().Add(name, position, salary, expirence, contactNo, email, password);
                    result.data = $"New emplyee added with system Id: {employee.system_id}";
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
using Common.Utils;
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
    public partial class Employees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetEmployees
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetEmployees()
        {
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new DataTableResponse<Models.Employee>(draw);
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin user can view this page");
                }

                var emplyess = new EmployeeHelper().GetEmployees();
                List<Models.Employee> converted = emplyess.ConvertAll(x => new Models.Employee()
                {
                    id = x.id,

                    name = x.name,
                    position = EnumExtension.ToEnum<EmployeeType>(x.position).ToString(),
                    salary = x.salary,
                    expirence = x.previous_expirence_in_months,
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

        #region DeleteEmployee
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object DeleteEmployee()
        {
            var result = new Models.AjaxResponse<string>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var employeeId = HttpContext.Current.Request.Params["employeeId"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin user can delete employees");
                }

                if (new EmployeeHelper().IsAdmin(employeeId) && new EmployeeHelper().GetAdminCount() == 1)
                {
                    throw new Exception("Cant delete the one and only Admin user");
                }

                new EmployeeHelper().DeleteEmployee(employeeId);
                result.isSuccess = true;
                result.data = $"Emplyee with system id {employeeId} is deleted.";
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
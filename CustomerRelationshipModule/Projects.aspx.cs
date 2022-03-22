﻿using Common.Utils;
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
    public partial class Projects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetProjects
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetProjects()
        {
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new DataTableResponse<Models.Project>(draw);
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserType = HttpContext.Current.Request.Params["currentUserType"];

                var project = new ProjectHelper().GetProjects();

                if (currentUserType == "Customer")
                {
                    project = project.Where(p => p.Customer.system_id == currentUserId).ToList();
                }
                else if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin can view this page");
                }

                List<Models.Project> converted = project.ConvertAll(x => new Models.Project()
                {
                    ID = x.id,

                    Name = x.name,
                    Budget = x.budget,
                    CustomerName = x.Customer.name
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
            #endregion
        }
        /*#endregion
        
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
        */
    }
}
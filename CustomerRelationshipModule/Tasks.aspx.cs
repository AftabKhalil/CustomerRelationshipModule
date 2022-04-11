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
    public partial class Tasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetTasks
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetTasks()
        {
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new DataTableResponse<Models.Task>(draw);
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserType = HttpContext.Current.Request.Params["currentUserType"];

                var task = new TaskHelper().GetTasks();

                if (currentUserType == "Customer")
                {
                    var customer = new CustomerHelper().GetCustomer(currentUserId);
                    task = task.Where(p => p.Project.customer_id == customer.id).ToList();
                }
                else if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin can view this page");
                }

                List<Models.Task> converted = task.ConvertAll(x => new Models.Task()
                {
                    ID = x.id,

                    Name = x.name,
                    ProjectName = x.Project.name
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


        #region DeleteTasks
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object DeleteTasks()
        {
            var result = new Models.AjaxResponse<string>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var TaskId = int.Parse(HttpContext.Current.Request.Params["TaskId"]);

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only admin user can delete employees");
                }

                new TaskHelper().DeleteTask(TaskId);
                result.isSuccess = true;
                result.data = $"customer with system id {TaskId} is deleted.";
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
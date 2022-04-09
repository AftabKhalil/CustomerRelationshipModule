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
    public partial class TaskAssignmnet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetTaskAssignment
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetTaskAssignment()
        {
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new DataTableResponse<Models.TaskAssignment>(draw);
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserType = HttpContext.Current.Request.Params["currentUserType"];
                var taskId = HttpContext.Current.Request.Params["taskId"];

                if (!int.TryParse(taskId, out int TaskId))
                    throw new Exception("No task Id in request");

                var taskAssingmnets = new TaskAssignmentHelper().GetTaskAssignments(TaskId);

                List<Models.TaskAssignment> converted = taskAssingmnets.ConvertAll(x => new Models.TaskAssignment()
                {
                    Id = x.id,
                    EmployeeId = x.employee_id.ToString(),
                    EmployeeName = x.Employee.name,
                    Rating = x.sentiment.ToString(),
                    Review = x.message,
                    AssignmentType = EnumExtension.ToEnum<TaskType>(x.task_type).ToString(),
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

                var taskId = int.Parse(HttpContext.Current.Request.Params["taskId"]);
                var employeeId = int.Parse(HttpContext.Current.Request.Params["employeeId"]);
                var assignmentType = int.Parse(HttpContext.Current.Request.Params["assignmentType"]);

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only Admin user can add/edit new tasks");
                }

                if (new TaskAssignmentHelper().GetTaskAssignment(employeeId, taskId, assignmentType) != null)
                {
                    throw new Exception("Same assignment already assigned to same employee");
                }

                var task = new TaskAssignmentHelper().Add(employeeId, taskId, assignmentType);
                result.data = $"New task assignment added";

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
using CustomerRelationshipModule.Utils;
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
    public partial class AddTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region GetTask
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object GetTask()
        {
            var result = new Models.AjaxResponse<Models.Task>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserTpe = HttpContext.Current.Request.Params["currentUserType"];
                var TaskId = int.Parse(HttpContext.Current.Request.Params["TaskId"]);

                if (currentUserTpe == "Admin")
                {
                    var Task = new TaskHelper().GetTask(TaskId);
                    result.data = new Models.Task()
                    {
                        ID = Task.id,

                        Name = Task.name,
                        ProjectName = Task.Project.name,
                        ProjectId = Task.project_id,
                    };
                }
                else
                {
                    throw new Exception("Only admin user can view this page");
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

                var TaskName = HttpContext.Current.Request.Params["taskName"];
                var projectId = HttpContext.Current.Request.Params["project"];
                var mode = HttpContext.Current.Request.Params["mode"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only Admin user can add/edit new tasks");
                }

                if (mode == "UPDATE")
                {
                    var taskId = HttpContext.Current.Request.Params["taskId"];
                    var task = new TaskHelper().Update(int.Parse(taskId), TaskName, int.Parse(projectId));
                    result.data = $"task updated with name {task.name}";
                    EmailHelper.SendEmailForTaskAddUpdate(int.Parse(projectId), task.name);
                }
                else
                {
                    var task = new TaskHelper().Add(TaskName, int.Parse(projectId));
                    result.data = $"New task added with name {task.name}";
                    EmailHelper.SendEmailForTaskAddUpdate(int.Parse(projectId), task.name);
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
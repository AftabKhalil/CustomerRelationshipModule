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
    public partial class Project : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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

                var projectName = HttpContext.Current.Request.Params["projectName"];
                var budget = HttpContext.Current.Request.Params["budget"];
                var customerId = HttpContext.Current.Request.Params["customer"];
                var mode = HttpContext.Current.Request.Params["mode"];

                if (!new EmployeeHelper().IsAdmin(currentUserId))
                {
                    throw new Exception("Only Admin user can add/edit new projects");
                }

                if (mode == "UPDATE")
                {
                    var projectId = HttpContext.Current.Request.Params["projectId"];
                    var project = new ProjectHelper().Update(int.Parse(projectId), projectName, int.Parse(budget), int.Parse(customerId));
                    result.data = $"Project updated with name {project.name}";
                }
                else
                {
                    var project = new ProjectHelper().Add(projectName, int.Parse(budget), int.Parse(customerId));
                    result.data = $"New project added with name {project.name}";
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
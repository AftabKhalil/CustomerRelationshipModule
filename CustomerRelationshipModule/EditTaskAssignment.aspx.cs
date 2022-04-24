using Common.Utils;
using CustomerRelationshipModule.Models;
using Data.ORMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerRelationshipModule
{
    public partial class EditTaskAssignment : System.Web.UI.Page
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
            var result = new Response<TaskAssignment>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserType = HttpContext.Current.Request.Params["currentUserType"];
                var taskAssignmentId = HttpContext.Current.Request.Params["taskAssignmentId"];

                if (!int.TryParse(taskAssignmentId, out int TaskAssignmentId))
                    throw new Exception("No task Id in request");

                var taskAssingmnets = new TaskAssignmentHelper().GetTaskAssignment(TaskAssignmentId);

                Models.TaskAssignment converted = new Models.TaskAssignment()
                {
                    Id = taskAssingmnets.id,
                    EmployeeId = taskAssingmnets.employee_id.ToString(),
                    EmployeeSystemId = taskAssingmnets.Employee.system_id,
                    EmployeeName = taskAssingmnets.Employee.name,
                    Rating = taskAssingmnets.sentiment.ToString(),
                    Review = taskAssingmnets.message,
                    AssignmentType = EnumExtension.ToEnum<TaskType>(taskAssingmnets.task_type).ToString(),
                    IsCompleted = taskAssingmnets.is_completed,
                };

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
            int.TryParse(HttpContext.Current.Request.Params["draw"], out int draw);
            var result = new Response<bool>();
            try
            {
                var currentUserId = HttpContext.Current.Request.Params["currentUserId"];
                var currentUserType = HttpContext.Current.Request.Params["currentUserType"];
                var taskAssignmentId = HttpContext.Current.Request.Params["taskAssignmentId"];
                var review = HttpContext.Current.Request.Params["review"];
                var rating = HttpContext.Current.Request.Params["rating"];
                var isCompleted = HttpContext.Current.Request.Params["isCompleted"];

                if (!int.TryParse(taskAssignmentId, out int TaskAssignmentId))
                    throw new Exception("No task assignment Id in request");

                if (currentUserType == "Admin")
                {
                    if (!int.TryParse(rating, out int Rating))
                        throw new Exception("Only integer values supported for rating");
                    new TaskAssignmentHelper().EditRating(TaskAssignmentId, Rating);
                }
                else if (currentUserType == "Employee" && isCompleted == "true")
                {
                    SendEmailToCustomerForTaskCompletion(TaskAssignmentId);
                    new TaskAssignmentHelper().MarkDone(TaskAssignmentId);
                }
                else if (currentUserType == "Customer")
                {
                    new TaskAssignmentHelper().EditReview(TaskAssignmentId, review);
                }
                else
                {
                    throw new Exception("You are not authoried to edit the task");
                }

                result.data = true;
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

        private static void SendEmailToCustomerForTaskCompletion(int taskAssignmentId)
        {
            try
            {
                var senderMail = "aaftabkhalil@gmail.com";
                var senderPass = "ujcggwmmmewvspag";

                var taskAssignment = new TaskAssignmentHelper().GetTaskAssignment(taskAssignmentId);
                var project = new ProjectHelper().GetProject(taskAssignment.Task.project_id);
                var customer = project.Customer;

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(senderMail);
                message.To.Add(new MailAddress(customer.email_id));
                message.Subject = "Task Completion";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = $"<h1>Hi {customer.name},</h1><p>Your task <b>{taskAssignment.Task.name}</b> has been completed for <b>{EnumExtension.ToEnum<TaskType>(taskAssignment.task_type)}</b> by {taskAssignment.Employee.name}.";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(senderMail, senderPass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
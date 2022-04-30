using System;
using System.Net;
using System.Net.Mail;

using Common.Utils;
using Data.ORMHelper;

namespace CustomerRelationshipModule.Utils
{
    public class EmailHelper
    {
        protected static void DispatchMail(string reciepent, string subject, string body)
        {
            try
            {
                var senderMail = "aaftabkhalil@gmail.com";
                var senderPass = "ujcggwmmmewvspag";

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(senderMail);
                message.To.Add(new MailAddress(reciepent));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
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

        public static void SendCustomerAddUpdateEmail(string customerEmail, string customerName, string CustomerPassword)
        {
            var subject = "Signup to CRM";
            var body = $"<h1>Hi {customerName},</h1><p>Your account is created successfully for email <b>{customerEmail}</b> with password <b>{CustomerPassword }</b>.";

            DispatchMail(customerEmail, subject, body);
        }

        public static void SendProjectAddUpdateEmail(int customerId, string projectName, string projectBudget)
        {

            var customer = new CustomerHelper().GetCustomer(customerId);

            var subject = "Project added to CRM";
            var body = $"<h1>Hi {customer.name},</h1><p>Your project <b>{projectName}</b> is created successfully with budget <b>{projectBudget }</b>.";

            DispatchMail(customer.email_id, subject, body);
        }

        public static void SendEmailForTaskAddUpdate(int projectId, string taskName)
        {
            var project = new ProjectHelper().GetProject(projectId);
            var customer = new CustomerHelper().GetCustomer(project.customer_id);

            var subject = "Task added for Project";
            var body = $"<h1>Hi {customer.name},</h1><p>Your task <b>{taskName}</b> is added successfully in Project <b>{project.name}</b>";

            DispatchMail(customer.email_id, subject, body);
        }


        public static void SendEmailForTaskAssignmentCreation(int EmployeeId, int TaskId, int AssignmentType)
        {
            var employee = new EmployeeHelper().GetEmployee(EmployeeId);
            var task = new TaskHelper().GetTask(TaskId);
            var project = new ProjectHelper().GetProject(task.project_id);
            var customer = project.Customer;

            var subject = "Task Assigned";
            var body = $"<h1>Hi {customer.name},</h1><p>Your task <b>{task.name}</b> has been assigned to <b>{employee.name}</b>for <b>{EnumExtension.ToEnum<TaskType>(AssignmentType)}</b>.";

            DispatchMail(customer.email_id, subject, body);
        }

        public static void SendEmailToCustomerForTaskCompletion(int taskAssignmentId)
        {
            var taskAssignment = new TaskAssignmentHelper().GetTaskAssignment(taskAssignmentId);
            var project = new ProjectHelper().GetProject(taskAssignment.Task.project_id);
            var customer = project.Customer;

            var subject = "Task Completed";
            var body = $"<h1>Hi {customer.name},</h1><p>Your task <b>{taskAssignment.Task.name}</b> has been completed for <b>{EnumExtension.ToEnum<TaskType>(taskAssignment.task_type)}</b> by {taskAssignment.Employee.name}.";

            DispatchMail(customer.email_id, subject, body);
        }
    }
}
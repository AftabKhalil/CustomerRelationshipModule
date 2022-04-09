using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ORMHelper
{
    public class TaskAssignmentHelper
    {
        public List<TaskAssignment> GetTaskAssignments(int taskId)
        {
            var db = new CRMEntities();
            var taskAssignments = db.TaskAssignments.Include("Employee").Where(ta => ta.task_id == taskId).ToList();
            return taskAssignments;
        }

        public TaskAssignment GetTaskAssignment(int EmployeeId, int TaskId, int AssignmentType)
        {
            var db = new CRMEntities();
            var taskAssignments = db.TaskAssignments.Where(ta => ta.employee_id == EmployeeId && ta.task_id == TaskId && ta.task_type == AssignmentType).FirstOrDefault();
            return taskAssignments;
        }

        public TaskAssignment Add(int EmployeeId, int TaskId, int AssignmentType)
        {
            var db = new CRMEntities();
            var p = new TaskAssignment()
            {
                employee_id = EmployeeId,
                task_id = TaskId,
                task_type = AssignmentType,
                sentiment = 0,
                message = "",
            };

            db.TaskAssignments.Add(p);
            db.SaveChanges();
            return p;
        }
    }
}

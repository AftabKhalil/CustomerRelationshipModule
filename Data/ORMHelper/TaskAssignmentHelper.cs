using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ORMHelper
{
    public class TaskAssignmentHelper
    {
        public List<TaskAssignment> GetTaskAssignmetForSentimentExcel()
        {
            var db = new CRMEntities();
            var taskAssignments = db.TaskAssignments.ToList();
            return taskAssignments;
        }

        public List<TaskAssignment> GetTaskAssignments(int taskId)
        {
            var db = new CRMEntities();
            var taskAssignments = db.TaskAssignments.Include("Employee").Where(ta => ta.task_id == taskId).ToList();
            return taskAssignments;
        }

        public TaskAssignment GetTaskAssignment(int taskAssignmnetId)
        {
            var db = new CRMEntities();
            var taskAssignment = db.TaskAssignments.Include("Employee").Include("Task").Where(ta => ta.id == taskAssignmnetId).First();
            return taskAssignment;
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

        public TaskAssignment EditReview(int taskAssignmnetId, string review)
        {
            var db = new CRMEntities();
            var e = db.TaskAssignments.FirstOrDefault(x => x.id == taskAssignmnetId);

            if (e == null)
            {
                throw new Exception("No Task Assignment found to edit");
            }
            e.message = review;

            db.SaveChanges();
            db.Dispose();
            return e;
        }

        public TaskAssignment EditRating(int taskAssignmnetId, int rating)
        {
            var db = new CRMEntities();
            var e = db.TaskAssignments.FirstOrDefault(x => x.id == taskAssignmnetId);

            if (e == null)
            {
                throw new Exception("No Task Assignment found to edit");
            }
            e.sentiment = rating;

            db.SaveChanges();
            db.Dispose();
            return e;
        }

        public TaskAssignment MarkDone(int taskAssignmnetId)
        {
            var db = new CRMEntities();
            var e = db.TaskAssignments.FirstOrDefault(x => x.id == taskAssignmnetId);

            if (e == null)
            {
                throw new Exception("No Task Assignment found to edit");
            }
            e.is_completed = true;

            db.SaveChanges();
            db.Dispose();
            return e;
        }

    }
}

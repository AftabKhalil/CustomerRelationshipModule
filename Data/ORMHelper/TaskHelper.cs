using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.ORMHelper
{
    public class TaskHelper
    {
        public bool IsAdmin()
        {
            var db = new CRMEntities();
            var employee = db.Employees.Any();
            return employee;
        }

        public List<Task> GetTasks()
        {
            var db = new CRMEntities();
            var tasks = db.Tasks.Include("Project").ToList();
            return tasks;
        }

        public void DeleteTask(int TaskId)
        {
            var db = new CRMEntities();
            var task = db.Tasks.Where(p => p.id == TaskId).FirstOrDefault();

            if (task != null)
                db.Tasks.Remove(task);

            db.SaveChanges();
            db.Dispose();
        }


        public Task GetTask(int TaskId)
        {
            var db = new CRMEntities();
            var task = db.Tasks.FirstOrDefault(e => e.id == TaskId);
            return task;
        }

        public Task Add(string TaskName, int projectId)
        {
            var db = new CRMEntities();
            var p = new Task()
            {
                name = TaskName,
                project_id = projectId,

            };

            db.Tasks.Add(p);
            db.SaveChanges();
            return p;
        }

        public Task Update(int taskId, string TaskName, int projectId)
        {
            var db = new CRMEntities();
            var e = db.Tasks.FirstOrDefault(x => x.id == taskId);

            if (e == null)
            {
                throw new Exception("No Task found to edit");
            }

            e.name = TaskName;

            e.project_id = projectId;

            db.SaveChanges();
            db.Dispose();
            return e;
        }
    }
}
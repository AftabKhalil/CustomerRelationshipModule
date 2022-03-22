using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.ORMHelper
{
    public class ProjectHelper
    {
        public bool IsAdmin()
        {
            var db = new CRMEntities();
            var employee = db.Employees.Any();
            return employee;
        }

        public List<Project> GetProjects()
        {
            var db = new CRMEntities();
            var projects = db.Projects.Include("Customer").ToList();
            return projects;
        }

        public void DeleteProject(int projectId)
        {
            var db = new CRMEntities();
            var project = db.Projects.Where(p => p.id == projectId).FirstOrDefault();

            if (project != null)
                db.Projects.Remove(project);

            db.SaveChanges();
            db.Dispose();
        }


        public Project GetProject(int projectId)
        {
            var db = new CRMEntities();
            var project = db.Projects.FirstOrDefault(e => e.id == projectId);
            return project;
        }

        public Project Add(string projectName, int budget, int customerId)
        {
            var db = new CRMEntities();
            var p = new Project()
            {
                name = projectName,
                budget = budget,
                customer_id = customerId
            };

            db.Projects.Add(p);
            db.SaveChanges();
            return p;
        }

        public Project Update(int projectId, string projectName, int budget, int customerId)
        {
            var db = new CRMEntities();
            var e = db.Projects.FirstOrDefault(x => x.id == projectId);

            if (e == null)
            {
                throw new Exception("No project found to edit");
            }

            e.name = projectName;
            e.budget = budget;
            e.customer_id = customerId;

            db.SaveChanges();
            db.Dispose();
            return e;
        }
    }
}

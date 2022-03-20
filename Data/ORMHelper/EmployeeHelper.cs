using Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.ORMHelper
{
    public class EmployeeHelper
    {
        public string GetSystemId(string email, string password)
        {
            var db = new CRMEntities();
            var employee = db.Employees.FirstOrDefault(e => e.email_id == email && e.password == password);
            return employee?.system_id;
        }

        public bool IsAdmin(string systemId)
        {
            var db = new CRMEntities();
            var employee = db.Employees.Any(e => e.system_id == systemId && e.position == (int)EmployeeType.Admin);
            return employee;
        }

        public int GetAdminCount()
        {
            var db = new CRMEntities();
            var employee = db.Employees.Where(e => e.position == (int)EmployeeType.Admin).Count();
            return employee;
        }

        public List<Employee> GetEmployees()
        {
            var db = new CRMEntities();
            var employees = db.Employees.ToList();
            return employees;
        }

        public Employee GetEmployee(string systemId)
        {
            var db = new CRMEntities();
            var employee = db.Employees.FirstOrDefault(e => e.system_id == systemId);
            return employee;
        }

        public void DeleteEmployee(string systemId)
        {
            var db = new CRMEntities();
            var employee = db.Employees.FirstOrDefault(e => e.system_id == systemId);

            if (employee != null)
                db.Employees.Remove(employee);

            db.SaveChanges();
            db.Dispose();
        }


        public Employee GetEmployeeByEmail(string emailId)
        {
            var db = new CRMEntities();
            var employee = db.Employees.FirstOrDefault(e => e.email_id == emailId);
            return employee;
        }

        public Employee Add(string name, int position, int salary, int previous_expirence_in_months, string contact_no, string email_id, string password)
        {
            var db = new CRMEntities();
            var e = new Employee()
            {
                name = name,
                email_id = email_id,
                position = position,
                salary = salary,
                previous_expirence_in_months = previous_expirence_in_months,
                contact_no = contact_no,
                password = password,
                system_id = Guid.NewGuid().ToString(),
            };

            db.Employees.Add(e);
            db.SaveChanges();
            return e;
        }

        public Employee Update(string emplyeeId, string name, int position, int salary, int previous_expirence_in_months, string contact_no, string email_id, string password)
        {
            var db = new CRMEntities();
            var e = db.Employees.FirstOrDefault(x => x.system_id == emplyeeId);

            if (e == null)
            {
                throw new Exception("No employee found to edit");
            }

            e.name = name;
            e.position = position;
            e.salary = salary;
            e.previous_expirence_in_months = previous_expirence_in_months;
            e.contact_no = contact_no;
            e.email_id = email_id;
            e.password = password;

            db.SaveChanges();
            db.Dispose();
            return e;
        }

    }
}
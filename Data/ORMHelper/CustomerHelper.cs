using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ORMHelper
{
    public class CustomerHelper
    {
        public string GetSystemId(string email, string password)
        {
            var db = new CRMEntities();
            var customer = db.Customers.FirstOrDefault(e => e.email_id == email && e.password == password);
            return customer?.system_id;
        }

        public bool IsCustomer(string systemId)
        {
            var db = new CRMEntities();
            var Customer = db.Customers.Any(e => e.system_id == systemId);
            return Customer;
        }
        

        public List<Customer> GetCustomers()
        {
            var db = new CRMEntities();
            var customer = db.Customers.ToList();
            return customer;
        }

        public Customer GetCustomer(string systemId)
        {
            var db = new CRMEntities();
            var customer = db.Customers.FirstOrDefault(e => e.system_id == systemId);
            return customer;
        }
        public Customer GetCustomer(int customerId)
        {
            var db = new CRMEntities();
            var customer = db.Customers.FirstOrDefault(e => e.id == customerId);
            return customer;
        }


        public void DeleteCustomer(string systemId)
        {
            var db = new CRMEntities();
            var customer = db.Customers.FirstOrDefault(e => e.system_id == systemId);

            if (customer != null)
                db.Customers.Remove(customer);

            db.SaveChanges();
            db.Dispose();
        }


        public Customer GetCustomerByEmail(string emailId)
        {
            var db = new CRMEntities();
            var customer = db.Customers.FirstOrDefault(e => e.email_id == emailId);
            return customer;
        }

        public Customer Add(string name, string contact_no, string email_id,string password)
        {
            var db = new CRMEntities();
            var c = new Customer()
            {
                name = name,
                email_id = email_id,
                contact_no = contact_no,
                password=password,
                system_id = Guid.NewGuid().ToString(),
            };

            db.Customers.Add(c);
            db.SaveChanges();
            return c;
        }

        public Customer Update(string CustomerId, string name, string contact_no, string email_id)
        {
            var db = new CRMEntities();
            var c = db.Customers.FirstOrDefault(x => x.system_id == CustomerId);

            if (c == null)
            {
                throw new Exception("No customer found to edit");
            }

            c.name = name;
            c.contact_no = contact_no;
            c.email_id = email_id;

            db.SaveChanges();
            db.Dispose();
            return c;
        }

    }
}

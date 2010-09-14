using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;
using NUnit.Framework;
using Vivina.Erp.BusinessRules;
using Vivina.Erp.DataClasses;
using InfoControl;
using InfoControl.Data;
using InfoControl.Configuration;
using InfoControl.Data.Configuration;


namespace Test.BusinessRules.Administration
{
    [TestFixture]
    public class CustomerManagerTest 
    {
        [SetUp]
        public void Init()
        {
            //  _customerManager = new CustomerManager(null);
        }

        [Test]
        public void TestInsertValidCustomer()
        {

            var customerManager = new CustomerManager(null);

            var profile = new Profile();

            profile.Name = "Test";
            profile.CPF = "778.310.205-00";
            profile.ModifiedDate = DateTime.Now;

            var customer = new Customer();
            customer.ModifiedDate = DateTime.Now;
            customer.CreatedDate = DateTime.Now;
            customer.CompanyId = 1;

            customer.Profile = profile;

            customerManager.Insert(customer);

            Assert.AreNotEqual(customer.CustomerId, 0);

        }
    }
}

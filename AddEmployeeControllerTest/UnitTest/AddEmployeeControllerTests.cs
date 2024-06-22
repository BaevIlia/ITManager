using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITManager.Controller;
using ITManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITManager.Controller.Tests
{
    [TestClass()]
    public class AddEmployeeControllerTests
    {
        [TestMethod()]
        public void AddEmployeeTestPositive()
        {
            //Arrange
            Random random = new Random();
            EmployeeUnion employeeForAdd = new()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Patronymic = Guid.NewGuid().ToString(),
                Title = "Программист",
                RoleName = "IT-специалист",
                Login = $"test_tt{random.Next(1,100)}",
                Password = "12345"
            };

            //Act
            var controller = new AddEmployeeController();

           
            int result = controller.AddEmployee(employeeForAdd);

            //Assert
            Assert.AreEqual(1, result);
        }
       [TestMethod()]
        public void AddEmployeeTestNegative()
        {
            //Arrange
            Random random = new Random();
            EmployeeUnion employeeForAdd = new()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Patronymic = Guid.NewGuid().ToString(),
                Title = "Программист",
                RoleName = "IT-специалист",
                Login = null,
                Password = "12345"
            };

            //Act
            var controller = new AddEmployeeController();


            int result = controller.AddEmployee(employeeForAdd);

            //Assert
            Assert.AreEqual(1, result);
        }

    }
}
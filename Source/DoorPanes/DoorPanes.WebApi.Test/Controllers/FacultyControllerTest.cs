using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoorPanes.WebApi.Controllers;
using Moq;
using DoorPanes.Services.DAL;
using DoorPanes.Services.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace DoorPanes.WebApi.Test.Controllers
{
    [TestClass]
    public class FacultyControllerTest
    {
        [TestMethod]
        public void FacultyController()
        {
            try
            {
                // check that we can create an instance of the controller               
                var instance = new FacultyController();
                Assert.IsInstanceOfType(instance, typeof(FacultyController));
            }
            catch
            {
                // fail test if creating an instance of the controller fails
                Assert.Fail();
            }
        }

        [TestMethod]
        public void FacultyController_Test()
        {
            try
            {
                Mock<IFacultyRepository> mockFacultyRepository = new Mock<IFacultyRepository>();
                // check that we can create an instance of the controller               
                var instance = new FacultyController(mockFacultyRepository.Object);
                Assert.IsInstanceOfType(instance, typeof(FacultyController));
            }
            catch
            {
                // fail test if creating an instance of the controller fails
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetFacultyMembers_TestReturnsFacultyMembers()
        {
            var faculty = new List<FacultyModel>();
            var personid1 = Guid.NewGuid();
            var personid2 = Guid.NewGuid();
            var personid3 = Guid.NewGuid();
            var personid4 = Guid.NewGuid();

            faculty.Add(new Services.Models.FacultyModel { PersonID = personid1, FirstName = "Christer", LastName = "Karlsson" });
            faculty.Add(new Services.Models.FacultyModel { PersonID = personid2, FirstName = "Larry", LastName = "Pyeatt" });
            faculty.Add(new Services.Models.FacultyModel { PersonID = personid3, FirstName = "Jeff", LastName = "McGough" });
            faculty.Add(new Services.Models.FacultyModel { PersonID = personid4, FirstName = "Paul", LastName = "Hinker" });

            var facultySorted = new List<FacultyModel>();
            facultySorted.Add(new Services.Models.FacultyModel { PersonID = personid4, FirstName = "Paul", LastName = "Hinker" });
            facultySorted.Add(new Services.Models.FacultyModel { PersonID = personid1, FirstName = "Christer", LastName = "Karlsson" });
            facultySorted.Add(new Services.Models.FacultyModel { PersonID = personid3, FirstName = "Jeff", LastName = "McGough" });
            facultySorted.Add(new Services.Models.FacultyModel { PersonID = personid2, FirstName = "Larry", LastName = "Pyeatt" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(facultySorted);
       
            // mock the repository and add the fake data
            Mock<IFacultyRepository> mockFacultyRepository = new Mock<IFacultyRepository>();
            mockFacultyRepository.Setup(m => m.GetAllFacultyMembers()).Returns(faculty);

            // tell the calendar controller we are using a fake repository
            var controller = new FacultyController(mockFacultyRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetFacultyMembers();
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestMethod]
        public void GetFacultyMembers_TestReturnsNotFound()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<IFacultyRepository> mockFacultyRepository = new Mock<IFacultyRepository>();
            mockFacultyRepository.Setup(m => m.GetAllFacultyMembers()).Returns(new List<FacultyModel>());

            var controller = new FacultyController(mockFacultyRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetFacultyMembers();
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

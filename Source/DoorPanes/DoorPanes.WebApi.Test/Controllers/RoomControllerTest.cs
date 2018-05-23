using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoorPanes.WebApi.Controllers;
using DoorPanes.Services.DAL;
using Moq;
using DoorPanes.Services.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace DoorPanes.WebApi.Test.Controllers
{
    [TestClass]
    public class RoomControllerTest
    {
        [TestMethod]
        public void RoomController()
        {
            try
            {
                // check that we can create an instance of the controller               
                var instance = new RoomController();
                Assert.IsInstanceOfType(instance, typeof(RoomController));
            }
            catch
            {
                // fail test if creating an instance of the controller fails
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Room_Controller_Test()
        {
            try
            {
                Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
                // check that we can create an instance of the controller               
                var instance = new RoomController(mockRoomRepository.Object);
                Assert.IsInstanceOfType(instance, typeof(RoomController));
            }
            catch
            {
                // fail test if creating an instance of the controller fails
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetAllRooms_ReturnsRooms()
        {
            var rooms = new List<RoomModel>();
            var sortedRooms = new List<RoomModel>();

            var roomid1 = Guid.NewGuid();
            var roomid2 = Guid.NewGuid();
            var roomid3 = Guid.NewGuid();
            var roomid4 = Guid.NewGuid();

            rooms.Add( new RoomModel { RoomID = roomid1, RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });
            rooms.Add( new RoomModel { RoomID = roomid2, RoomNumber = "315", RoomOwner = "C. Karlsson", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office });
            rooms.Add( new RoomModel { RoomID = roomid3, RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });
            rooms.Add( new RoomModel { RoomID = roomid4, RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });

            sortedRooms.Add(new RoomModel { RoomID = roomid1, RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });
            sortedRooms.Add(new RoomModel { RoomID = roomid3, RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });
            sortedRooms.Add(new RoomModel { RoomID = roomid4, RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom });
            sortedRooms.Add(new RoomModel { RoomID = roomid2, RoomNumber = "315", RoomOwner = "C. Karlsson", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(sortedRooms);

            // mock the repository and add the fake data
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockRoomRepository.Setup(m => m.GetAllRooms()).Returns(rooms);

            // tell the calendar controller we are using a fake repository
            var controller = new RoomController(mockRoomRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetAllRooms();
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetAllRooms_TestReturnsNotFound()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockRoomRepository.Setup(m => m.GetAllRooms()).Returns(new List<RoomModel>());

            var controller = new RoomController(mockRoomRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetAllRooms();
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

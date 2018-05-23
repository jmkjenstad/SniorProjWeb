using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoorPanes.WebApi.Controllers;
using DoorPanes.Services.DAL;
using Moq;
using System.Collections.Generic;
using System.Linq;
using DoorPanes.Services.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Web.Http;

namespace DoorPanes.WebApi.Test.Controllers
{
    [TestClass]
    public class CalendarControllerTest
    {
        [TestMethod]
        public void CalendarController_Test()
        {
            try
            {
                var instance = new CalendarController();
                Assert.IsInstanceOfType(instance, typeof(CalendarController));
            }
            catch
            {
                Assert.Fail();
            }
        }

       /* [TestMethod]
        public void GetCalendarEvents_TestReturnsNull()
        {
             // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEvent>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEvents();
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }*/

        [TestMethod]
        public void GetCalendarEvents_TestReturnsNotFound()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEvents();
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetCalendarEvents_TestReturnEvents()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = room, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Hinker", Room = room, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(events);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);


            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetCalendarEvents();
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByOwner_ReturnsEvents()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });

            // create dummy data set for expected result
            var eventsForHinker = new List<CalendarEventModel>();
            eventsForHinker.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            eventsForHinker.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsForHinker);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByOwner( "Hinker" );
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void GetCalendarEventsByOwner_ReturnsNotFound()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByOwner("Hinker");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }
        [TestMethod]
        public void GetCalendarEventsByOwner_ReturnsBadRequest()
        {
            {
                // Arrange
                // Create a mock respository and let the controller know it will be using a fake repo
                Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
                mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

                var controller = new CalendarController(mockCalRepository.Object);
                controller.Request = new HttpRequestMessage();
                controller.Configuration = new HttpConfiguration();

                // Act
                HttpResponseMessage response = controller.GetCalendarEventsByOwner(null);
                var task = response.Content.ReadAsStringAsync();

                // Assert
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            }
        }
        [TestMethod]
        public void GetCalendarEventsByRoom_ReturnsEvents()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });

            // create dummy data set for expected result
            var eventsFor306 = new List<CalendarEventModel>();
            eventsFor306.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            eventsFor306.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsFor306);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRoom( "306");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);

        }
        [TestMethod]
        public void GetCalendarEventsByRoom_ReturnsNotFound()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRoom("303");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRoom_ReturnsBadRequest()
        {
            {
                // Arrange
                // Create a mock respository and let the controller know it will be using a fake repo
                Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
                mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

                var controller = new CalendarController(mockCalRepository.Object);
                controller.Request = new HttpRequestMessage();
                controller.Configuration = new HttpConfiguration();

                // Act
                HttpResponseMessage response = controller.GetCalendarEventsByRoom(null);
                var task = response.Content.ReadAsStringAsync();

                // Assert
                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            }
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsEventsForSum()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create dummy data set for expected result
            var eventsForHinkerForSum = new List<CalendarEventModel>();
            eventsForHinkerForSum.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            eventsForHinkerForSum.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsForHinkerForSum);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Owner("summer", "Hinker");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsEventsForSpr()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create dummy data set for expected result
            var eventsForHinkerForSpr = new List<CalendarEventModel>();
            eventsForHinkerForSpr.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            eventsForHinkerForSpr.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsForHinkerForSpr);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Owner("spring", "Hinker");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsEventsForFall()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create dummy data set for expected result
            var eventsForHinkerForFall = new List<CalendarEventModel>();
            eventsForHinkerForFall.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            eventsForHinkerForFall.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsForHinkerForFall);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Owner("FaLl", "Hinker");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsBadRequestforOwner()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Owner("summer", null);

            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsNotFoundforOwner()
        {            
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Owner("spring", "Hinker");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsNotFoundforSemester()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Owner("fall", "McGough");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRange_Owner_ReturnsBadRequestforSemester()
        { 
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 11, 23, 9, 00, 00), EndTime = new DateTime(2017, 11, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Owner("Winter", "McGough");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsEventsForSum()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create dummy data set for expected result
            var eventsFor205ForSum = new List<CalendarEventModel>();
            eventsFor205ForSum.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });


            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsFor205ForSum);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Room("summer", "205");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsEventsForSpr()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create dummy data set for expected result
            var eventsFor205ForSpr = new List<CalendarEventModel>();
            eventsFor205ForSpr.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            eventsFor205ForSpr.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });


            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsFor205ForSpr);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Room("spring", "205");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsEventsForFall()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room3 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 4, Title = "Demo4", StartTime = new DateTime(2017, 11, 27, 9, 00, 00), EndTime = new DateTime(2017, 11, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 5, Title = "Demo5", StartTime = new DateTime(2017, 6, 27, 9, 00, 00), EndTime = new DateTime(2017, 6, 27, 10, 00, 00), EventOwner = "Hinker", Room = room2, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 6, Title = "Demo6", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Room = room3, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 7, Title = "Demo7", StartTime = new DateTime(2017, 7, 27, 9, 00, 00), EndTime = new DateTime(2017, 7, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create dummy data set for expected result
            var eventsFor205ForFall = new List<CalendarEventModel>();
            eventsFor205ForFall.Add(new Services.Models.CalendarEventModel { EventID = 8, Title = "Demo8", StartTime = new DateTime(2017, 9, 27, 9, 00, 00), EndTime = new DateTime(2017, 9, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var expectedResult = JsonConvert.SerializeObject(eventsFor205ForFall);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new CalendarController(mockCalRepository.Object);

            // set up request data
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();

            // make endpoint call
            var response = controller.GetCalendarEventsByRange_Room("FaLl", "205");
            var task = response.Content.ReadAsStringAsync();
            var actualResult = task.Result;

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsBadRequestforRoom()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Room("summer", null);

            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsBadRequestforNullSemester()
        {
            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(new List<CalendarEventModel>());

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Room(null, "205");

            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsNotFound_NoEventsforRoom()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Room("fall", "303");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsNotFound_NoEventsforSemester()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Room("fall", "205");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsBadRequest_NotASemester()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 11, 23, 9, 00, 00), EndTime = new DateTime(2017, 11, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.GetCalendarEventsByRange_Room("Winter", "205");
            var task = response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public void GetCalendarEventsByRange_Room_ReturnsNotFoundforEmptyDatabase()
        {

        }

        [TestMethod]
        public void DeleteAllEvents_ReturnsOK()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.DeleteAllEvents();
            var task = response.Content.ReadAsStringAsync();


            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        /*[TestMethod]
        public void DeleteAllEvents_ThrowsException()
        {
            // create dummy data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };
            RoomModel room2 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "McGough", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Pyeatt", Room = room2, Description = "Class" });

            // Arrange
            // Create a mock respository and let the controller know it will be using a fake repo
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Throws<InternalTestFailureException>;

            var controller = new CalendarController(mockCalRepository.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            HttpResponseMessage response = controller.DeleteAllEvents();
            var task = response.Content.ReadAsStringAsync();


            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);

        }*/
    }
}

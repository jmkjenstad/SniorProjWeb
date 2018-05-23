using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoorPanes.WebApp.Controllers;
using System.Web;
using Moq;
using DoorPanes.Services.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using DoorPanes.Services.DAL;
using DoorPanes.WebApi.Controllers;
using System.Net.Http;
using System.Linq;
using System.Security.Principal;

namespace DoorPanes.WebApp.Test.ControllerTests
{
    [TestClass]
    public class DashboardControllerTest
    {
        [TestMethod]
        public void DashboardControllerTest_ControllerTest()
        {
            try
            {
                // check that we can create an instance of the controller               
                var instance = new DashboardController()
                {
                    ControllerContext = CreateUserContext("Hinker", "Staff").Object
                };
                Assert.IsInstanceOfType(instance, typeof(DashboardController));
            }
            catch
            {
                // fail test if creating an instance of the controller fails
                Assert.Fail();
            }
        }

        [TestMethod]
        public void DashboardControllerTest_Index()
        {
            DashboardController homeController = new DashboardController()
            {
                ControllerContext = CreateUserContext("Hinker", "Staff").Object
            };
            ActionResult result = homeController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DashboardControllerTest_GetEvents_InvalidOwner()
        {
            var expectedResult = "\"Invalid user!\"";

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Staff").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetEvents("", "");
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DashboardControllerTest_GetEvents_EventsFound_Staff()
        {
            // create dummie data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", RoomOwner = "Hinker", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var fullCalEvents = new List<FullCalendarModel>();
            foreach (var calEvent in events)
                fullCalEvents.Add(new FullCalendarModel(calEvent));
            var expectedResult = JsonConvert.SerializeObject(fullCalEvents);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Staff").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetEvents(room1.RoomOwner, room1.RoomNumber);
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DashboardControllerTest_GetEvents_EventsFound_Faculty()
        {
            // create dummie data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", RoomOwner = "Hinker", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var fullCalEvents = new List<FullCalendarModel>();
            foreach (var calEvent in events)
                fullCalEvents.Add(new FullCalendarModel(calEvent));
            var expectedResult = JsonConvert.SerializeObject(fullCalEvents);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Faculty").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetEvents(room1.RoomOwner, room1.RoomNumber);
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DashboardControllerTest_GetEvents_EventsFound_Student()
        {
            // create dummie data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", RoomOwner = "Hinker", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 2, Title = "Demo2", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });
            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // create the reponse we think should be returned from the endpoint
            var fullCalEvents = new List<FullCalendarModel>();
            foreach (var calEvent in events)
                fullCalEvents.Add(new FullCalendarModel(calEvent));
            var expectedResult = JsonConvert.SerializeObject(fullCalEvents);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Student").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetEvents(room1.RoomOwner, room1.RoomNumber);
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DashboardControllerTest_GetEvents_EventsNotFound()
        {
            // create dummie data set that will be returned from mocking
            var events = new List<CalendarEventModel>();

            // create the reponse we think should be returned from the endpoint
            var fullCalEvents = new List<FullCalendarModel>();
            foreach (var calEvent in events)
                fullCalEvents.Add(new FullCalendarModel(calEvent));
            var expectedResult = JsonConvert.SerializeObject(fullCalEvents);

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Staff").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetEvents("Hinker", "Staff");
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }

        //[TestMethod]
        //public void DashboardControllerTest_Insert_Works()
        //{
        //    // create dummie data set that will be returned from mocking
        //    var calEvent = new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = new RoomModel { RoomNumber = "315" }, Description = "Class" };

        //    // mock the repository and add the fake data
        //    Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
        //    Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();

        //    // tell the calendar controller we are using a fake repository
        //    var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object);

        //    // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
        //    var response = controller.InsertCalendarEvent(Convert.ToInt32(calEvent.EventID), calEvent.Title,
        //        calEvent.StartTime.ToString("yyyy-MM-dd") + "T" + calEvent.StartTime.ToString("HH:mm:ss"),
        //        calEvent.EndTime.ToString("yyyy-MM-dd") + "T" + calEvent.EndTime.ToString("HH:mm:ss"), calEvent.Room.RoomNumber, calEvent.EventOwner, "");

        //    // check the database
        //    var result = mockCalRepository.Object.GetCalendarEvents().ToList();

        //    // compare expected result with actual result
        //    //Assert.AreEqual(expectedResult, response);

        //    Assert.AreEqual(calEvent, result.FirstOrDefault());
        //}

        [TestMethod]
        public void DashboardControllerTest_GetNextId_Works()
        {
            // create dummie data set that will be returned from mocking
            var events = new List<CalendarEventModel>();
            RoomModel room1 = new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom };

            events.Add(new Services.Models.CalendarEventModel { EventID = 3, Title = "Demo3", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Room = room1, Description = "Class" });

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
            mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object);

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetNextId();

            // compare expected result with actual result
            Assert.AreEqual(events[0].EventID + 1, Convert.ToInt64(response.Data));
        }

        //[TestMethod]
        //public void DashboardControllerTest_DeleteCalendarEvent_Success()
        //{
        //    // create dummie data set that will be returned from mocking
        //    var events = new List<CalendarEventModel>();
        //    events.Add(new Services.Models.CalendarEventModel { EventID = 1, Title = "Demo1", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Room = new RoomModel { RoomNumber = "315" }, Description = "Class" });

        //    // mock the repository and add the fake data
        //    Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
        //    Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();
        //    mockCalRepository.Setup(m => m.GetCalendarEvents()).Returns(events);
        //    //mockCalRepository.

        //    // tell the calendar controller we are using a fake repository
        //    var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
        //    {
        //        ControllerContext = CreateUserContext("Hinker", "Staff").Object
        //    };

        //    var expectedCount = controller.GetEvents("Hinker", "");

        //    // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
        //    var response = controller.DeleteCalendarEvent(events[1].EventID);
        //    //var actualResult = JsonConvert.SerializeObject(response.Data);

        //    var db = mockCalRepository.Object;

        //    // compare expected result with actual result
        //    //Assert.AreEqual(expectedResult, actualResult);
        //}

        [TestMethod]
        public void DashboardControllerTest_GetCurrentUser()
        {
            var expectedResult = JsonConvert.SerializeObject("Hinker");

            // mock the repository and add the fake data
            Mock<ICalendarEventRepository> mockCalRepository = new Mock<ICalendarEventRepository>();
            Mock<IRoomRepository> mockRoomRepository = new Mock<IRoomRepository>();

            // tell the calendar controller we are using a fake repository
            var controller = new DashboardController(mockCalRepository.Object, mockRoomRepository.Object)
            {
                ControllerContext = CreateUserContext("Hinker", "Staff").Object
            };

            // make endpoint call (shouldn't ever go down into the database becuase it's mocked)
            var response = controller.GetCurrentUser();
            var actualResult = JsonConvert.SerializeObject(response.Data);

            // compare expected result with actual result
            Assert.AreEqual(expectedResult, actualResult);
        }


        private Mock<ControllerContext> CreateUserContext(string username, string role)
        {
            // create mock principal
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns(username);
            mockPrincipal.Setup(p => p.IsInRole(role)).Returns(true);

            // create mock controller context
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            return mockContext;
        }
    }
}

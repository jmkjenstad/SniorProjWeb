namespace DoorPanes.Services.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<DoorPanes.Services.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DoorPanes.Services.Models.ApplicationDbContext context)
        {
            // build room data
            var roomList = new List<RoomModel>
            {
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "101", RoomName = "CBE Graduates Office", RoomOwner = "TA", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "102", RoomName = "3D Printing", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "103", RoomOwner = "R. Krohn", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "104A", RoomOwner = "Tiospaye", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "104B", RoomOwner = "TIospaye", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "104C", RoomOwner = "Tiospaye", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "106", RoomName = "CBE Graduates Office", RoomOwner = "TA", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "107", RoomOwner = "R. Ruby-Hinker", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "108", RoomName = "Mobile Lab", RoomOwner = "", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "109", RoomName = "CBE Graduates Office", RoomOwner = "TA", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "110", RoomOwner = "ROTC", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "111", RoomName = "L3 Lab", RoomOwner = "L3", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "112A", RoomName = "Physics Lab", RoomOwner = "Physics Dept", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "113", RoomName = "Robotics Lab", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "201", RoomOwner = "J. McGough", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "202", RoomName = "WISE", RoomOwner = "Carlson & Folsland", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "203A", RoomOwner = "L. Pyeatt", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "203B", RoomOwner = "K. Brauman", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "203C", RoomOwner = "K. Caudle", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "204", RoomOwner = "TA", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "205", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "208", RoomOwner = "L. Rebenitsch", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "209", RoomOwner = "P. Hinker", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "210", RoomName = "MCS Conference Room", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.ConferenceRoom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211A", RoomOwner = "TA Office", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211B", RoomOwner = "TA Office", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211C", RoomOwner = "K. Corwin", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211D", RoomName = "USGS EROS Data Center", RoomOwner = "G. Schmidt", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211E", RoomOwner = "W. Leonard", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "211F", RoomOwner = "ACM", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "213", RoomName = "MCS Student Project Lab", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "214", RoomOwner = "Schrader", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "215", RoomName = "OPP Lab", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "302", RoomOwner = "J. Dahl", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "303", RoomName = "MCS Library", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.ConferenceRoom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "304", RoomName = "150 Tablet PC Lab", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Lab },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "306", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "308", RoomName = "Reta", RoomOwner = "", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "308A", RoomOwner = "Riley", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "309", RoomOwner = "V. Manes", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "310", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "311", RoomOwner = "J. Weiss", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "312", RoomOwner = "M. Qiao", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "313", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Classroom },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "314A", RoomOwner = "R. Johnson", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "314B", RoomOwner = "M. Garlick", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "314C", RoomOwner = "P. Grieve", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "314D", RoomOwner = "T. Kowlaski", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "315", RoomOwner = "C. Karlsson", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "316A", RoomOwner = "M. Richard-Greer", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "316B", RoomOwner = "B. Deschamp", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "316D", RoomOwner = "D. Bienert", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "316E", RoomOwner = "P. Fleming", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
                new RoomModel { RoomID = Guid.NewGuid(), RoomNumber = "316F", RoomOwner = "D. Teets", Building = "McLaury", Campus = "Main Campus", City = "Rapid City", Type = RoomType.Office },
            };

            // seed room data if room table is empty
            if (context.Rooms.ToList().Count == 0)
            {
                roomList.ForEach(s => context.Rooms.AddOrUpdate(p => p.RoomID, s));
                context.SaveChanges();
            }
        }
    }
}

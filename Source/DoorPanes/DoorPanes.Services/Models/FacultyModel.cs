using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoorPanes.Services.Models
{
    public class FacultyModel : PersonModel
    {
        public RoomModel Office { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
    }
}
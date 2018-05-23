using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.Models
{
    public class StaffModel : PersonModel
    {
        public RoomModel Office { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
    }
}

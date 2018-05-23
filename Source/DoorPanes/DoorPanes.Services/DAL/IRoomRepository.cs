using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.DAL
{
    public interface IRoomRepository
    {
        IEnumerable<RoomModel> GetAllRooms();
        void UpdateRoom(RoomModel room);
        void Save();
    }
}

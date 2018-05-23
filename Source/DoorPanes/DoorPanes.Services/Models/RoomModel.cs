using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoorPanes.Services.Models
{
    public class RoomModel
    {
        [Key]
        public Guid RoomID { get; set; }

        public string RoomNumber { get; set; }
        public string RoomName { get; set; }
        public string RoomOwner { get; set; }
        public string Building { get; set; }
        public string Campus { get; set; }
        public string City { get; set; }
        public RoomType Type { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RoomModel()
        {
            RoomID = Guid.NewGuid();
        }

        /// <summary>
        /// SDSMT Class constructor.
        /// </summary>
        /// <param name="number"></param>
        public RoomModel(string roomNumber, string owner, RoomType type)
        {
            RoomID = Guid.NewGuid();
            RoomNumber = roomNumber;
            RoomOwner = owner;
            Building = "McLaury";
            Campus = "South Dakota School of Mines and Technology";
            City = "Rapid City";
            Type = type;
        }
    }

    public enum RoomType
    {
        Classroom,
        ConferenceRoom,
        Lab,
        Office
    }
}
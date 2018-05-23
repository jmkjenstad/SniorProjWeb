using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.DAL
{
    public class RoomRepository : IRoomRepository, IDisposable
    {
        // instance of database context
        private ApplicationDbContext context;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="context"></param>
        public RoomRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Method to return all the room events.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoomModel> GetAllRooms()
        {
            return context.Rooms.ToList();
        }

        /// <summary>
        /// Updates a room model.
        /// </summary>
        /// <param name="room">The new room data to update.</param>
        public void UpdateRoom(RoomModel room)
        {
            if (room == null)
                return;

            // update room model?
            context.Entry(room).State = EntityState.Modified;
        }

        /// <summary>
        /// Method to save the database context changes.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

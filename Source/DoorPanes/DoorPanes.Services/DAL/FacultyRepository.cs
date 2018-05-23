using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.DAL
{
    public class FacultyRepository :IFacultyRepository, IDisposable
    {
        // database context
        private ApplicationDbContext context;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="context"></param>
        public FacultyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Method to return all faculty members.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FacultyModel> GetAllFacultyMembers()
        {
            return context.Faculty.ToList();
        }

        /// <summary>
        /// Method to save the database context.
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

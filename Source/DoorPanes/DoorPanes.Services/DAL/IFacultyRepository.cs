using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.DAL
{
    public interface IFacultyRepository
    {
        IEnumerable<FacultyModel> GetAllFacultyMembers();
        void Save();
    }
}

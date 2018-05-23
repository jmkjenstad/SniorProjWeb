using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoorPanes.Services.Models
{
    // code from: https://code.msdn.microsoft.com/Getting-Started-with-dd0e2ed8

    public abstract class PersonModel
    {
        [Key]
        public Guid PersonID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters.")]
        public string FirstName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public string Username { get; set; }

        public PersonType Type { get; set; }
    }

    public enum PersonType
    {
        Student,
        Faculty,
        Staff,
        Unknown
    }
}
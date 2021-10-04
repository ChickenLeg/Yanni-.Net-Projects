using System;
using System.ComponentModel.DataAnnotations;

namespace Lab3.Models
{
    public class Person
    {
        [Required]
        [StringLength(100)]
        public String FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public String LastName { get; set; }
        public int Age { get; set; }
        public String Email { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }
        public String Password { get; set; }
        public String Description { get; set; }
    }
}

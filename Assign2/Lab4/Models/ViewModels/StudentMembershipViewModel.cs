using System.Collections.Generic;

namespace Lab4.Models.ViewModels
{
    public class StudentMembershipViewModel
    {
        public List<Student> Student { get; set; }
        public List<Community> Memberships { get; set; }
    }
}

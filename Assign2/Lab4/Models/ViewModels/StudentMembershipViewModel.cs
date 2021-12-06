using System.Collections.Generic;

namespace Lab4.Models.ViewModels
{
    public class StudentMembershipViewModel
    {
        public string CommunityId { get; set; }
        public string Title { get; set; }
        public bool IsMember { get; set; }

        public List<Student> Student { get; set; }
        public List<Community> Memberships { get; set; }
    }
}

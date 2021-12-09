using System.Collections.Generic;

namespace Lab4.Models.ViewModels
{
    public class CommunityMembershipViewModel
    {
        public string CommunityId { get; set; }
        public string Title { get; set; }
        public bool IsMember { get; set; }
        public Student Student { get; set; }
        public List<StudentMembershipViewModel> Membership { get; set; }
    }
}
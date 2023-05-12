using System.Collections.Generic;

namespace WebAppMain.Models
{
    public class VisitingListVM
    {
        public List<Visiting> Visiting { get; set; }
        public IEnumerable<Halls> Halls { get; set; }
        public IEnumerable<Visitors> Visitors { get; set; }
        public IEnumerable<Services> Services { get; set; }
        public IEnumerable<Computer> Computer { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string SearchString { get; set; }
        public string constantFilter { get; set; }
    }
}

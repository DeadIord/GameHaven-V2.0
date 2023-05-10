using System;

namespace WebAppMain.Models
{
    public class AddUserViewModel
    {

        public DateTime DateAndTimeOfTheVisit { get; set; }
        public DateTime DateAndTimeOfTheVisitEnd { get; set; }
        public string Status { get; set; }
        public int NumberOfHours { get; set; }

        public int ServicecId { get; set; }
        public int HallsId { get; set; }
        public int ComputerId { get; set; }
        public int VisitorsId { get; set; }
        public int VisitingId { get; set; }
        public string Name { get; set; }

        public string Addres { get; set; }
        public string ApplicationUserId { get;set; }

        public DateTime? DateOfBirth { get; set; }

    }
}


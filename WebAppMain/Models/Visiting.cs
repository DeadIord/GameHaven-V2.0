
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Xml.Linq;

namespace WebAppMain.Models
{
    public class Visiting
    {
        public int VisitingId { get; set; }
        public DateTime DateAndTimeOfTheVisit { get; set; }
        public DateTime DateAndTimeOfTheVisitEnd { get; set; }
        public string Status { get; set; }
        public int NumberOfHours { get; set; }
        public double TotalCost { get; set; }

        public int ServicecId { get; set; }
        public int HallsId { get; set;}
        public int ComputerId { get;set; }
        public int? VisitorsId { get; set; }
        public string ApplicationUserId { get; set; }

        public Visitors Visitor { get; set; }
        public Services Services { get; set; }
        public Halls Halls { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}

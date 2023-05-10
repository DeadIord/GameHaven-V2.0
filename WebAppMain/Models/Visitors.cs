using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppMain.Models
{
    public class Visitors
    {
        public int VisitorsId { get; set; }
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; }

        public string Addres { get; set; }

        public string Constant { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Visiting Visiting { get; set; }
    }
}

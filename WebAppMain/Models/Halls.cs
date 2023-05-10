using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppMain.Models
{
     public class Halls
    {
        public int HallsId { get; set; }
        [Display(Name = "Зал")]
        [Required(ErrorMessage = "Зал не указан")]
        public string HallsName { get; set;}


        public ICollection<Visiting> Visitings { get; set; }
        public ICollection<Computer> Computers { get; set; }

    }
}

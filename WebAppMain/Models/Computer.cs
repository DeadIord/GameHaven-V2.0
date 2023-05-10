using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebAppMain.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }

        [Display(Name = "Название компьютера")]
        [Required(ErrorMessage = "Не указано название компьютера")]
        public string ComputerName { get; set; }
        [Display(Name = "Характеристики")]
        [Required(ErrorMessage = "Не указаны характеристики")]
        public string CompanyName { get; set; }
        [Display(Name = "дата обслуживания")]
        [Required(ErrorMessage = "Не указана дата обслуживания")]
        public DateTime DateOfLastService { get; set; }
        public int HallsId { get; set; }
        public virtual Halls Halls { get; set; }
    }
}

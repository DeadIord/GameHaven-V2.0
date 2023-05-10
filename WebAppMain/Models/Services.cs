using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace WebAppMain.Models
{
    public class Services
    {
        public int ServicesId { get; set; }
        [Display(Name = "Услуги")]
        [Required(ErrorMessage = "Услуга не указана")]
        public string NameOfTheService { get; set; }
        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Стоимость не указана")]
        public string PricePerService { get; set; }

        public ICollection<Visiting> Visitings { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebAppMain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public byte[] ProfilePicture { get; set; }
        public DateTime Date_birth { get; set; }
        public ICollection<Visiting> Visitings { get; set; }
    }
}

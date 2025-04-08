using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainResApp.Models
{
    public class RegisterUserDto
    {
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
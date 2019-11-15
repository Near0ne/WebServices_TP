using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices_TP.Models
{
    public class Account
    {
        [Key]
        public string UserId { get; set; }

        public string UserGender {get; set;}

        public string UserFirstName {get; set;}

        public string UserLastName {get; set;}

        public DateTime UserBirthdate {get; set;}

        public string UserAddress {get; set;}

        public string UserPassword { get; set; }

        public decimal UserAmount { get; set; }
    }
}

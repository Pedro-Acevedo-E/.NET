using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Login.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Contact>? Contacts { get; set; }
    }

    public class Contact
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int UserId { get; set; }
        
        [ValidateNever]
        public User User { get; set; }
    }
}
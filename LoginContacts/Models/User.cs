using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Login.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Contact>? Contacts { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

using static SMS.Data.DataConstants;

namespace SMS.Data.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(MaxUserLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string CartId { get; set; }

        [Required]
        public Cart Cart { get; set; }
    }

    //Has an Id – a string, Primary Key
    //Has a Username – a string with min length 5 and max length 20 (required)
    //Has an Email – a string, which holds only valid email(required)
    //Has a Password – a string with min length 6 and max length 20 - hashed in the database(required)
    //Has a Cart – a Cart object (required)
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static Git.Data.DataConstants;

namespace Git.Data.Models
{
    public class User
    {
        [Key]
        [MaxLength(IdMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Repository> Repositories { get; init; } = new List<Repository>();

        public ICollection<Commit> Commits { get; init; } = new List<Commit>();
    }

    //Has an Id – a string, Primary Key
    //Has a Username – a string with min length 5 and max length 20 (required)
    //Has an Email - a string (required)
    //Has a Password – a string with min length 6 and max length 20  - hashed in the database(required)
    //Has Repositories collection – a Repository type
    //    Has Commits collection – a Commit type
}

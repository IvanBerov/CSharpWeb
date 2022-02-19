using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static SharedTrip.Data.DataConstants;

namespace SharedTrip.Data.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(MaxLengthModels)]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<UserTrip> UserTrips { get; set; } = new HashSet<UserTrip>();
    }
}

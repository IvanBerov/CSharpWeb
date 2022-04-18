using System;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.ViewModels
{
    public class TripsAddFormModel
    {
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string DepartureTime { get; set; }

        public string ImagePath { get; set; }

        [Range(2, 7)]
        public int Seats { get; set; }


        [StringLength(80, ErrorMessage = "Description must be less 80 characters")]
        public string Description { get; set; }
    }
}

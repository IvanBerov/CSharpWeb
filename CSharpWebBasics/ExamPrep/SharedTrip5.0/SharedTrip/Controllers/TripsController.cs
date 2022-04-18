using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Data.Models;
using SharedTrip.Services;
using SharedTrip.ViewModels;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        public readonly IValidator validator;

        public readonly ApplicationDbContext data;

        public TripsController(IValidator _validator, ApplicationDbContext _data)
        {
            this.validator = _validator;
            this.data = _data;
        }

        [Authorize]
        public HttpResponse Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(TripsAddFormModel model)
        {
            var errors = validator.IsValidTripFormModel(model);

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                Description = model.Description,
                Seats = model.Seats,
                ImagePath = model.ImagePath
            };

            DateTime date;

            DateTime.TryParseExact(
                model.DepartureTime,
                "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);

            trip.DepartureTime = date;

            data.Trips.Add(trip);

            data.SaveChanges();

            return this.Redirect("All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var allTrips = data.Trips
                .Select(t => new TripsAllModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    Seats = t.Seats,
                    DepartureTime = t.DepartureTime,
                    Description = t.Description,
                }).ToList();

            return this.View(allTrips);
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var errors = new List<string>();

            var currentTrip = data.Trips
                .Any(t => t.Id == tripId);

            if (!currentTrip)
            {
                errors.Add("Trip does not exist!");

                return View("Error", errors);
            }

            var existingTrip = data.Trips
                .Where(t => t.Id == tripId)
                .Select(t => new TripDetailsViewModel
                {
                    Id = t.Id,
                    ImagePath = t.ImagePath,
                    Description = t.Description,
                    EndPoint = t.EndPoint,
                    StartPoint = t.StartPoint,
                    Seats = t.Seats,
                    DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                })
                .FirstOrDefault();

            return this.View(existingTrip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var errors = new List<string>();

            var currentTrip = data.Trips
                .Any(t => t.Id == tripId);

            if (!currentTrip)
            {
                errors.Add("Trip does not exist!");

                return View("Error", errors);
            }

            var userTrip = new UserTrip
            {
                TripId = tripId,
                UserId = this.User.Id
            };

            if (data.UserTrips.Any(ut => ut.UserId == this.User.Id && ut.TripId == tripId))
            {
                errors.Add("You are already in the trip!");

                return View("/Error", errors);
            }

            data.UserTrips.Add(userTrip);

            data.SaveChanges();

            return Redirect("/");
        }
    }
}

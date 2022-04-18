﻿using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Cars;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly IValidator validator;
        private readonly IUserService users; 
        private readonly ApplicationDbContext data;

        public CarsController(IValidator _validator, IUserService _users, ApplicationDbContext _data)
        {
            this.validator = _validator;
            this.users = _users;
            this.data = _data;
        }

        public HttpResponse All()
        {
            var carsQuery = this.data
                .Cars
                .AsQueryable();

            if (this.users.IsMechanic(this.User.Id))
            {
                carsQuery = carsQuery
                    .Where(c => c.Issues.Any(i => !i.IsFixed));
            }
            else
            {
                carsQuery = carsQuery
                    .Where(c => c.OwnerId == this.User.Id);
            }

            var cars = carsQuery
                .Select(c => new CarListViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Image = c.PictureId,
                    PlateNumber = c.PlateNumber,
                    RemainingIssues = c.Issues.Count(i => !i.IsFixed),
                    FixedIssues = c.Issues.Count(i => i.IsFixed)
                })
                .ToList();

            return View(cars);
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (this.users.IsMechanic(this.User.Id))
            {
                return Error("Mechanics cannot add cars.");
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCarFormModel model)
        {
            var modelErrors = this.validator.ValidateCar(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var car = new Car
            {
                Model = model.Model,
                Year = model.Year,
                PictureId = model.Image,
                PlateNumber = model.PlateNumber,
                OwnerId = this.User.Id
            };

            data.Cars.Add(car);

            data.SaveChanges();

            return Redirect("/Cars/All");
        }
    }
}

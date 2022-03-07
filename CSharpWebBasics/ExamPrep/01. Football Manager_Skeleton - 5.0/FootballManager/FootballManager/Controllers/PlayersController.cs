using System.Collections.Generic;
using System.Linq;
using FootballManager.Data;
using FootballManager.Data.Models;
using FootballManager.Services;
using FootballManager.ViewModels.Players;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace FootballManager.Controllers
{
    public class PlayersController : Controller
    {
        public readonly IValidator validator;

        public readonly FootballManagerDbContext data;

        public PlayersController(IValidator _validator, FootballManagerDbContext _data)
        {
            this.validator = _validator;
            this.data = _data;
        }

        [Authorize]
        public HttpResponse All()
        {
            var allPlayers = data.Players
                .Select(p => new PlayersAddAllFormModel()
                {
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    FullName = p.FullName,
                    Position = p.Position,
                    Speed = p.Speed,
                    Endurance = p.Endurance
                }).ToList();

            return this.View(allPlayers);
        }

        [Authorize]
        public HttpResponse Collection()
        {
            var collection = this.data.UserPlayers
                .Where(x => x.UserId == User.Id)
                .Select(x => new PlayersCollectionModel
                {
                    Id = x.Player.Id,
                    FullName = x.Player.FullName,
                    ImageUrl = x.Player.ImageUrl,
                    Position = x.Player.Position,
                    Speed = x.Player.Speed,
                    Endurance = x.Player.Endurance,
                    Description = x.Player.Description,
                }).ToList();

            return this.View(collection);
        }

        [Authorize]
        public HttpResponse Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(PlayersAddAllFormModel model)
        {
            var errors = validator.IsValidPlayerFormModel(model);

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            var player = new Player()
            {
                FullName = model.FullName,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                Position = model.Position,
                Speed = model.Speed,
                Endurance = model.Endurance
            };

            data.Players.Add(player);

            data.SaveChanges();

            return this.Redirect("/Players/All");
        }

        [Authorize]
        public HttpResponse AddToCollection(int playerId)
        {
            var player = data.Players.FirstOrDefault(x => x.Id == playerId);

            if (player == null)
            {
                return this.Redirect("Players/All");
            }

            if (this.data.UserPlayers.Any(p => p.UserId == User.Id && p.PlayerId == playerId))
            {
                return this.Error("You already have that player in your collection. Please choose another one.");
            }

            var playerToAdd = new UserPlayer
            {
                UserId = User.Id,
                PlayerId = playerId
            };

            this.data.UserPlayers.Add(playerToAdd);

            this.data.SaveChanges();

            return Redirect("Players/All");
        }

       

        [Authorize]
        public HttpResponse AddPlayerToUser(int playerId)
        {
            var errors = new List<string>();

            var currentPlayer = data.Players
                .Any(p => p.Id == playerId);

            if (!currentPlayer)
            {
                errors.Add("Player does not exist!");

                return View("Error", errors);
            }

            var userPlayer = new UserPlayer()
            {
                PlayerId = playerId,
                UserId = this.User.Id
            };

            if (data.UserPlayers.Any(ut => ut.UserId == this.User.Id && ut.PlayerId == playerId))
            {
                errors.Add("Player already exist!");

                return View("/Error", errors);
            }

            data.UserPlayers.Add(userPlayer);

            data.SaveChanges();

            //return Redirect("/"); //?

            return Redirect("/Players/Collection");
        }

        public HttpResponse RemoveFromCollection(int id)
        {
            var player = this.data.Players
                .FirstOrDefault(c => c.Id == id);

            this.data.Players.Remove(player);

            this.data.SaveChanges();

            return this.Redirect("/Players/Collection");
        }
    }
}

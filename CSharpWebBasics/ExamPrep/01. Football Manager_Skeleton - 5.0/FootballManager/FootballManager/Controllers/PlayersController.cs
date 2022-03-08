using FootballManager.Data;
using FootballManager.Data.Models;
using FootballManager.Services;
using FootballManager.ViewModels.Players;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

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
                .Select(p => new AllPlayersModel()
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    ImageUrl = p.ImageUrl,
                    Position = p.Position,
                    Speed = p.Speed,
                    Endurance = p.Endurance,
                    Description = p.Description,
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
            if (!User.IsAuthenticated)
            {
                return this.Redirect("/Users/Register");
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddPlayerModel model)
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
                Position = model.Position,
                Speed = model.Speed,
                Endurance = model.Endurance,
                Description = model.Description
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

        public HttpResponse RemoveFromCollection(int playerId)
        {
            var player = this.data.Players.Find(playerId);

            if (player == null)
            {
                return this.Redirect("/Players/Collection");
            }

            var removePlayer = this.data.UserPlayers
                .FirstOrDefault(x => x.UserId == User.Id && x.PlayerId == playerId);

            if (removePlayer == null)
            {
                return this.Redirect("/Players/All");
            }

            this.data.UserPlayers.Remove(removePlayer);
            this.data.SaveChanges();

            return this.Redirect("/Players/Collection");
        }
    }
}

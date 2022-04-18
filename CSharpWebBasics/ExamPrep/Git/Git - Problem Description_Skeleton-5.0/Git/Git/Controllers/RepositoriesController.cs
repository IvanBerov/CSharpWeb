using System.Linq;
using Git.Data;
using Git.Data.Models;
using Git.Models.Repositories;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;

using static Git.Data.DataConstants;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public RepositoriesController(IValidator _validator, ApplicationDbContext _data)
        {
            this.validator = _validator;
            this.data = _data;
        }

        public HttpResponse All()
        {
            var repositoryQuery = data
                .Repositories
                .AsQueryable();

            if (this.User.IsAuthenticated)
            {
                repositoryQuery = repositoryQuery
                    .Where(r => r.IsPublic || r.OwnerId == this.User.Id);
            }
            else
            {
                repositoryQuery = repositoryQuery.Where(r => r.IsPublic);
            }

            var repository = repositoryQuery
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new RepositoryListingViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToLocalTime().ToString("F"),
                    CommitsCount = r.Commits.Count()
                })
                .ToList();

            return View(repository);
        }

        [Authorize]
        public HttpResponse Create() => View();

        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateRepositoryFormModel model)
        {
            var modelError = this.validator.ValidateRepository(model);

            if (modelError.Any())
            {
                return Error(modelError);
            }

            var repository = new Repository
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == RepositoryPublicType,
                OwnerId = this.User.Id
            };

            this.data.Repositories.Add(repository);

            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }
    }
}

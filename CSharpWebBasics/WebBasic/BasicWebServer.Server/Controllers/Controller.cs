using System.Runtime.CompilerServices;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Identity;
using BasicWebServer.Server.Responses;

namespace BasicWebServer.Server.Controllers
{
    public class Controller
    {
        private UserIdentity userIdentity;

        protected Request Request { get; set; }

        public Controller(Request request)
        {
            this.Request = request;
        }

        protected UserIdentity User
        {
            get
            {
                if (userIdentity == null)
                {
                    this.userIdentity = this.Request.Session.ContainsKey(Session.SessionUserKey)
                        ? new UserIdentity { Id = this.Request.Session[Session.SessionUserKey] }
                        : new();
                }

                return userIdentity;
            }
        }

        protected void SignIn(string userId)
        {
            Request.Session[Session.SessionUserKey] = userId;
            userIdentity = new UserIdentity { Id = userId };
        }

        protected void SignOut()
        {
            Request.Session.Clear();
            userIdentity = new();
        }

        protected Response Text(string text) => new TextResponse(text);

        protected Response Html(string text) => new HtmlResponse(text);

        protected Response Html(string html, CookieCollection cookies)
        {
            var response = new HtmlResponse(html);

            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    response.Cookies.Add(cookie.Name, cookie.Value);
                }
            }

            return response;
        }

        protected Response BadRequest() => new BadRequestResponse();

        protected Response Unauthorized() => new UnauthorizedResponse();

        protected Response NotFound() => new NotFoundResponse();

        protected Response Redirect(string location) => new RedirectResponse(location);

        protected Response File(string fileName) => new FileResponse(fileName);

        protected Response View([CallerMemberName] string viewName = "")
            => new ViewResponse(viewName, GetControllerName());
        protected Response View(object model, [CallerMemberName] string viewName = "")
            => new ViewResponse(viewName, GetControllerName(), model);

        private string GetControllerName()
            => this.GetType()
                .Name
                .Replace(nameof(Controller), string.Empty);
    }
}

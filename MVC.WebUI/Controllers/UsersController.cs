namespace MVC.WebUI.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Core.Exceptions;
    using Models.Users;
    using Services.Account.Transfer;
    using Services.Account;

    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: /users
        public ActionResult Index()
        {
            var model = new UsersIndexViewModel();
            var response = this.userService.GetUsers(null);

            if (response.Status == StatusCode.OK)
            {
                model.Users = response.Users.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

            return View(model);
        }

        // GET: /users/create
        public ViewResult Create()
        {
            return View();
        }

        // POST: /users/create
        [HttpPost]
        public ActionResult Create(UsersCreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = new CreateUserRequest
                {
                    User = new UserDto
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        Email = model.Email
                    }
                };

                var response = this.userService.CreateUser(request);

                if (response.Status == StatusCode.OK)
                {
                    this.TempData["SuccessMessage"] = string.Format("You have successfully added '{0} {1}'.", request.User.FirstName, request.User.LastName);
                    return this.RedirectToAction("Index");
                }
                //else if (response.Status == StatusCode.BadRequest)
                //{
                //    this.ModelState.AddModelError(string.Empty, $"Your account was not created for the following reason: {this.GetErrorMessage(response.CreateAccountStatus)}");
                //}
                else
                {
                    this.ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            return View(model);
        }
    }
}
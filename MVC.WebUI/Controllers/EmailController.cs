namespace MVC.WebUI.Controllers
{
    using System.Web.Mvc;

    public class EmailController : Controller
    {
        // GET: /email/activateaccount
        public ViewResult ActivateAccount(string token, string name)
        {
            ViewBag.Token = token;
            ViewBag.Name = name;
            return this.View();
        }

        // GET /email/password
        public ViewResult Password()
        {
            return this.View();
        }
    }
}
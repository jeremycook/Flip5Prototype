using BrockAllen.MembershipReboot.Mvc.Areas.UserAccount.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrockAllen.MembershipReboot.Mvc.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        UserAccountService userAccountService;

        public RegisterController(UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel { EmailIsUsername = userAccountService.Configuration.EmailIsUsername });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(RegisterInputModel model)
        {
            model.EmailIsUsername = userAccountService.Configuration.EmailIsUsername;

            if (ModelState.IsValid)
            {
                try
                {
                    var account = this.userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    ViewData["RequireAccountVerification"] = this.userAccountService.Configuration.RequireAccountVerification;
                    return View("Success", model);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(string foo)
        {
            try
            {
                this.userAccountService.RequestAccountVerification(User.GetUserID());
                return View("Success");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult Cancel(string id)
        {
            try
            {
                bool closed;
                this.userAccountService.CancelVerification(id, out closed);
                if (closed)
                {
                    return View("Closed");
                }
                else
                {
                    return View("Cancel");
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View("Error");
        }
    }
}

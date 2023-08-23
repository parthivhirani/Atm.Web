using ATM.Web.Repository;
using ATM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ATM.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                var result = _userRepository.CreateUser(registerViewModel);
                if (result != null)
                {
                    return View("RegistrationSuccess", result);
                    //return View("Index", "Home");
                }
                ViewBag.Error = "User can't registered";
                return View(registerViewModel);
            }
            return View(registerViewModel);
        }
    }
}

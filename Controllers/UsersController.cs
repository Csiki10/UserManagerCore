using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using UserManagerCore.Helpers;
using UserManagerCore.Models;
using UserManagerCore.Repositories;
using UserManagerCore.ViewModels;

namespace UserManagerCore.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(context.HttpContext.Request.Query["culture"]))
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(context.HttpContext.Request.Query["culture"]);
            }
            base.OnActionExecuting(context);
        }

        [HttpGet]
        [AuthorizeUser]
        public IActionResult Index()
        {
            var users = _userRepository.ReadUsersFromFile();
            return View(users);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var res = _userRepository.LoginUser(model.Username, model.Password);

                if (res.State == LoginState.Succes)
                {
                    HttpContext.Session.SetString("Username", model.Username);
                    return RedirectToAction("Index", "Users");
                }

                // Invalid login attempt
                ModelState.AddModelError("", res.Error);
                return View(model);
            }
            catch (Exception e)
            {
                // to errorhandling
                Console.WriteLine(e.Message);
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        [AuthorizeUser]
        public IActionResult SaveToXml()
        {
            try
            {
                _userRepository.SaveToXml();
                return Ok();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Users");
            }   
        }

        [HttpGet]
        [AuthorizeUser]
        public IActionResult Edit(int id)
        {
            try
            {
                var user = _userRepository.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                var model = new EditUserViewModel
                {
                    ID = user.ID,
                    Username = user.Username,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    DateOfBirth = user.DateOfBirth,
                    PlaceOfBirth = user.PlaceOfBirth,
                    PlaceOfResidence = user.PlaceOfResidence
                };

                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Users");
            }
            
        }

        [HttpPost]
        [AuthorizeUser]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Itt lehetne szebb DTO / egyéb átadás konverzió
                _userRepository.EditUser(new UserModel
                {
                    ID = model.ID,
                    Username = model.Username,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    DateOfBirth = model.DateOfBirth,
                    PlaceOfBirth = model.PlaceOfBirth,
                    PlaceOfResidence = model.PlaceOfResidence
                });

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Users");
            }      
        }

        public IActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            var users = _userRepository.ReadUsersFromFile();
            return Json(users.ToDataSourceResult(request));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

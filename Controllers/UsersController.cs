using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserRepository userRepo, ILogger<UsersController> logger)
        {
            _userRepository = userRepo;
            _logger = logger;
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

                if (res.State == LoginState.InvalidPassword)
                {
                    ModelState.AddModelError("Password", res.Error);

                }

                if (res.State == LoginState.InvalidUserName)
                {
                    ModelState.AddModelError("Username", res.Error);
                }

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while logging in user {Username}", model.Username);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }

        [AuthorizeUser]
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
                _logger.LogError(e, "An error occurred while saving users to XML.");
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
                _logger.LogError(e, "An error occurred while retrieving user {UserId} for editing.", id);
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
                _logger.LogError(e, "An error occurred while updating user {UserId}.", model.ID);
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

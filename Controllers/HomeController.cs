using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSharpBeltExamSajaAlhimiary.Models;

namespace CSharpBeltExamSajaAlhimiary.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult RegisterForm(User user)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(i => i.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username is already in Use! Try Another one");
                    return View("Register");
                }
                else
                {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", user.UserId); // add seesion
                return RedirectToAction("Hobby");
                }
            }
            else
            {
            return View("Register");
            }
        }

         [HttpGet("login")]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult LoginForm(LoginUser log_in)
        {
            if(ModelState.IsValid)
            {
                User db_user = _context.Users.FirstOrDefault(i => i.Username == log_in.LoginUsername);
                if(db_user == null)
                {
                    ModelState.AddModelError("LoginUsername", "Invalid Login!");
                    return View("Login");
                }
                else
                {
                    PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(log_in, db_user.Password, log_in.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Invalid Login!");
                        return View("Login");
                    }
                    else
                    {
                    HttpContext.Session.SetInt32("userId", db_user.UserId); // add session
                    return RedirectToAction("Hobby");
                    }
            }    }
            else
            { 
                return View("Login");
            }
        }

        [HttpGet("Hobby")]
        public IActionResult Hobby()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if(userId == null)
            {
                return RedirectToAction("Login");
            }
            List<Hobby> allHobbies = _context.Hobbies
                .Include( e => e.Creator)
                .Include( e => e.Enthus)
                .ThenInclude( a => a.User)
                .ToList();
            return View("Hobby", allHobbies);
        }

        [HttpGet("Hobby/New")]
        public IActionResult NewHobby()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if(userId == null)
            {
                return RedirectToAction("Login");
            }
            User user = _context.Users.FirstOrDefault(i => i.UserId == userId);
            return View("NewHobby");
        }

        [HttpPost("CreateHobby")]
        public IActionResult CreateHobby(Hobby H)
        {
            if(ModelState.IsValid)
            {
                if(_context.Hobbies.Any(i => i.Name == H.Name))
                {
                    ModelState.AddModelError("Name", "This hobby already exists! Try Another one!");
                    return View("NewHobby");
                }
                H.UserId = (int)HttpContext.Session.GetInt32("userId");
                _context.Hobbies.Add(H);
                _context.SaveChanges();
                return RedirectToAction("Hobby");
            }
            else
            {
                return View("NewHobby");
            }
        }

        [HttpGet("Hobby/{id}")]
        public ViewResult Details(int id)
        {
            Hobby thisHobby = _context.Hobbies
            .Include( h => h.Enthus)
            .ThenInclude( a => a.User)
            .FirstOrDefault(h => h.HobbyId == id);
            return View("Details", thisHobby);
        }

        [HttpGet("Hobby/Add/{id}")]
        public RedirectToActionResult AddHobby(int id)
        {
            Enthusiast e = new Enthusiast();
            e.UserId = (int)HttpContext.Session.GetInt32("userId");
            e.HobbyId = id;
            _context.Enthusiasts.Add(e);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

        [HttpGet("Hobby/Drop/{id}")]
        public RedirectToActionResult DropHobby(int id)
        {
            Enthusiast e = _context.Enthusiasts.FirstOrDefault( a => a.HobbyId == id && a.UserId == (int)HttpContext.Session.GetInt32("userId"));
            _context.Enthusiasts.Remove(e);
            _context.SaveChanges();
            return RedirectToAction("Details");
        }

        [HttpGet("Hobby/Edit/{id}")]
        public ViewResult EditHobby(int id)
        {
            Hobby h = _context.Hobbies.FirstOrDefault(h => h.HobbyId == id);

            return View("EditHobby", h);
        }

        [HttpPost("Hobby/ProcessEdit/{id}")]
        public IActionResult ProcessEdit(int id, Hobby EditedHobby)
        {
            Hobby h = _context.Hobbies.FirstOrDefault(h => h.HobbyId == id);
            if(ModelState.IsValid)
            {
                h.Name = EditedHobby.Name;
                h.Describtion = EditedHobby.Describtion;
                h.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return Redirect($"/Hobby/{h.HobbyId}");
            }
            else
            {
                return View("EditHobby", h);
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}


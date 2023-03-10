
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {

        private readonly IUserProfileRepository _profileRepo;

        public UserProfileController (IUserProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }
        // GET: UserProfileController

        [Authorize(Roles ="admin")]
        public ActionResult Index()
        {
            List<UserProfile> users = _profileRepo.GetAllUsers();

            return View(users);
        }

        // GET: UserProfileController/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult Details(int id)
        {
            UserProfile user = _profileRepo.GetById(id);
            if (user == null)
            {
                return NotFound(             );
            }
            return View(user);
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserProfile user)
        {
            try
            {
                _profileRepo.AddUser(user);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View(user);
            }
        }

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TabloidMVC.Repositories;
using TabloidMVC.Models;
using System;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            List<Category> categories = _categoryRepo.GetAll();

            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepo.AddCategory(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            Category category = _categoryRepo.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Owners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepo.UpdateCategory(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }
        // GET: Owners/Delete/5
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepo.GetCategoryById(id);

            return View(category);
        }

        // POST: Owners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepo.DeleteCategory(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
            }
        }
    }
}

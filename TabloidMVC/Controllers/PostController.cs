using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
   
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        //GET Edit/Id
        public IActionResult Edit(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, userId);


            var vm = new PostEditViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            vm.Post = post;
                       if (post == null) 
            { return NotFound(); }
            return View(vm);
        }

        //POST Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);
                return RedirectToAction("Details", new {Id = @Id });
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        //GET
        public IActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, userId);
            if (post == null)
            { return NotFound(); }
            return View(post);
        }

        //POST 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }
    }
}

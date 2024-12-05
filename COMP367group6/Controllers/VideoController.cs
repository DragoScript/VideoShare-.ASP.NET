using COMP367group6.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using COMP367group6.Models;
using Microsoft.EntityFrameworkCore;


namespace COMP367group6.Controllers
{
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Upload(Video video)
        {
            if (ModelState.IsValid)
            {
                // Insert data into SQL Server
                var newEntry = new Video
                {
                    Title = video.Title,
                    Thumbnail = video.Thumbnail,
                    Url = video.Url,
                    UploadDate = video.UploadDate,
                    User = video.User
                };

                _context.Videos.Add(newEntry);
                _context.SaveChanges();

                // If success Redirect to a home page.
                return RedirectToAction("Index");

            }

            // If validation fails, return to the same view
            return View(video);
        }

        public IActionResult Player(Video video)
        {
            var VideoModel = new Video
            {
                Title = video.Title,
                Url = video.Url,
                Thumbnail = video.Thumbnail,
                UploadDate = video.UploadDate,
                User = video.User
            };

            return View(VideoModel);
        }
    }
}

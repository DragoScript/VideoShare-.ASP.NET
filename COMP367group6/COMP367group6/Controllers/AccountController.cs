using COMP367group6.Data;
using Microsoft.AspNetCore.Mvc;
using COMP367group6.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace COMP367group6.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Password hashing method
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Password verification method
        private bool VerifyPassword(string hashedPassword, string password)
        {
            var parts = hashedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash == hashed;
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View();
                }

                // Hash password before saving
                user.Password = HashPassword(user.Password);

                // Add user to database and save changes
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            var dbUser = _context.Users
                .FirstOrDefault(u => u.Username == user.Username);

            if (dbUser != null && VerifyPassword(dbUser.Password, user.Password))
            {
                // Store the user ID in the session
                HttpContext.Session.SetInt32("UserId", dbUser.Id);

                // Update last login date
                dbUser.LastLoginDate = DateTime.UtcNow;
                _context.SaveChanges();

                return RedirectToAction("Index", "Movies");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace J_Myers_Atlast_TaskFlow.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Store the returnUrl in ViewData to pass it to the view
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            // Store the returnUrl in ViewData to pass it to the view in case of an error
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter both a username and password.";
                return View();
            }

            // User Personas (All use password: Password123!)
            var normalizedUser = username.Trim().ToLower();
            string? role = null;
            string? displayName = null;

            //  Check the password and assign roles and display names based on the username
            if (password == "Password123!")
            {
                //  Assign roles and display names based on the username
                switch (normalizedUser)
                {
                    case "zenora":
                        role = "Specialist";
                        displayName = "Zenora Jenkins (Prior Auth Specialist)";
                        break;
                    case "alex":
                        role = "Specialist";
                        displayName = "Alex Rivera (Prior Auth Specialist)";
                        break;
                    case "tyree":
                        role = "Management";
                        displayName = "Tyree Vance (Management)";
                        break;
                    case "deasia":
                        role = "SurgicalCoordinator";
                        displayName = "De’Asia King (Surgical Coordinator)";
                        break;
                    case "jacoby":
                        role = "Admissions";
                        displayName = "Jacoby Montana (Admissions Secretary)";
                        break;
                    case "joann":
                        role = "DenialSpecialist";
                        displayName = "Joann Morales (Denial Specialist)";
                        break;
                }
            }

            //  If the role is null, it means the login attempt was invalid
            if (role == null)
            {
                ViewBag.ErrorMessage = "Invalid login attempt. Please check your credentials.";
                return View();
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, normalizedUser),
                new Claim("DisplayName", displayName!),
                new Claim(ClaimTypes.Role, role)
            };

            // Create a ClaimsIdentity and sign in the user
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Authorizations");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and clear the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            // Return the AccessDenied view to inform the user that they do not have permission to access the requested resource.
            return View();
        }
    }
}
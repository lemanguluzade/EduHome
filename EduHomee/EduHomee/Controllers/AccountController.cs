using EduHomee.Helper;
using EduHomee.Models;
using EduHomee.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;

namespace EduHomee.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser>userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser>signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user= await _userManager.FindByNameAsync(loginVM.Username); //name-i yoxlasin
            if(user==null)
            {
                ModelState.AddModelError("","Username or Password is wrong");
                return View();
            }
            if(user.IsDeactive)
            {
                ModelState.AddModelError("", "Your Account is deactive");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult= await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.IsRemember,true);
                                                                                                              //burada true yazmaq (lockoutOnFailure) o demekdir
                                                                                                             //ki,biz Startup'da 5 qoymusuqsa burda da ele gotursun
                                                                                                            //yeni,5 defe sehv yazdiqda bloklansin....
            if (signInResult.IsLockedOut)
            {
                    ModelState.AddModelError("", "Your Account is blocked");
                    return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            AppUser newUser = new AppUser
            {
                UserName = registerVM.Username,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);  //error-lari gosterir
                }                        //1-ci dirnaq bos qaldi cunki errorun neye aid oldugu bilinmir.
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
            await _signInManager.SignInAsync(newUser, registerVM.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Create Roles
        //public async Task CreateRoles() 
        //{
        //    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole {Name= Roles.Admin.ToString() });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
        //    }   
        //}
        #endregion
    }
}

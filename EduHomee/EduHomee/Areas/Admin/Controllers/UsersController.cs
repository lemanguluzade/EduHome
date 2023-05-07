using EduHomee.Helper;
using EduHomee.Models;
using EduHomee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHomee.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<AppUser> _signInManager;
        public UsersController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager
                                 //SignInManager<AppUser> signInManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> dbUsers = await _userManager.Users.ToListAsync();
            List<UserVM> usersVm = new List<UserVM>();
            foreach (AppUser dbUser in dbUsers)
            {
                UserVM userVm = new UserVM
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
                    Surname = dbUser.Surname,
                    Username = dbUser.UserName,
                    Email = dbUser.Email,
                    IsDeactive = dbUser.IsDeactive,
                    Role=(await _userManager.GetRolesAsync(dbUser))[0], 
                };
                usersVm.Add(userVm);
            }
            return View(usersVm);
        }


        #region Create
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString(),
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM userVM,string role)
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString(),
            };
            AppUser newUser = new AppUser
            {
                UserName = userVM.Username,
                Name = userVM.Name,
                Surname = userVM.Surname,
                Email = userVM.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, userVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);  //error-lari gosterir
                }                        //1-ci dirnaq bos qaldi cunki errorun neye aid oldugu bilinmir.
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, role);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            if (dbUser.IsDeactive)
            {
                dbUser.IsDeactive = false;
            }
            else
            {
                dbUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        #endregion


        #region Update
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Surname = dbUser.Surname,
                Email = dbUser.Email,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString(),
            };
            return View(dbUpdateVM);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id,UpdateVM updateVM,string role)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Surname = dbUser.Surname,
                Email = dbUser.Email,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.Member.ToString(),
            };
            dbUser.Name= updateVM.Name;
            dbUser.UserName = updateVM.Username;
            dbUser.Surname = updateVM.Surname;
            dbUser.Email = updateVM.Email;

            if (dbUpdateVM.Role!=role)
            {
               IdentityResult removeIdentityResult= await _userManager.RemoveFromRoleAsync(dbUser, dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(dbUser, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }

            await _userManager.UpdateAsync(dbUser);
          return RedirectToAction("Index");
        }
        #endregion



    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Data;
using NextwoIdentity.Models.ViewModels;

namespace NextwoIdentity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        
        
        #region Configration

        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<IdentityUser> _userManager,
           SignInManager<IdentityUser> _SignInManage,
           RoleManager<IdentityRole> _RoleManager)

        {
            userManager = _userManager;
            signInManager = _SignInManage;
            roleManager = _RoleManager;
        }
        #endregion

        #region Users
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.EMail,
                    Email = model.EMail,
                    PhoneNumber = model.Phone

                };
                var result = await userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync
                    (model.Email!, model.Password!, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid User Or Password");

                return View(model);

            }
            return View();
        }

        public  async Task <IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

        #endregion

        #region Role

        [Authorize(Roles ="admin")]
        public IActionResult CreateRole()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
               IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                var result=await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");   
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);


            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult RolesList()
        {

            return View(roleManager.Roles);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");

            }
            var role=await roleManager.FindByIdAsync(id);   
            if(role == null)
            {
                return RedirectToAction("RolesList");

            }
            EditRoleViewModel Model = new EditRoleViewModel
            {

                RoleId = role.Id,
                RoleName = role.Name
            };

            foreach(var user in userManager.Users)
            {if(await userManager.IsInRoleAsync(user,role.Name!))
                {
                    Model.Users!.Add(user.UserName!);
                }
            }


            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.RoleId!);

                if (role == null)
                {
                    return RedirectToAction(nameof(ErrorPage));

                }
                role.Name = model.RoleName;
                var result=await roleManager.UpdateAsync(role);
                if(result.Succeeded) {
                    return RedirectToAction(nameof(RolesList));

                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }


            return View(model);
        }

        public IActionResult ErrorPage()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ModifyUserInRole(string id)
        {
            if (id == null) { return RedirectToAction(nameof(RolesList)); }

            var role= await roleManager.FindByIdAsync(id);

            if(role== null)
            {
                return RedirectToAction(nameof(ErrorPage));
            }
            List<UserRoleViewModel> models = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users) {

               UserRoleViewModel userRoles = new UserRoleViewModel
                {
                  UserId= user.Id,  UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name!))
                {

                  userRoles.IsSelected = true;
                }
                else
                {
                    userRoles.IsSelected = false;
                }
                models.Add(userRoles);

            }
            return View(models);
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> ModifyUsersInRole(string id, List<UserRoleViewModel> models)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(RolesList));
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction(nameof(ErrorPage));
            }
            IdentityResult result = new IdentityResult();
            for (int i = 0; i < models.Count; i++)
            {

                var user = await userManager.FindByIdAsync(models[i].UserId!);
                if (models[i].IsSelected && (!await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.AddToRoleAsync(user!, role.Name!);
                }
                else if (!models[i].IsSelected && (await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);
                }

            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RolesList));
            }
            return View(models);

        }



      
    }

}

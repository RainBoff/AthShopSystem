using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using asp_mvc_std_v2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using asp_mvc_std_v2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_mvc_std_v2.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoleController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public void FixEmptyRoles()
        {
            var Users = _context.Users;
            var UsersAndRoles = GetUsersAndRoles();
            foreach (var userU in Users)
            {
                bool haverole = false;
                foreach (var userUR in UsersAndRoles.userList)
                {
                    if(userU.Id == userUR.UserId && userUR.role is not null ){ haverole = true; }
                }

                if (!haverole)
                {
                   var UR = new IdentityUserRole<string>();
                    UR.RoleId = "User";
                    UR.UserId = userU.Id;
                    _context.UserRoles.Add(UR);
                    _context.SaveChanges();
                }
            }

        }
        public UsersAndRoles GetUsersAndRoles()
        {
            var UsersAndRoles = new UsersAndRoles();
            foreach(var user in _context.Users)
            {
                var User = new User();
                User.UserId = user.Id;
                User.UserName = user.UserName;
                foreach (var roleid in _context.UserRoles.Where(x => x.UserId == User.UserId)) 
                {
                    var Role = new Role();

                    if (roleid.UserId == user.Id) 
                    {
                        Role.roleID = roleid.RoleId;
                        Role.roleName = _context.Roles.Where(x => x.Id == Role.roleID).FirstOrDefault().Name;
                    }
                    User.role = Role;
                }
              

                foreach (var shopid in _context.UsersAndShops.Where(x => x.UserId == User.UserId))
                {
                    var Shop = new Shop();

                    if (shopid.UserId == user.Id)
                    {
                        Shop.ShopId = shopid.ShopId;
                        if (_context.Sklep.Where(x => x.Id == Shop.ShopId).FirstOrDefault() is null) { Shop.ShopName = "Brak"; }
                        else
                        Shop.ShopName = _context.Sklep.Where(x => x.Id == Shop.ShopId).FirstOrDefault().Nazwa;
                    }
                    User.shop = Shop;
                }
                UsersAndRoles.userList.Add(User);
            }
            foreach(var role in _context.Roles) 
            {
                var Role = new Role();
                Role.roleID = role.Id;
                Role.roleName = role.Name;
                UsersAndRoles.rolesList.Add(Role);
            }

            foreach (var shop in _context.Sklep)
            {
                var Shop = new Shop();
                Shop.ShopId = shop.Id;
                Shop.ShopName = shop.Nazwa;
                UsersAndRoles.shopList.Add(Shop);
            }


            return UsersAndRoles;
        }

        
        public UsersAndRoles UpdateUsersAndRoles(UsersAndRoles usersAndRoles)
        {
            
            foreach (var user in usersAndRoles.userList)
            {
                //_context.UserRoles.Where(x => x.UserId == user.UserId).FirstOrDefault().RoleId = user.role.roleID;
                var newUserRole = new IdentityUserRole<string>();
                var newUserShop = new UsersAndShops();
                newUserShop.ShopId = user.shop.ShopId;
                newUserShop.UserId = user.UserId;
                newUserRole.RoleId = user.role.roleID;
                newUserRole.UserId = user.UserId;

                var oldUserShop = _context.UsersAndShops.Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (oldUserShop is not null)
                {
                    _context.UsersAndShops.Remove(oldUserShop);
                    _context.SaveChanges();
                }

                var oldUserRole = _context.UserRoles.Where(x => x.UserId == user.UserId).FirstOrDefault();
                if(oldUserRole is not null){
                    _context.UserRoles.Remove(oldUserRole);
                    _context.SaveChanges();
                }
                _context.UserRoles.Add(newUserRole);
                _context.UsersAndShops.Add(newUserShop);
                _context.SaveChanges();
            }

            _context.SaveChanges();
            
            return usersAndRoles;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var Roles = _context.Roles.ToList();
            return View(Roles);
        }

        public IActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(IdentityRole Role)
        {
            _context.Roles.Add(Role);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles="Admin")]
        public IActionResult AdminPage()
        {
            var usersAndRoles = GetUsersAndRoles();
            foreach (var UR in usersAndRoles.userList.Where(x => x.role is null))
            {
                var newRole = new Role();
                newRole.roleID = "User";
                newRole.roleName = "User";
                UR.role = newRole;
            }
            foreach (var UR in usersAndRoles.userList.Where(x => x.shop is null))
            {
                var newShop = new Shop();
                newShop.ShopId = 0;
                newShop.ShopName = "Brak";
                UR.shop = newShop;
            }



            return View(UpdateUsersAndRoles(usersAndRoles));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RoleManagerPage()
        {
            return View();
        }

        public ActionResult Roles(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserWithRoles =  GetUsersAndRoles().userList.Where(x => x.UserId == id).FirstOrDefault();
           

            if (UserWithRoles == null)
            {
                return NotFound();
            }
            return View(UserWithRoles);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserManagerPage()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserPage()
        {
            var Users = _context.Users.ToList();         
            return View(Users);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoles(int id, [Bind("UserId,roleName")] User user) 
        {
            var usersAndRoles = GetUsersAndRoles();
            usersAndRoles.userList.Where(x => x.UserId == user.UserId).FirstOrDefault().role = user.role;

            return RedirectToAction("AdminPage", UpdateUsersAndRoles(usersAndRoles));
        }



        [Authorize(Roles ="Test")]
        public IActionResult TestPage()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersAndRoles = GetUsersAndRoles();

            var User = usersAndRoles.userList.Where(x => x.UserId == id).FirstOrDefault();
            usersAndRoles.userList.Clear();
            usersAndRoles.userList.Add(User);
            
            


            return View(usersAndRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string uuserId, string rroleId)
        {

            var BDUsersAndRoles = GetUsersAndRoles();
            foreach(var UR in BDUsersAndRoles.userList.Where(x=>x.role is null)) 
            {
                var newRole = new Role();
                newRole.roleID = "User";
                newRole.roleName = "User";
                UR.role = newRole;
            } 
            
            
                BDUsersAndRoles.userList.Where(x => x.UserId == uuserId).FirstOrDefault().role.roleID = rroleId;
                BDUsersAndRoles.userList.Where(x => x.UserId == uuserId).FirstOrDefault().role.roleName = BDUsersAndRoles.rolesList.Where(x => x.roleID == rroleId).FirstOrDefault().roleName;
            
                return View("AdminPage", UpdateUsersAndRoles(BDUsersAndRoles));
        }
        
    
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditShop(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersAndRoles = GetUsersAndRoles();

            var User = usersAndRoles.userList.Where(x => x.UserId == id).FirstOrDefault();
            usersAndRoles.userList.Clear();
            usersAndRoles.userList.Add(User);

            return View(usersAndRoles);
        }

     
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShop(string userId, int shopId)
        {

            var BDUsersAndRoles = GetUsersAndRoles();
            foreach (var UR in BDUsersAndRoles.userList.Where(x => x.shop is null))
            {
                var newShop = new Shop();
                newShop.ShopId = 0;
                newShop.ShopName = "Brak";
                UR.shop = newShop;
            }
         

            BDUsersAndRoles.userList.Where(x => x.UserId == userId).FirstOrDefault().shop.ShopId = shopId;
            BDUsersAndRoles.userList.Where(x => x.UserId == userId).FirstOrDefault().shop.ShopName = BDUsersAndRoles.shopList.Where(x => x.ShopId == shopId).FirstOrDefault().ShopName;

            return View("AdminPage", UpdateUsersAndRoles(BDUsersAndRoles));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RegisterRole()
        {
            ViewBag.Name = new SelectList(_context.Roles.ToList(), "Name", "Name");
            ViewBag.UserName = new SelectList(_context.Users.ToList(), "UserName", "UserName");
            return View();
        }


      /*  // POST: /Account/RegisterRole
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterRole(RegisterViewModel model,)
        {
            var userId = _context.Users.Where(i => i.UserName == user.UserName).Select(s => s.Id);
            string updateId = "";
            foreach (var i in userId)
            {
                updateId = i.ToString();
            }

            //assign roles
            await this.UserManager.AddToRoleAsync(updateId, model.Name);
            return RedirectToAction("Index", "Home");
        }

        */
    }
}

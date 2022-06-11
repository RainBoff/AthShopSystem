using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Models
{
    
    public class Role 
    {
        public string roleID;
        public string roleName;
    }

    public class User
    {
        public string UserId;
        public string UserName;
        public Role role;
        public Shop shop;
    }

    public class Shop 
    {
        public int ShopId;
        public string ShopName;
    }
    public class UsersAndRoles
    {
        public List<User> userList;
        public List<Role> rolesList;
        public List<Shop> shopList;
        public UsersAndRoles() 
        {
            userList = new List<User>();
            rolesList = new List<Role>();
            shopList = new List<Shop>();
        }
    }
}

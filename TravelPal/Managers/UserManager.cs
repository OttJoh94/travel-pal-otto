using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; } = new()
        {
            new User("user", "password", Country.Australien),
            new Admin("admin", "password", Country.Australien)
        };
        public static IUser? SignedInUser { get; set; }

        public static bool AddUser(IUser user)
        {
            return true;
        }
        public static void RemoveUser(IUser user)
        {

        }

        public static bool UpdateUsername(IUser user, string username)
        {
            return true;
        }

        private static bool ValidateUsername(string username)
        {
            return true;
        }

        public static bool SignInUser(string username, string password)
        {
            foreach (var user in Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    SignedInUser = user;
                    return true;
                }
            }
            return false;
        }
    }
}

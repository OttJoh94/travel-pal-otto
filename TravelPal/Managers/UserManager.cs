using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; } = new();
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

        public static bool SigInUser(string username, string password)
        {
            return true;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class UserManager
    {
        public static List<IUser> Users { get; set; } = new()
        {
            new User("user", "password", Country.Sweden),
            new Admin("admin", "password", Country.Sweden)
        };
        public static IUser? SignedInUser { get; set; }

        //Vet inte varför denna måste vara en bool?
        public static bool AddUser(IUser user)
        {
            Users.Add(user);
            return true;
        }
        public static void RemoveUser(IUser user)
        {

        }

        public static bool UpdateUsername(IUser user, string username)
        {
            return true;
        }

        //Returnerar false om användarnamnet är upptaget.
        //Gjorde om till public för att kunna validera redan när det skrivs in så man får feedback direkt istället för när man klickar på "Register"
        public static bool ValidateUsername(string username)
        {
            foreach (var user in Users)
            {
                if (user.Username == username)
                {
                    return false;
                }
            }

            if (username.Count() < 3)
            {
                return false;
            }
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

        //Returnerar true om password är minst 5 characters.
        public static bool ValidatePassword(string password)
        {
            return password.Count() > 4;
        }
    }
}

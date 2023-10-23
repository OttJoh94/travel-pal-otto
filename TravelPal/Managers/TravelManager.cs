using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class TravelManager
    {
        public static List<Travel> Travels { get; set; } = new()
        {
            new Travel("Sydney", Country.Australia, 4, new List<PackingListItem>(), (User)UserManager.Users.Find(User => User.Username == "user")!, new System.DateTime(2023, 06, 05), new System.DateTime(2023, 06, 12)),
            new Travel("Berlin", Country.Germany, 2, new List<PackingListItem>(), (User)UserManager.Users.Find(User => User.Username == "user")!, new System.DateTime(2023, 06, 01), new System.DateTime(2023, 06, 15)),
        };

        public static void AddTravel(Travel travel)
        {

        }

        public static void RemoveTravel(Travel travel)
        {

        }

    }
}

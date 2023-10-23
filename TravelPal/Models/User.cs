using System.Collections.Generic;

namespace TravelPal.Models
{
    public class User : IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Country Location { get; set; }

        public List<Travel> Travels { get; set; } = new();

        public User(string username, string password, Country location)
        {
            Username = username;
            Password = password;
            Location = location;
        }
    }
}

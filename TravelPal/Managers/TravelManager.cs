﻿using System;
using System.Collections.Generic;
using System.Windows;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public static class TravelManager
    {
        public static Travel SelectedTravel { get; set; }
        public static List<Travel> Travels { get; set; } = new()
        {
            new Vacation("Sydney", Country.Australia, 4, new List<PackingListItem>() {new TravelDocument("Passport", true), new OtherItem("Candy", 20)}, (User)UserManager.Users.Find(User => User.Username == "user")!, new System.DateTime(2023, 06, 05), new System.DateTime(2023, 06, 12), true),
            new WorkTrip("Berlin", Country.Germany, 2, new List<PackingListItem>() {new TravelDocument("Passport", false), new OtherItem("Socks", 4)}, (User)UserManager.Users.Find(User => User.Username == "user")!, new System.DateTime(2023, 06, 01), new System.DateTime(2023, 06, 15), "Gotta do stuff"),
        };

        public static void AddTravel(Travel travel)
        {
            Travels.Add(travel);
        }

        public static void RemoveTravel(Travel travel)
        {
            Travels.Remove(travel);
        }

        public static void UpdateTravel(Travel travel)
        {
            //Hittar rätt objekt. Kommer vara den som selectas när man klickar in på details
            Travel selectedTravel = Travels.Find(t => t.Destination == SelectedTravel.Destination);
            Travels.Remove(selectedTravel);
            AddTravel(travel);
            SelectedTravel = travel;

        }

        public static bool PassportRequired(IUser user, Country country)
        {
            //IF bor utanför EU, pass required
            if (!IsEuropeanCountry(user.Location))
            {
                return true;
            }
            //IF bor i EU men reser utanför, pass required
            else if (!IsEuropeanCountry(country))
            {
                return true;
            }
            //IF bor i EU och reser inanför, pass inte required
            else
            {
                return false;
            }

        }

        private static bool IsEuropeanCountry(Country country)
        {
            foreach (EuropeanCountry europeanCountry in Enum.GetValues(typeof(EuropeanCountry)))
            {
                if (europeanCountry.ToString() == country.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool TryingToRemovePassport(PackingListItem item)
        {
            if (item.Name == "Passport")
            {
                MessageBox.Show("You can't remove your passport", "Warning");
                return true;
            }
            return false;
        }
    }
}

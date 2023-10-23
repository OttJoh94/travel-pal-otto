using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class Travel
    {

        public string Destination { get; set; }
        public Country Country { get; set; }
        public int Travellers { get; set; }
        public List<PackingListItem> PackingList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TravelDays { get; set; }

        public Travel(string destination, Country country, int travellers, List<PackingListItem> packingList, DateTime startDate, DateTime endDate)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            PackingList = packingList;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays(startDate, endDate);
        }

        private int CalculateTravelDays(DateTime startDate, DateTime endDate)
        {
            int days = startDate.DayOfYear - endDate.DayOfYear;
            return days;
        }

        public virtual string GetInfo()
        {
            return "";
        }
    }
}

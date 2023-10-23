using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class Vacation : Travel
    {
        public bool AllInclusive { get; set; }

        public Vacation(string destination, Country country, int travellers, List<PackingListItem> packingList, User user, DateTime startDate, DateTime endDate, bool allInclusive) : base(destination, country, travellers, packingList, user, startDate, endDate)
        {
            AllInclusive = allInclusive;
        }

        public override string GetInfo()
        {
            return base.GetInfo();
        }

        public override object GetLstInfo()
        {
            return new { Destination = Destination, Country = Country.ToString(), Travel = "Vacation", TravelDays = TravelDays };
        }
    }
}

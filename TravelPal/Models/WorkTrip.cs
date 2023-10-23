using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string destination, Country country, int travellers, List<PackingListItem> packingList, User user, DateTime startDate, DateTime endDate, string meetingDetails) : base(destination, country, travellers, packingList, user, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }

        public override string GetInfo()
        {
            return base.GetInfo();
        }

        public override object GetLstInfo()
        {
            return new { Destination = Destination, Country = Country.ToString(), Travel = "Work trip", TravelDays = TravelDays };
        }
    }
}

using System;
using System.Collections.Generic;

namespace TravelPal.Models
{
    public class WorkTrip : Travel
    {
        public string MeetingDetails { get; set; }

        public WorkTrip(string destination, Country country, int travellers, List<PackingListItem> packingList, DateTime startDate, DateTime endDate, string meetingDetails) : base(destination, country, travellers, packingList, startDate, endDate)
        {
            MeetingDetails = meetingDetails;
        }

        public override string GetInfo()
        {
            return base.GetInfo();
        }
    }
}

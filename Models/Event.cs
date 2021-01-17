using System.Collections.Generic;

namespace UltraPlayMarkets.Models
{
    public class Event
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public bool IsLive { get; set; }

        public int CategoryId { get; set; }

        public List<Match> Matches { get; set; }
    }
}

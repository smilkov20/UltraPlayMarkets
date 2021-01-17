using System.Collections.Generic;

namespace UltraPlayMarkets.Models
{
    public class Event
    {
        public string Name { get; set; }

        public string Tournament { get; set; }

        public int Id { get; set; }

        public bool IsLive { get; set; }

        public int CategoryId { get; set; }

        public int SportId { get; set; }

        public List<Match> Matches { get; set; }
    }
}

using System.Collections.Generic;

namespace UltraPlayMarkets.Models
{
    public class Sport
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<Event> Events { get; set; }

    }
}

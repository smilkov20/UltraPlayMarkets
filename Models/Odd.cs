namespace UltraPlayMarkets.Models
{
    public class Odd
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public double Value { get; set; }

        public string SpecialBetValue { get; set; }

        public bool IsGuest { get; set; }

        public int BetId { get; set; }
    }
}

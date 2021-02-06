using System;

namespace IC3000.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public Int16 Year { get; set; }

        public string Name { get; set; }

        public ClaimTypes Type { get; set; }

        public decimal DamageCost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightReservation.Entities
{
    public class Luggage
    {
        private int weight;

        public Luggage(int weight)
        {
            Weight = weight;
        }

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public override string ToString()
        {
            return $"This luggage weight {Weight} kg.";
        }
    }
}

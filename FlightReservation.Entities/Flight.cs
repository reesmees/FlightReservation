using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightReservation.Entities
{
    public class Flight
    {
        private int flightNo;
        private string origin;
        private string destination;
        private int totalLoadCapacity;
        private List<Luggage> luggage;
        private DateTime departureTime;
        private int passengerCapacity;
        private decimal price;

        public Flight(decimal price, int passengerCapacity, DateTime departureTime, int totalLoadCapacity, string destination, string origin, int flightNo)
        {
            Price = price;
            PassengerCapacity = passengerCapacity;
            DepartureTime = departureTime;
            Luggage = new List<Luggage>();
            TotalLoadCapacity = totalLoadCapacity;
            Destination = destination;
            Origin = origin;
            FlightNo = flightNo;
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public int PassengerCapacity
        {
            get { return passengerCapacity; }
            set { passengerCapacity = value; }
        }

        public DateTime DepartureTime
        {
            get { return departureTime; }
            set { departureTime = value; }
        }

        public List<Luggage> Luggage
        {
            get { return luggage; }
            set { luggage = value; }
        }

        public int TotalLoadCapacity
        {
            get { return totalLoadCapacity; }
            set { totalLoadCapacity = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public int FlightNo
        {
            get { return flightNo; }
            set { flightNo = value; }
        }

        public bool AddLuggage(Luggage luggage)
        {
            bool success = true;
            if (luggage.Weight < AvailableLoadCapacity())
            {
                Luggage.Add(luggage);
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }

        public void RemoveLuggage(Luggage l)
        {
            if (Luggage.Exists(x => x == l))
            {
                Luggage.Remove(l);
            }
        }

        public int AvailableLoadCapacity()
        {
            int currentWeight = 0;
            foreach (Luggage l in Luggage)
            {
                currentWeight += l.Weight;
            }
            int currentCapacity = TotalLoadCapacity - currentWeight;
            return currentCapacity;
        }
    }
}

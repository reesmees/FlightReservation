using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightReservation.Entities
{
    public class Ticket
    {
        private decimal totalPrice;
        private decimal moms;
        private bool hasHandLuggage;
        private Flight flight;
        private Passenger passenger;
        private Luggage luggage;
        private int totalWeight;
        private int ticketNo;
        private string date;
        private string time;
        private string travelClass;

        public Ticket(string travelClass, int ticketNo, Luggage luggage, Passenger passenger, Flight flight, bool hasHandLuggage)
        {
            TravelClass = travelClass;
            TicketNo = ticketNo;
            Luggage = luggage;
            Passenger = passenger;
            Flight = flight;
            HasHandLuggage = hasHandLuggage;
            CalculateTotalPrice();
            CalculateTotalWeight();
            Date = flight.DepartureTime.ToString("dd-MM-yyyy");
            Time = flight.DepartureTime.ToString("HH:mm:ss");
        }

        public string TravelClass
        {
            get { return travelClass; }
            set { travelClass = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public int TicketNo
        {
            get { return ticketNo; }
            set { ticketNo = value; }
        }

        public int TotalWeight
        {
            get { return totalWeight; }
            set { totalWeight = value; }
        }

        public Luggage Luggage
        {
            get { return luggage; }
            set { luggage = value; }
        }

        public Passenger Passenger
        {
            get { return passenger; }
            set { passenger = value; }
        }

        public Flight Flight
        {
            get { return flight; }
            set { flight = value; }
        }

        public bool HasHandLuggage
        {
            get { return hasHandLuggage; }
            set { hasHandLuggage = value; }
        }

        public decimal Moms
        {
            get { return moms; }
            set { moms = value; }
        }


        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        public void CalculateTotalPrice()
        {
            decimal price = Flight.Price;
            if (TravelClass == "First Class")
            {
                price = (price * 1.25m) + 1000;
            }
            if (HasHandLuggage)
            {
                price *= 1.1125m;
            }
            Moms = price * 0.25m;
            price += Moms;
            TotalPrice = price;
        }

        public void CalculateTotalWeight()
        {
            int weight = Luggage.Weight;
            if (HasHandLuggage)
            {
                weight += 5;
            }
            TotalWeight = weight;
        }
    }
}

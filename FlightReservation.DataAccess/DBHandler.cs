using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightReservation.Entities;

namespace FlightReservation.DataAccess
{
    public class DBHandler
    {
        private string conString;

        public DBHandler(string conString)
        {
            ConString = conString;
        }

        public string ConString
        {
            get { return conString; }
            set { conString = value; }
        }

        public List<Flight> CheckFlightsByOriginAndDestination(string origin, string destination)
        {
            List<Flight> flights = new List<Flight>();
            DataSet ds = GetDataSet("GetFlights");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int flightID = row.Field<int>("ID");
                string flightOrigin = row.Field<string>("Origin");
                string flightDestination = row.Field<string>("Destination");
                decimal flightPrice = row.Field<decimal>("Price");
                int flightPassengerCapacity = row.Field<int>("PassengerCapacity");
                int flightBaggageCapacity = row.Field<int>("BaggageCapacity");
                DateTime flightTakeOffTime = row.Field<DateTime>("TakeOffTime");
                if (origin == flightOrigin && destination == flightDestination)
                {
                    flights.Add(new Flight(flightPrice, flightPassengerCapacity, flightTakeOffTime, flightBaggageCapacity, flightDestination, flightOrigin, flightID));
                }
            }
            return flights;
        }

        public int GetBaggageCapacity(Flight flight)
        {
            DataSet ds = GetDataSet("GetTickets");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (flight.FlightNo == row.Field<int>("FligtID"))
                {
                    flight.AddLuggage(new Luggage(row.Field<int>("Baggage")));
                }
            }
            return flight.AvailableLoadCapacity();
        }

        public int GetPassengerCapacity(Flight flight)
        {
            int passengerCapacity = flight.PassengerCapacity;
            DataSet ds = GetDataSet("GetTickets");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row.Field<int>("FligtID") == flight.FlightNo)
                {
                    passengerCapacity--;
                }
            }
            return passengerCapacity;
        }

        public DataSet GetDataSet(string procedure)
        {
            DataSet ds = new DataSet();
            using(SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand(procedure, con) { CommandType = CommandType.StoredProcedure })
            {
                con.Open();
                SqlDataAdapter a = new SqlDataAdapter(com);
                a.Fill(ds);
            }
            return ds;
        }

        public void InsertIntoTickets(Ticket ticket)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand("InsertIntoTickets", con) { CommandType = CommandType.StoredProcedure })
            {
                com.Parameters.AddWithValue("@FlightID", ticket.Flight.FlightNo);
                com.Parameters.AddWithValue("@PassengerName", ticket.Passenger.Name);
                com.Parameters.AddWithValue("@Class", ticket.TravelClass);
                com.Parameters.AddWithValue("@HandBaggage", ticket.HasHandLuggage);
                com.Parameters.AddWithValue("@Baggage", ticket.TotalWeight);
                con.Open();
                com.ExecuteNonQuery();
            }
        }

        public int GetTicketCount()
        {
            DataSet ds = GetDataSet("GetTickets");
            int ticketCount = ds.Tables[0].Rows.Count;
            return ticketCount;
        }
    }
}

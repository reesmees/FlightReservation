using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightReservation.DataAccess;
using FlightReservation.Entities;

namespace FlightReservation.Business
{
    class Program
    {
        public static DBHandler handler;
        public static int ticketNo;
        static void Main(string[] args)
        {
            handler = new DBHandler(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlightReservationDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            while (true)
            {
                ticketNo = handler.GetTicketCount() + 1;
                Console.WriteLine("Indtast afg.");
                string origin = Console.ReadLine();
                Console.WriteLine("Indtast ank.");
                string destination = Console.ReadLine();
                List<Flight> eligibleFlights = handler.CheckFlightsByOriginAndDestination(origin, destination);
                if (eligibleFlights.Count() == 0)
                {
                    Console.WriteLine("Der er desværre ingen fly der matcher søgningen.");
                    Console.ReadKey();
                }
                else if (eligibleFlights.Count() == 1)
                {
                    Flight f = eligibleFlights[0];
                    int cap = handler.GetPassengerCapacity(f);
                    Console.WriteLine($"Fly nr. {f.FlightNo} har {cap} ledige pladser.");
                    if (cap >= 1)
                    {
                        Console.WriteLine("Book billet? Y/N");
                        string answer = Console.ReadLine();
                        if (answer.ToLower() == "y")
                        {
                            Ticket t = CreateTicket(f);
                            if (t != null)
                            {
                                PrintTicket(t);
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Booking afbrudt.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Der er desværre ingen ledige pladser på flyet.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    foreach (Flight f in eligibleFlights)
                    {
                        int cap = handler.GetPassengerCapacity(f);
                        Console.WriteLine($"Fly nr. {f.FlightNo} har {cap} ledige pladser.");
                    }
                    Console.WriteLine($"Hvilket fly ønskes booket? Indtast 'Cancel' for at afbrydde.");
                    string answer = Console.ReadLine();
                    if (answer == "Cancel")
                    {
                        Console.WriteLine("Booking afbrudt.");
                    }
                    else
                    {
                        Flight f = eligibleFlights.Find(x => x.FlightNo.Equals(answer));
                        Ticket t = CreateTicket(f);
                        if (t != null)
                        {
                            PrintTicket(t);
                            Console.ReadKey();
                        }
                    }
                }

                Console.Clear();
            }
        }
        
        public static Ticket CreateTicket(Flight flight)
        {
            Console.WriteLine("Indtast kundens navn.");
            string name = Console.ReadLine();
            Console.WriteLine("Indtast vægten på kundens baggage.");
            int.TryParse(Console.ReadLine(), out int weight);
            Console.WriteLine("Ønsker kunden at rejse på første klasse? Y/N");
            string answer = Console.ReadLine();
            string travelClass = "Economy";
            if (answer.ToLower() == "y")
            {
                travelClass = "First Class";
            }
            Console.WriteLine("Ønsker kunden håndbaggage? Y/N");
            answer = Console.ReadLine();
            bool hasHandLuggage = true;
            if (answer.ToLower() == "n")
            {
                hasHandLuggage = false;
                if ((weight + 5) > handler.GetBaggageCapacity(flight))
                {
                    Console.WriteLine("Der er desværre ikke plads til kundens baggage.");
                    Console.ReadKey();
                    return null;
                }
            }
            else
            {
                if (weight > handler.GetBaggageCapacity(flight))
                {
                    Console.WriteLine("Der er desværre ikke plads til kundens baggage.");
                    Console.ReadKey();
                    return null;
                }
            }
            Passenger p = new Passenger(name);
            Luggage l = new Luggage(weight);
            Ticket t = new Ticket(travelClass, ticketNo, l, p, flight, hasHandLuggage);
            handler.InsertIntoTickets(t);
            return t;
        }

        public static void PrintTicket(Ticket t)
        {
            Console.WriteLine("<-----      RYG&REJS BILLET      ----->");
            Console.WriteLine();
            Console.WriteLine($"Fra/til...: {t.Flight.Origin} – {t.Flight.Destination}");
            Console.WriteLine($"Fly.......: {t.Flight.FlightNo}");
            Console.WriteLine($"Dato......: {t.Date} kl.: {t.Time}");
            Console.WriteLine();
            if (t.TravelClass == "First Class")
            {
                Console.WriteLine("Klasse....: 1.: X     alm.:  ");
            }
            else
            {
                Console.WriteLine("Klasse....: 1.:       alm.: X");
            }
            Console.WriteLine();
            Console.WriteLine($"Navn......: {t.Passenger.Name}");
            if (t.HasHandLuggage)
            {
                Console.WriteLine("Håndbagage: ja: X      nej:  ");
            }
            else
            {
                Console.WriteLine("Håndbagage: ja:        nej: X");
            }
            Console.WriteLine($"Baggage...: {t.Luggage.Weight} kg.");
            Console.WriteLine($"Total kg..: {t.TotalWeight} kg.");
            Console.WriteLine();
            Console.WriteLine($"Pris for rejsen...........: {t.Flight.Price} DKK");
            if (t.HasHandLuggage)
            {
                Console.WriteLine($"Håndbagage (11,25%).......: {t.Flight.Price * 0.1125m} DKK");
            }
            else
            {
                Console.WriteLine("Håndbagage (11,25%).......: 0 DKK");
            }
            Console.WriteLine($"Moms......................: {t.Moms} DKK");
            Console.WriteLine();
            Console.WriteLine($"Total pris................: {t.TotalPrice} DKK");
            Console.WriteLine();
            Console.WriteLine($"Billetnr..: {t.TicketNo}");
            Console.WriteLine();
            Console.WriteLine("<-----      RYG&REJS BILLET      ----->");
        }
    }
}

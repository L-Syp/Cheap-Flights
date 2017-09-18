using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Awesomium.Core;

namespace CheapFlights
{
    public abstract class Multisearcher
    {
        protected List<Country> _CountryList = new List<Country>();
        public List<Country> CountryList
        {
            get
            {
                return this._CountryList;
            }
        }
        protected Dictionary<string, string> _PassengersCodes = new Dictionary<string, string>(); //key, web address counterpart
        protected Dictionary<string, string> _CabinClasses = new Dictionary<string, string>(); //key, web address counterpart
        protected string _strLink;
        public enum FlightTypes
        {
            OneWay,
            RoundTrip,            
            MultiCity
        }
        public string GetSource(string link)  //Get the website's source
        {           
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(link);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string strResponse = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return strResponse;
        }
    }

    public sealed class Kayak : Multisearcher
    {
        public static string RegExProgress
        {
            get { return @"aria-valuenow=\""(.*?)"".*"; }
        }
        public static string RegExMinPrice
        {
            get { return @"id=\""low_price\"" class=\""min\"">(.*?)<.span>"; }
        }     

        public Kayak()
        {
            //Kolejnosc w linku klasa/dorosli/(dzieci)
            #region AddCountries
            _CountryList.Add(new Country("Argentina", "ARS", "http://www.kayak.com.ar/flights/"));
            _CountryList.Add(new Country("Australia", "AUD", "http://www.kayak.com.au/flights/"));
            _CountryList.Add(new Country("Belgium", "GBP", "http://www.kayak.ie/flights/"));
            _CountryList.Add(new Country("Brazil", "BRL", "http://www.kayak.com.br/flights/"));
            _CountryList.Add(new Country("Canada", "CAD", "http://www.ca.kayak.com/flights/"));
            _CountryList.Add(new Country("China", "CNY", "http://www.cn.kayak.com/flights/"));
            _CountryList.Add(new Country("Denmark", "DKK", "http://www.kayak.dk/flights/"));
            _CountryList.Add(new Country("Germany", "EUR", "http://www.kayak.de/flights/"));
            _CountryList.Add(new Country("Spain", "EUR", "http://www.kayak.es/flights/"));
            _CountryList.Add(new Country("France", "EUR", "http://www.kayak.fr/flights/"));
            _CountryList.Add(new Country("Greece", "EUR", "http://www.gr.kayak.com/flights/"));
            _CountryList.Add(new Country("Honk Kong", "HKD", "http://www.kayak.com.hk/flights/"));
            _CountryList.Add(new Country("India", "INR", "http://www.kayak.co.in/flights/"));
            _CountryList.Add(new Country("Ireland", "GBP", "http://www.kayak.ie/flights/"));
            _CountryList.Add(new Country("Italy", "EUR", "http://www.kayak.it/flights/"));
            _CountryList.Add(new Country("Japan", "JPY", "http://www.kayak.co.jp/flights/"));
            _CountryList.Add(new Country("Mexico", "MXN", "http://www.kayak.com.mx/flights/"));
            _CountryList.Add(new Country("Netherlands", "EUR", "http://www.kayak.nl/flights/"));
            _CountryList.Add(new Country("New Zealand", "NZD", "http://www.nz.kayak.com/flights/"));
            _CountryList.Add(new Country("Norway", "NOK", "http://www.kayak.no/flights/"));
            _CountryList.Add(new Country("Poland", "PLN", "https://www.kayak.pl/flights/"));
            _CountryList.Add(new Country("Portugal", "EUR", "http://www.kayak.pt/flights/"));
            _CountryList.Add(new Country("Russia", "RUB", "http://www.kayak.ru/flights/"));
            _CountryList.Add(new Country("Switzerland", "CHF", "http://www.kayak.ch/flights/"));
            _CountryList.Add(new Country("Singapour", "SGD", "http://www.kayak.sg/flights/"));
            _CountryList.Add(new Country("Finland", "EUR", "http://www.fi.kayak.com/flights/"));
            _CountryList.Add(new Country("Sweden", "SEK", "http://www.kayak.se/flights/"));
            _CountryList.Add(new Country("Turkey", "TRY", "http://www.kayak.com.tr/flights/"));
            _CountryList.Add(new Country("United Kingdom", "GBP", "http://www.co.uk/flights/"));
            #endregion

            #region PassengersCodes
            _PassengersCodes.Add("Adult", "Adults"); //Liczba przed 18-64
            //_PassengersCodes.Add("Senior", "Adults"); //Liczba przed 65+
            _PassengersCodes.Add("Youth", "-17-"); // 12-17
            _PassengersCodes.Add("Child", "-11-"); // 2-11
            _PassengersCodes.Add("Seat infant", "-1S-");
            _PassengersCodes.Add("Lap intant", "-1L-");
            //jesli jest jakiekolwiek dziecko na koniec linku /children
            #endregion

            #region CabinClasses
            _CabinClasses.Add("Economy", "");
            _CabinClasses.Add("Premium Economy", "premium");
            _CabinClasses.Add("Business", "business");
            _CabinClasses.Add("First", "first");
            #endregion            
        }
                
        public static string FetchWebsite(string pageSource, string regEx)
        {
            Regex r = new Regex(regEx, RegexOptions.Compiled);
            MatchCollection match = r.Matches(pageSource); //Search the site's source to find the elements that match a regular expression
            return (match[0].Groups[1].ToString().Replace("&nbsp", ""));
        }
     

        public string OneWayLink (string DepartureAirport, bool NearbyDeparture, string TargetAiport, bool NearbyTarget, 
                                  int Adults, int Youth, int Children, int SeatInfant, int LapInfant, DateTime DepartureDate, bool FlexibleDates , Country SiteVersion)
        {
            string Link = SiteVersion.WebDomain;

            if (NearbyDeparture == true)
                Link += DepartureAirport + ",nearby-";
            else
                Link += DepartureAirport + "-";
            if (NearbyTarget == true)
                Link += TargetAiport + ",nearby/";
            else
                Link += TargetAiport + "/";
            if (FlexibleDates == true)
                Link += DepartureDate.ToShortDateString() + "-flexible/";
            else
                Link += DepartureDate.ToShortDateString() + "/";
            Link += Adults.ToString() + "adults/";
             if (Youth + Children + SeatInfant + LapInfant != 0)
             {
                 Link += "children-";

                 for (int i = 0; i< Youth; i++)
                 {Link += "-17-";}
                 for (int i = 0; i< Children; i++)
                 {Link += "-11-";}
                 for (int i = 0; i < SeatInfant; i++)
                 {Link += "-1S-";}
                 for (int i = 0; i < LapInfant; i++)
                 {Link += "-1L-";}
             }
             return Link.Replace("--", "-").TrimEnd('-');            
        }
        public string RoundTripLink(string DepartureAirport, bool NearbyDeparture, string TargetAiport, bool NearbyTarget,
                                 int Adults, int Youth, int Children, int SeatInfant, int LapInfant, DateTime DepartureDate, DateTime ReturnDate, bool FlexibleDeparture, bool FlexibleReturn, Country SiteVersion)
        {
            string Link = SiteVersion.WebDomain;

            //Airports
            if (NearbyDeparture == true)
                Link += DepartureAirport + ",nearby-";
            else
                Link += DepartureAirport + "-";
            if (NearbyTarget == true)
                Link += TargetAiport + ",nearby/";
            else
                Link += TargetAiport + "/";

            //Departure date
            if (FlexibleDeparture == true)
                Link += DepartureDate.ToShortDateString() + "-flexible/";
            else
                Link += DepartureDate.ToShortDateString() + "/";

            //Return date
            if (FlexibleReturn == true)
                Link += ReturnDate.ToShortDateString() + "-flexible/";
            else
                Link += ReturnDate.ToShortDateString() + "/";

            //Passengers
            Link += Adults.ToString() + "adults/";
            if (Youth + Children + SeatInfant + LapInfant != 0)
            {
                Link += "children-";

                for (int i = 0; i < Youth; i++)
                { Link += "-17-"; }
                for (int i = 0; i < Children; i++)
                { Link += "-11-"; }
                for (int i = 0; i < SeatInfant; i++)
                { Link += "-1S-"; }
                for (int i = 0; i < LapInfant; i++)
                { Link += "-1L-"; }
            }
            return Link.Replace("--", "-").TrimEnd('-');
        }
        public string MultiCityLink(string DepartureAirportOne, bool NearbyDepartureOne, string TargetAiportOne, bool NearbyTargetOne, string DepartureAirportTwo, bool NearbyDepartureTwo, string TargetAiportTwo, bool NearbyTargetTwo,
                                 int Adults, int Youth, int Children, int SeatInfant, int LapInfant, DateTime DepartureDateOne, DateTime DepartureDateTwo, Country SiteVersion)
        {
            string Link = SiteVersion.WebDomain;

            //Airport no 1
            if (NearbyDepartureOne == true)
                Link += DepartureAirportOne + ",nearby-";
            else
                Link += DepartureAirportOne + "-";
            if (NearbyTargetOne == true)
                Link += TargetAiportOne + ",nearby/";
            else
                Link += TargetAiportOne + "/";

            //Date no 1
             Link += DepartureDateOne.ToShortDateString() + "-flexible/";
            

            //Airport no 2
            if (NearbyDepartureTwo == true)
                Link += DepartureAirportTwo + ",nearby-";
            else
                Link += DepartureAirportTwo + "-";
            if (NearbyTargetTwo == true)
                Link += TargetAiportTwo + ",nearby/";
            else
                Link += TargetAiportTwo + "/";

            //Date no 2
                Link += DepartureDateTwo.ToShortDateString() + "-flexible/";

            //Passengers
            Link += Adults.ToString() + "adults/";
            if (Youth + Children + SeatInfant + LapInfant != 0)
            {
                Link += "children-";

                for (int i = 0; i < Youth; i++)
                { Link += "-17-"; }
                for (int i = 0; i < Children; i++)
                { Link += "-11-"; }
                for (int i = 0; i < SeatInfant; i++)
                { Link += "-1S-"; }
                for (int i = 0; i < LapInfant; i++)
                { Link += "-1L-"; }
            }
            return Link.Replace("--", "-").TrimEnd('-');
        }        
    }
}

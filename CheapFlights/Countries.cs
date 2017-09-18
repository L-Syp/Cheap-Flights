using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheapFlights
{
    public class Country
    {
        protected string _strName;
        protected string _strCurrency;
        protected string _strWebDomain;
        public string Name
        {
            get
            {
                return _strName;
            }
        }
        public string WebDomain
        {
            get
            {
                return _strWebDomain;
            }
        }
    /*    public double GetCurrencyValue(string TargetCurrency)
        {             
            CurrencyConverter Converter = new Zayko.Finance.CurrencyConverter();
            CurrencyData Data = Converter.GetCurrencyData(_strCurrency, TargetCurrency); 
            return Data.Rate;
        }
        public double GetCurrencyValue() //default returns value in PLN
        {
            CurrencyConverter Converter = new Zayko.Finance.CurrencyConverter();
            CurrencyData Data = Converter.GetCurrencyData(_strCurrency, "PLN");
            return Data.Rate;
        }
     */  
        public Country(string Name, string Currency, string WebDomain)
        {         
            _strName = Name;
            _strCurrency = Currency;
            _strWebDomain = WebDomain;
        }    
    }
}
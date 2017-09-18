using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CheapFlights
{
   public static class RegExFetchWebsite
    {
        public static string ShowMinPrice(string pageSource)
        {
            Regex regex = new Regex(@"<span id=""low_price"" class=""min"">(.*)</span><span id=""price_hilo_separ"">", RegexOptions.Compiled);
            MatchCollection match = regex.Matches(pageSource); //Search the site's source to find the elements that match a regular expression
            return (match[0].Groups[1].ToString());
        }
        public static string ShowProgress(string pageSource)
        {
            Regex regex = new Regex(@"aria-valuenow=\""(.*)\""><div class=", RegexOptions.Compiled);
            MatchCollection match = regex.Matches(pageSource); //Search the site's source to find the elements that match a regular expression
            return (match[0].Groups[1].ToString());
        }
    }
}

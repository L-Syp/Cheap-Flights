using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Timers;

namespace CheapFlights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region SetUI
            Kayak searcher = new Kayak();
            foreach (var c in searcher.CountryList)
            {
                ListBoxCountries.Items.Add(c.Name);
            }
            ListBoxCountries.SelectedValue = "Poland";
            CalendarReturn.IsEnabled = false;
            txtBoxCodeTargetAirport.IsEnabled = true;
            txtBoxCodeTargetAirport2.IsEnabled = false;
            txtBoxCodeDepartureAirport.IsEnabled = true;
            txtBoxCodeDepartureAirport2.IsEnabled = false;
            chkBoxFlexDepartureDate.IsEnabled = true;
            chkBoxFlexReturnDate.IsEnabled = false;
            chkBoxFlexDeparture2.IsEnabled = false;
            chkBoxFlexTarget2.IsEnabled = false;
            CalendarDeparture.SelectedDate = DateTime.Today;
            CalendarReturn.SelectedDate = DateTime.Today.AddDays(7);
            CalendarDeparture.DisplayDateStart = DateTime.Today;
            CalendarReturn.DisplayDateStart = DateTime.Today;
            ListBoxCountries.Focus();
            ListBoxCountries.SelectedItem = "Ireland";
            #endregion
            chrome.Source = new Uri("https://www.kayak.de/flights/MLA-GDN,nearby/2015-07-01-flexible");
        }
                
        public static bool onlyNumeric(string text) //Method which ensures user inputs only numeric values into a textbox
        {
            Regex regex = new Regex("[^A-Z.-]+[^a-z.-]+"); //regex that allows numeric input only
            return !regex.IsMatch(text); // 
        }       
      
        
        public void CheckFlightsInExternalBrowser(Kayak.FlightTypes flightType)
        {
            Kayak searcher = new Kayak();
            DateTime DepartureDate = CalendarDeparture.SelectedDate.Value;
            DateTime ReturnDate = CalendarReturn.SelectedDate.Value;

            #region Error Check
            //Error check           
            if (txtBoxCodeDepartureAirport.Text.Length != 3)
            {
                MessageBox.Show("Incorrect departure airport code!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("My error message");
            }
            else if (ListBoxCountries.SelectedItems.Count == 0)
                MessageBox.Show("At least one language version must be selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if ((txtBoxCodeTargetAirport.Text.Length != 3) || (txtBoxCodeDepartureAirport.Text.Length != 3) || (txtBoxCodeTargetAirport2.Text.Length != 3) || (txtBoxCodeDepartureAirport2.Text.Length != 3))
                MessageBox.Show("Incorrect target airport code!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (intAdults.Value < 1)
                MessageBox.Show("The number of the adults cannot be less than 1!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (intAdults.Value + intYouth.Value + intChildren.Value + intSeatInfant.Value + intLapInfant.Value > 6)
                MessageBox.Show("The number of the passengers cannot exceed 6!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (chkBoxAllLanguages.IsChecked.Value == true)
            #endregion
            {
                switch (flightType)
                {
                    case Multisearcher.FlightTypes.OneWay:
                        {
                            for (int i = 0; i < searcher.CountryList.Count; i++)
                            {
                                Process.Start(searcher.OneWayLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value,
                                        Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                        Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, chkBoxFlexDepartureDate.IsChecked.Value, searcher.CountryList[i]));
                            }
                        }
                        break;
                    case Multisearcher.FlightTypes.MultiCity:
                        {
                            for (int i = 0; i < searcher.CountryList.Count; i++)
                            {
                                Process.Start(searcher.MultiCityLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value, txtBoxCodeDepartureAirport2.Text,
                                    chkBoxFlexDeparture2.IsChecked.Value, txtBoxCodeTargetAirport2.Text, chkBoxFlexTarget2.IsChecked.Value, Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                        Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, ReturnDate, searcher.CountryList[i]));
                            }
                        }
                        break;
                    case Multisearcher.FlightTypes.RoundTrip:
                        {
                            for (int i = 0; i < searcher.CountryList.Count; i++)
                            {
                                Process.Start(searcher.RoundTripLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value,
                                        Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                        Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, ReturnDate, chkBoxFlexDepartureDate.IsChecked.Value, chkBoxFlexReturnDate.IsChecked.Value, searcher.CountryList[i]));
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (flightType)
                {
                    case Multisearcher.FlightTypes.OneWay:
                        {
                            foreach (var foo in ListBoxCountries.SelectedItems)
                            {
                                Country result = searcher.CountryList.Find(x => x.Name == foo.ToString());
                                Process.Start(searcher.OneWayLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value,
                                        Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                        Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, chkBoxFlexDepartureDate.IsChecked.Value, result));
                            }
                        }
                        break;
                         case Multisearcher.FlightTypes.RoundTrip:
                        {
                            foreach (var foo in ListBoxCountries.SelectedItems)
                            {
                                Country result = searcher.CountryList.Find(x => x.Name == foo.ToString());
                                Process.Start(searcher.RoundTripLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value,
                                        Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                        Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, ReturnDate, chkBoxFlexDepartureDate.IsChecked.Value, chkBoxFlexReturnDate.IsChecked.Value, result));
                            }
                        }
                        break;
                         case Multisearcher.FlightTypes.MultiCity:
                        {
                            foreach (var foo in ListBoxCountries.SelectedItems)
                            {
                                Country result = searcher.CountryList.Find(x => x.Name == foo.ToString());
                                Process.Start(searcher.MultiCityLink(txtBoxCodeDepartureAirport.Text, chkBoxFlexDeparture.IsChecked.Value, txtBoxCodeTargetAirport.Text, chkBoxFlexTarget.IsChecked.Value, txtBoxCodeDepartureAirport2.Text,
                                     chkBoxFlexDeparture2.IsChecked.Value, txtBoxCodeTargetAirport2.Text, chkBoxFlexTarget2.IsChecked.Value, Convert.ToInt32(intAdults.Value), Convert.ToInt32(intYouth.Value), Convert.ToInt32(intChildren.Value),
                                         Convert.ToInt32(intSeatInfant.Value), Convert.ToInt32(intLapInfant.Value), DepartureDate, ReturnDate, result));
                            }
                        }
                        break;
                }
            }
        }

        public void CheckFlightsSmartWay()
        {
       
        }
                
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          /*  Kayak K = new Kayak();
            MessageBox.Show(K.CountryList[K.CountryList.FindIndex(x => x.Name == "France")].GetCurrencyValue("PLN").ToString());
            

            System.IO.StreamReader sr = new System.IO.StreamReader("strona.html");
             string s = sr.ReadToEnd();
             sr.Close();
             MessageBox.Show(RegExShowMinPrice.ShowMinPrice(s));
         */
            if (rBtnOneWay.IsChecked.Value == true)
            CheckFlightsInExternalBrowser(Kayak.FlightTypes.OneWay); 
            else if (rBtnRoundTrip.IsChecked.Value == true)
            CheckFlightsInExternalBrowser(Kayak.FlightTypes.RoundTrip);
            else if (rBtnMultiCity.IsChecked.Value == true)
                CheckFlightsInExternalBrowser(Kayak.FlightTypes.MultiCity);
        }


        private void txtBoxCodeDepartureAirport_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !onlyNumeric(e.Text);
        }

        private void chkBoxAllLanguages_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxCountries.SelectAll();
        }
        private void chkBoxAllLanguages_Unchecked(object sender, RoutedEventArgs e)
        {
            ListBoxCountries.SelectedIndex = -1;
        }

        private void rBtnOneWay_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "rBtnOneWay":
                    {
                        try
                        {
                            CalendarReturn.IsEnabled = false;
                            txtBoxCodeTargetAirport.IsEnabled = true;
                            txtBoxCodeTargetAirport2.IsEnabled = false;
                            txtBoxCodeDepartureAirport.IsEnabled = true;
                            txtBoxCodeDepartureAirport2.IsEnabled = false;
                            chkBoxFlexDepartureDate.IsEnabled = true;
                            chkBoxFlexReturnDate.IsEnabled = false;
                            chkBoxFlexDeparture2.IsEnabled = false;
                            chkBoxFlexTarget2.IsEnabled = false;
                        }
                        catch { }
                    }
                    break;
                case "rBtnMultiCity":
                    {
                        CalendarReturn.IsEnabled = true;
                        txtBoxCodeTargetAirport.IsEnabled = true;
                        txtBoxCodeTargetAirport2.IsEnabled = true;
                        txtBoxCodeDepartureAirport.IsEnabled = true;
                        txtBoxCodeDepartureAirport2.IsEnabled = true;
                        chkBoxFlexDepartureDate.IsEnabled = false;
                        chkBoxFlexReturnDate.IsEnabled = false;
                        chkBoxFlexDeparture2.IsEnabled = true;
                        chkBoxFlexTarget2.IsEnabled = true;
                    }
                    break;
                case "rBtnRoundTrip":
                    {
                        CalendarReturn.IsEnabled = true;
                        txtBoxCodeTargetAirport.IsEnabled = true;
                        txtBoxCodeTargetAirport2.IsEnabled = false;
                        txtBoxCodeDepartureAirport.IsEnabled = true;
                        txtBoxCodeDepartureAirport2.IsEnabled = false;
                        chkBoxFlexDepartureDate.IsEnabled = true;
                        chkBoxFlexReturnDate.IsEnabled = true;
                        chkBoxFlexDeparture2.IsEnabled = false;
                        chkBoxFlexTarget2.IsEnabled = false;
                    }
                    break;
            }
        }

        private void txtBoxCodeDepartureAirport2_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as TextBox).IsEnabled == false)
            { if ((sender as TextBox).Text.Length != 3)
                (sender as TextBox).Text = "GDN";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = chrome.ExecuteJavascriptWithResult("document.documentElement.outerHTML");
            Regex regex = new Regex(Kayak.RegExProgress, RegexOptions.Compiled);
            MatchCollection match = regex.Matches(s); //Search the site's source to find the elements that match a regular expression

            if (match.Count != 0)
            {
                progressBar.Value = Convert.ToInt32(match[0].Groups[1].ToString().Replace("&nbsp;", ""));
            }
        }
    }
}
    


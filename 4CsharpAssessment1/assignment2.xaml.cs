using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4CsharpAssessment1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class assignment2 : Page
    {
        public assignment2()
        {
            this.InitializeComponent();
        }

        //validation for Errors in the car sales App. Paul Stuart C# Intro Assessment 1 - 1.11.19

        /* PSEUDOCODE
         * INPUT = Customer Fields, Vehicle Price fields
         * PROCESS = subAmout (vehiclePrice - TradeInPrice)
         * PROCESS = gstAmount (subAmount * 0.1)
         * PROCESS = finalAmount (subAmount + GST)
         * OUTPUT = finalAmount
         */
   
        // Save Button Event Handler------------------------------------------------------------------------------------------------------
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //Validation of Customer Name Field being left blank
            if (customerNameTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Error! Please enter a name ");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            //Validation of Customer Phone being left blank
            if (customerPhoneTextBox.Text == "")
            {
                var dialogMessage = new MessageDialog("Error! Please enter a phone number ");
                await dialogMessage.ShowAsync();
                customerPhoneTextBox.Focus(FocusState.Programmatic);
                customerPhoneTextBox.SelectAll();
                return;
            }

            else
            {
                customerPhoneTextBox.IsEnabled = false;
                customerNameTextBox.IsEnabled = false;
                vehiclePriceTextBlock.Focus(FocusState.Programmatic);
            }
            
        }

        //Reset Button Event Handler----------------------------------------------------------------------------------------------------
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            customerPhoneTextBox.Text = "";
            customerNameTextBox.Text = "";
            vehiclePriceTextBox.Text = "";
            tradeTextBox.Text = "";
            subAmountTextBox.Text = "";
            gstTextBox.Text = "";
            finalAmountTextBox.Text = "";
            customerPhoneTextBox.IsEnabled = true;
            customerNameTextBox.IsEnabled = true;
            customerNameTextBox.Focus(FocusState.Programmatic);
            under25Radio.IsChecked = false;
            over25Radio.IsChecked = false;
            insuranceToggle.IsOn = false;
            windowtint150Check.IsChecked = false;
            gpsNav320Check.IsChecked = false;
            deluxsound350Check.IsChecked = false;
            ducoProtection180Check.IsChecked = false;
            vehicleWarrantyCombo.SelectedValue = "1 year at 0%";
        }

        //Toggle Switch for Insurance Event Handler ------------------------------------------------------------------------------------
        private void insuranceToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Toggle Switch is Kept off by default and disables the 2 radio buttons
            if (insuranceToggle.IsOn == false)
            {
                under25Radio.IsEnabled = false;
                over25Radio.IsEnabled = false;
                under25Radio.IsChecked = false;
                over25Radio.IsChecked = false;
            }
            else
            {
                under25Radio.IsEnabled = true;
                over25Radio.IsEnabled = true;
                under25Radio.IsChecked = true;
            }
        }

        //Summary Button Event Handler-----------------------------------------------------------------------------------------------------
        private async void summaryButton_Click(object sender, RoutedEventArgs e)
        {

            //Constants and Variables used in the Summary Button-----------------------------------------------------------------------
            const double GST_RATE = 0.1;
            double subAmount;
            double gstAmount;
            double finalAmount;
            double tradeIn;
            double vehicleWarranty;
            double vehiclePrice;
            double optionalExtras;
            double accidentInsurance;

            //Try Catch for the Vehicle Price, must be numeric
            try
            {
                vehiclePrice = double.Parse(vehiclePriceTextBox.Text);
            }
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! Please enter numerical data only for vehicle Price. " + theException.Message);
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                vehiclePriceTextBox.SelectAll();
                return;

            }

            //Try Catch for the Trade in Price, must be numeric
            try
            {
                tradeIn = double.Parse(tradeTextBox.Text);
            }
            catch (Exception theException)
            {
                var dialogMessage = new MessageDialog("Error! PLease enter numerical data only for the Trade in. " + theException.Message);
                await dialogMessage.ShowAsync();
                tradeTextBox.Focus(FocusState.Programmatic);
                tradeTextBox.SelectAll();
                return;
            }

            // Vehicle Price being <= 0 Validation                     

            if (vehiclePrice <= 0)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a value more than 0 for Vehicle Price");
                await dialogMessage.ShowAsync();
                vehiclePriceTextBox.Focus(FocusState.Programmatic);
                vehiclePriceTextBox.SelectAll();
                return;
            }

            //Trade in Price <= 0 Validation

            if (tradeIn < 0)
            {
                var dialogMessage = new MessageDialog("Error! Please enter a value more than 0 for Trade in Price");
                await dialogMessage.ShowAsync();
                tradeTextBox.Focus(FocusState.Programmatic);
                tradeTextBox.SelectAll();
                return;
            }

            // Vehicle Price < trade in Price Validation

            if (tradeIn > vehiclePrice)
            {
                var dialogMessage = new MessageDialog("Error! Trade in Price Cannot be higher than Vehicle Price");
                await dialogMessage.ShowAsync();
                tradeTextBox.Focus(FocusState.Programmatic);
                tradeTextBox.SelectAll();
                vehiclePriceTextBox.SelectAll();
                return;
            }

            //Sets the Trade in Field to 0 if its left blank

            if (tradeTextBox.Text == null)
            {
                tradeTextBox.Text = "0";
            }

            //Processing of the Final Price and output to TextBlock-----------------------------------------------------------------------------          

            //Methods Called for Warranty, Extras and Insurance
            vehicleWarranty = calcVehicleWarranty(vehiclePrice);
            optionalExtras = calcOptionalExtras();
            accidentInsurance = calcAccidentInsurance(vehiclePrice, optionalExtras);

            subAmount = vehiclePrice - tradeIn;
            subAmountTextBox.Text = subAmount.ToString("C");
            gstAmount = subAmount * GST_RATE;
            gstTextBox.Text = gstAmount.ToString("C");
            finalAmount = subAmount + gstAmount + vehicleWarranty + optionalExtras + accidentInsurance;
            finalAmountTextBox.Text = finalAmount.ToString("C");

            //Output to the Summary Text Box
            string outputMessage = "Summary of Cost Breakdown:\n";

            outputMessage = outputMessage + "Vehicle Warranty Sub Total: " + vehicleWarranty + "\n" + "Optional Extras Sub Total: " +
            optionalExtras + "\n" + "Insurance Sub Total: " + accidentInsurance + "\n" + "GST Sub Total: " + gstAmount + "\n" +
            "Vehicle Price: " + vehiclePrice + "\n" + "Sub Amount:" + subAmount + "\n" + "Total Cost: " + finalAmount + "\n";

            summaryTextBlock.Text = outputMessage.ToString();
        }

        //Method for the optional Extras ---------------------------------------------------------------------------------------------------
        private double calcOptionalExtras()
        {
            double extras = 0;
            const double WINDOWTINT = 150;
            double DUCO = 180;
            double GPS = 320;
            double SOUND = 350;

            //Calculate the Checkboxes chosen and tally up for optionalExtras Variable
            if (windowtint150Check.IsChecked == true)
            {
                extras = extras + WINDOWTINT;
            }
            if (ducoProtection180Check.IsChecked == true)
            {
                extras = extras + DUCO;
            }
            if (gpsNav320Check.IsChecked == true)
            {
                extras = extras + GPS;
            }
            if (deluxsound350Check.IsChecked == true)
            {
                extras = extras + SOUND;
            }
            return extras;

        }

        //Event Handler for Warranty sets default to 0--
        private void vehicleWarrantyCombo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (vehicleWarrantyCombo.SelectedValue == "")
            {
               vehicleWarrantyCombo.SelectedValue = "1 year at 0%";
            }
        }

        //Method for Calculating the Vehicle Warranty---------------------------------------------------------------------------------------
        private  double calcVehicleWarranty(double vehiclePrice)
            {
                const double WARRANTY_2YR = 0.05;
                const double WARRANTY_3YR = 0.10;
                const double WARRANTY_5YR = 0.20;
                double warranty = 0;

                //Calculate the Warranty Ammount when Vehicle Price is Parsed 
                if (vehicleWarrantyCombo.SelectedValue.ToString() == "1 year at 0%")
                {
                    warranty = 0;
                }
                if (vehicleWarrantyCombo.SelectedValue.ToString() == "2 years at 5%")
                {
                    warranty = (vehiclePrice * WARRANTY_2YR);
                }
                if (vehicleWarrantyCombo.SelectedValue.ToString() == "3 years at 10%")
                {
                    warranty = (vehiclePrice * WARRANTY_3YR);
                }
                if (vehicleWarrantyCombo.SelectedValue.ToString() == "5 years at 20%")
                {
                    warranty = (vehiclePrice * WARRANTY_5YR);
                }             
                return warranty;

            }

        //Method for the Insurance---------------------------------------------------------------------------------------------------------
        private double calcAccidentInsurance(double vehiclePrice, double optionalExtras)
        {
            const double INSURANCE_UNDER25 = 0.20;
            const double INSURANCE_OVER25 = 0.10;
            double insurance = 0;

            if (under25Radio.IsChecked == true)
            {
                insurance = ((vehiclePrice + optionalExtras) * INSURANCE_UNDER25);
            }
            if (over25Radio.IsChecked == true)
            {
                insurance = ((vehiclePrice + optionalExtras) * INSURANCE_OVER25);
            }
            return insurance;

        }

        
    }

    }



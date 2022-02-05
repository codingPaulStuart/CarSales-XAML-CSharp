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

namespace assessment1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class assignment1 : Page
    {
        public assignment1()
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

        // Save Button Event Handler
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            customerPhoneTextBox.IsEnabled = false;
            customerNameTextBox.IsEnabled = false;
            vehiclePriceTextBlock.Focus(FocusState.Programmatic);
        }

        //Reset Button Event Handler
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
        }

        //Summary Button Event Handler
        private async void summaryButton_Click(object sender, RoutedEventArgs e)
        {
            const double GST_RATE = 01;
            double subAmount;
            double gstAmount;
            double finalAmount;
            double vehiclePrice;
            double tradeIn;

            //Try Catch for Validation and Errors including:

            /* non-numeric in the customer Phone
             * non-numeric in the vehicle Price
             * non-numeric in the trade in           
             */

           

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

            if (tradeIn <= 0)
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

            //Processing of the Vehicle and Trade in Prices, Error Checking with IF statements

            subAmount = vehiclePrice - tradeIn;
            subAmountTextBox.Text = subAmount.ToString("C");

            //Final Processing from the Summary Button

            gstAmount = subAmount * GST_RATE;
            gstTextBox.Text = gstAmount.ToString("C");

            finalAmount = subAmount + gstAmount;
            finalAmountTextBox.Text = finalAmount.ToString("C");

            summaryTextBlock.Text = "Summary data displayed here";
        }


    }
}

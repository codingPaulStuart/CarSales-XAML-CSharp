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
    public sealed partial class assignment3 : Page
    {
        public assignment3()
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
            vehicleWarrantyCombo.SelectedValue = "Please Choose";
            summaryTextBlock.Text = "";
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

        //Method for Calculating the Vehicle Warranty---------------------------------------------------------------------------------------
        private double calcVehicleWarranty(double vehiclePrice)
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

        //-----------------------------------Binary Search Buttons and Methods Assignment Part 3 -------------------------------------------------//


        //Declare Arrays at Class Level
        string[] vehicleMakes = new string[10];
        int[] customerPhone = new int[10];
        string[] customerName = new string[10];

        //Page Loaded Event for the Arrays
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            customerName[0] = "Jim";
            customerName[1] = "Mary";
            customerName[2] = "Liz";
            customerName[3] = "Rhonda";
            customerName[4] = "Adam";
            customerName[5] = "Liam";
            customerName[6] = "Angela";
            customerName[7] = "Margaret";
            customerName[8] = "Stephan";
            customerName[9] = "Tom";

            vehicleMakes[0] = "Toyota";
            vehicleMakes[1] = "Holden";
            vehicleMakes[2] = "Mitsubishi";
            vehicleMakes[3] = "Ford";
            vehicleMakes[4] = "BMW";
            vehicleMakes[5] = "Mazda";
            vehicleMakes[6] = "Volkswagen";
            vehicleMakes[7] = "Mini";

            customerPhone[0] = 0123456789;
            customerPhone[1] = 1234567890;
            customerPhone[2] = 0466897854;
            customerPhone[3] = 1245689526;
            customerPhone[4] = 0135489756;
            customerPhone[5] = 0123568794;
            customerPhone[6] = 0345687989;
            customerPhone[7] = 0465897135;
            customerPhone[8] = 0456821578;
            customerPhone[9] = 0456897456;

        }

        //Search Name Button, Sequential Search the Names String Array and if there is a match, output the corresponding Customer Phone
        private async void searchNameButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0; // to track position in array 
            bool found = false; // found will be true or false depending if name found
            string criteria = customerNameTextBox.Text.ToUpper();
           
            if (string.IsNullOrEmpty(customerNameTextBox.Text))
            {
                var dialogMessage = new MessageDialog("Please enter a Name into the search box");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            // do sequential search of string array until match found or end of array reached 
            while (!found && counter < customerName.Length)
            { 
                if (criteria == customerName[counter].ToUpper()) // check if the name is found 
                    found = true; 
                else counter++; // if no match move to next element in array 
            }            
            if (counter < customerName.Length) // if a name has been found 
            {
                string output = customerPhone[counter].ToString();
                customerPhoneTextBox.Text = output;
            } 
            else // not found 
            {
                summaryTextBlock.Text = " not found "; 
            }
        }


        //Display Customer Button with corresponding Phone Number
        private void displayCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            string output = "";
            Array.Sort(customerName);
            for (int index = 0; index < customerName.Length; index++) //Loop through Array
            {
                output = output + customerName[index] + " ," + customerPhone[index] + "\n";
            }
            summaryTextBlock.Text = "\n Customer Names \n \n" + output;
        }


        //Delete a Customer Button
        private async void deleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0; //Stores the postion of the array
            bool found = false; //Tracks if a Customer is found

            //Validation Checking for Blank Text Box
            if (string.IsNullOrEmpty(customerNameTextBox.Text)) 
            {
                var dialogMessage = new MessageDialog("Please enter a Customer Name into the search box");
                await dialogMessage.ShowAsync();
                customerNameTextBox.Focus(FocusState.Programmatic);
                customerNameTextBox.SelectAll();
                return;
            }

            //Search Criteria
            string criteria = customerNameTextBox.Text.ToUpper();
            //Sequential Search
            while (!found && counter < customerName.Length)
            {
                if (criteria == customerName[counter].ToUpper())
                    found = true;
                else
                    counter++;
            }
            //IF statement to delete from the array
            if (counter < customerName.Length)
            {
                for (int i = counter; i < customerName.Length -1; i++)
                {
                    customerName[i] = customerName[i + 1]; //Shuffles the array down to the next position in the Names Array
                }
                for (int i = counter; i < customerPhone.Length -1; i++)
                {
                    customerPhone[i] = customerPhone[i + 1]; //Shuffles the array down to the next position in the Phone Array
                }
                Array.Resize(ref customerName, customerName.Length - 1);
                Array.Resize(ref customerPhone, customerPhone.Length - 1);
                deleteCustomerButton_Click(sender, e);
                var dialogMessage = new MessageDialog(criteria + " Name and Phone deleted, list updated " + customerName.Length + customerPhone.Length);
                await dialogMessage.ShowAsync();

                //Display new list after the method is parsed through
                displayCustomerButton_Click(sender, e);
            }
            else
            {
                summaryTextBlock.Text = "Name does not exist to delete";
            }
        }

        //Display Car Makes Button
        private void displayMakesButton_Click(object sender, RoutedEventArgs e)
        {
            string output = "";
            Array.Sort(vehicleMakes);
            for (int index = 0; index < vehicleMakes.Length; index++) //Loop through Array
            {
                output = output + vehicleMakes[index] + "\n";
            }
            summaryTextBlock.Text = "\n Vehicle Makes \n \n" + output;
        }

        //Method for Binary Searching Vehicles
        public static int vehicleBinarySearch(string[] data, string item)
        {
            int min = 0;
            int max = data.Length - 1;
            int mid;
            item = item.ToUpper(); //Stores the item that is being searched against the array

            do
            {
                mid = (min + max) / 2; //This finds the half way point in the array to split it in 2 (binary)
                if (data[mid].ToUpper() == item) //This states that if the item is found return the index mid
                    return mid;
                if (item.CompareTo(data[mid].ToUpper()) > 0) //Checks if the item is in the top hald of the array
                    min = mid + 1; //This means it is in the top half of the array
                else
                    max = mid - 1; //This means it is in the bottom half of the array
            }
            while (min <= max);
            return -1; //-1 is returned if nothing found
        }

        //Binary Search Button for Vehicles in the Array
        private async void binarysearchButton_Click(object sender, RoutedEventArgs e)
        {
            string criteria;
            criteria = insertVehicleTextBox.Text.ToUpper();
            Array.Sort(vehicleMakes); //Sort Data for searching
            displayMakesButton_Click(sender, e);

            //Validation Checking for Blank Text Box
            if (string.IsNullOrEmpty(insertVehicleTextBox.Text))
            {
                var dialogMessage = new MessageDialog("Please enter a Vehicle Name into the search box");
                await dialogMessage.ShowAsync();
                insertVehicleTextBox.Focus(FocusState.Programmatic);
                insertVehicleTextBox.SelectAll();
                return;
            }

            int foundPos = vehicleBinarySearch(vehicleMakes, criteria); //Call the Method for Search ** Check with Julie
            if (foundPos == -1)
            {
                var dialogMessage = new MessageDialog(criteria + " not found ");
                await dialogMessage.ShowAsync();
            }
            else
            {
                var dialogMessage = new MessageDialog(criteria + " found at index " + foundPos);
                await dialogMessage.ShowAsync();
            }
        }

        //Insert Vehicle Button
        private async void insertVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0; //Track the position in Array
            bool found = false; // Tracks the Vehicle found
            string criteria = insertVehicleTextBox.Text.ToUpper();
            
            //Validation Checking for Blank Fields
            if (string.IsNullOrEmpty(insertVehicleTextBox.Text))
            {
                var dialogMessage = new MessageDialog("Please enter a Vehicle Make");
                await dialogMessage.ShowAsync();
                insertVehicleTextBox.Focus(FocusState.Programmatic);
                insertVehicleTextBox.SelectAll();
                return;
            }

            //Sequential Search for string length until a match is found or the end of array is reached
            while (!found && counter < vehicleMakes.Length)
            {
                if (criteria == vehicleMakes[counter].ToUpper())
                    found = true;
                else
                    counter++;
            }
            if (counter < vehicleMakes.Length) //If the searched vehicle is a match, do not add to the array
            {
                summaryTextBlock.Text = criteria + " ALREADY EXISTS";
            }
            else
            {
                Array.Resize(ref vehicleMakes, vehicleMakes.Length + 1); //Resize the Array
                vehicleMakes[vehicleMakes.Length - 1] = insertVehicleTextBox.Text; //Assign the new vehicle to the list
                var dialogMessage = new MessageDialog("Vehicle Makes updated Length " + vehicleMakes.Length);
                await dialogMessage.ShowAsync(); //Display the new Array of Vehicles in the Pop up Dialog

                //Display new list after the method is parsed through
                displayMakesButton_Click(sender, e);

            }
        }

        
    }
}

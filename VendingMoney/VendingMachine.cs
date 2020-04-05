using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMoney;

namespace VendingMoneyAPI
{
    class VendingMachine
    {
        static Dictionary<String, double> PriceList = new Dictionary<string, double>();
        static void Main(string[] args)
        {
            #region Application Banner
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("**********                                                         **********");
            Console.WriteLine("**********                 THE OREOCLE VENDER                      **********");
            Console.WriteLine("**********                                                         **********");
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("*****************************************************************************");
            #endregion Application Banner

            Console.WriteLine("THE MACHINE HAS STARTED UP...\n\n");
            var Vending = new VendingMoney();
            var isAdmin = false;

            Console.WriteLine("Are you and Admin? Y or N ");
            var adminResponse = Console.ReadLine();
            var CoinsForChange = new Dictionary<CoinType, int>();
            PopulatePriceList();
            if (adminResponse == "Y" || adminResponse == "y")
            {
                AdminViewAndEditBalance(Vending, CoinsForChange);
                isAdmin = true;
            }

            #region Display Products
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("**********                                                         **********");
            Console.WriteLine("**********                PRODUCTS AVAILABLE                       **********");
            
            DisplayPriceList();
            #endregion Display Products

            Console.WriteLine("Please Select an Option" );
            var option = Console.ReadLine();

            var UserEnteredCoins = new Dictionary<CoinType, int>();
            var price = GetPriceforSelectedOption(int.Parse(option));

        EnterDetails:
            Console.WriteLine($"Please Enter £{price}... Coins Accepted are 10p, 20p, 50p and £1");

            #region UserSelection
            var userEnteringCoins = true;
            while(userEnteringCoins)
            {
                Console.Write("CoinType:  ");
                var cointype = Console.ReadLine();
                CoinType _coinType = 0;
                
                try
                {
                   _coinType = GetCoinType(cointype);
                }
                catch(InvalidCoinTypeException ex)
                {
                    Console.WriteLine("\nPlease Enter either 10p, 20p, 50p or £1");
                    goto EnterDetails;
                }

                
                Console.Write($"How many {cointype} Coins are you putting in?:  \n");
                var quantity = Console.ReadLine();
                int _quantity = 0;
                if (!int.TryParse(quantity, out _quantity))
                    Console.Write(" Invalid Number");
                if (UserEnteredCoins.ContainsKey(_coinType))
                    UserEnteredCoins[_coinType] += _quantity;
                else
                    UserEnteredCoins.Add(_coinType, _quantity);

                
                var moneyentered = Vending.CalculateTotalAmountFromCoins(UserEnteredCoins);
                if (moneyentered < price)
                {
                    Console.WriteLine($" Please Add £{price - moneyentered}\n");
                    userEnteringCoins = true;
                }
                else
                {
                    userEnteringCoins = false;
                }  
            }
            #endregion 
            
            var UserEnteredMoneyTotal = Vending.CalculateTotalAmountFromCoins(UserEnteredCoins);
            Console.WriteLine($"\nYou Entered {UserEnteredMoneyTotal}");
            
            Console.WriteLine("\n\nEnjoy.. Your Change is being dispensed");
            
            
            var customerChange = Math.Round(Vending.CalculateCustomerChange(price, UserEnteredMoneyTotal), 1);
            var customerChangeCoins = Vending.ReturnCustomerChangeInCoins(customerChange);

            #region Display Transaction Information
            Console.WriteLine("***************************************************");
            Console.WriteLine($"Total Change to be returned: £{customerChange} \n");
            Console.WriteLine($"Actual change given   £{Vending.CalculateTotalAmountFromCoins(customerChangeCoins)}");
            Console.WriteLine("Coins Returned:");
            DisplayCoins(customerChangeCoins);
            #endregion  

            if (isAdmin)
                Console.WriteLine($"Final Money in Vending Machine {Vending.TotalInMachine}");


            Console.ReadLine();

        }
        #region Helper Functions
        /// <summary>Gets the type of the coin.</summary>
        /// <param name="userInput">The user input.</param>
        /// <returns>The cointype</returns>
        /// <exception cref="InvalidCoinTypeException"></exception>
        static CoinType GetCoinType(string userInput)
        {
            switch (userInput)
            {
                case "10p":
                    return CoinType.TEN_Pence;
                case "20p":
                    return CoinType.TWENTY_Pence;
                case "50p":
                    return CoinType.FIFTY_Pence;
                case "£1":
                    return CoinType.POUND;
                default:
                    throw new InvalidCoinTypeException(Resources.Invalid_Coin_Type);
            }
                
        }

        /// <summary>Displays the coins in the entered by the admin.</summary>
        /// <param name="CoinsEntered">The coins entered.</param>
        static void DisplayCoins(Dictionary<CoinType, int>CoinsEntered)
        {
            foreach (var kvp in CoinsEntered)
            {
                Console.WriteLine($"Coin Type: {kvp.Key}    number of {kvp.Key} coins: {kvp.Value}");
            }
        }

        /// <summary>Displays the price list.</summary>
        static void DisplayPriceList()
        {
            Console.WriteLine("\n\tOption\t|\tProduct\t|\tPrice");
            
            var i = 1;
            foreach (var kvp in PriceList)
            {
                Console.WriteLine($"\t{i}\t|\t{kvp.Key}\t|\t{kvp.Value}");
                i++;
            }
        }

        static double GetPriceforSelectedOption(int option)
        {
            switch (option)
            {
                case 1:
                    return PriceList["Snickers"];
                case 2:
                    return PriceList["Mars"];
                case 3:
                    return PriceList["Oreos"];
                case 4:
                    return PriceList["Flapjack"];
                case 5:
                    return PriceList["Haribos"];
                case 6:
                    return PriceList["Laces"];
                default:
                    return 0.0;
            }
        }


        /// <summary>Populates the price list.</summary>
        static void PopulatePriceList()
        {
            PriceList.Add("Snickers", 0.70);
            PriceList.Add("Mars", 0.50);
            PriceList.Add("Oreos", 1.20);
            PriceList.Add("Flapjack", 1.00);
            PriceList.Add("Haribos", 0.90);
            PriceList.Add("Laces", 0.30);
        }

        static void AdminViewAndEditBalance(VendingMoney Vending, Dictionary<CoinType, int> CoinsForChange)
        {
            Console.WriteLine($"The Total money held in the machine is *** {Vending.TotalInMachine} ***");
            Console.WriteLine($"\nThe Breakdown of the the Money in coins is:");
            DisplayCoins(Vending.MoneyInCoins);
            Console.WriteLine("\nWould You Like to Add More Money to the Machine?   Y or N");
            var addCoins = Console.ReadLine();
            if(addCoins == "Y" || addCoins == "y")
            {
                Console.WriteLine("Please Enter some coins");
                Console.WriteLine("Would you Like to Enter Coins Now?");
                Console.WriteLine("\nCOIN TYPES ACCEPTED: 10p, 20p, 50p and £1 ");

                var addingCoins = "Y";
                while (addingCoins == "Y" || addingCoins == "y")
                {
                    Console.Write(" Enter CoinType:  ");
                    var cointype = Console.ReadLine();
                    var _coinType = GetCoinType(cointype);
                    Console.Write($" How many {cointype} Coins are you putting in?:  ");
                    var quantity = Console.ReadLine();
                    int _quantity = 0;
                    if (!int.TryParse(quantity, out _quantity))
                        Console.Write(" Invalid Number");
                    if (CoinsForChange.ContainsKey(_coinType))
                        CoinsForChange[_coinType] += _quantity;
                    else
                        CoinsForChange.Add(_coinType, _quantity);

                    Console.Write(" More Coins to Add?  Y or N");
                    addingCoins = Console.ReadLine();
                }

                Vending.AddMoneyForChange(CoinsForChange);
                Console.WriteLine($"\nThe Total Amount Added is : {Vending.TotalInMachine}");
                DisplayCoins(Vending.MoneyInCoins);
            }
            
            else
            {
                Console.WriteLine($"\nWARNING:No Money was Added, Vending Machine Using Current Balance");
                Console.WriteLine("Displaying Price List\n");
            }
        }
        #endregion
    }
}

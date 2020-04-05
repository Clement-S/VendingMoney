using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMoney;

namespace VendingMoneyAPI
{

    /// <summary>Money for Vending</summary>
    public class VendingMoney
    {

        /// <summary>Gets or sets the total in machine.</summary>
        /// <value>The total in machine.</value>
        public double TotalInMachine { get; set; }

        /// <summary>Gets or sets the money in coins.</summary>
        /// <value>
        ///  The breakdown of the money in coins.
        /// </value>
        public Dictionary<CoinType, int> MoneyInCoins;


        public VendingMoney()
        {
            MoneyInCoins = new Dictionary<CoinType, int>();

            foreach (var cointype in Enum.GetValues(typeof(CoinType)))
            {
                MoneyInCoins.Add((CoinType)cointype, 2);
            }

            TotalInMachine = CalculateTotalAmountFromCoins(MoneyInCoins);

        }

        /// <summary>Add coins to the machine.</summary>
        /// <param name="coins">The number of coins.</param>
        public void AddMoneyForChange(Dictionary<CoinType, int> coins)
        {
            // Check incoming coins
            if (!coins.Any())
            {
                throw new NoCoinsAddedException(Resources.No_Coins_Added);
            }

            this.MoneyInCoins = coins;
            this.TotalInMachine = CalculateTotalAmountFromCoins(coins);
        }

        /// <summary>Calculates the total amount from coins entered.</summary>
        /// <param name="coins">The coins.</param>
        /// <returns>The total value of all the coins entered</returns>
        public double CalculateTotalAmountFromCoins(Dictionary<CoinType, int> coins)
        {
            double totalAmount = 0.00;

            foreach (var coin in coins)
            {
                totalAmount += (double)((int)coin.Key * coin.Value) / 100;
            }

            return totalAmount;
        }

        /// <summary>Registers the amount for each coin type entered.</summary>
        /// <param name="coins">The coins.</param>
        /// <returns>The total amount for each coin type</returns>
        public Dictionary<CoinType, int> RegisterAmountForCoinsDeposited(Dictionary<CoinType, int> coins)
        {
            var SumOfEachCoin = new Dictionary<CoinType, int>();

            foreach (var coin in coins)
            {
                SumOfEachCoin.Add(coin.Key, ((int)coin.Key * coin.Value) / 100);
            }

            return SumOfEachCoin;
        }

        /// <summary>Calculates the customer change to be returned.</summary>
        /// <param name="productPrice">The product price.</param>
        /// <param name="moneyEntered">The money entered.</param>
        /// <returns>Change to be given to customer</returns>
        public double CalculateCustomerChange(double productPrice, double moneyEntered)
        {
            if (moneyEntered > productPrice)
            {
                return moneyEntered - productPrice;
            }

            return 0.00;
        }

        /// <summary>Determines whether the machine has enough change to return.</summary>
        /// <param name="changeToBeReturned">The change to be returned to the customer</param>
        /// <returns>
        ///   <c>true</c> if [has enough change to return] otherwise, <c>false</c>.</returns>
        private bool HasEnoughChangeToReturn(double changeToBeReturned)
        {
            if (TotalInMachine > changeToBeReturned)
            {
                return true;
            }

            return false;
        }


        /// <summary>Returns the customer change in coins.</summary>
        /// <param name="changeToBeReturned">The amount to be returned.</param>
        /// <returns>The coins that amount to the change</returns>
        /// <exception cref="InsufficientChangeException"></exception>
        public Dictionary<CoinType, int> ReturnCustomerChangeInCoins(double changeToBeReturned)
        {
            // check if machine has sufficient change
            if (!HasEnoughChangeToReturn(changeToBeReturned))
            {
                throw new InsufficientChangeException(Resources.Insufficient_Change);
            }

            //if coin type does not exist, return error
            if (changeToBeReturned < (int)CoinType.TEN_Pence / 100)
            {
                throw new InsufficientChangeException(Resources.CoinType_Unavailabe);
            }

            var CoinsToReturn = new Dictionary<CoinType, int>();
            var changeToBeReturnedActual = Math.Round(changeToBeReturned, 1);
            
            // get CoinTypes and sort in desc order
            var orderedCoinTypes = Enum.GetValues(typeof(CoinType));
            Array.Sort(orderedCoinTypes);
            Array.Reverse(orderedCoinTypes);

            while (changeToBeReturnedActual > 0.0)
            {
                foreach(var coinType in orderedCoinTypes)
                {
                    var _coinType = (CoinType)coinType;

                    // return final value when no more coins held
                    if(_coinType == CoinType.TEN_Pence && MoneyInCoins[_coinType] == 0)
                    {
                        this.TotalInMachine = this.CalculateTotalAmountFromCoins(MoneyInCoins);

                        return CoinsToReturn;
                    }

                    if ((changeToBeReturnedActual >= ((double)_coinType) / 100) && MoneyInCoins[_coinType] > 0)
                    {
                        if (CoinsToReturn.ContainsKey(_coinType))
                            CoinsToReturn[_coinType] += 1;
                        else
                            CoinsToReturn.Add(_coinType, 1);

                        // reduce the coins held in machine
                        MoneyInCoins[_coinType] -= 1;
                        
                        changeToBeReturnedActual = changeToBeReturnedActual - ((double)_coinType)/100;
                        
                        changeToBeReturnedActual = Math.Round(changeToBeReturnedActual, 1);
                    }

                    
                }
            }

            // update total amount held
            this.TotalInMachine = this.CalculateTotalAmountFromCoins(MoneyInCoins);

            return CoinsToReturn;
        }


        
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VendingMoneyAPI;

namespace VendingMoneyTests
{
    [TestClass]
    public class VendingMoneyTests
    {
        Dictionary<CoinType, int> TestCoins = new Dictionary<CoinType, int>();

        public VendingMoneyTests()
        {
            PopulateCoins();
        }

        [TestMethod]
        public void GivenASetOfCoinsShouldReturnATotal()
        {
            var vendingMoney = new VendingMoneyAPI.VendingMoney();

            var total = vendingMoney.CalculateTotalAmountFromCoins(TestCoins);

            Assert.AreEqual(7.2, total);
        }

        [TestMethod]
        public void GivenChangeShouldReturnCoins()
        {
            // Arrange
            var vendingMoney = new VendingMoneyAPI.VendingMoney();
            var customerChange = 1.2;

            // Act
            var total = vendingMoney.ReturnCustomerChangeInCoins(customerChange);

            
            Assert.IsTrue(customerChange == vendingMoney.CalculateTotalAmountFromCoins(total));
            Assert.AreEqual(2, total.Count);
        }
        public void PopulateCoins()
        {
            TestCoins.Add(CoinType.TEN_Pence, 4);
            TestCoins.Add(CoinType.TWENTY_Pence, 4);
            TestCoins.Add(CoinType.FIFTY_Pence, 4);
            TestCoins.Add(CoinType.POUND, 4);
        }
    }
}

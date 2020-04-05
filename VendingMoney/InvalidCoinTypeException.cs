using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMoneyAPI
{
    class InvalidCoinTypeException:Exception
    {
        public InvalidCoinTypeException()
        {
        }

        public InvalidCoinTypeException(string message)
            : base(message)
        {
        }

        public InvalidCoinTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

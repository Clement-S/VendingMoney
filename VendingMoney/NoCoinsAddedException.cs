using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMoneyAPI
{
    class NoCoinsAddedException:Exception
    {
        public NoCoinsAddedException()
        {
        }

        public NoCoinsAddedException(string message)
            : base(message)
        {
        }

        public NoCoinsAddedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMoneyAPI
{
    class InsufficientChangeException:Exception
    {
        public InsufficientChangeException()
        {
        }

        public InsufficientChangeException(string message)
            : base(message)
        {
        }

        public InsufficientChangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

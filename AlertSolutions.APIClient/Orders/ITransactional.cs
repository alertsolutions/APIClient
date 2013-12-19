using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertSolutions.API.Orders
{
    // enforces fields so orders using it will have these fields accessible to the user
    public interface ITransactional : IOrder
    {
        // nothing
    }

    // common code for transactional orders
    internal class TransactionalInfo
    {
        // none right now
    }
}

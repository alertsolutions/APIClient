using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AlertSolutions.API.Orders;

namespace AlertSolutions.API.Messages
{
    // IMessage is a user friendly name rather than using ITransactional

    // enforces fields so orders using it will have these fields accessible to the user
    public interface IMessage : IOrder
    {
        // nothing
    }

    // common code for transactional orders
    [Serializable]
    internal class TransactionalInfo
    {
        // none right now
    }
}

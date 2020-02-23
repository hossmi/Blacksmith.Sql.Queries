using System;
using System.Runtime.Serialization;

namespace Blacksmith.Sql.Queries.MsSql.Exceptions
{
    [Serializable]
    public class AlreadyAddedOrderClauseQueryException : Exception
    {
        public AlreadyAddedOrderClauseQueryException(OrderClause item) 
            : base($"The order clause is already part of the order colecction.")
        {
            this.OrderClause = item;
        }

        protected AlreadyAddedOrderClauseQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public OrderClause OrderClause { get; }
    }
}
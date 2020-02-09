using System;
using System.Collections.Generic;

namespace Blacksmith.Sql.Queries
{
    public interface IOrderedQuery : IQuery
    {
        ICollection<OrderClause> Order { get; }
    }
}

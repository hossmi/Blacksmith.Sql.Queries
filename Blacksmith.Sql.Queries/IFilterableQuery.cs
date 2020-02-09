using System.Collections.Generic;

namespace Blacksmith.Sql.Queries
{
    public interface IFilterableQuery : IQuery
    {
        ICollection<string> Filters { get; }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blacksmith.Sql.Queries
{
    public interface IQuery
    {
        string Statement { get; }
        StringBuilder Columns { get; }
        StringBuilder Tables { get; }
        ICollection<IDbDataParameter> Parameters { get; }
    }
}

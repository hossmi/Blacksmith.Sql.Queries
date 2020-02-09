using System.Collections.Generic;
using System.Data;

namespace Blacksmith.Sql.Queries
{
    public interface ISqlStatement
    {
        string Statement { get; }
        ICollection<IDbDataParameter> Parameters { get; }

        IDbDataParameter createParameter();
    }
}

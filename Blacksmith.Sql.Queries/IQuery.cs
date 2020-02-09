using System.Text;

namespace Blacksmith.Sql.Queries
{
    public interface IQuery : ISqlStatement
    {
        StringBuilder Columns { get; }
        StringBuilder Tables { get; }
    }
}

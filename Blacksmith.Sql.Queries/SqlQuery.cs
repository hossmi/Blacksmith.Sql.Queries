using System.Data;

namespace Blacksmith.Sql.Queries.MsSql
{
    public class SqlQuery<TdbParameter> : AbstractSqlQuery where TdbParameter : IDbDataParameter, new()
    {
        public SqlQuery() : base()
        {
        }

        protected override IDbDataParameter prv_createParameter()
        {
            return new TdbParameter();
        }
    }
}

using System.Data;

namespace Blacksmith.Sql.Queries
{
    public class SqlStatement<TDbParameter> : AbstractSqlStatement 
        where TDbParameter : IDbDataParameter , new()
    {
        public SqlStatement(string statement, params IDbDataParameter[] parameters) 
            : base(statement, parameters)
        {

        }

        protected override IDbDataParameter prv_createParameter()
        {
            return new TDbParameter();
        }
    }
}

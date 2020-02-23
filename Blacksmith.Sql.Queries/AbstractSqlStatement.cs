using Blacksmith.Validations;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Blacksmith.Sql.Queries.MsSql
{
    public abstract class AbstractSqlStatement : ISqlStatement
    {
        protected readonly IValidator assert;

        public AbstractSqlStatement(string statement, params IDbDataParameter[] parameters)
        {
            this.assert = Asserts.Default;
            this.assert.isFilled(statement);
            this.assert.isNotNull(parameters);

            this.Statement = statement;
            this.Parameters = parameters.ToList();
        }

        public string Statement { get; }
        public ICollection<IDbDataParameter> Parameters { get; }

        public IDbDataParameter createParameter()
        {
            IDbDataParameter parameter;

            parameter = prv_createParameter();
            this.assert.isNotNull(parameter);

            return parameter;
        }

        protected abstract IDbDataParameter prv_createParameter();
    }
}

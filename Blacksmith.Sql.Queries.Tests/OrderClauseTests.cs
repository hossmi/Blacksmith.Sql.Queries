using FluentAssertions;
using System;
using Xunit;

namespace Blacksmith.Sql.Queries.Tests
{
    public class OrderClauseTests
    {
        [Fact]
        public void test2()
        {
            Func<string, OrderDirection, OrderClause> createInstance;
            OrderClause orderClause;

            createInstance = (column, orderDirection) => new OrderClause(column, orderDirection);
            createInstance
                .Invoking(c => c(null, (OrderDirection)666))
                .Should()
                .Throw<ArgumentNullException>();

        }
    }
}

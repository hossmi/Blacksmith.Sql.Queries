using Blacksmith.Sql.Queries.Extensions;
using FluentAssertions;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace Blacksmith.Sql.Queries.Tests
{
    public class QueryBuilderTests
    {
        [Fact]
        public void selectFromWhere()
        {
            IQuery query;
            IDbDataParameter parameter;

            query = new SqlQuery<SqlParameter>()
                .addColumns("SELECT p.name, p.description, p.addedDate, pv.price, pv.date")
                .addTables("FROM product as p")
                .addTables("LEFT JOIN productValue as pv ON p.id = pv.id")
                .addFilter("p.name LIKE @name", new SqlParameter
                {
                    ParameterName = "@name",
                    Value = "Red globes",
                    SqlDbType = SqlDbType.NVarChar,
                });

            query.Statement.Should().Be(@"SELECT p.name, p.description, p.addedDate, pv.price, pv.date
FROM product as p
LEFT JOIN productValue as pv ON p.id = pv.id
WHERE p.name LIKE @name
");

            query.Parameters.Should().HaveCount(1);
            parameter = query.Parameters.First();

            parameter.Should().BeOfType<SqlParameter>();
            parameter.Value.Should().Be("Red globes");
            parameter.ParameterName.Should().Be("@name");
        }

        [Fact]
        public void selectFrom()
        {
            IQuery query;

            query = new SqlQuery<SqlParameter>()
                .addColumns("SELECT p.name, p.description, p.addedDate, pv.price, pv.date")
                .addTables("FROM product as p")
                .addTables("LEFT JOIN productValue as pv ON p.id = pv.id");

            query.Statement.Should().Be(@"SELECT p.name, p.description, p.addedDate, pv.price, pv.date
FROM product as p
LEFT JOIN productValue as pv ON p.id = pv.id
");

            query.Parameters.Should().BeEmpty();
        }

        [Fact]
        public void selectTopFromWhere()
        {
            IQuery query;
            IDbDataParameter parameter;

            query = new SqlQuery<SqlParameter>()
                .setPage(3, 10)
                .addColumns("SELECT p.name, p.description, p.addedDate, pv.price, pv.date")
                .addTables("FROM product as p")
                .addTables("LEFT JOIN productValue as pv ON p.id = pv.id")
                .addFilter("p.name LIKE @name", new SqlParameter
                {
                    ParameterName = "@name",
                    Value = "Red globes",
                    SqlDbType = SqlDbType.NVarChar,
                })
                .setOrder("p.addedDate", OrderDirection.Descendant)
                .setOrder("p.description");

            query.Statement.Should().Be(@"SELECT p.name, p.description, p.addedDate, pv.price, pv.date
FROM product as p
LEFT JOIN productValue as pv ON p.id = pv.id
WHERE p.name LIKE @name
ORDER BY p.addedDate DESC, p.description ASC
OFFSET 30 ROWS FETCH NEXT 10 ROWS ONLY
");

            query.Parameters.Should().HaveCount(1);
            parameter = query.Parameters.First();

            parameter.Should().BeOfType<SqlParameter>();
            parameter.Value.Should().Be("Red globes");
            parameter.ParameterName.Should().Be("@name");
        }

        [Fact]
        public void subSelect()
        {
            IQuery query, subQuery;
            IDbDataParameter parameter;

            subQuery = new SqlQuery<SqlParameter>()
                .addColumns("SELECT c.name AS lastCategory")
                .addTables("FROM productCategory as pc")
                .addTables("INNER JOIN category as c ON pc.categoryId = c.id")
                .addFilter("pc.productId = p.id")
                .setOrder("pc.addedDate")
                .setPage(0, 1);

            query = new SqlQuery<SqlParameter>()
                .setPage(3, 10)
                .addColumns("SELECT p.name, p.description, p.addedDate, pv.price, pv.date")
                .addColumns($"({subQuery.Statement})")
                .addTables("FROM product as p")
                .addTables("LEFT JOIN productValue as pv ON p.id = pv.id")
                .addFilter("p.name LIKE @name", new SqlParameter
                {
                    ParameterName = "@name",
                    Value = "Red globes",
                    SqlDbType = SqlDbType.NVarChar,
                })
                .setOrder("p.addedDate", OrderDirection.Descendant)
                .setOrder("p.description");

            query.Statement.Should().Be(@"SELECT p.name, p.description, p.addedDate, pv.price, pv.date
(SELECT c.name AS lastCategory
FROM productCategory as pc
INNER JOIN category as c ON pc.categoryId = c.id
WHERE pc.productId = p.id
ORDER BY pc.addedDate ASC
OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
)
FROM product as p
LEFT JOIN productValue as pv ON p.id = pv.id
WHERE p.name LIKE @name
ORDER BY p.addedDate DESC, p.description ASC
OFFSET 30 ROWS FETCH NEXT 10 ROWS ONLY
");

            query.Parameters.Should().HaveCount(1);
            parameter = query.Parameters.First();

            parameter.Should().BeOfType<SqlParameter>();
            parameter.Value.Should().Be("Red globes");
            parameter.ParameterName.Should().Be("@name");
        }
    }
}

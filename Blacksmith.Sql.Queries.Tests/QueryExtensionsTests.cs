using AutoFixture.Xunit2;
using Xunit;
using Blacksmith.Sql.Queries.Extensions;
using FluentAssertions;
using System.Linq;
using Blacksmith.Sql.Queries.Tests.Models;

namespace Blacksmith.Sql.Queries.Tests
{
    public class QueryExtensionsTests
    {
        [Theory]
        [InlineAutoData]
        public void can_add_columns_to_query(string column1, string column2, string column3)
        {
            IQuery query;

            query = new FakeQuery();

            query
                .addColumns(column1)
                .addColumns(column2)
                .addColumns(column3);

            query.Columns.ToString().Should().Be(@$"{column1}
{column2}
{column3}
");
        }

        [Theory]
        [InlineAutoData]
        public void can_add_tables_to_query(string table1, string table2, string table3)
        {
            IQuery query;

            query = new FakeQuery();

            query
                .addTables(table1)
                .addTables(table2)
                .addTables(table3);

            query.Tables.ToString().Should().Be(@$"{table1}
{table2}
{table3}
");
        }

        [Theory]
        [InlineAutoData]
        public void can_set_query_pagination(int page, int size)
        {
            FakeQuery query;

            query = new FakeQuery()
                .setPage(page, size);

            query.Pagination.Should().NotBeNull();
            query.Pagination.Page.Should().Be(page);
            query.Pagination.Size.Should().Be(size);
        }

        [Theory]
        [InlineAutoData]
        public void can_set_query_order(string column, OrderDirection direction)
        {
            FakeQuery query;

            query = new FakeQuery()
                .setOrder(column, direction);

            query.Order.Should().NotBeNull();
            query.Order.Should().HaveCount(1);
            query.Order.First().Column.Should().Be(column);
            query.Order.First().Direction.Should().Be(direction);
        }

        [Theory]
        [InlineAutoData]
        public void can_set_query_parameters_from_object(Product product)
        {
            FakeQuery query;

            query = new FakeQuery().setParameters(product);

            query.Parameters.Should().HaveCount(3);
            
            query.Parameters
                .Single(p => p.ParameterName == nameof(Product.Id))
                .Value.Should().Be(product.Id);
            
            query.Parameters
                .Single(p => p.ParameterName == nameof(Product.Name))
                .Value.Should().Be(product.Name);
            
            query.Parameters
                .Single(p => p.ParameterName == nameof(Product.Price))
                .Value.Should().Be(product.Price);
        }
    }
}

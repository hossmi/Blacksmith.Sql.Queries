using AutoFixture.Xunit2;
using Blacksmith.Validations.Exceptions;
using FluentAssertions;
using System;
using Xunit;

namespace Blacksmith.Sql.Queries.Tests
{
    public class OrderClauseTests
    {
        private Func<string, OrderDirection, OrderClause> createInstance;

        public OrderClauseTests()
        {
            this.createInstance = (column, orderDirection) => new OrderClause(column, orderDirection);
        }

        [Fact]
        public void new_OrderClause_instance_throws_exception_on_null_column_name()
        {
            this.createInstance
                .Invoking(c => c(null, OrderDirection.Ascendant))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineAutoData]
        public void new_OrderClause_instance_throws_exception_on_invalid_OrderDirection_value(string columnName)
        {
            this.createInstance
                .Invoking(c => c(columnName, (OrderDirection)666))
                .Should()
                .Throw<ValidEnumValueExpectedAssertException>();
        }

        [Theory]
        [InlineAutoData]
        public void new_OrderClause_has_expected_values(string columnName, OrderDirection orderDirection)
        {
            OrderClause orderClause;

            orderClause = new OrderClause(columnName, orderDirection);

            orderClause.Should().NotBeNull();
            orderClause.Column.Should().Be(columnName);
            orderClause.Direction.Should().Be(orderDirection);
        }

        [Theory]
        [InlineAutoData]
        public void two_OrderClause_instances_with_same_properties_are_equals(OrderClause orderClause)
        {
            OrderClause otherOrderClause;

            otherOrderClause = new OrderClause(orderClause.Column, orderClause.Direction);

            otherOrderClause.Equals(orderClause).Should().BeTrue();
        }

        [Theory]
        [InlineAutoData]
        public void two_OrderClause_instances_with_different_properties_are_not_equals(OrderClause orderClause1, OrderClause orderClause2)
        {
            orderClause1.Equals(orderClause2).Should().BeFalse();
        }
    }
}

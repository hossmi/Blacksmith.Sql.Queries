using FluentAssertions;
using System;
using Xunit;

namespace Blacksmith.Sql.Queries.Tests
{
    public class PaginationTests
    {
        [Fact]
        public void default_pagination_instance_tests()
        {
            Pagination pagination;

            pagination = new Pagination();

            pagination.Should().NotBeNull();
            pagination.Page.Should().Be(0);
            pagination.Size.Should().Be(int.MaxValue);

            pagination
                .Invoking(p => p.Page = -1)
                .Should()
                .Throw<ArgumentOutOfRangeException>();

            pagination
                .Invoking(p => p.Size = 0)
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void pagination_cannot_have_negative_page()
        {
            Pagination pagination;

            pagination = new Pagination();

            pagination
                .Invoking(p => p.Page = -1)
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void pagination_cannot_have_negative_pageSize()
        {
            Pagination pagination;

            pagination = new Pagination();

            pagination
                .Invoking(p => p.Size = 0)
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }
    }
}

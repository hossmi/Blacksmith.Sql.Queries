using Blacksmith.Validations;
using System;

namespace Blacksmith.Sql.Queries
{
    public class OrderClause : IEquatable<OrderClause>
    {
        private static IValidator assert;

        static OrderClause()
        {
            assert = Asserts.Default;
        }

        public OrderClause(string column, OrderDirection direction)
        {
            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentNullException(nameof(column));
            
            assert.isValidEnum(direction);

            this.Column = column;
            this.Direction = direction;
        }

        public string Column { get; }
        public OrderDirection Direction { get; }

        public bool Equals(OrderClause other)
        {
            return this.Column == other.Column
                && this.Direction == other.Direction;
        }
    }
}

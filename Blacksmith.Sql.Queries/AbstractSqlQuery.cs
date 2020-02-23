using Blacksmith.Sql.Queries.Exceptions;
using Blacksmith.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blacksmith.Sql.Queries
{
    public abstract class AbstractSqlQuery : IFilterableQuery, IPaginableQuery, IOrderedQuery
    {
        protected readonly IValidator assert;

        public AbstractSqlQuery()
        {
            this.assert = Asserts.Default;
            this.Filters = new List<string>();
            this.Columns = new StringBuilder();
            this.Tables = new StringBuilder();
            this.Parameters = new List<IDbDataParameter>();
            this.Order = new PrvOrderList();
        }

        public ICollection<string> Filters { get; }
        public StringBuilder Columns { get; }
        public StringBuilder Tables { get; }
        public ICollection<IDbDataParameter> Parameters { get; }
        public Pagination Pagination { get; set; }
        public ICollection<OrderClause> Order { get; }

        public string Statement
        {
            get
            {
                StringBuilder builder;
                bool hasOrder;

                builder = new StringBuilder();
                builder
                    .AppendLine(this.Columns.ToString().Trim())
                    .AppendLine(this.Tables.ToString().Trim());

                prv_addFilters(this.Filters.AsEnumerable(), builder);
                prv_addOrder(this.Order.AsEnumerable(), builder, out hasOrder);
                prv_addPagination(this.Pagination, hasOrder, builder);

                return builder.ToString();
            }
        }

        public IDbDataParameter createParameter()
        {
            IDbDataParameter parameter;

            parameter = prv_createParameter();
            this.assert.isNotNull(parameter);

            return parameter;
        }

        protected abstract IDbDataParameter prv_createParameter();

        private static void prv_addPagination(Pagination pagination, bool hasOrder, StringBuilder builder)
        {
            if (pagination != null)
            {
                int offset;

                if (false == hasOrder)
                    throw new MissingRequiredOrderByForPaginatedQueryException();

                offset = pagination.Page * pagination.Size;

                builder.AppendLine($"OFFSET {offset} ROWS FETCH NEXT {pagination.Size} ROWS ONLY");
            }
        }

        private static void prv_addOrder(IEnumerable<OrderClause> orderClauses, StringBuilder builder, out bool hasOrder)
        {
            hasOrder = false;

            using (IEnumerator<OrderClause> enumerator = orderClauses.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    builder.Append($"ORDER BY {prv_getOrderCondition(enumerator.Current)}");
                    hasOrder = true;
                }

                while (enumerator.MoveNext())
                {
                    builder.Append($", {prv_getOrderCondition(enumerator.Current)}");
                    hasOrder = true;
                }

                if (hasOrder)
                    builder.AppendLine();
            }
        }

        private static void prv_addFilters(IEnumerable<string> filters, StringBuilder builder)
        {
            using (IEnumerator<string> enumerator = filters.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    builder.AppendLine($"WHERE {enumerator.Current}");

                while (enumerator.MoveNext())
                    builder.AppendLine($"AND   {enumerator.Current}");
            }
        }

        private static string prv_getOrderCondition(OrderClause clause)
        {
            string direction;

            direction = clause.Direction == OrderDirection.Ascendant
                ? "ASC"
                : "DESC";

            return $"{clause.Column} {direction}";
        }

        private class PrvOrderList : ICollection<OrderClause>
        {
            private readonly IList<OrderClause> items;

            public PrvOrderList()
            {
                this.items = new List<OrderClause>();
            }

            public int Count => this.items.Count;
            public bool IsReadOnly => this.items.IsReadOnly;

            public void Add(OrderClause item)
            {
                if (this.items.Contains(item))
                    throw new AlreadyAddedOrderClauseQueryException(item);

                this.items.Add(item);
            }

            public void Clear()
            {
                this.items.Clear();
            }

            public bool Contains(OrderClause item)
            {
                return this.items.Contains(item);
            }

            public void CopyTo(OrderClause[] array, int arrayIndex)
            {
                this.items.CopyTo(array, arrayIndex);
            }

            public bool Remove(OrderClause item)
            {
                if (this.items.Count == 0)
                    throw new EmptyOrderClauseStackException();

                return this.Remove(item);
            }

            public IEnumerator<OrderClause> GetEnumerator()
            {
                return this.items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.items.GetEnumerator();
            }
        }
    }
}

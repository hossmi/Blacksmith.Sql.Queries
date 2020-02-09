using System;
using System.Data;

namespace Blacksmith.Sql.Queries.Extensions
{
    public static class QueryExtensions
    {
        public static T addColumns<T>(this T query, string columns, params IDbDataParameter[] parameters) where T : IQuery
        {
            if (string.IsNullOrWhiteSpace(columns))
                throw new ArgumentNullException(nameof(columns));

            query.Columns.AppendLine(columns);
            prv_addParameters(query, parameters);

            return query;
        }

        public static T addTables<T>(this T query, string tables, params IDbDataParameter[] parameters) where T : IQuery
        {
            if (string.IsNullOrWhiteSpace(tables))
                throw new ArgumentNullException(nameof(tables));

            query.Tables.AppendLine(tables);
            prv_addParameters(query, parameters);

            return query;
        }

        public static T addFilter<T>(this T query, string condition, params IDbDataParameter[] parameters) where T : IFilterableQuery
        {
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentNullException(nameof(condition));

            query.Filters.Add(condition);
            prv_addParameters(query, parameters);

            return query;
        }

        public static T setPage<T>(this T query, int page, int size) where T: IPaginableQuery
        {
            if (query.Pagination == null)
                query.Pagination = new Pagination();

            query.Pagination.Page = page;
            query.Pagination.Size = size;

            return query;
        }

        public static T setOrder<T>(this T query, string column, OrderDirection direction = OrderDirection.Ascendant) where T : IOrderedQuery
        {
            OrderClause orderClause;

            orderClause = new OrderClause(column, direction);
            query.Order.Add(orderClause);

            return query;
        }

        private static void prv_addParameters<T>(T query, IDbDataParameter[] parameters) where T : IQuery
        {
            if (parameters != null)
                foreach (IDbDataParameter parameter in parameters)
                    query.Parameters.Add(parameter);
        }

    }
}

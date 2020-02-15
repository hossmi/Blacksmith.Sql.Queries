using Blacksmith.Extensions.Enumerables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

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

        public static T setPage<T>(this T query, int page, int size) where T : IPaginableQuery
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

        public static TStatement setParameters<TStatement>(this TStatement query, object parameters)
            where TStatement : ISqlStatement
        {
            parameters
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .forEach(p => prv_addParameter(query, p.Name, p.GetValue(parameters)));

            return query;
        }

        public static TStatement setParameters<TStatement>(this TStatement query, IEnumerable<KeyValuePair<string, object>> parameters)
            where TStatement : ISqlStatement
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
                setParameter(query, parameter.Key, parameter.Value);

            return query;
        }

        public static TStatement setParameter<TStatement>(this TStatement query, string name, object value)
            where TStatement : ISqlStatement
        {
            prv_addParameter(query, name, value);

            return query;
        }

        public static TQuery clearParameters<TQuery>(this TQuery query) where TQuery : IQuery
        {
            query.Parameters.Clear();
            return query;
        }

        private static void prv_addParameters<T>(T query, IDbDataParameter[] parameters) where T : IQuery
        {
            if (parameters != null)
                foreach (IDbDataParameter parameter in parameters)
                    query.Parameters.Add(parameter);
        }

        private static void prv_addParameter<TStatement>(TStatement query, string name, object value)
            where TStatement : ISqlStatement
        {
            IDbDataParameter parameter;

            parameter = query.createParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            query.Parameters.Add(parameter);
        }

    }
}

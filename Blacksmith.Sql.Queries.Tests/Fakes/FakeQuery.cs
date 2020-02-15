using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Blacksmith.Sql.Queries.Tests
{
    public class FakeQuery : IFilterableQuery, IOrderedQuery, IPaginableQuery
    {
        public FakeQuery()
        {
            this.Columns = new StringBuilder();
            this.Tables = new StringBuilder();
            this.Parameters = new List<IDbDataParameter>();
            this.Filters = new List<string>();
            this.Order = new List<OrderClause>();
        }

        public string Statement { get; }
        public StringBuilder Columns { get; }
        public StringBuilder Tables { get; }
        public ICollection<IDbDataParameter> Parameters { get; }
        public ICollection<string> Filters { get; }
        public ICollection<OrderClause> Order { get; }
        public Pagination Pagination { get; set; }

        public IDbDataParameter createParameter()
        {
            return new SqlParameter();
        }
    }
}
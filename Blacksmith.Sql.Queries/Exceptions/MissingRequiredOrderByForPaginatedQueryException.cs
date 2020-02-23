using System;
using System.Runtime.Serialization;

namespace Blacksmith.Sql.Queries.MsSql.Exceptions
{
    [Serializable]
    public class MissingRequiredOrderByForPaginatedQueryException : Exception
    {
        public MissingRequiredOrderByForPaginatedQueryException()
            : base($"Order columns are required in order to generate paginated query.")
        {
        }

        protected MissingRequiredOrderByForPaginatedQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
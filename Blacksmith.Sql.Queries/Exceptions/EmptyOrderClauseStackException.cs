using System;
using System.Runtime.Serialization;

namespace Blacksmith.Sql.Queries.Exceptions
{
    [Serializable]
    public class EmptyOrderClauseStackException : Exception
    {
        public EmptyOrderClauseStackException() 
            : base($"Cannot pop order clause because order clause collection is empty.")
        {
        }

        protected EmptyOrderClauseStackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
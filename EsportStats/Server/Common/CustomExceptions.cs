using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EsportStats.Server.Common
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
        {

        }

        public TooManyRequestsException(string message)
            : base(message)
        {

        }

        public TooManyRequestsException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    public class ApiArgumentException : ArgumentException
    {
        public ApiArgumentException()
        {

        }

        public ApiArgumentException(string message)
            : base(message)
        {

        }

        public ApiArgumentException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ApiArgumentException(string paramname, string message)
            : base(paramname, message)
        {

        }

        public ApiArgumentException(string paramname, string message, Exception inner)
            : base(paramname, message, inner)
        {

        }
    }

    public class ApiArgumentNullException : ArgumentNullException
    {
        public ApiArgumentNullException()
        {

        }

        public ApiArgumentNullException(string paramname)
            : base(paramname)
        {

        }

        public ApiArgumentNullException(string paramname, string message)
            : base(paramname, message)
        {

        }

        public ApiArgumentNullException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    public class ApiArgumentOutOfRangeException : ArgumentOutOfRangeException
    {
        public ApiArgumentOutOfRangeException()
        {

        }

        public ApiArgumentOutOfRangeException(string paramname)
            : base(paramname)
        {

        }

        public ApiArgumentOutOfRangeException(string paramname, string message)
            : base(paramname, message)
        {

        }

        public ApiArgumentOutOfRangeException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ApiArgumentOutOfRangeException(string paramname, object? actualValue, string message)
            : base(paramname, actualValue, message)
        {

        }
    }
}
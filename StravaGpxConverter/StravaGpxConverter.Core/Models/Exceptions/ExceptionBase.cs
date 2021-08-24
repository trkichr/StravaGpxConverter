using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGpxConverter.Core.Models.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase(string message) : base(message)
        {

        }

        public ExceptionBase(string message, Exception exception)
            : base(message, exception)
        {

        }

        public abstract LogLevel Level { get; }
    }
}

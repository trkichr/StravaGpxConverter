using Microsoft.Extensions.Logging;
using System;

namespace StravaGpxConverter.Core.Models.Exceptions
{
    public sealed class GpxFileNotSelectedException : ExceptionBase
    {
        public GpxFileNotSelectedException(string message) : base(message)
        {
        }

        public GpxFileNotSelectedException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public override LogLevel Level => LogLevel.Error;
    }
}

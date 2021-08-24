using Microsoft.Extensions.Logging;
using System;

namespace StravaGpxConverter.Core.Models.Exceptions
{
    public sealed class GpxFileNotLoadedException : ExceptionBase
    {
        public GpxFileNotLoadedException(string message)
            : base(message)
        {
        }

        public GpxFileNotLoadedException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public override LogLevel Level => LogLevel.Error;
    }
}

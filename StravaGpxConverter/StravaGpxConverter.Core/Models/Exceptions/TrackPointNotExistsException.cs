using Microsoft.Extensions.Logging;
using System;

namespace StravaGpxConverter.Core.Models.Exceptions
{
    public sealed class TrackPointNotDeleteException : ExceptionBase
    {
        public TrackPointNotDeleteException(string message, Exception exception) : base(message, exception)
        {
        }

        public override LogLevel Level => LogLevel.Error;
    }
}

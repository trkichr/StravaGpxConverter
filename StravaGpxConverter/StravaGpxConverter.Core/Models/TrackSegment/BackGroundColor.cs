using System;
using System.Drawing;
using System.Windows.Media;

namespace StravaGpxConverter.Core.Models.TrackSegment
{
    public class BackGroundColor : ValueObject<BackGroundColor>
    {
        public SolidColorBrush BrushColor { get; }

        public BackGroundColor(System.TimeSpan waitingTime)
        {
            var colorIndex = Convert.ToInt32((int)KnownColor.White);

            if (waitingTime.TotalMinutes > 60)
            {
                colorIndex = Convert.ToInt32((int)KnownColor.Red);
            }
            else if (waitingTime.TotalMinutes > 30)
            {
                colorIndex = Convert.ToInt32((int)KnownColor.Yellow);
            }
            else if (waitingTime.TotalMinutes > 10)
            {
                colorIndex = Convert.ToInt32((int)KnownColor.AliceBlue);
            }

            BrushColor = (SolidColorBrush)new BrushConverter()
                .ConvertFromString(Enum.GetName(typeof(KnownColor), colorIndex));
        }

        protected override bool EqualCore(BackGroundColor other)
        {
            return BrushColor == other.BrushColor;
        }
    }
}

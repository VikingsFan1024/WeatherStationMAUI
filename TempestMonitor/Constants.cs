using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using Serilog;

namespace TempestMonitor;

public class Constants
{
    public static readonly int UdpListeningPort = 50222;
    public static readonly long DoubleToLongMultiplier = 100000;
    public static readonly byte DigitsToRoundBeforeConvertingToLong = 5;
    public static readonly string BaseForecastURL = @"https://swd.weatherflow.com/swd/rest/better_forecast";
    public static readonly int NumberOfHoursInForecastToKeep = 72;


    public class DegreesToCardinal(string longName, string shortName, double degrees)
    {
        private const long halfRange = (long)(22.50 * 100000 / 2);
        public static long MaxDegrees => 360 * LongToDoubleMultiplier;
        public static long LongToDoubleMultiplier => 100000;
        public string LongName { get; set; } = longName;
        public string ShortName { get; set; } = shortName;
        public double Degrees { get; set; } = degrees;

        public long InternalDegrees => (long)Math.Abs(Degrees * LongToDoubleMultiplier);

        long _internalRangeStart = long.MaxValue;
        public long InternalRangeStart
        {
            get
            {
                if (_internalRangeStart == long.MaxValue)
                {
                    if (InternalDegrees - halfRange < 0)
                    {
                        _internalRangeStart = 0;
                    }
                    else
                    {
                        _internalRangeStart = InternalDegrees - halfRange;
                    }
                }
                return _internalRangeStart;
            }
        }

        long _internalRangeEnd = long.MinValue;
        public long InternalRangeEnd
        {
            get
            {
                if (_internalRangeEnd == long.MinValue)
                {
                    if (InternalDegrees + halfRange - 1 > MaxDegrees)
                    {
                        _internalRangeEnd = MaxDegrees - 1;
                    }
                    else
                    {
                        _internalRangeEnd = InternalDegrees + halfRange - 1;
                    }
                }
                return _internalRangeEnd;
            }
        }
    }
    sealed private class WindDirectionDegreesToCardinality
    {
        private static readonly DegreesToCardinal[] degreesToCardinal =
        [
            new DegreesToCardinal("North","N",0.00),
            new DegreesToCardinal("North-Northeast","NNE",22.50),
            new DegreesToCardinal("Northeast","NE",45.00),
            new DegreesToCardinal("East-Northeast","ENE",67.50),
            new DegreesToCardinal("East","E",90.00),
            new DegreesToCardinal("East-Southeast","ESE",112.50),
            new DegreesToCardinal("Southeast","SE",135.00),
            new DegreesToCardinal("South-Southeast","SSE",157.50),
            new DegreesToCardinal("South","S",180.00),
            new DegreesToCardinal("South-Southwest","SSW",202.50),
            new DegreesToCardinal("Southwest","SW",225.00),
            new DegreesToCardinal("West-Southwest","WSW",247.50),
            new DegreesToCardinal("West","W",270.00),
            new DegreesToCardinal("West-Northwest","WNW",292.50),
            new DegreesToCardinal("Northwest","NW",315.00),
            new DegreesToCardinal("North-Northwest","NNW",337.50),
            new DegreesToCardinal("North","N",360.00)
        ];

        internal static DegreesToCardinal GetCardinal(long degrees)
        {
            var result = degreesToCardinal.FirstOrDefault(
                x => degrees >= x.InternalRangeStart && degrees < x.InternalRangeEnd
            );

            return result ?? degreesToCardinal[0];
        }
    }
    public static string GetShortCardinalDirection(long incomingInternalDegrees)
    {
        return WindDirectionDegreesToCardinality.GetCardinal(incomingInternalDegrees).ShortName;
    }
    public static string GetLongCardinalDirection(long incomingInternalDegrees)
    {
        return WindDirectionDegreesToCardinality.GetCardinal(incomingInternalDegrees).ShortName;
    }
    public static long DoubleToLong(JsonElement jsonElement)
    {
        return Convert.ToInt64(Math.Round(jsonElement.GetDouble(), DigitsToRoundBeforeConvertingToLong) * DoubleToLongMultiplier);
    }
    public static long DoubleToLong(double doubleValue)
    {
        return Convert.ToInt64(Math.Round(doubleValue, DigitsToRoundBeforeConvertingToLong) * DoubleToLongMultiplier);
    }
    public static double LongToDouble(long longValue)
    {
        return (double)longValue / DoubleToLongMultiplier;
    }
    public static DateTime UnixSecondsToLocalTime(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime().DateTime;
    }
    public static long UnixSecondsToLocalHour(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime().Hour;
    }
}

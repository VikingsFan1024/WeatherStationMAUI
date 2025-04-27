namespace TempestMonitor;

public class Constants
{
    public static readonly int UdpListeningPort = 50222;
    public static readonly long DoubleToLongMultiplier = 100000;
    public static readonly byte DigitsToRoundBeforeConvertingToLong = 5;
    public static readonly string BaseForecastURL = @"https://swd.weatherflow.com/swd/rest/better_forecast";
    public static readonly int NumberOfHoursInForecastToKeep = 72;


    public class DegreesToCardinal(string longName, string shortName, long startDegrees, long endDegrees)
    {
        private const long halfRange = (long)(22.50 / 2);
        public static long MaxDegrees => 360;
        public string LongName { get; set; } = longName;
        public string ShortName { get; set; } = shortName;
        public long StartDegrees = startDegrees;

        public long EndDegrees = endDegrees;
    }
    sealed private class WindDirectionDegreesToCardinality
    {
        private static readonly DegreesToCardinal[] degreesToCardinal =
        [
            new DegreesToCardinal("North","N",0, 11),
            new DegreesToCardinal("North-Northeast","NNE",12, 34),
            new DegreesToCardinal("Northeast","NE",35, 56),
            new DegreesToCardinal("East-Northeast","ENE",57, 79),
            new DegreesToCardinal("East","E",80, 101),
            new DegreesToCardinal("East-Southeast","ESE",102, 124),
            new DegreesToCardinal("Southeast","SE",125, 146),
            new DegreesToCardinal("South-Southeast","SSE",147, 169),
            new DegreesToCardinal("South","S",170, 191),
            new DegreesToCardinal("South-Southwest","SSW",192, 214),
            new DegreesToCardinal("Southwest","SW",215, 236),
            new DegreesToCardinal("West-Southwest","WSW",237, 259),
            new DegreesToCardinal("West","W",260, 281),
            new DegreesToCardinal("West-Northwest","WNW",282,304),
            new DegreesToCardinal("Northwest","NW",305, 326),
            new DegreesToCardinal("North-Northwest","NNW",327, 349),
            new DegreesToCardinal("North","N",350, 360),
        ];

        internal static DegreesToCardinal GetCardinal(long degrees)
        {
            var result = degreesToCardinal.FirstOrDefault(
                x => degrees >= x.StartDegrees && degrees <= x.EndDegrees
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
    public static DateTime UnixSecondsToDateTime(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
    }
    public static long UnixSecondsToLocalHour(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime().Hour;
    }
}

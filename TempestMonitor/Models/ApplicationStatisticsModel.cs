namespace TempestMonitor.Models;
public struct ApplicationStatisticsModel
{
    private static long _deviceStatusReceivedCount;
    private static long _forecastReceivedCount;
    private static long _hubStatusReceivedCount;
    private static long _lightningStrikeReceivedCount;
    private static long _observationReadingReceivedCount;
    private static long _rainStartReceivedCount;
    private static long _windReadingReceivedCount;

    public static long DeviceStatusReceivedCount => _deviceStatusReceivedCount;
    public static long ForecastReceivedCount => _forecastReceivedCount;
    public static long HubStatusReceivedCount => _hubStatusReceivedCount;
    public static long LightningStrikeReceivedCount => _lightningStrikeReceivedCount;
    public static long ObservationReadingReceivedCount => _observationReadingReceivedCount;
    public static long RainStartReceivedCount => _rainStartReceivedCount;
    public static long WindReadingReceivedCount => _windReadingReceivedCount;


    public static DateTime? LastDeviceStatusReceivedDateTime { get; private set; }
    public static DateTime? LastForecastReceivedDateTime { get; private set; }
    public static DateTime? LastHubStatusReceivedDateTime { get; private set; }
    public static DateTime? LastLightningStrikeReceivedDateTime { get; private set; }
    public static DateTime? LastObservationReadingReceivedDateTime { get; private set; }
    public static DateTime? LastRainStartReceivedDateTime { get; private set; }
    public static DateTime? LastWindReadingReceivedDateTime { get; private set; }

    public static void IncrementDeviceStatusReceivedCount() { _deviceStatusReceivedCount++; LastDeviceStatusReceivedDateTime = DateTime.Now; }
    public static void IncrementForecastReceivedCount() { _forecastReceivedCount++; LastForecastReceivedDateTime = DateTime.Now; }
    public static void IncrementHubStatusReceivedCount() { _hubStatusReceivedCount++; LastHubStatusReceivedDateTime = DateTime.Now; }
    public static void IncrementLightningStrikeReceivedCount() { _lightningStrikeReceivedCount++; LastLightningStrikeReceivedDateTime = DateTime.Now; }
    public static void IncrementObservationReadingReceivedCount() { _observationReadingReceivedCount++; LastObservationReadingReceivedDateTime = DateTime.Now; }
    public static void IncrementRainStartReceivedCount() { _rainStartReceivedCount++; LastRainStartReceivedDateTime = DateTime.Now; }
    public static void IncrementWindReadingReceivedCount() { _windReadingReceivedCount++; LastWindReadingReceivedDateTime = DateTime.Now; }

    public static int UdpReadingCount { get; private set; }
    public static DateTime? LastUdpReadingDateTime { get; private set; }
    public static long LastUdpReadingWaitMilliseconds { get; private set; }
    public static long UdpWaitTimeTotalMilliseconds { get; private set; }
    public static bool AreUdpBroadcastsBeingListenedFor { get; private set; }

    public static int HttpResponseCount { get; private set; }
    public static DateTime? LastHttpResponseDateTime { get; private set; }
    public static long LastHttpResponseWaitMilliseconds { get; private set; }
    public static long HttpResponseWaitTimeTotalMilliseconds { get; private set; }
    public static bool AreHttpRequestsBeingMade { get; private set; }

    public static void SetUdpBroadcastsBeingListenedForToFalse() { AreUdpBroadcastsBeingListenedFor = false; }
    internal static void SetHttpRequestsBeingMadeToFalse() { AreHttpRequestsBeingMade = false; }
    internal static void SetLastUdpReading(long milliseconds) 
    {
        UdpReadingCount++;
        LastUdpReadingWaitMilliseconds = milliseconds;
        LastUdpReadingDateTime = DateTime.Now;
        UdpWaitTimeTotalMilliseconds += milliseconds;
    }

    internal static void SetLastHttpResponse(long milliseconds)
    {
        HttpResponseCount++;
        LastHttpResponseWaitMilliseconds = milliseconds;
        LastHttpResponseDateTime = DateTime.Now;
        HttpResponseWaitTimeTotalMilliseconds += milliseconds;
    }

    internal static void StartOrResumeUdpStatistics() { AreUdpBroadcastsBeingListenedFor = true; }

    internal static void StartOrResumeHttpStatistics() { AreHttpRequestsBeingMade = true; }
}
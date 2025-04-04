// using directives for precision in what specific classes are employed
using DateTime = System.DateTime;

namespace TempestMonitor.Models;

public struct ApplicationStatisticsModel
{
    private static long _airObservationReceivedCount;
    private static long _deviceStatusReceivedCount;
    private static long _hubStatusReceivedCount;
    private static long _lightningStrikeReceivedCount;
    private static long _observationReadingReceivedCount;
    private static long _rainStartReceivedCount;
    private static long _skyObservationReceivedCount;
    private static long _windReadingReceivedCount;

    private static long _forecastCurrentConditionsSavedToDatabaseCount;
    private static long _forecastDailiesSavedToDatabaseCount;
    private static long _forecastHourliesSavedToDatabaseCount;
    private static long _forecastsSavedToDatabaseCount;
    private static long _forecastStationsSavedToDatabaseCount;
    private static long _forecastStatusesSavedToDatabaseCount;
    private static long _forecastUnitsSavedToDatabaseCount;

    private static long _airObservationSavedToDatabaseCount;
    private static long _deviceStatusSavedToDatabaseCount;
    private static long _hubStatusSavedToDatabaseCount;
    private static long _lightingStrikeSavedToDatabaseCount;
    private static long _observationReadingSavedToDatabaseCount;
    private static long _rainStartSavedToDatabaseCount;
    private static long _skyObservationSavedToDatabaseCount;
    private static long _windReadingSavedToDatabaseCount;

    public static long AirObservationReceivedCount => _airObservationReceivedCount;
    public static long DeviceStatusReceivedCount => _deviceStatusReceivedCount;
    public static long HubStatusReceivedCount => _hubStatusReceivedCount;
    public static long LightningStrikeReceivedCount => _lightningStrikeReceivedCount;
    public static long ObservationReadingReceivedCount => _observationReadingReceivedCount;
    public static long RainStartReceivedCount => _rainStartReceivedCount;
    public static long SkyObservationReceivedCount => _skyObservationReceivedCount;
    public static long WindReadingReceivedCount => _windReadingReceivedCount;


    public static long ForecastCurrentConditionsSavedToDatabaseCount => _forecastCurrentConditionsSavedToDatabaseCount;
    public static long ForecastDailiesSavedToDatabaseCount => _forecastDailiesSavedToDatabaseCount;
    public static long ForecastHourliesSavedToDatabaseCount => _forecastHourliesSavedToDatabaseCount;
    public static long ForecastsSavedToDatabaseCount => _forecastsSavedToDatabaseCount;
    public static long ForecastStationsSavedToDatabaseCount => _forecastStationsSavedToDatabaseCount;
    public static long ForecastStatusesSavedToDatabaseCount => _forecastStatusesSavedToDatabaseCount;
    public static long ForecastUnitsSavedToDatabaseCount => _forecastUnitsSavedToDatabaseCount;

    public static long AirObservationSavedToDatabaseCount => _airObservationSavedToDatabaseCount;
    public static long DeviceStatusSavedToDatabaseCount => _deviceStatusSavedToDatabaseCount;
    public static long HubStatusSavedToDatabaseCount => _hubStatusSavedToDatabaseCount;
    public static long LightingStrikeSavedToDatabaseCount => _lightingStrikeSavedToDatabaseCount;
    public static long ObservationReadingSavedToDatabaseCount => _observationReadingSavedToDatabaseCount;
    public static long RainStartSavedToDatabaseCount => _rainStartSavedToDatabaseCount;
    public static long SkyObservationSavedToDatabaseCount => _skyObservationSavedToDatabaseCount;
    public static long WindReadingSavedToDatabaseCount => _windReadingSavedToDatabaseCount;

    public static DateTime? LastAirObservationReceivedDateTime { get; private set; }
    public static DateTime? LastDeviceStatusReceivedDateTime { get; private set; }
    public static DateTime? LastHubStatusReceivedDateTime { get; private set; }
    public static DateTime? LastLightningStrikeReceivedDateTime { get; private set; }
    public static DateTime? LastObservationReadingReceivedDateTime { get; private set; }
    public static DateTime? LastRainStartReceivedDateTime { get; private set; }
    public static DateTime? LastSkyObservationReceivedDateTime { get; private set; }
    public static DateTime? LastWindReadingReceivedDateTime { get; private set; }

    public static void IncrementAirObservationReceivedCount() { _airObservationReceivedCount++; LastAirObservationReceivedDateTime = DateTime.Now; }
    public static void IncrementDeviceStatusReceivedCount() { _deviceStatusReceivedCount++; LastDeviceStatusReceivedDateTime = DateTime.Now; }
    public static void IncrementHubStatusReceivedCount() { _hubStatusReceivedCount++; LastHubStatusReceivedDateTime = DateTime.Now; }
    public static void IncrementLightningStrikeReceivedCount() { _lightningStrikeReceivedCount++; LastLightningStrikeReceivedDateTime = DateTime.Now; }
    public static void IncrementObservationReadingReceivedCount() { _observationReadingReceivedCount++; LastObservationReadingReceivedDateTime = DateTime.Now; }
    public static void IncrementRainStartReceivedCount() { _rainStartReceivedCount++; LastRainStartReceivedDateTime = DateTime.Now; }
    public static void IncrementSkyObservationReceivedCount() { _skyObservationReceivedCount++; LastSkyObservationReceivedDateTime = DateTime.Now; }
    public static void IncrementWindReadingReceivedCount() { _windReadingReceivedCount++; LastWindReadingReceivedDateTime = DateTime.Now; }

    public static void IncrementForecastCurrentConditionsSavedToDatabaseCount() { _forecastCurrentConditionsSavedToDatabaseCount++; }
    public static void IncrementForecastDailiesSavedToDatabaseCount() { _forecastDailiesSavedToDatabaseCount++; }
    public static void IncrementForecastHourliesSavedToDatabaseCount() { _forecastHourliesSavedToDatabaseCount++; }
    public static void IncrementForecastsSavedToDatabaseCount() { _forecastsSavedToDatabaseCount++; }
    public static void IncrementForecastStationsSavedToDatabaseCount() { _forecastStationsSavedToDatabaseCount++; }
    public static void IncrementForecastStatusesSavedToDatabaseCount() { _forecastStatusesSavedToDatabaseCount++; }
    public static void IncrementForecastUnitsSavedToDatabaseCount() { _forecastUnitsSavedToDatabaseCount++; }

    public static void IncrementAirObservationSavedToDatabaseCount() { _airObservationSavedToDatabaseCount++; }
    public static void IncrementDeviceStatusSavedToDatabaseCount() { _deviceStatusSavedToDatabaseCount++; }
    public static void IncrementHubStatusSavedToDatabaseCount() { _hubStatusSavedToDatabaseCount++; }
    public static void IncrementLightningStrikeSavedToDatabaseCount() { _lightingStrikeSavedToDatabaseCount++; }
    public static void IncrementObservationReadingSavedToDatabaseCount() { _observationReadingSavedToDatabaseCount++; }
    public static void IncrementRainStartSavedToDatabaseCount() { _rainStartSavedToDatabaseCount++; }
    public static void IncrementSkyObservationSavedToDatabaseCount() { _skyObservationSavedToDatabaseCount++; }
    public static void IncrementWindReadingSavedToDatabaseCount() { _windReadingSavedToDatabaseCount++; }

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
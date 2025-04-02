using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TempestMonitor;

sealed internal class WeatherUtilities
{
    // This class contains utility methods for calculating weather-related values
    // The code was downloaded and then modified but I have been unable to locate the original source code but will continue to try
    //   and provide proper attribution
    public static double CalculateFeelsLike(double temperatureInFahrenheit, double relativeHumidity, 
        double windspeedInMPH)
    {
        var integerTemperatureInFahrenheit = (int)Math.Round(temperatureInFahrenheit, 0);
        var integerWindspeedInMPH = (int)Math.Round(windspeedInMPH, 0);
        // Wind chill formula is not valid for temperatures above 50°F or wind speeds below 3 mph
        if (integerTemperatureInFahrenheit <= 50 && integerWindspeedInMPH > 3)
            return CalculateWindChill(temperatureInFahrenheit, windspeedInMPH);

        if (integerTemperatureInFahrenheit > 80)
            return CalculateHeatIndex(temperatureInFahrenheit, relativeHumidity);

        return temperatureInFahrenheit;
    }
    public static double CalculateWindChill(double temperatureInFahrenheit, double windspeedInMPH)
    {
        var integerTemperatureInFahrenheit = (int)Math.Round(temperatureInFahrenheit, 0);
        var integerWindspeedInMPH = (int)Math.Round(windspeedInMPH, 0);

        // Wind chill formula is not valid for temperatures above 50°F or wind speeds below 3 mph
        if (integerTemperatureInFahrenheit > 50 || integerWindspeedInMPH <= 3)
            return temperatureInFahrenheit;

        return
            35.74 +
            (0.6215 * temperatureInFahrenheit) -
            (35.75 * Math.Pow(windspeedInMPH, 0.16)) +
            (0.4275 * temperatureInFahrenheit * Math.Pow(windspeedInMPH, 0.16));
    }
    public static double CalculateHeatIndex(double temperatureInFahrenheit, double relativeHumidity)
    {
        var integerTemperatureInFahrenheit = (int)Math.Round(temperatureInFahrenheit, 0);
        var integerRelativeHumidity = Math.Round(relativeHumidity, 2) * 100;

        // Heat index formula is not valid for temperatures below 80°F or Humidity below 40%
        if (integerTemperatureInFahrenheit < 80 || integerRelativeHumidity < 40)
            return temperatureInFahrenheit;

        // Constants for the heat index formula
        double c1 = -42.379;
        double c2 = 2.04901523;
        double c3 = 10.14333127;
        double c4 = -0.22475541;
        double c5 = -6.83783e-3;
        double c6 = -5.481717e-2;
        double c7 = 1.22874e-3;
        double c8 = 8.5282e-4;
        double c9 = -1.99e-6;

        // Calculate heat index
        return
            c1 + 
            (c2 * temperatureInFahrenheit) + 
            (c3 * relativeHumidity) + 
            (c4 * temperatureInFahrenheit * relativeHumidity) +
            (c5 * Math.Pow(temperatureInFahrenheit, 2)) + 
            (c6 * Math.Pow(relativeHumidity, 2)) +
            (c7 * Math.Pow(temperatureInFahrenheit, 2) * relativeHumidity) + 
            (c8 * temperatureInFahrenheit * Math.Pow(relativeHumidity, 2)) +
            (c9 * Math.Pow(temperatureInFahrenheit, 2) * Math.Pow(relativeHumidity, 2));
    }
}

namespace TempestMonitor.Models;
public class WeatherForecastGraph : DatabaseBaseModel_VW
{
    public CurrentConditions current_conditions { get; set; }
    public Forecast forecast { get; set; }
    public double latitude { get; set; }
    public string location_name { get; set; }
    public double longitude { get; set; }
    public long source_id_conditions { get; set; }
    public Station station { get; set; }
    public Status status { get; set; }
    public string timezone { get; set; }
    public long timezone_offset_minutes { get; set; }
    public Units units { get; set; }

    private TempestRedStarMapping? _tempestRedStarMapping = null;
    public TempestRedStarMapping GetTempestRedStarMapping()
    {
        if (_tempestRedStarMapping != null)
            return _tempestRedStarMapping;

        return _tempestRedStarMapping = new TempestRedStarMapping(units);
    }

    public class CurrentConditions
    {
        public double air_density { get; set; }
        public double air_temperature { get; set; }
        public long brightness { get; set; }
        public string conditions { get; set; }
        public double delta_t { get; set; }
        public double dew_point { get; set; }
        public double feels_like { get; set; }
        public string icon { get; set; }
        public bool is_precip_local_day_rain_check { get; set; }
        public bool is_precip_local_yesterday_rain_check { get; set; }
        public long lightning_strike_count_last_1hr { get; set; }
        public long lightning_strike_count_last_3hr { get; set; }
        public long lightning_strike_last_distance { get; set; }
        public string lightning_strike_last_distance_msg { get; set; }
        public long lightning_strike_last_epoch { get; set; }
        public double precip_accum_local_day { get; set; }
        public double precip_accum_local_yesterday { get; set; }
        public long precip_minutes_local_day { get; set; }
        public long precip_minutes_local_yesterday { get; set; }
        public long precip_probability { get; set; }
        public string pressure_trend { get; set; }
        public long relative_humidity { get; set; }
        public double sea_level_pressure { get; set; }
        public long solar_radiation { get; set; }
        public double station_pressure { get; set; }
        public long time { get; set; }
        public long uv { get; set; }
        public double wet_bulb_globe_temperature { get; set; }
        public double wet_bulb_temperature { get; set; }
        public double wind_avg { get; set; }
        public long wind_direction { get; set; }
        public string wind_direction_cardinal { get; set; }
        public double wind_gust { get; set; }
    }

    public class Forecast
    {
        public Daily[] daily { get; set; }
        public Hourly[] hourly { get; set; }
    }

    public class Daily
    {
        public double air_temp_high { get; set; }
        public double air_temp_low { get; set; }
        public string conditions { get; set; }
        public long day_num { get; set; }
        public long day_start_local { get; set; }
        public string icon { get; set; }
        public long month_num { get; set; }
        public string precip_icon { get; set; }
        public long precip_probability { get; set; }
        public string precip_type { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

    public class Hourly
    {
        public double air_temperature { get; set; }
        public string conditions { get; set; }
        public double feels_like { get; set; }
        public string icon { get; set; }
        public long local_day { get; set; }
        public long local_hour { get; set; }
        public double precip { get; set; }
        public string precip_icon { get; set; }
        public long precip_probability { get; set; }
        public string precip_type { get; set; }
        public long relative_humidity { get; set; }
        public double sea_level_pressure { get; set; }
        public double station_pressure { get; set; }
        public long time { get; set; }
        public double uv { get; set; }
        public double wind_avg { get; set; }
        public long wind_direction { get; set; }
        public string wind_direction_cardinal { get; set; }
        public double wind_gust { get; set; }
    }

    public class Station
    {
        public double agl { get; set; }
        public double elevation { get; set; }
        public bool is_station_online { get; set; }
        public long state { get; set; }
        public long station_id { get; set; }
    }

    public class Status
    {
        public long status_code { get; set; }
        public string status_message { get; set; }
    }

    public class Units
    {
        public string units_air_density { get; set; }
        public string units_brightness { get; set; }
        public string units_distance { get; set; }
        public string units_other { get; set; }
        public string units_precip { get; set; }
        public string units_pressure { get; set; }
        public string units_solar_radiation { get; set; }
        public string units_temp { get; set; }
        public string units_wind { get; set; }
        public string units_elevation => "m";
    }
}

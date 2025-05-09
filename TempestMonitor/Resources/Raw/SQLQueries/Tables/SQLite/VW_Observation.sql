create view VW_Observation
as
select
	Id
    , timestamp_utc
	, unixepoch(timestamp_utc, 'unixepoch', 'localtime') 						 timestamp_local
	, datetime(unixepoch(timestamp_utc, 'unixepoch', 'localtime'), 'unixepoch')	 timestamp_local_datetime

	, air_temperature
	, battery
	, hub_sn
	, illuminance
	, lightning_strike_average_distance
	, lightning_strike_count
	-- 0 = none, 1 = rain, 2 = hail, 3 = rain + hail (experimental)
	, precipitation_type
	, rain_accumulation_over_the_previous_minute
	, relative_humidity
	, reporting_interval
	, serial_number
	, solar_radiation
	, station_pressure
	, uv
	, wind_average
	, wind_direction
	, wind_gust
	, wind_lull
	, wind_sample_interval
from
    Observation
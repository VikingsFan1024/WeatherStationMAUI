create view VW_HourlyObservationSummary
as
with data as (
	-- ToDo: Look into what time series data optimizations may exist
	SELECT
		Id
		, timestamp_utc

		, air_temperature
		, battery
		, hub_sn
		, illuminance
		, lightning_strike_average_distance
		, lightning_strike_count
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
		-- ToDo: More flexible if passed in this base time with a default to 'now'
		,(unixepoch('now') - O.timestamp_utc) / 3600 hours_back
	FROM
		Observation O
		-- ToDo: Possibly more performant if passed in time range and use in where clause vs relying on query
		-- optimizer at query execution time
)
SELECT
	hours_back
	
	,MIN(unixepoch(timestamp_utc, 'unixepoch', 'localtime'))	min_timestamp_local
	,MAX(unixepoch(timestamp_utc, 'unixepoch', 'localtime'))	max_timestamp_local
	,AVG(unixepoch(timestamp_utc, 'unixepoch', 'localtime'))	avg_timestamp_local

	,datetime(MIN(unixepoch(timestamp_utc, 'unixepoch', 'localtime')), 'unixepoch')	min_timestamp_local_datetime
	,datetime(MAX(unixepoch(timestamp_utc, 'unixepoch', 'localtime')), 'unixepoch')	max_timestamp_local_datetime
	,datetime(AVG(unixepoch(timestamp_utc, 'unixepoch', 'localtime')), 'unixepoch')	avg_timestamp_local_datetime

	,SUBSTR(datetime(MIN(unixepoch(timestamp_utc, 'unixepoch', 'localtime')), 'unixepoch'), 12, 5) min_hour_minutes

	,COUNT(*)											count

	,MIN(air_temperature)								min_air_temperature
	,MAX(air_temperature)								max_air_temperature
	,AVG(air_temperature)								avg_air_temperature

	,MIN(battery)										min_battery
	,MAX(battery)										max_battery
	,AVG(battery)										avg_battery

	,MIN(Id)											min_id

	,MIN(illuminance)									min_illuminance
	,MAX(illuminance)									max_illuminance
	,AVG(illuminance)									avg_illuminance

	,MIN(lightning_strike_average_distance)				min_lightning_strike_average_distance
	,MAX(lightning_strike_average_distance)				max_lightning_strike_average_distance
	,AVG(lightning_strike_average_distance)				avg_lightning_strike_average_distance

	,MIN(lightning_strike_count)						min_lightning_strike_count
	,MAX(lightning_strike_count)						max_lightning_strike_count
	,AVG(lightning_strike_count)						avg_lightning_strike_count

	,AVG(precipitation_type)							avg_precipitation_type
	,COUNT(DISTINCT precipitation_type)					count_distinct_precipitation_type

	,MIN(rain_accumulation_over_the_previous_minute)	min_rain_accumulation_over_the_previous_minute
	,MAX(rain_accumulation_over_the_previous_minute)	max_rain_accumulation_over_the_previous_minute
	,AVG(rain_accumulation_over_the_previous_minute)	avg_rain_accumulation_over_the_previous_minute
	,SUM(rain_accumulation_over_the_previous_minute)	sum_rain_accumulation_over_the_previous_minute

	,MIN(relative_humidity)								min_relative_humidity
	,MAX(relative_humidity)								max_relative_humidity
	,AVG(relative_humidity)								avg_relative_humidity

	,MIN(reporting_interval)							min_reporting_interval
	,MAX(reporting_interval)							max_reporting_interval
	,AVG(reporting_interval)							avg_reporting_interval
	,COUNT(DISTINCT reporting_interval)					count_distinct_reporting_interval

	,MIN(solar_radiation)								min_solar_radiation
	,MAX(solar_radiation)								max_solar_radiation
	,AVG(solar_radiation)								avg_solar_radiation

	,MIN(station_pressure)								min_station_pressure
	,MAX(station_pressure)								max_station_pressure
	,AVG(station_pressure)								avg_station_pressure

	,MIN(timestamp_utc)									min_timestamp
	,MAX(timestamp_utc)									max_timestamp
	,AVG(timestamp_utc)									avg_timestamp

	,MIN(uv)											min_uv
	,MAX(uv)											max_uv
	,AVG(uv)											avg_uv

	,MIN(wind_average)									min_wind_average
	,MAX(wind_average)									max_wind_average
	,AVG(wind_average)									avg_wind_average

	,MIN(wind_direction)								min_wind_direction
	,MAX(wind_direction)								max_wind_direction
	,AVG(wind_direction)								avg_wind_direction

	,MIN(wind_gust)										min_wind_gust
	,MAX(wind_gust)										max_wind_gust
	,AVG(wind_gust)										avg_wind_gust

	,MIN(wind_lull)										min_wind_lull
	,MAX(wind_lull)										max_wind_lull
	,AVG(wind_lull)										avg_wind_lull

	,MIN(wind_sample_interval)							min_wind_sample_interval
	,MAX(wind_sample_interval)							max_wind_sample_interval
	,AVG(wind_sample_interval)							avg_wind_sample_interval
	,COUNT(DISTINCT wind_sample_interval)				count_distinct_wind_sample_interval
FROM
	data
GROUP BY
	hours_back
ORDER BY
	hours_back
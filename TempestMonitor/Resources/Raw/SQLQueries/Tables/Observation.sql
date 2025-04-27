create table Observation
(
	id                                           integer primary key autoincrement
    , json_document          					 text
	, serial_number                              text 		generated always as (json_extract(json_document, '$.serial_number')) stored
	, timestamp_utc								 integer 	generated always as (json_extract(json_document, '$.obs[0][0]')) stored
	, type                                       text 		generated always as (json_extract(json_document, '$.type')) stored
	
	, air_temperature                            real 		generated always as (json_extract(json_document, '$.obs[0][7]')) stored
	, battery                                    real 		generated always as (json_extract(json_document, '$.obs[0][16]')) stored
	, hub_sn                                     text 		generated always as (json_extract(json_document, '$.hub_sn')) stored
	, illuminance                                real 		generated always as (json_extract(json_document, '$.obs[0][9]')) stored
	, lightning_strike_average_distance          real 		generated always as (json_extract(json_document, '$.obs[0][14]')) stored
	, lightning_strike_count                     integer 	generated always as (json_extract(json_document, '$.obs[0][15]')) stored
	, precipitation_type                         integer 	generated always as (json_extract(json_document, '$.obs[0][13]')) stored
	, rain_accumulation_over_the_previous_minute real 		generated always as (json_extract(json_document, '$.obs[0][12]')) stored
	, relative_humidity                          real 		generated always as (json_extract(json_document, '$.obs[0][8]')) stored
	, reporting_interval                         integer 	generated always as (json_extract(json_document, '$.obs[0][17]')) stored
	, solar_radiation                            real 		generated always as (json_extract(json_document, '$.obs[0][11]')) stored
	, station_pressure                           real 		generated always as (json_extract(json_document, '$.obs[0][6]')) stored
	, uv                                         real 		generated always as (json_extract(json_document, '$.obs[0][10]')) stored
	, wind_average                               real 		generated always as (json_extract(json_document, '$.obs[0][2]')) stored
	, wind_direction                             integer 	generated always as (json_extract(json_document, '$.obs[0][4]')) stored
	, wind_gust                                  numeric 	generated always as (json_extract(json_document, '$.obs[0][3]')) stored
	, wind_lull                                  real 		generated always as (json_extract(json_document, '$.obs[0][1]')) stored
	, wind_sample_interval                       integer 	generated always as (json_extract(json_document, '$.obs[0][5]')) stored
)
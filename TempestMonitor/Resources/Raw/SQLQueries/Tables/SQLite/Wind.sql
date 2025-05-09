create table Wind
(
	id					integer primary key autoincrement
	, json_document		text
	, serial_number		text	generated always as (json_extract(json_document, '$.serial_number')) stored
	, timestamp_utc		text	generated always as (json_extract(json_document, '$.ob[0]')) stored
	, type				text	generated always as (json_extract(json_document, '$.type')) stored
	
	, hub_sn			text	generated always as (json_extract(json_document, '$.hub_sn')) stored
	, wind_direction	integer	generated always as (json_extract(json_document, '$.ob[2]')) stored
	, wind_speed		real	generated always as (round(json_extract(json_document, '$.ob[1]'))) stored
)
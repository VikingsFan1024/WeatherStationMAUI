create table DeviceStatus
(
    id                  integer primary key autoincrement
    , json_document     text
    , serial_number     text 	generated always as (json_extract(json_document, '$.serial_number')) stored
    , timestamp_utc     text 	generated always as (json_extract(json_document, '$.timestamp')) stored
	, type              text 	generated always as (json_extract(json_document, '$.type')) stored

	, debug				integer	generated always as (json_extract(json_document, '$.debug')) stored
	, firmware_revision	integer	generated always as (json_extract(json_document, '$.firmware_revision')) stored
	, hub_rssi			integer	generated always as (json_extract(json_document, '$.hub_rssi')) stored
	, rssi				integer	generated always as (json_extract(json_document, '$.rssi')) stored
	, sensor_status		integer	generated always as (json_extract(json_document, '$.sensor_status')) stored
	, uptime			integer	generated always as (json_extract(json_document, '$.uptime')) stored
	, voltage			real	generated always as (json_extract(json_document, '$.voltage')) stored
    , hub_sn            text 	generated always as (json_extract(json_document, '$.hub_sn')) stored
)
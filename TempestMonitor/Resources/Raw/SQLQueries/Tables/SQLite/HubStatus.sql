create table HubStatus
(
    id                     				integer primary key autoincrement
    , json_document        				text
    , serial_number        				text 	generated always as (json_extract(json_document, '$.serial_number')) stored
    , timestamp_utc        				integer	generated always as (json_extract(json_document, '$.timestamp')) stored
	, type                 				text 	generated always as (json_extract(json_document, '$.type')) stored

	, cell_rssi							integer	generated always as (json_extract(json_document, '$.cell_rssi')) stored
	, cellular_status					integer	generated always as (json_extract(json_document, '$.cellular_status')) stored
	, firmware_revision					integer	generated always as (json_extract(json_document, '$.firmware_revision')) stored
	, freq								integer	generated always as (json_extract(json_document, '$.freq')) stored
	, hardware_id						integer	generated always as (json_extract(json_document, '$.hardware_id')) stored
	, hw_version						integer	generated always as (json_extract(json_document, '$.hw_version')) stored
	, radio_stats_i2c_bus_error_count	integer	generated always as (json_extract(json_document, '$.radio_stats[2]')) stored
	, radio_stats_radio_network_id		integer	generated always as (json_extract(json_document, '$.radio_stats[4]')) stored		
	, radio_stats_radio_status			integer	generated always as (json_extract(json_document, '$.radio_stats[3]')) stored
	, radio_stats_reboot_count			integer	generated always as (json_extract(json_document, '$.radio_stats[1]')) stored
	, radio_stats_version				integer	generated always as (json_extract(json_document, '$.radio_stats[0]')) stored
	, rssi								integer	generated always as (json_extract(json_document, '$.rssi')) stored
	, seq								integer	generated always as (json_extract(json_document, '$.seq')) stored		
	, uptime							integer	generated always as (json_extract(json_document, '$.uptime')) stored
    , reset_flags          				text 	generated always as (json_extract(json_document, '$.reset_flags')) stored		
)
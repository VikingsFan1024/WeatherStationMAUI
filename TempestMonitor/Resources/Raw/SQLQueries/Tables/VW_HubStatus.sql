create view VW_HubStatus
as
select
	Id
    , timestamp_utc
	, unixepoch(timestamp_utc, 'unixepoch', 'localtime') 						 timestamp_local
	, datetime(unixepoch(timestamp_utc, 'unixepoch', 'localtime'), 'unixepoch')	 timestamp_local_datetime

	, cell_rssi
	, cellular_status
	, firmware_revision
	, freq
	, hardware_id
	, hw_version
	, radio_stats_I2C_bus_error_count
	, radio_stats_radio_network_id
	, radio_stats_radio_status
	, radio_stats_reboot_count
	, radio_stats_version
	, reset_flags
	, rssi
	, seq
	, serial_number
	, uptime
from
    HubStatus
create view VW_DeviceStatus
as
select
	Id
    , timestamp_utc
	, unixepoch(timestamp_utc, 'unixepoch', 'localtime') 						 timestamp_local
	, datetime(unixepoch(timestamp_utc, 'unixepoch', 'localtime'), 'unixepoch')	 timestamp_local_datetime

	, debug									
	, firmware_revision						
	, hub_rssi								
	, hub_sn               					
	, rssi									
	, sensor_status							
	, serial_number        					
	, uptime								
	, voltage								
from
    DeviceStatus
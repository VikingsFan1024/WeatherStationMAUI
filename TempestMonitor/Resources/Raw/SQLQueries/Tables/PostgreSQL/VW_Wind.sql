create view VW_Wind
as
select
    Id
    , timestamp_utc
	, unixepoch(timestamp_utc, 'unixepoch', 'localtime') 						 timestamp_local
	, datetime(unixepoch(timestamp_utc, 'unixepoch', 'localtime'), 'unixepoch')	 timestamp_local_datetime
	
    , hub_sn
    , serial_number
    , wind_direction
    , wind_speed
from
    Wind
create view VW_RainStart
as
select
	Id
    , timestamp_utc
	, unixepoch(timestamp_utc, 'unixepoch', 'localtime') 						 timestamp_local
	, datetime(unixepoch(timestamp_utc, 'unixepoch', 'localtime'), 'unixepoch')	 timestamp_local_datetime
	
	, serial_number
from
    RainStart
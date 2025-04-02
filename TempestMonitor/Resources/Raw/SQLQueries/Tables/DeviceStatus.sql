CREATE TABLE DeviceStatus
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,hub_sn            							TEXT		NOT NULL
	,firmware_revision							INTEGER		NOT NULL
	,DeviceStatusTimestamp						INTEGER		NOT NULL
	,uptime										INTEGER		NOT NULL
	,voltage									INTEGER		NOT NULL
	,rssi										INTEGER		NOT NULL
	,hub_rssi									INTEGER		NOT NULL
	,sensor_status								INTEGER		NOT NULL
	,debug										INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
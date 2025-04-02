CREATE TABLE HubStatus
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,firmware_revision							INTEGER		NOT NULL
	,HubStatusTimestamp							INTEGER		NOT NULL
	,uptime										INTEGER		NOT NULL
	,rssi										INTEGER		NOT NULL
	,reset_flags								TEXT		NOT NULL
	,seq										INTEGER		NOT NULL
	,RadioVersion								INTEGER		NOT NULL
	,RadioRebootCount							INTEGER		NOT NULL
	,I2CBusErrorCount							INTEGER		NOT NULL
	,RadioStatus								INTEGER		NOT NULL
	,RadioNetworkId								INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
CREATE TABLE AirObservation
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,hub_sn            							TEXT		NOT NULL
	,firmware_revision							INTEGER		NOT NULL
	,AirObservationTimestamp					INTEGER		NOT NULL
	,StationPressure							INTEGER		NOT NULL
	,AirTemperature								INTEGER		NOT NULL
	,RelativeHumidity							INTEGER		NOT NULL
	,LightningStrikeCount						INTEGER		NOT NULL
	,LightningStrikeAverageDistance				INTEGER		NOT NULL
	,Battery									INTEGER		NOT NULL
	,ReportInterval								INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
CREATE TABLE Observation
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,hub_sn            							TEXT		NOT NULL
	,firmware_revision							INTEGER		NOT NULL
	,ObservationTimestamp						INTEGER		NOT NULL
	,WindLull    								INTEGER		NOT NULL
	,WindAverage								INTEGER		NOT NULL
	,WindGust									INTEGER		NOT NULL
	,WindDirection								INTEGER		NOT NULL	
	,WindSampleInterval							INTEGER		NOT NULL
	,StationPressure							INTEGER		NOT NULL
	,AirTemperature								INTEGER		NOT NULL
	,RelativeHumidity							INTEGER		NOT NULL
	,Illuminance								INTEGER		NOT NULL
	,UV											INTEGER		NOT NULL
	,SolarRadiation								INTEGER		NOT NULL
	,RainAccumulationOverThePreviousMinute		INTEGER		NOT NULL
	,PrecipitationType							INTEGER		NOT NULL
	,LightningStrikeAverageDistance				INTEGER		NOT NULL
	,LightningStrikeCount						INTEGER		NOT NULL
	,Battery									INTEGER		NOT NULL
	,ReportInterval								INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
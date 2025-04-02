CREATE TABLE SkyObservation
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,hub_sn            							TEXT		NOT NULL
	,firmware_revision							INTEGER		NOT NULL
	,SkyObservationTimestamp					INTEGER		NOT NULL
	,Illuminance								INTEGER		NOT NULL
	,UV											INTEGER		NOT NULL
	,RainAccumulationOverThePreviousMinute		INTEGER		NOT NULL
	,WindLull    								INTEGER		NOT NULL
	,WindAverage								INTEGER		NOT NULL
	,WindGust									INTEGER		NOT NULL
	,WindDirectionDegrees						INTEGER		NOT NULL	
	,SolarRadiation								INTEGER		NOT NULL
	,LocalDayRainAccumulation					INTEGER		NOT NULL
	,PrecipitationType							INTEGER		NOT NULL
	,WindSampleInterval							INTEGER		NOT NULL
	,Battery									INTEGER		NOT NULL
	,ReportInterval								INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
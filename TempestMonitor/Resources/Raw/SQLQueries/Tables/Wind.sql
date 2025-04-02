CREATE TABLE Wind
(
	Id						TEXT		NOT NULL	UNIQUE
	,JsonElementString		TEXT		NOT NULL
	,type					TEXT		NOT NULL
	,serial_number			TEXT		NOT NULL
	,Timestamp				INTEGER		NOT NULL
	,hub_sn					TEXT		NOT NULL
	,WindTimestamp	 		INTEGER		NOT NULL
	,Windspeed				INTEGER		NOT NULL
	,WindDirection			INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
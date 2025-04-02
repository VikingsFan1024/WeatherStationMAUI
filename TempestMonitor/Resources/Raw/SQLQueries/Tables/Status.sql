CREATE TABLE Status
(
	Id					TEXT		NOT NULL	UNIQUE
	,ForecastId			TEXT		NOT NULL	UNIQUE
	,JsonElementString	TEXT		NOT NULL
	,status_code    	INTEGER		NOT NULL
	,status_message		TEXT		NOT NULL
	,PRIMARY KEY(ForecastId)
)
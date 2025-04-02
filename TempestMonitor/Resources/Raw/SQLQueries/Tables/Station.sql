CREATE TABLE Station
(
	Id				 	TEXT		NOT NULL	UNIQUE
	,ForecastId			TEXT		NOT NULL	UNIQUE
	,JsonElementString	TEXT		NOT NULL
	,agl               	INTEGER		NOT NULL
	,elevation         	INTEGER		NOT NULL
	,is_station_online	INTEGER		NOT NULL
	,state            	INTEGER		NOT NULL
	,station_id      	INTEGER		NOT NULL
	,PRIMARY KEY(ForecastId)
)
CREATE TABLE Forecast (
	Id							TEXT	NOT NULL UNIQUE
	,JsonElementString			TEXT	NOT NULL
	,Timestamp					INTEGER	NOT NULL
	,latitude					INTEGER NOT NULL
	,location_name				TEXT	NOT NULL
    ,longitude					INTEGER NOT NULL
    ,timezone					TEXT	NOT NULL
    ,timezone_offset_minutes	INTEGER NOT NULL
    ,source_id_conditions		INTEGER NOT NULL
	,PRIMARY KEY(id)
)
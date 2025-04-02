CREATE TABLE Daily
(
	Id                  TEXT		NOT NULL	UNIQUE
	,ForecastId			TEXT		NOT NULL
	,JsonElementString	TEXT		NOT NULL
	,air_temp_high      INTEGER		NOT NULL
	,air_temp_low       INTEGER		NOT NULL
	,conditions         TEXT		NOT NULL
	,day_num            INTEGER		NOT NULL
	,day_start_local    INTEGER		NOT NULL
	,icon               TEXT		NOT NULL
	,month_num          INTEGER		NOT NULL
	,precip_icon        TEXT		NOT NULL
	,precip_probability INTEGER		NOT NULL
	,precip_type        TEXT		NOT NULL
	,sunrise            INTEGER		NOT NULL
	,sunset             INTEGER		NOT NULL
	,PRIMARY KEY(ForecastId,id)
)
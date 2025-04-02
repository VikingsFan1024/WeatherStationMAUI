CREATE TABLE Units
(
	Id						TEXT	NOT NULL	UNIQUE
	,ForecastId				TEXT	NOT NULL	UNIQUE
	,JsonElementString		TEXT	NOT NULL
	,units_air_density     	TEXT 	NOT NULL	
	,units_brightness      	TEXT 	NOT NULL
	,units_distance        	TEXT 	NOT NULL
	,units_other           	TEXT 	NOT NULL
	,units_precip          	TEXT 	NOT NULL
	,units_pressure       	TEXT 	NOT NULL
	,units_solar_radiation	TEXT 	NOT NULL
	,units_temp           	TEXT 	NOT NULL
	,units_wind            	TEXT 	NOT NULL
	,PRIMARY KEY(ForecastId)
)
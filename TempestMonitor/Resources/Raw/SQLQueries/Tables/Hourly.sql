CREATE TABLE Hourly
(
	Id                      		TEXT		NOT NULL	UNIQUE
	,ForecastId						TEXT		NOT NULL
	,JsonElementString				TEXT		NOT NULL
	,air_temperature				INTEGER		NOT NULL
	,conditions              		TEXT		NOT NULL
	,feels_like              		INTEGER		NOT NULL
	,icon                    		TEXT		NOT NULL
	,local_day               		INTEGER		NOT NULL
	,local_hour              		INTEGER		NOT NULL
	,precip                  		INTEGER		NOT NULL
	,precip_icon             		TEXT		NOT NULL
	,precip_probability      		INTEGER		NOT NULL
	,precip_type             		TEXT		NOT NULL
	,relative_humidity       		INTEGER		NOT NULL
	,sea_level_pressure      		INTEGER		NOT NULL
	,station_pressure        		INTEGER		NOT NULL
	,time                    		INTEGER		NOT NULL
	,uv                      		INTEGER		NOT NULL
	,wind_avg                		INTEGER		NOT NULL
	,wind_direction          		INTEGER		NOT NULL
	,wind_direction_cardinal		TEXT		NOT NULL
	,wind_gust               		INTEGER		NOT NULL
	,PRIMARY KEY(ForecastId,id)
)
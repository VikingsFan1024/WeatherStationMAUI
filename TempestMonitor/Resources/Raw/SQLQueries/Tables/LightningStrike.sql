CREATE TABLE LightningStrike
(
	Id											TEXT		NOT NULL	UNIQUE
	,JsonElementString							TEXT		NOT NULL
	,type										TEXT		NOT NULL
	,serial_number								TEXT		NOT NULL
	,Timestamp     								INTEGER		NOT NULL
	,hub_sn            							TEXT		NOT NULL
	,StrikeTimestamp							INTEGER		NOT NULL
	,Distance									INTEGER		NOT NULL
	,Energy										INTEGER		NOT NULL
	,PRIMARY KEY(id)
)
create table LightningStrike
(
    id               integer primary key autoincrement
    , json_document  text
    , serial_number  text 	 generated always as (json_extract(json_document, '$.serial_number')) stored
	, timestamp_utc	 integer generated always as (json_extract(json_document, '$.evt[0]')) stored
	, type           text 	 generated always as (json_extract(json_document, '$.type')) stored

	, distance		 integer generated always as (json_extract(json_document, '$.evt[1]')) stored
	, energy		 integer generated always as (json_extract(json_document, '$.evt[2]')) stored		
    , hub_sn         text 	 generated always as (json_extract(json_document, '$.hub_sn')) stored		
)
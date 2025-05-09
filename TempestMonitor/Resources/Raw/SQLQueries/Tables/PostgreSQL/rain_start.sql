drop table if exists public.rain_start;

create table rain_start
(
     id						integer not null	generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original	json 	not null
	,json_document			jsonb	not null	generated always as (json_document_original) stored
	,serial_number			text				generated always as (json_document_original -> 'serial_number') stored
	,timestamp_utc			integer				generated always as ((json_document_original -> 'evt' ->> 0)::integer) stored
	,type					text				generated always as (json_document_original -> 'type') stored

	,hub_sn					text				generated always as (json_document_original -> 'hub_sn') stored
	
    ,constraint observation_pkey primary key (id)
)

tablespace pg_default;

alter table if exists public.rain_start
    owner to dan;
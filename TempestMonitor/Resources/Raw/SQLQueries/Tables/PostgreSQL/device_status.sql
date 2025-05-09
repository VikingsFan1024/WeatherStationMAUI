-- drop table if exists public.device_status;
alter table device_status rename constraint device_status_pkey to device_status_old_pkey;
alter table device_status rename to device_status_old;
create table if not exists public.device_status
(
    id						integer	not null	generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original	json 	not null
	,json_document			jsonb	not null	generated always as (json_document_original) stored
	,serial_number			text				generated always as (json_document_original->'serial_number') stored
	,timestamp_utc			bigint				generated always as ((json_document_original->>'timestamp')::bigint) stored
	,type					text				generated always as (json_document_original->'type') stored
			
	,firmware_revision		integer				generated always as ((json_document_original->>'firmware_revision')::integer) stored
	,hub_sn					text				generated always as (json_document_original->'hub_sn') stored
			
	,debug					integer				generated always as ((json_document_original->>'debug')::integer) stored
	,hub_rssi				integer				generated always as ((json_document_original->>'hub_rssi')::integer) stored
	,rssi					integer				generated always as ((json_document_original->>'rssi')::integer) stored
	,sensor_status			integer				generated always as ((json_document_original->>'sensor_status')::integer) stored
	,uptime					integer				generated always as ((json_document_original->>'uptime')::integer) stored
	,voltage				real				generated always as ((json_document_original->>'voltage')::real) stored
	
    ,constraint device_status_pkey primary key (id)
);

alter table if exists public.device_status owner to dan;

with data as (
select distinct cast(json_document_original as text) col1 from device_status_old
)
insert into device_status (json_document_original)
select cast(col1 as json) from data
-- drop table if exists public.hub_status;
alter table hub_status rename constraint hub_status_pkey to hub_status_old_pkey;
alter table hub_status rename to hub_status_old;
create table hub_status
(
    id									integer not null generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original				json 	not null
	,json_document						jsonb	not null generated always as (json_document_original) stored
	,serial_number						text	generated always as (json_document_original->'serial_number') stored
	,timestamp_utc						bigint	generated always as ((json_document_original->>'timestamp')::bigint) stored
	,type								text	generated always as (json_document_original->'type') stored

	,firmware_revision					integer	generated always as ((json_document_original->>'firmware_revision')::integer) stored

	,cell_rssi							integer	generated always as ((json_document_original->>'cell_rssi')::integer) stored
	,cellular_status					integer	generated always as ((json_document_original->>'cellular_status')::integer) stored
	,freq								integer	generated always as ((json_document_original->>'freq')::integer) stored
	,hardware_id						integer	generated always as ((json_document_original->>'hardware_id')::integer) stored
	,hw_version							integer	generated always as ((json_document_original->>'hw_version')::integer) stored
	,radio_stats_i2c_bus_error_count	integer	generated always as ((json_document_original->'radio_stats'->>2)::integer) stored
	,radio_stats_radio_network_id		integer	generated always as ((json_document_original->'radio_stats'->>4)::integer) stored
	,radio_stats_radio_status			integer	generated always as ((json_document_original->'radio_stats'->>3)::integer) stored
	,radio_stats_reboot_count			integer	generated always as ((json_document_original->'radio_stats'->>1)::integer) stored
	,radio_stats_version				integer	generated always as ((json_document_original->'radio_stats'->>0)::integer) stored
	,rssi								integer	generated always as ((json_document_original->>'rssi')::integer) stored
	,seq								integer	generated always as ((json_document_original->>'seq')::integer) stored
	,uptime								integer	generated always as ((json_document_original->>'uptime')::integer) stored
    ,reset_flags          				text 	generated always as (json_document_original->'reset_flags') stored
	
    ,constraint hub_status_pkey primary key (id)
);
alter table if exists public.hub_status owner to dan;
with data as (
select distinct cast(json_document_original as text) col1 from hub_status_old
)
insert into hub_status (json_document_original)
select cast(col1 as json) from data
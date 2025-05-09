--drop table if exists public.wind;
alter table wind rename constraint wind_pkey to wind_old_pkey;
alter table wind rename to wind_old;
create table if not exists public.wind
(
    id						integer not null 	generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original	json 	not null
	,json_document			jsonb	not null 	generated always as (json_document_original) stored
	,serial_number			text				generated always as (json_document_original->'serial_number') stored
	,timestamp_utc			bigint				generated always as ((json_document_original->'ob'->>0)::bigint) stored
	,type					text				generated always as (json_document_original->'type') stored
			
	,hub_sn					text				generated always as (json_document_original->'hub_sn') stored
			
	,wind_direction			integer				generated always as ((json_document_original->'ob'->>2)::integer) stored
	,wind_speed				real				generated always as ((json_document_original->'ob'->>1)::real) stored
	
    ,constraint wind_pkey primary key (id)
);
alter table if exists public.wind owner to dan;
with data as (
select distinct cast(json_document_original as text) col1 from wind_old
)
insert into wind (json_document_original)
select cast(col1 as json) from data
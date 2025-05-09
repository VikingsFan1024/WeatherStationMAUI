--drop table if exists public.lightning_strike;
alter table lightning_strike rename constraint lightning_strike_pkey to lightning_strike_old_pkey;
alter table lightning_strike rename to lightning_strike_old;
create table lightning_strike
(
     id						integer not null generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original	json 	not null
	,json_document			jsonb	not null generated always as (json_document_original) stored
	,serial_number			text	generated always as (json_document_original->'serial_number') stored
	,timestamp_utc			bigint	generated always as ((json_document_original->'evt'->>0)::bigint) stored
	,type					text	generated always as (json_document_original->'type') stored

	,hub_sn					text	generated always as (json_document_original->'hub_sn') stored

	,distance				integer	generated always as ((json_document_original->'evt'->>1)::integer) stored
	,energy					integer	generated always as ((json_document_original->'evt'->>2)::integer) stored		
	
    ,constraint lightning_strike_pkey primary key (id)
);
alter table if exists public.lightning_strike owner to dan;
with data as (
select distinct cast(json_document_original as text) col1 from lightning_strike_old
)
insert into lightning_strike (json_document_original)
select cast(col1 as json) from data
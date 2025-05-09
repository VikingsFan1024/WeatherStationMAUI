--drop table if exists public.weather_forecast;
alter table weather_forecast rename constraint weather_forecast_pkey to weather_forecast_old_pkey;
alter table weather_forecast rename to weather_forecast_old;
create table weather_forecast
(
     id						integer not null	generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
    ,json_document_original	json 	not null
	,json_document			jsonb	not null	generated always as (json_document_original) stored

    ,constraint weather_forecast_pkey primary key (id)
);
alter table if exists public.weather_forecast owner to dan;
with data as (
select distinct cast(json_document_original as text) col1 from weather_forecast_old
)
insert into weather_forecast (json_document_original)
select cast(col1 as json) from data
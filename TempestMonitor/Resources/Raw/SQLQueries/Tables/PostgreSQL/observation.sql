alter table observation rename constraint observation_pkey to observation_old_pkey;
alter table observation rename to observation_old;
-- table: public.observation
-- drop table if exists public.observation;
create table if not exists public.observation
    (
        id 												integer	not null	generated always as identity ( increment 1 start 1 minvalue 1 maxvalue 2147483647 cache 1 )
        , json_document_original 						json	not null
        , json_document 								jsonb 				generated always as (json_document_original) stored
        , serial_number                              	text 				generated always as ((json_document_original->'serial_number'::text)) stored
        , timestamp_utc                              	bigint 				generated always as (((((json_document_original->'obs'::text)->0)->>0))::bigint) stored
        , type                                       	text				generated always as ((json_document_original->'type'::text)) stored
		
        , hub_sn                                     	text				generated always as ((json_document_original->'hub_sn'::text)) stored
		
        , air_temperature                            	real				generated always as (((((json_document_original->'obs'::text)->0)->>7))::real) stored
        , battery                                    	real				generated always as (((((json_document_original->'obs'::text)->0)->>16))::real) stored
        , illuminance                                	real				generated always as (((((json_document_original->'obs'::text)->0)->>9))::real) stored
        , lightning_strike_average_distance          	real				generated always as (((((json_document_original->'obs'::text)->0)->>14))::real) stored
        , lightning_strike_count                     	integer				generated always as (((((json_document_original->'obs'::text)->0)->>15))::integer) stored
        , precipitation_type                         	integer				generated always as (((((json_document_original->'obs'::text)->0)->>13))::integer) stored
        , rain_accumulation_over_the_previous_minute	real				generated always as (((((json_document_original->'obs'::text)->0)->>12))::real) stored
        , relative_humidity                          	real				generated always as (((((json_document_original->'obs'::text)->0)->>8))::real) stored
        , reporting_interval                         	integer				generated always as (((((json_document_original->'obs'::text)->0)->>17))::integer) stored
        , solar_radiation                            	real				generated always as (((((json_document_original->'obs'::text)->0)->>11))::real) stored
        , station_pressure                           	real				generated always as (((((json_document_original->'obs'::text)->0)->>6))::real) stored
        , uv                                         	real				generated always as (((((json_document_original->'obs'::text)->0)->>10))::real) stored
        , wind_average                               	real				generated always as (((((json_document_original->'obs'::text)->0)->>2))::real) stored
        , wind_direction                             	integer				generated always as (((((json_document_original->'obs'::text)->0)->>4))::integer) stored
        , wind_gust                                  	real				generated always as (((((json_document_original->'obs'::text)->0)->>3))::real) stored
        , wind_lull                                  	real				generated always as (((((json_document_original->'obs'::text)->0)->>1))::real) stored
        , wind_sample_interval                       	integer				generated always as (((((json_document_original->'obs'::text)->0)->>5))::integer) stored
		
        , constraint observation_pkey primary key (id)
    );
	
alter table if exists public.observation owner to dan;

with data as (
select distinct cast(json_document_original as text) col1 from observation_old
)
insert into observation (json_document_original)
select cast(col1 as json) from data

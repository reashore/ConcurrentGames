
create table nfl.market(
  market_id serial primary key,
  name varchar(45) default null,
  ot integer default null,
  inplay integer default null,
  prematch integer default null,
  abbr varchar(16) default null,
  period integer default '0'
);


create or replace function nfl.get_games(number_game_days integer DEFAULT 7)
    returns TABLE(game_id uuid, start_time timestamp without time zone, home_id uuid, away_id uuid)
    language plpgsql
as
$$
declare
    start_date date;
    end_date date;
begin
    select
        *
    from
        nfl.get_min_and_max_game_start_dates(number_game_days)
    into
        start_date,
        end_date;

    return query
        select
            game_id,
            start_time,
            home_id,
            away_id
        from
            nfl.game
        where
            start_time between start_date and end_date and
            lower(status) != 'closed'
        order by
            start_time;
end
$$;

create or replace function nfl.get_team(in team_id_in uuid)
returns
table (
	name varchar(45),
	-- todo rename to short_name
	alias varchar(4),
	market varchar(45)
)
as
$$
	select
		name,
		alias,
		market
	from
		nfl.team
	where
		team_id = team_id_in
	limit 1;
$$
language sql;

create or replace function nfl.get_players(in team_id_in uuid)
returns
table (
	player_id uuid,
	full_name varchar(45),
	jersey varchar(4)
)
as
$$
	select
		player_id,
		full_name,
		jersey
	from
		nfl.player  --p left join
		--nfl.injury i on i.player_id = p.player_id
	where
		status = 'a01' and
		-- i.player_id is null and
		team_id = team_id_in;
$$
language sql;

create or replace function nfl.get_markets()
returns
table
(
	market_id integer,
	name varchar(255)
)
as
$$
	select
		market_id,
		name
	from
		nfl.market;
$$
language sql;

create or replace function nfl.get_intsf(
	in team_id_in uuid,
    in side_in varchar(1),
    in stats_aspect_scope_in varchar(2),
    in statistic_type_in varchar(20),
    in period_in varchar(2),
    in number_games_in integer
    )
returns
table (
   	team_id uuid,
	imtr varchar(4),
	its integer,
	itvh varchar(1),
	isas varchar(2),
	iqt varchar(3),
	season varchar(8),
	m0 double precision,
	m1 double precision,
	m2 double precision,
	m3 double precision,
	m4 double precision,
	m5 double precision,
	m6 double precision,
	m7 double precision,
	m8 double precision,
	m9 double precision,
	m10 double precision,
	m11 double precision,
	m12 double precision,
	m13 double precision,
	m14 double precision,
	sd_m0 double precision,
	sd_m1 double precision,
	sd_m2 double precision,
	sd_m3 double precision,
	sd_m4 double precision,
	sd_m5 double precision,
	sd_m6 double precision,
	sd_m7 double precision,
	sd_m8 double precision,
	sd_m9 double precision,
	sd_m10 double precision,
	sd_m11 double precision,
	sd_m12 double precision,
	sd_m13 double precision,
	sd_m14 double precision,
	lg_m0 double precision,
	lg_m1 double precision,
	lg_m2 double precision,
	lg_m3 double precision,
	lg_m4 double precision,
	lg_m5 double precision,
	lg_m6 double precision,
	lg_m7 double precision,
	lg_m8 double precision,
	lg_m9 double precision,
	lg_m10 double precision,
	lg_m11 double precision,
	lg_m12 double precision,
	lg_m13 double precision,
	lg_m14 double precision,
	lg_m0sd double precision,
	lg_m1sd double precision,
	lg_m2sd double precision,
	lg_m3sd double precision,
	lg_m4sd double precision,
	lg_m5sd double precision,
	lg_m6sd double precision,
	lg_m7sd double precision,
	lg_m8sd double precision,
	lg_m9sd double precision,
	lg_m10sd double precision,
	lg_m11sd double precision,
	lg_m12sd double precision,
	lg_m13sd double precision,
	lg_m14sd double precision,
	f_m0 double precision,
	f_m1 double precision,
	f_m2 double precision,
	f_m3 double precision,
	f_m4 double precision,
	f_m5 double precision,
	f_m6 double precision,
	f_m7 double precision,
	f_m8 double precision,
	f_m9 double precision,
	f_m10 double precision,
	f_m11 double precision,
	f_m12 double precision,
	f_m13 double precision,
	f_m14 double precision
)
as
$$
	select
		team_id, imtr, its, itvh, isas, iqt, season,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14,
        sd_m0, sd_m1, sd_m2, sd_m3, sd_m4, sd_m5, sd_m6, sd_m7, sd_m8, sd_m9, sd_m10, sd_m11, sd_m12, sd_m13, sd_m14,
        lg_m0, lg_m1, lg_m2, lg_m3, lg_m4, lg_m5, lg_m6, lg_m7, lg_m8, lg_m9,lg_m10, lg_m11, lg_m12, lg_m13, lg_m14,
        lg_m0sd, lg_m1sd, lg_m2sd, lg_m3sd, lg_m4sd, lg_m5sd, lg_m6sd, lg_m7sd, lg_m8sd, lg_m9sd,lg_m10sd, lg_m11sd, lg_m12sd, lg_m13sd, lg_m14sd,
        f_m0, f_m1, f_m2, f_m3, f_m4, f_m5, f_m6, f_m7, f_m8, f_m9,f_m10, f_m11, f_m12, f_m13, f_m14
	from
		nfl.intsf
	where
		team_id = team_id_in and
		imtr = statistic_type_in and
		its = number_games_in and
		itvh = side_in and
		isas = stats_aspect_scope_in and
		iqt = period_in
	order by
		created_on desc;
$$
language sql;

create or replace function nfl.get_team_intsf(in team_id_in uuid, in side_in varchar(1))
returns
table (
   	team_id uuid,
	imtr varchar(4),
	its integer,
	itvh varchar(1),
	isas varchar(2),
	iqt varchar(3),
	m0 double precision,
	m1 double precision,
	m2 double precision,
	m3 double precision,
	m4 double precision,
	m5 double precision,
	m6 double precision,
	m7 double precision,
	m8 double precision,
	m9 double precision,
	m10 double precision,
	m11 double precision,
	m12 double precision,
	m13 double precision,
	m14 double precision,
	f_m0 double precision,
	f_m1 double precision,
	f_m2 double precision,
	f_m3 double precision,
	f_m4 double precision,
	f_m5 double precision,
	f_m6 double precision,
	f_m7 double precision,
	f_m8 double precision,
	f_m9 double precision,
	f_m10 double precision,
	f_m11 double precision,
	f_m12 double precision,
	f_m13 double precision,
	f_m14 double precision
)
as
$$
	select distinct
		team_id, imtr, its, itvh, isas, iqt,
        m0,m1,m2,m3,m4,m5,m6,m7,m8,m9,m10,m11,m12,m13,m14,
        f_m0,f_m1,f_m2,f_m3,f_m4,f_m5,f_m6,f_m7,f_m8,f_m9,f_m10,f_m11,f_m12,f_m13,f_m14
	from
		nfl.intsf
	where
        imtr = 'P' and
		team_id = team_id_in and
        itvh = side_in
	order by
		its desc
	limit 8;
$$
language sql;

create or replace function nfl.get_team_insc(in team_id_in uuid, in side_in varchar(1))
returns
table (
	quarter integer,
	avg_m0 numeric,
	avg_m1 numeric,
	avg_m2 numeric,
	avg_m3 numeric,
	avg_m4 numeric,
	avg_m5 numeric,
	avg_m6 numeric,
	avg_m7 numeric,
	avg_m8 numeric,
	avg_m9 numeric,
	avg_m10 numeric,
	avg_m11 numeric,
	avg_m12 numeric,
	avg_m13 numeric,
	avg_m14 numeric
)
as
$$
	select distinct
		period as quarter,
		avg(m0) as avg_m0,
		avg(m1) as avg_m1,
		avg(m2) as avg_m2,
		avg(m3) as avg_m3,
		avg(m4) as avg_m4,
		avg(m5) as avg_m5,
		avg(m6) as avg_m6,
		avg(m7) as avg_m7,
		avg(m8) as avg_m8,
		avg(m9) as avg_m9,
		avg(m10) as avg_m10,
		avg(m11) as avg_m11,
		avg(m12) as avg_m12,
		avg(m13) as avg_m13,
		avg(m14) as avg_m14
	from
		nfl.teams_gm_min_box
	where
		team_id = team_id_in and
        period < 5 and
        home = side_in
	group by
		period;
$$
language sql;



create or replace function nfl.get_intsf(
	in team_id_in uuid,
    in side_in varchar(1),
    in stats_aspect_scope_in varchar(2),
    in statistic_type_in varchar(20),
    in period_in varchar(2),
    in number_games_in int
    )
returns
table (
    team_id uuid,
	imtr varchar(4),
	its integer,
	itvh varchar(1),
	isas varchar(2),
	iqt varchar(3),
	season varchar(8),
	m0 double precision,
	m1 double precision,
	m2 double precision,
	m3 double precision,
	m4 double precision,
	m5 double precision,
	m6 double precision,
	m7 double precision,
	m8 double precision,
	m9 double precision,
	m10 double precision,
	m11 double precision,
	m12 double precision,
	m13 double precision,
	m14 double precision,
	sd_m0 double precision,
	sd_m1 double precision,
	sd_m2 double precision,
	sd_m3 double precision,
	sd_m4 double precision,
	sd_m5 double precision,
	sd_m6 double precision,
	sd_m7 double precision,
	sd_m8 double precision,
	sd_m9 double precision,
	sd_m10 double precision,
	sd_m11 double precision,
	sd_m12 double precision,
	sd_m13 double precision,
	sd_m14 double precision,
	lg_m0 double precision,
	lg_m1 double precision,
	lg_m2 double precision,
	lg_m3 double precision,
	lg_m4 double precision,
	lg_m5 double precision,
	lg_m6 double precision,
	lg_m7 double precision,
	lg_m8 double precision,
	lg_m9 double precision,
	lg_m10 double precision,
	lg_m11 double precision,
	lg_m12 double precision,
	lg_m13 double precision,
	lg_m14 double precision,
	lg_m0sd double precision,
	lg_m1sd double precision,
	lg_m2sd double precision,
	lg_m3sd double precision,
	lg_m4sd double precision,
	lg_m5sd double precision,
	lg_m6sd double precision,
	lg_m7sd double precision,
	lg_m8sd double precision,
	lg_m9sd double precision,
	lg_m10sd double precision,
	lg_m11sd double precision,
	lg_m12sd double precision,
	lg_m13sd double precision,
	lg_m14sd double precision,
	f_m0 double precision,
	f_m1 double precision,
	f_m2 double precision,
	f_m3 double precision,
	f_m4 double precision,
	f_m5 double precision,
	f_m6 double precision,
	f_m7 double precision,
	f_m8 double precision,
	f_m9 double precision,
	f_m10 double precision,
	f_m11 double precision,
	f_m12 double precision,
	f_m13 double precision,
	f_m14 double precision
)
as
$$
	select
		team_id, imtr, its, itvh, isas, iqt, season,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14,
        sd_m0, sd_m1, sd_m2, sd_m3, sd_m4, sd_m5, sd_m6, sd_m7, sd_m8, sd_m9, sd_m10, sd_m11, sd_m12, sd_m13, sd_m14,
        lg_m0, lg_m1, lg_m2, lg_m3, lg_m4, lg_m5, lg_m6, lg_m7, lg_m8, lg_m9,lg_m10, lg_m11, lg_m12, lg_m13, lg_m14,
        lg_m0sd, lg_m1sd, lg_m2sd, lg_m3sd, lg_m4sd, lg_m5sd, lg_m6sd, lg_m7sd, lg_m8sd, lg_m9sd,lg_m10sd, lg_m11sd, lg_m12sd, lg_m13sd, lg_m14sd,
        f_m0, f_m1, f_m2, f_m3, f_m4, f_m5, f_m6, f_m7, f_m8, f_m9,f_m10, f_m11, f_m12, f_m13, f_m14
	from
		nfl.intsf
	where
		team_id = team_id_in and
		imtr = statistic_type_in and
		its = number_games_in and
		itvh = side_in and
		isas = stats_aspect_scope_in and
		iqt = period_in
	order by
		created_on desc;
$$
language sql;

create or replace function nfl.get_xs(
	team_id_in uuid,
    side_in varchar(1)
    )
returns
table (
    team_id uuid,
	itvh varchar(2),
    fga double precision,
	pa double precision,
	sca double precision
)
as
$$
	select
		team_id, itvh, fga, pa, sca
	from
		nfl.xs
	where
		team_id = team_id_in and
		itvh = side_In
	order by
	    its desc
	limit 1;
$$
language sql;

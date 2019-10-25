
create or replace function nba.get_games()
returns
table (
	gameid uuid,
	schedule timestamp,
	-- todo change to uuid
	home_id varchar(45),
	away_id varchar(45)
)
as
$$
	select
		game_id,
		schedule,
		home_id,
		away_id
	 from
		nba.game
	 where
		status != 'closed' and
		public.is_in_game_window(schedule)
	order by
		schedule asc;
$$
language sql;

create or replace function nba.get_team(in team_id_in uuid)
returns
table (
    team_id uuid,
    name varchar(45),
    alias varchar(45)
)
as
$$
	select
	    team_id,
        name,
        alias
	from
		nba.team
	where
		team_id = team_id_in;
$$
language sql;

create or replace function nba.get_players(in team_id_in varchar(36))
returns
table (
    player_id uuid,
    full_name varchar(45),
    jersey integer
)
as
$$
	select
		player_id,
		full_name,
        jersey
	from
		nba.player
	where
        status = 1 and
		team_id = team_id_in;
    -- this is dangerous as it could truncate the best player
    -- limit 15;
$$
language sql;

create or replace function nba.get_pop(in player_id_in uuid, in side_in varchar(1))
returns
table (
	imtr varchar(4),
	iqt varchar(3),
	its integer,
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
	m11 double precision
)
as
$$
	select
		imtr, iqt, its,
	    m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11
	from
		nba.pop
	where
		player_id = player_id_in  and
        itvh = side_in and
        iqt in ('q1','q2','q3','q4')
	order by
		its;
$$
language sql;

create or replace function nba.get_posc(in player_id_in uuid)
returns
table (
   	imtr varchar(4),
	its integer,
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
	m11 double precision
)
as
$$
	select
        imtr, its, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11
	from
		nba.posc
	where
		player_id = player_id_in  and
        iqt in ('q1','q2','q3','q4')
	order by
		its;
$$
language sql;

create or replace function nba.get_potm(
	in player_id_in uuid,
    in side_in varchar(2),
    in opponent_team_id_in uuid)
returns
table (
   	imtr varchar(4),
	its integer,
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
	m11 double precision
)
as
$$
	select
	    imtr, its, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11
	from
		nba.potm
	where
		player_id = player_id_in  and
        opponent_id = opponent_team_id_in and
        itvh = side_in
	order by
		its;
$$
language sql;

create or replace function nba.get_psco(in player_id_in uuid, in side_in varchar(2))
returns
table (
   	imtr varchar(4),
	its integer,
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
	m11 double precision
)
as
$$
	select
        imtr, its, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11
	from
		nba.psco
	where
		player_id = player_id_in  and
        itvh = side_in and
        iqt in ('q1','q2','q3','q4')
	order by
		its;
$$
language sql;

create or replace function nba.get_sdom(in player_id_in uuid)
returns
table (
	imtr varchar(4),
	cg double precision
)
as
$$
    -- use common table expression
	select
        a.imtr,
		a.cg
	from
		nba.sdom a inner join (
            select
				player_id,
                max(its) its
			from
				nba.sdom
			group by
				player_id
             ) b on a.player_id = b.player_id and a.its = b.its
	where
		a.player_id = player_id_in;
$$
language sql;

create or replace function nba.get_team_inlsf(in quarter_in varchar(12))
returns
table (
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
	iqt varchar(3)
)
as
$$
	select
		lg_m0, lg_m1, lg_m2, lg_m3, lg_m4, lg_m5, lg_m6, lg_m7, lg_m8, lg_m9, lg_m10, lg_m11, iqt
	from
		nba.intsf
	where
		iqt = quarter_in
	order by
		its desc
	limit 1;
$$
language sql;

create or replace function nba.get_team_insc(in team_id_in varchar(36), in side_in varchar(2))
-- todo change varchar(36) to uuid
returns
table (
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
	quarter integer
)
as
$$
	select distinct
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
		period as quarter
	from
		nba.teams_gm_min_box
	where
		team_id = team_id_in and
		period < 5 and
		home = side_in
	group by
		period;
$$
language sql;

create function nba.get_team_insc(in team_id_in varchar(36), in side_in varchar(2))
-- change varchar(36) to uuid
    returns table(
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
        quarter integer)
    language sql
as
$$
select distinct
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
		period as quarter
	from
		nba.teams_gm_min_box
	where
		team_id = team_id_in and
		period < 5 and
	    -- rename home to side
		home = side_in
	group by
		period;
$$;

create or replace function nba.get_team_intsf(in team_id_in uuid, side_in varchar(1))
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
	season varchar(8),
	season_type varchar(5),
	created_on timestamp
)
as
$$
	select distinct
		team_id, imtr, its, itvh, isas, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11,
		sd_m0, sd_m1, sd_m2, sd_m3, sd_m4, sd_m5, sd_m6, sd_m7, sd_m8, sd_m9, sd_m10, sd_m11,
		lg_m0, lg_m1,lg_m2, lg_m3,lg_m4, lg_m5,lg_m6, lg_m7,lg_m8, lg_m9, lg_m10, lg_m11,
		lg_m0sd, lg_m1sd, lg_m2sd, lg_m3sd, lg_m4sd, lg_m5sd, lg_m6sd, lg_m7sd, lg_m8sd, lg_m9sd, lg_m10sd, lg_m11sd,
		f_m0, f_m1, f_m2, f_m3, f_m4, f_m5, f_m6, f_m7, f_m8, f_m9, f_m10, f_m11,
		season, season_type, created_on
	from
		nba.intsf
	where
		imtr = 'p' and
		team_id = team_id_in and
		itvh = side_in
	order by
		its desc
	limit 8;
$$
language sql;

create or replace function nba.get_team_intss_fge(in team_id_in uuid, in side_in varchar(1))
returns
table (
    fge double precision
)
as
$$
declare
	declare f2a integer default 0;
	declare f2m integer default 0;
	declare f3a integer default 0;
	declare f3m integer default 0;
begin
	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f2a
	from
		nba.teams_gm_min_box
	where
		statistic = 'twopointattempted' and
		team_id = team_id_in and
        home = side_in
	group by
		team_id;

	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f2m
	from
		nba.teams_gm_min_box
	where
		statistic = 'twopointmade' and
		team_id = team_id_in and
        home = side_in
	group by
		team_id;

	select  distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f3a
	from
		nba.teams_gm_min_box
	where
		statistic = 'threepointattempted' and
		team_id = team_id_in and
        home = side_in
	group by
		team_id;

	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f3m
	from
		nba.teams_gm_min_box
	where
		statistic = 'threepointmade' and
		team_id = team_id_in and
        home = side_in
	group by
		team_id;

	select cast((f2m + f3m) as double precision) / cast((f2a + f3a) as double precision) as fge;
end;
$$
language plpgsql;

create or replace function nba.get_ttm(in team_id_in uuid)
returns
table (
   	imtr varchar(3),
	its integer,
	cg double precision,
	h1 double precision,
	h2 double precision,
	q1 double precision,
	q2 double precision,
	q3 double precision,
	q4 double precision
)
as
$$
	select
		imtr, its,
	    cg, h1, h2, q1, q2, q3, q4
	from
		nba.ttm
	where
		team_id = team_id_in
	order by
		its desc
	limit 12;
$$
language sql;

create or replace function nba.get_markets()
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
		nba.market;
$$
language sql;

--create or replace function nba.get_intss(in team_id_in uuid, side_in varchar(1))
--returns
--table (
--	team_id uuid,
--	its integer,
--	itvh varchar(1),
--	f3 double precision,
--	f2 double precision,
--	ft double precision,
--	f3acc double precision,
--	f2acc double precision,
--	ftacc double precision,
--	f3r double precision,
--	f2r double precision,
--	ftr double precision,
--	sd_f3 double precision,
--	sd_f2 double precision,
--	sd_ft double precision,
--	sd_f3acc double precision,
--	sd_f2acc double precision,
--	sd_ftacc double precision,
--	sd_f3r double precision,
--	sd_f2r double precision,
--	sd_ftr double precision,
--	season varchar(8),
--	season_type varchar(5),
--	created_on timestamp
--)
--as
--$$
--	select
--		team_id,
--		its,
--		itvh,
--		f3,
--		f2,
--		ft,
--		f3acc,
--		f2acc,
--		ftacc,
--		f3r,
--		f2r,
--		ftr,
--		sd_f3,
--		sd_f2,
--		sd_ft,
--		sd_f3acc,
--		sd_f2acc,
--		sd_ftacc,
--		sd_f3r,
--		sd_f2r,
--		sd_ftr,
--		season,
--		season_type,
--		created_on
--	from
--		nba.intss
--	where
--		team_id = team_id_in and
--		itvh = side_in
--	order by
--		its desc
--	limit 1;
--$$
--language sql;

--create or replace function nba.get_team_inlsf(in quarter_in varchar(12))
--returns
--table (
--	lg_m0 double precision,
--	lg_m1 double precision,
--	lg_m2 double precision,
--	lg_m3 double precision,
--	lg_m4 double precision,
--	lg_m5 double precision,
--	lg_m6 double precision,
--	lg_m7 double precision,
--	lg_m8 double precision,
--	lg_m9 double precision,
--	lg_m10 double precision,
--	lg_m11 double precision,
--	iqt varchar(3)
--)
--as
--$$
--	select
--		lg_m0, lg_m1, lg_m2, lg_m3, lg_m4, lg_m5, lg_m6, lg_m7, lg_m8, lg_m9, lg_m10, lg_m11, iqt
--	from
--		nba.intsf
--	where
--		iqt = quarter_in
--	order by
--		its desc
--	limit 1;
--$$
--language sql;

--create or replace function nba.get_team_intsf(in team_id_in uuid, side_in varchar(1))
--returns
--table (
--	team_id uuid,
--	imtr varchar(4),
--	its integer,
--	itvh varchar(1),
--	isas varchar(2),
--	iqt varchar(3),
--	m0 double precision,
--	m1 double precision,
--	m2 double precision,
--	m3 double precision,
--	m4 double precision,
--	m5 double precision,
--	m6 double precision,
--	m7 double precision,
--	m8 double precision,
--	m9 double precision,
--	m10 double precision,
--	m11 double precision,
--	sd_m0 double precision,
--	sd_m1 double precision,
--	sd_m2 double precision,
--	sd_m3 double precision,
--	sd_m4 double precision,
--	sd_m5 double precision,
--	sd_m6 double precision,
--	sd_m7 double precision,
--	sd_m8 double precision,
--	sd_m9 double precision,
--	sd_m10 double precision,
--	sd_m11 double precision,
--	lg_m0 double precision,
--	lg_m1 double precision,
--	lg_m2 double precision,
--	lg_m3 double precision,
--	lg_m4 double precision,
--	lg_m5 double precision,
--	lg_m6 double precision,
--	lg_m7 double precision,
--	lg_m8 double precision,
--	lg_m9 double precision,
--	lg_m10 double precision,
--	lg_m11 double precision,
--	lg_m0sd double precision,
--	lg_m1sd double precision,
--	lg_m2sd double precision,
--	lg_m3sd double precision,
--	lg_m4sd double precision,
--	lg_m5sd double precision,
--	lg_m6sd double precision,
--	lg_m7sd double precision,
--	lg_m8sd double precision,
--	lg_m9sd double precision,
--	lg_m10sd double precision,
--	lg_m11sd double precision,
--	f_m0 double precision,
--	f_m1 double precision,
--	f_m2 double precision,
--	f_m3 double precision,
--	f_m4 double precision,
--	f_m5 double precision,
--	f_m6 double precision,
--	f_m7 double precision,
--	f_m8 double precision,
--	f_m9 double precision,
--	f_m10 double precision,
--	f_m11 double precision,
--	season varchar(8),
--	season_type varchar(5),
--	created_on timestamp
--)
--as
--$$
--	select distinct
--		team_id, imtr, its, itvh, isas, iqt,
--		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11,
--		sd_m0, sd_m1, sd_m2, sd_m3, sd_m4, sd_m5, sd_m6, sd_m7, sd_m8, sd_m9, sd_m10, sd_m11,
--		lg_m0, lg_m1,lg_m2, lg_m3,lg_m4, lg_m5,lg_m6, lg_m7,lg_m8, lg_m9, lg_m10, lg_m11,
--		lg_m0sd, lg_m1sd, lg_m2sd, lg_m3sd, lg_m4sd, lg_m5sd, lg_m6sd, lg_m7sd, lg_m8sd, lg_m9sd, lg_m10sd, lg_m11sd,
--		f_m0, f_m1, f_m2, f_m3, f_m4, f_m5, f_m6, f_m7, f_m8, f_m9, f_m10, f_m11,
--		season, season_type, created_on
--	from
--		nba.intsf
--	where
--		imtr = 'p' and
--		team_id = team_id_in and
--		itvh = side_in
--	order by
--		its desc
--	limit 8;
--$$
--language sql;

--create or replace function nba.get_intss(in team_id_in uuid, side_in varchar(1))
--returns
--table (
--	team_id uuid,
--	its integer,
--	itvh varchar(1),
--	f3 double precision,
--	f2 double precision,
--	ft double precision,
--	f3acc double precision,
--	f2acc double precision,
--	ftacc double precision,
--	f3r double precision,
--	f2r double precision,
--	ftr double precision,
--	sd_f3 double precision,
--	sd_f2 double precision,
--	sd_ft double precision,
--	sd_f3acc double precision,
--	sd_f2acc double precision,
--	sd_ftacc double precision,
--	sd_f3r double precision,
--	sd_f2r double precision,
--	sd_ftr double precision,
--	season varchar(8),
--	season_type varchar(5),
--	created_on timestamp
--)
--as
--$$
--	select
--		team_id,
--		its,
--		itvh,
--		f3,
--		f2,
--		ft,
--		f3acc,
--		f2acc,
--		ftacc,
--		f3r,
--		f2r,
--		ftr,
--		sd_f3,
--		sd_f2,
--		sd_ft,
--		sd_f3acc,
--		sd_f2acc,
--		sd_ftacc,
--		sd_f3r,
--		sd_f2r,
--		sd_ftr,
--		season,
--		season_type,
--		created_on
--	from
--		nba.intss
--	where
--		team_id = team_id_in and
--		itvh = side_in
--	order by
--		its desc
--	limit 1;
--$$
--language sql;


create or replace function wnba.get_games(number_game_days integer DEFAULT 7)
    returns TABLE(
        game_id uuid,
        start_time timestamp without time zone,
        home_id uuid,
        away_id uuid
    )
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
        wnba.get_min_and_max_game_start_dates(number_game_days)
    into
        start_date,
        end_date;

    return query
        select
            g.game_id,
            g.start_time,
            g.home_id,
            g.away_id
        from
            wnba.game g
        where
            g.start_time::date between start_date and end_date and
            lower(status) != 'closed'
        order by
            start_time;
end;
$$;

create or replace function wnba.get_team(in team_id_in uuid)
returns
table (
	team_id uuid,
	name varchar(45),
	alias varchar(4)
)
as
$$
	select
		team_id,
		name,
		alias
	from
		wnba.team
	where
		team_id = team_id_in
	limit 1;
$$
language sql;

create or replace function wnba.get_players(in team_id_in uuid)
returns
table (
    player_id uuid,
    full_name varchar(45),
    jersey integer
)
as
$$
	select
		p.player_id,
		p.full_name,
		p.jersey
	from
		wnba.player p left join
		wnba.injury i on i.player_id = p.player_id
	where
		p.status = 'act' and
		i.player_id is null and
		p.team_id = team_id_in;
$$
language sql;

create or replace function wnba.get_intss(in team_id_in uuid, in side_in varchar(1))
returns
table (
	team_id uuid,
	its integer,
	itvh varchar(1),
	f3 double precision,
	f2 double precision,
	ft double precision,
	f3acc double precision,
	f2acc double precision,
	ftacc double precision,
	f3r double precision,
	f2r double precision,
	ftr double precision,
	sd_f3 double precision,
	sd_f2 double precision,
	sd_ft double precision,
	sd_f3acc double precision,
	sd_f2acc double precision,
	sd_ftacc double precision,
	sd_f3r double precision,
	sd_f2r double precision,
	sd_ftr double precision,
	season varchar(8),
	season_type varchar(3),
	created_on timestamp
)
as
$$
	select
        team_id, its, itvh,
        f3, f2, ft,
        f3acc, f2acc, ftacc,
        f3r, f2r, ftr,
        sd_f3, sd_f2, sd_ft,
        sd_f3acc, sd_f2acc, sd_ftacc,
        sd_f3r, sd_f2r, sd_ftr,
        season, season_type, created_on
    from
		wnba.intss
	where
		team_id = team_id_in and
		itvh = side_in
	order by
		its desc
	limit 1;
$$
language sql;

create or replace function wnba.get_posc(in player_id_in uuid)
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
	m9 double precision
)
as
$$
	select
	    imtr, iqt, its,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9
	from
		wnba.posc
	where
		player_id = player_id_in  and
		iqt in ('q1','q2','q3','q4')
	order by
		its;
$$
language sql;

create or replace function wnba.get_potm(
	in player_id_in uuid,
	in side_in varchar(1),
	in opponent_team_id_in uuid)
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
	m9 double precision
)
as
$$
	select
	    imtr, iqt, its,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9
	from
		wnba.potm
	where
		player_id = player_id_in  and
		opponent_id = opponent_team_id_in and
		itvh = side_in
	order by
		its;
$$
language sql;

create or replace function wnba.get_psco(in player_id_in uuid, in side_in varchar(1))
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
	m9 double precision
)
as
$$
	select
	    imtr, its, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9
	from
		wnba.psco
	where
		player_id = player_id_in  and
		itvh = side_in and
		iqt in ('q1','q2','q3','q4')
	order by
		its;
$$
language sql;

create or replace function wnba.get_sdom(in player_id_in uuid)
returns
table (
	imtr varchar(4),
	cg double precision
)
as
$$
	select
		a.imtr,
		a.cg
	from
		wnba.sdom a inner join(
			select
			    player_id,
				max(its) its
			from
			    wnba.sdom
			group by
			    player_id) b on a.player_id = b.player_id and a.its = b.its
	where
		a.player_id = player_id_in;
$$
language sql;

create or replace function wnba.get_sdvtm(in player_id_in uuid, in opponent_team_id_in uuid)
returns
table (
	imtr varchar(4),
	cg double precision
)
as
$$
	select
		a.imtr as imtr,
		a.cg as cg
	from
		wnba.sdvtm a inner join
		(
            select
                player_id,
                max(its) its
            from
                wnba.sdvtm
            where
                opponent_id = opponent_team_id_in
            group by
                player_id
		) b on a.player_id = b.player_id and a.its = b.its
	where
		a.player_id = player_id_in;
$$
language sql;

create or replace function wnba.get_team_inlsf(in quarter_in varchar(12))
returns
table (
	iqt varchar(3),
	lg_m0 double precision,
	lg_m1 double precision,
	lg_m2 double precision,
	lg_m3 double precision,
	lg_m4 double precision,
	lg_m5 double precision,
	lg_m6 double precision,
	lg_m7 double precision,
	lg_m8 double precision,
	lg_m9 double precision
)
as
$$
	select
	    iqt,
		lg_m0, lg_m1, lg_m2, lg_m3, lg_m4, lg_m5, lg_m6, lg_m7, lg_m8, lg_m9
	from
		wnba.intsf
	where
	    iqt = quarter_in
	order by
		its desc
	limit 1;
$$
language sql;

create or replace function wnba.get_team_insc(in team_id_in uuid, in side_in varchar(1))
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
    avg_m9 numeric
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
		avg(m9) as avg_m9
	from
		wnba.teams_gm_min_box
	where
		team_id = team_id_in and
		period < 5 and
		home = side_in
	group by
		period;
$$
language sql;

create or replace function wnba.get_team_intsf(in team_id_in uuid, in side_in varchar(1))
returns
table (
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
	sd_m0 double precision,
	sd_m1 double precision,
	sd_m2 double precision,
	sd_m3 double precision,
	sd_m4 double precision,
	sd_m5 double precision,
	sd_m6 double precision,
	sd_m7 double precision,
	sd_m8 double precision,
	sd_m9 double precision
)
as
$$
	select distinct
	    its, itvh, isas, iqt,
		m0, m1, m2, m3, m4, m5, m6, m7, m8, m9,
		sd_m0, sd_m1, sd_m2, sd_m3, sd_m4, sd_m5, sd_m6, sd_m7, sd_m8, sd_m9
	from
		wnba.intsf
	where
		imtr = 'p' and
		team_id = team_id_in and
		itvh = side_in
	order by
		its desc
	limit 8;
$$
language sql;

create or replace function wnba.get_ttm(in team_id_in uuid)
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
		imtr, its, cg, h1, h2, q1, q2, q3, q4
	from
		wnba.ttm
	where
		team_id = team_id_in
	order by
		its desc
	limit 12;
$$
language sql;

create or replace function wnba.get_team_intss_fge(in team_id_in uuid, in side varchar(1))
returns double precision
as
$$
declare
	f2a int not null default 0;
	f2m int not null default 0;
	f3a int not null default 0;
	f3m int not null default 0;
	fge double precision not null default 0;
begin
	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f2a
	from
		wnba.teams_gm_min_box
	where
		statistic = 'twopointattempted' and
		team_id = team_id_in and
		home = side
	group by
		team_id;

	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f2m
	from
		wnba.teams_gm_min_box
	where
		statistic = 'twopointmade' and
		team_id = team_id_in and
		home = side
	group by
		team_id;

	select  distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f3a
	from
		wnba.teams_gm_min_box
	where
		statistic = 'threepointattempted' and
		team_id = team_id_in and
		home = side
	group by
		team_id;

	select distinct
		sum(m0) + sum(m1) + sum(m2) + sum(m3) + sum(m4) + sum(m5) + sum(m6) + sum(m7) + sum(m8) + sum(m9) into f3m
	from
		wnba.teams_gm_min_box
	where
		statistic = 'threepointmade' and
		team_id = team_id_in and
		home = side
	group by
		team_id;

	select cast((f2m + f3m) as double precision) / cast((f2a + f3a) as double precision) as fge;
end
$$
language plpgsql;

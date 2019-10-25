
create or replace function mlb.get_games(number_game_days integer DEFAULT 7)
    returns TABLE(game_id uuid, start_time timestamp without time zone, home_id uuid, away_id uuid)
    language plpgsql
as
$$
declare
    start_date date = timezone('utc', now())::date;
    end_date date = nhl.get_max_game_start_date(number_game_days);
begin
    return query
        select
            g.game_id,
            g.start_time,
            g.home_id,
            g.away_id,
            g.home_pitcher_id,
            g.away_pitcher_id
        from
            mlb.game g
        where
            g.start_time::date between start_date and end_date and
            lower(status) != 'closed'
        order by
            start_time;
end;
$$;

create or replace function mlb.get_games_and_teams(number_game_days integer default 7)
    returns table (
        game_id uuid,
        start_time timestamp without time zone,
        home_team_id uuid,
        away_team_id uuid,
        home_name varchar(45),
        away_name varchar(45),
        home_short_name varchar(45),
        away_short_name varchar(45)
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
        mlb.get_min_and_max_game_start_dates(number_game_days)
    into
        start_date,
        end_date;

    return query
        select
            g.game_id,
            g.start_time,
            home.team_id as home_team_id,
            away.team_id as away_team_id,
            home.name as home_name,
            away.name as away_name,
            home.short_name as home_short_name,
            away.short_name as away_short_name
        from
            mlb.game g join
            mlb.team home on g.home_id = home.team_id join
            mlb.team away on g.away_id = away.team_id
        where
            g.start_time between start_date and end_date and
            mlb.is_valid_game_status(g.status)
        order by
            g.start_time;
end
$$;

create or replace function mlb.get_max_game_start_date(number_game_days integer)
returns date
language plpgsql
as
$$
declare
    min_start_date date =  timezone('utc', now())::date;
    max_start_date date;
begin
    with distinct_start_dates(game_date)
    as
    (
        select
            distinct start_time::date as game_date
        from
            mlb.game
        where
            start_time::date >= min_start_date
        order by
            game_date
        limit
            number_game_days
    )
    select
        Max(game_date) into max_start_date
    from
        distinct_start_dates;

    return max_start_date;
end;
$$;

create function get_games_with_null_pitchers(number_games integer DEFAULT 20)
    returns TABLE(game_id uuid, start_time timestamp without time zone, home_id uuid, away_id uuid, home_pitcher_id uuid, away_pitcher_id uuid, away_pitcher_hand character varying, home_pitcher_hand character varying)
    language sql
as
$$
select
        g.game_id,
        g.start_time,
        g.home_id,
        g.away_id,
        g.home_pitcher_id,
        g.away_pitcher_id,
        away.throw_hand as away_pitcher_hand,
        home.throw_hand as home_pitcher_hand
    from
        mlb.game g inner join
        mlb.player away on g.away_pitcher_id = away.player_id inner join
        mlb.player home on g.home_pitcher_id = home.player_id inner join
        mlb.get_next_games(number_games) gng on g.game_id = gng.game_id
    where
        lower(g.status) != 'closed'
    order by
        start_time asc;
$$;

create or replace function mlb.get_games_and_markets()
returns
table (
    game_id uuid,
	schedule timestamp,
	status varchar(45),
	home_game_id uuid,
	away_game_id uuid,
	home_pitcher_id uuid,
	away_pitcher_id uuid,
    home_name varchar(45),
	away_name varchar(45),
    home_team_market varchar(45),
	away_team_market varchar(45),
	home_pitcher_throw_hand varchar(16),
	away_pitcher_throw_hand varchar(16),
	home_pitcher_fullname varchar(45),
	away_pitcher_fullname varchar(45),
	-- should be uuid
	venue_id uuid,
	venue_city varchar(255),
	venue_name varchar(255),
	venue_state varchar(16),
	venue_zip varchar(16),
	venue_country varchar(16),
	fixture_id integer
)
as
$$
select
	game.game_id,
	game.schedule,
	game.status,
	game.home_id as home_game_id,
	game.away_id as away_game_id,
	game.home_pitcher_id,
	game.away_pitcher_id,
    game.home_name,
	game.away_name,
    h_team.market as home_team_market,
	a_team.market as away_team_market,
	h_pitcher.throw_hand as home_pitcher_hand,
	a_pitcher.throw_hand as away_pitcher_hand,
	h_pitcher.full_name as home_pitcher_name,
	a_pitcher.full_name as away_pitcher_name,
	venue.venue_id,
	venue.city as venue_city,
	venue.name as venue_name,
	venue.state as venue_state,
	venue.zip as venue_zip,
	venue.country as venue_country,
	gm.fixture_id
from
    mlb.game game left join
    mlb.player a_pitcher on game.away_pitcher_id = a_pitcher.player_id left join
    mlb.player h_pitcher on game.home_pitcher_id = h_pitcher.player_id left join
    mlb.team h_team on game.home_id = h_team.team_id left join
    mlb.team a_team on game.away_id = a_team.team_id left join
    mlb.venue on game.venue_id = venue.venue_id left join
    lsport.game_map gm on gm.game_id = game.game_id
where
    --date(schedule - interval 8 hour) >= date(current_date() - interval 8 hour) and date(schedule) <= date(current_date()  interval 2 day) and
    --public.is_in_game_window(schedule) and
    lower(game.status) != 'closed'
order by
    schedule asc;
$$
language sql;

create or replace function mlb.update_game(
   in game_id_in uuid,
   in schedule_in timestamp,
   in home_pitcher_id_in uuid,
   in away_pitcher_id_in uuid,
   in status_in varchar(45)
)
returns void
as
$$
begin
   update
       mlb.game
   set
       schedule = schedule_in,
       home_pitcher_id = home_pitcher_id_in,
       away_pitcher_id = away_pitcher_id_in,
       status = status_in
   where
       game_id = game_id_in;
end;
$$
language plpgsql;

create or replace function mlb.get_team(in team_id_in uuid)
returns
table (
	market varchar(45),
	name varchar(45),
	alias varchar(16)
)
as
$$
	select
		market,
		name,
		alias
		-- league,
		-- division
	from
		mlb.team
	where
		team_id = team_id_in;
$$
language sql;

create or replace function mlb.get_player(in player_id_in uuid)
returns
table (
    player_d uuid,
    full_name varchar(45),
    throw_hand varchar(16),
    jersey integer
)
as
$$
	select
		player_id,
		full_name,
		throw_hand,
		jersey
	from
		mlb.player
	where
		player_id = player_id_in;
$$
language sql;

create or replace function mlb.get_players(in team_id_in uuid)
returns
table (
	player_d uuid,
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
		mlb.player
	where
		team_id = team_id_in;
$$
language sql;

create or replace function mlb.get_game_pitchers(in game_id_in uuid)
returns
table (
	home_pitcher_id uuid,
	away_pitcher_id uuid
)
as
$$
	select
		home_pitcher_id,
		away_pitcher_id
	from
		mlb.game
	where
		game_id = game_id_in;
$$
language sql;

create or replace function mlb.get_markets()
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
		mlb.market;
$$
language sql;

create or replace function mlb.get_score_average(
	in team1_id_in uuid,
    in team2_id_in uuid,
    in team2_pitcher_id_in uuid,
    in home_or_visitor_in varchar(1)
)
returns
table(
    -- todo should side be text?
   	side text,
	i1 double precision,
	i2 double precision,
	i3 double precision,
	i4 double precision,
	i5 double precision,
	i6 double precision,
	i7 double precision,
	i8 double precision,
	i9 double precision
)
as
$$
	select
        side, i1, i2, i3, i4, i5, i6, i7, i8, i9
    from
		mlb.evs_team
	where
		opp_pitcher_id = team2_pitcher_id_in and
        team_id = team1_id_in and
        opponent_id = team2_id_in and
        side = home_or_visitor_in
	order by
		total_runs desc
	limit 1;
$$
language sql;

create or replace function mlb.get_score_average_pvl(
	in team1_id_in uuid,
    in team2_id_in uuid,
    in team2_pitcher_id_in uuid,
    in home_or_visitor_in varchar(1)
)
-- todo team1_id_in and team2_id_in is not used
returns
table(
   	side text,
	i1 double precision,
	i2 double precision,
	i3 double precision,
	i4 double precision,
	i5 double precision,
	i6 double precision,
	i7 double precision,
	i8 double precision,
	i9 double precision
)
as
$$
	select
        side, i1, i2, i3, i4, i5, i6, i7, i8, i9
    from
		mlb.evs_pitcher
	where
		opp_pitcher_id = team2_pitcher_id_in and
        side = home_or_visitor_in
	order by
		total_runs desc
	limit 1;
$$
language sql;

create or replace function mlb.get_score_average_tvl(
	in team1_id_in uuid,
    in team2_id_in uuid,
    in team2_pitcher_id_in uuid,
    in home_or_visitor_in varchar(1)
)
-- todo team2_id_in is not used
returns
table (
	side text,
	i1 double precision,
	i2 double precision,
	i3 double precision,
	i4 double precision,
	i5 double precision,
	i6 double precision,
	i7 double precision,
	i8 double precision,
	i9 double precision
)
as
$$
	select
        side, i1, i2, i3, i4, i5, i6, i7, i8, i9
	from
		mlb.evs_league
	where
		team_id = team1_id_in and
        side = home_or_visitor_in
	order by
		total_runs desc
	limit 1;
$$
language sql;

create or replace function mlb.get_score_average_tvt(
	team1_id_in uuid,
    team2_id_in uuid,
    team2_pitcher_id_in uuid,
    home_or_visitor_in varchar(1)
)
-- todo why is team2_pitcher_id_in not used?
returns
table (
    -- todo why is side text when it is a single char?
	side text,
   	i1 double precision,
	i2 double precision,
	i3 double precision,
	i4 double precision,
	i5 double precision,
	i6 double precision,
	i7 double precision,
	i8 double precision,
	i9 double precision
)
as
$$
	select
        side, i1, i2, i3, i4, i5, i6, i7, i8, i9
    from
		mlb.evs_tmvtmnop
	where
		team_id = team1_id_in and
        opponent_id = team2_id_in and
        side = home_or_visitor_in
	limit 1;
$$
language sql;

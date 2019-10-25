
create or replace function nhl.get_games(number_game_days integer DEFAULT 7)
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
            g.away_id
        from
            nhl.game g
        where
            g.start_time::date between start_date and end_date and
            Lower(status) != 'closed'
        order by
            start_time asc;
end;
$$;

create or replace function nhl.get_games_and_teams(number_game_days integer default 7)
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
        nhl.get_min_and_max_game_start_dates(number_game_days)
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
            nhl.game g join
            nhl.team home on g.home_id = home.team_id join
            nhl.team away on g.away_id = away.team_id
        where
            g.start_time between start_date and end_date and
            nhl.is_valid_game_status(g.status)
        order by
            g.start_time;
end
$$;

create or replace function nhl.get_max_game_start_date(number_game_days integer)
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
            nhl.game
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


create or replace function nhl.get_markets()
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
		nhl.market;
$$
language sql;


create or replace function nhl.get_team(team_id_in uuid)
    returns TABLE(name character varying, alias character varying, market character varying)
    language sql
as
$$
    select
		name,
		alias,
		market
	from
		nfl.team
	where
		team_id = team_id_in;
$$;

create or replace function nhl.get_players(in team_id_in uuid)
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
		nhl.player
	where
		team_id = team_id_in;
$$
language sql;

create or replace function nhl.get_sll(player_id_in uuid, number_games_in integer)
returns
table (
    average_goals numeric,
    average_assists numeric,
    average_blocks numeric,
    average_points numeric
)
language sql
as
$$
    with player_stats(goals, assists, blocks, points)
    as
    (
        select
            goals, assists, blocks, goals + assists as points
        from
            nhl.player_game_stats
        where
            player_id = player_id_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(goals) as average_goals,
        avg(assists) as average_assists,
        avg(blocks) as average_blocks,
        avg(points) as average_points
    from
        player_stats;
 $$;

 
create or replace function nhl.get_inssl(player_id_in uuid, statistic_in varchar(45), number_games_in integer)
    returns TABLE(
        average_p1 double precision,
        average_p2 double precision,
        average_p3 double precision,
        average_p4 double precision
    )
    language sql
as
$$
    with period_stats(p1, p2, p3, p4)
    as
    (
        select
            p1, p2, p3, p4
        from
            nhl.players_metric_percent_by_period
        where
            player_id = player_id_in and
            statistic = statistic_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(p1) as average_p1,
        avg(p2) as average_p2,
        avg(p3) as average_p3,
        avg(p4) as average_p3
    from
        period_stats;
$$;


create or replace function nhl.get_insst(player_id_in uuid, opponent_id_in uuid, statistic_in varchar(45), number_games_in integer)
    returns TABLE(average_p1 double precision, average_p2 double precision, average_p3 double precision, average_p4 double precision)
    language sql
as
$$
    with period_stats(p1, p2, p3, p4)
    as
    (
        select
            p1, p2, p3, p4
        from
            nhl.players_metric_percent_by_period
        where
            player_id = player_id_in and
            opponent_id = opponent_id_in and
            statistic = statistic_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(p1) as average_p1,
        avg(p2) as average_p2,
        avg(p3) as average_p3,
        avg(p4) as average_p4
    from
        period_stats;
$$;



create or replace function nhl.get_inssg(player_id_in uuid, goalie_id_in uuid, statistic_in varchar(45), number_games_in integer)
    returns TABLE(average_p1 double precision, average_p2 double precision, average_p3 double precision, average_p4 double precision)
    language sql
as
$$
    with player_stats(p1, p2, p3, p4)
    as
    (
        select
            p1, p2, p3, p4
        from
            nhl.players_metric_percent_by_period
        where
            player_id = player_id_in and
            opp_goalie_id = goalie_id_in and
            statistic = statistic_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(p1) as average_p1,
        avg(p2) as average_p2,
        avg(p3) as average_p3,
        avg(p4) as average_p4
    from
        player_stats;
$$;


create or replace function nhl.get_ingsl(goalie_id_in uuid, statistic_in varchar(45), number_games_in integer)
    returns TABLE(average_p1 double precision, average_p2 double precision, average_p3 double precision, average_p4 double precision)
    language sql
as
$$
    with player_stats(p1, p2, p3,p4)
    as
    (
        select
            p1, p2, p3, p4
        from
            nhl.players_metric_percent_by_period
        where
            player_id = goalie_id_in and
            statistic = statistic_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(p1) as average_p1,
        avg(p2) as average_p2,
        avg(p3) as average_p3,
        avg(p4) as average_p4
    from
        player_stats;
$$;

create or replace function nhl.get_ingst(goalie_id_in uuid, opponent_id_in uuid, statistic_in character varying, number_games_in integer)
    returns TABLE(average_p1 double precision, average_p2 double precision, average_p3 double precision, average_p4 double precision)
    language sql
as
$$
    with period_stats(p1, p2, p3, p4)
    as
    (
        select
            p1, p2, p3, p4
        from
            nhl.players_metric_percent_by_period
        where
            player_id = goalie_id_in and
            opp_goalie_id = goalie_id_in and
            statistic = statistic_in
        order by
            start_time desc
        limit
            number_games_in
    )
    select
        avg(p1) as average_p1,
        avg(p2) as average_p2,
        avg(p3) as average_p3,
        avg(p4) as average_p4
    from
        period_stats;
$$;

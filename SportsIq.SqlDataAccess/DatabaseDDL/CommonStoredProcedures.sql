
create function is_in_game_window(game_start_timestamp_in timestamp) returns boolean
language plpgsql
as
$$
declare
	start_date date;
	end_date date;
    game_start_date date;
begin
   	-- DATE(`schedule`) >= Date(current_date() - INTERVAL 8 HOUR) and
	-- DATE(`schedule`) <= Date(current_date() + INTERVAL 5 DAY) and

    -- does not take daylight saving into account
    -- our server runs in Vancouver which is 8 hours ahead of GMT

    game_start_date = game_start_timestamp_in::date;
    start_date = localtimestamp - interval '8 hours';
    end_date = start_date + interval '5 days';

    return date_gt(game_start_date, start_date) and timestamp_lt(game_start_date, end_date);
end;
$$;

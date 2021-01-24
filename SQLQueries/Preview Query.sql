CREATE VIEW [dbo].[GetPreviewMatches]
AS

	SELECT
		CAST(CASE WHEN COALESCE(LAG(HeaderRank) over (order by HeaderRank, MatchWinnerRow DESC),HeaderRank + 10) <> HeaderRank THEN 1 ELSE 0 END AS int) MainRow,
		CAST(AllMarkets as int) AllMarkets,
		CAST(HeaderRank AS int) HeaderRank,
		CAST(DetailsRank as int) DetailsRank,
		CAST(IsLive as bit) IsLive,
		EventId,
		EventName,
		MatchId,
		PlayerOne + CASE WHEN HomecomingSBV IS NOT NULL THEN ' ' + CAST(HomecomingSBV as nvarchar(20)) ELSE '' END AS PlayerOne,
		PlayerTwo + CASE WHEN GuestSBV IS NOT NULL THEN ' ' + CAST(GuestSBV as nvarchar(20)) ELSE '' END AS PlayerTwo,
		StartDate,
		BetId,
		BetName,
		HomecomingName,
		HomecomingValue,
		CAST(HomecomingSBV as nvarchar(10)) HomecomingSBV,
		GuestName,
		GuestValue,
		CAST(GuestSBV as nvarchar(10)) GuestSBV
	FROM(
		SELECT DISTINCT
			CASE WHEN BetName IN ('Match Winner', 'Map Advantage','Total Maps Played') THEN 1 ELSE 0 END Show,
			AllMarkets,
			MatchWinnerRow,
			RANK() over (order by MatchId) HeaderRank,
			RANK() over (order by MatchId,Match.BetId) DetailsRank,
			IsLive,
			EventId,
			EventName,
			MatchId,
			PlayerOne,
			PlayerTwo,
			Match.StartDate,
			Match.BetId,
			BetName,
			MAX(CASE WHEN Odd.IsGuest = 0 THEN PlayerOne ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingName,
			MAX(CASE WHEN Odd.IsGuest = 0 THEN Odd.Value ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingValue,
			MAX(CASE WHEN Odd.IsGuest = 0 THEN Odd.SpecialBetValue ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingSBV,
			MAX(CASE WHEN Odd.IsGuest = 1 THEN PlayerTwo ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestName,
			MAX(CASE WHEN Odd.IsGuest = 1 THEN Odd.Value ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestValue,
			MAX(CASE WHEN Odd.IsGuest = 1 THEN Odd.SpecialBetValue ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestSBV
		FROM (
			SELECT 
				COUNT(Bet.Id) over (partition by Event.Id, Match.Id) AllMarkets,
				CASE WHEN Bet.Name = 'Match Winner' THEN 1 ELSE 0 END MatchWinnerRow,
				Bet.IsLive,
				Event.Id EventId,
				Event.Name EventName,
				Match.Id MatchId,
				Match.Name PlayerOne,
				Match.OpponentName PlayerTwo,
				Match.StartDate,
				Bet.Id BetId,
				Bet.Name BetName
			FROM Event
			INNER JOIN Match ON Match.EventID = Event.Id AND Match.MatchType IN (0,1)
			INNER JOIN Bet ON Bet.MatchId = Match.Id			
		)Match
		INNER JOIN Odd ON Odd.BetId = Match.BetId
		WHERE 
			Match.StartDate >= GETDATE() AND 
			Match.StartDate <= DATEADD(DAY,1,GETDATE())
	)T
	WHERE Show = 1
GO
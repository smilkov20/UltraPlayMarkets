CREATE VIEW MatchDetails
AS
SELECT
	Match.IsLive,
	Match.MatchId,
	PlayerOne + CASE WHEN HomecomingSBV IS NOT NULL THEN ' ' + CAST(HomecomingSBV as nvarchar(20)) ELSE '' END AS PlayerOne,
	PlayerTwo + CASE WHEN GuestSBV IS NOT NULL THEN ' ' + CAST(GuestSBV as nvarchar(20)) ELSE '' END AS PlayerTwo,
	Match.StartDate,
	Match.BetId,
	Match.BetName,
	Match.HomecomingName,
	Match.HomecomingValue,
	CAST(Match.HomecomingSBV as nvarchar(10)) HomecomingSBV,
	Match.GuestName,
	Match.GuestValue,
	CAST(Match.GuestSBV as nvarchar(10)) GuestSBV
FROM(
	SELECT DISTINCT
		Match.IsLive,
		Match.MatchId,
		Match.PlayerOne,
		Match.PlayerTwo,
		Match.StartDate,
		Match.BetId,
		Match.BetName,
		MAX(CASE WHEN Odd.IsGuest = 0 THEN PlayerOne ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingName,
		MAX(CASE WHEN Odd.IsGuest = 0 THEN Odd.Value ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingValue,
		MAX(CASE WHEN Odd.IsGuest = 0 THEN Odd.SpecialBetValue ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) HomecomingSBV,
		MAX(CASE WHEN Odd.IsGuest = 1 THEN PlayerTwo ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestName,
		MAX(CASE WHEN Odd.IsGuest = 1 THEN Odd.Value ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestValue,
		MAX(CASE WHEN Odd.IsGuest = 1 THEN Odd.SpecialBetValue ELSE NULL END) over (partition by MatchId,Match.BetId, Odd.SpecialBetValue) GuestSBV
	FROM(
		SELECT
			Bet.IsLive,
			Match.Id MatchId,
			Match.Name PlayerOne,
			Match.OpponentName PlayerTwo,
			Match.StartDate,
			Bet.Id BetId,
			Bet.Name BetName
		FROM Match 
		INNER JOIN Bet ON Bet.MatchId = Match.Id
		WHERE Match.MatchType IN (0,1)
	)Match
	INNER JOIN Odd ON Odd.BetId = Match.BetId
)Match
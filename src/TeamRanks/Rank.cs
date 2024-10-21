namespace TeamRanks
{
    public class TeamRanks {
        private Dictionary<string, Team> teams = new Dictionary<string, Team>();

        public TeamRanks() {
            teams = new Dictionary<string, Team>();
        }

        public void SetMatchResult(string team1, string team2, int score1, int score2) {
            if(!teams.ContainsKey(team1)) teams[team1] = new Team(team1);
            if(!teams.ContainsKey(team2)) teams[team2] = new Team(team2);

            if(score1 > score2) {
                teams[team1].SetPoints(3);
                teams[team2].SetPoints(0);
            }
            else if (score1 < score2) {
                teams[team1].SetPoints(0);
                teams[team2].SetPoints(3);
            }
            else {
                teams[team1].SetPoints(1);
                teams[team2].SetPoints(1);
            }
        }

        public void SetMatchResult((string, string, int, int) t) => SetMatchResult(t.Item1, t.Item2, t.Item3, t.Item4);

        public List<(int rank, string team, int gamesPlayed, int points)> GetRankings() {
            var sortedTeams = teams.Values.OrderByDescending(t => t.Points).ThenBy(t => t.Name).ToList();
            var rankings = new List<(int rank, string team, int gamesPlayed, int points)>();

            int currentRank = 1;
            int? currentPoints = null;

            foreach(var team in sortedTeams)  {
                if (team.Points != currentPoints) {
                    currentRank = rankings.Count + 1;
                    currentPoints = team.Points;
                }

                rankings.Add((currentRank, team.Name, team.GamesPlayed, team.Points));
            }

            return rankings;
        }
    }
}

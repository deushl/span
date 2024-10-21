namespace TeamRanks.Tests;

public class IOTests {
    [Fact]
    public void ValidInput() {
        var (team1, team2, score1, score2) = IO.ProcessScores("Mötley Crüe 3,Poison 1");

        Assert.Equal("Mötley Crüe", team1);
        Assert.Equal("Poison", team2);
        Assert.Equal(3, score1);
        Assert.Equal(1, score2);
    }

    [Fact]
    public void InvalidInput() {
        Assert.Throws<FormatException>(() => IO.ProcessScores("Whitesnake,3,Def Leppard"));
    }
}
public class Tests {
    [Fact]
    public void TestRank() {
        var Ranks = new TeamRanks();

        Ranks.SetMatchResult(IO.ProcessScores("Iron Maiden 3, Slayer 1"));
        Ranks.SetMatchResult(IO.ProcessScores("Megadeth 0, Anthrax 0"));
        Ranks.SetMatchResult(IO.ProcessScores("Slayer 1, Megadeth 1"));

        var rankings = Ranks.GetRankings();

        Assert.Equal(4, rankings.Count);
        Assert.Equal(("Iron Maiden", 1, 1, 3), (rankings[0].team, rankings[0].rank, rankings[0].gamesPlayed, rankings[0].points));
        Assert.Equal(("Megadeth", 2, 2, 2), (rankings[1].team, rankings[1].rank, rankings[1].gamesPlayed, rankings[1].points));
        Assert.Equal(("Anthrax", 3, 1, 1), (rankings[2].team, rankings[2].rank, rankings[2].gamesPlayed, rankings[2].points));
        Assert.Equal(("Slayer", 3, 2, 1), (rankings[3].team, rankings[3].rank, rankings[3].gamesPlayed, rankings[3].points));
    }

    [Fact]
    public void TestRankTied() {
        var Ranks = new TeamRanks();

        Ranks.SetMatchResult(IO.ProcessScores("Motörhead 1, Judas Priest 1"));
        Ranks.SetMatchResult(IO.ProcessScores("Dio 1, Saxon 1"));

        var rankings = Ranks.GetRankings();

        Assert.Equal(4, rankings.Count);
        Assert.All(rankings, r => Assert.Equal(1, r.rank));
        Assert.Equal(new[] { "Dio", "Judas Priest", "Motörhead", "Saxon" }, rankings.Select(r => r.team));
    }

    [Fact]
    public void TestRankTiedOrder() {
        var Ranks = new TeamRanks();

        Ranks.SetMatchResult(IO.ProcessScores("Ratt 3, Twisted Sister 0"));
        Ranks.SetMatchResult(IO.ProcessScores("Quiet Riot 3, W.A.S.P. 0"));

        var rankings = Ranks.GetRankings();

        Assert.Equal(4, rankings.Count);
        Assert.Equal(("Quiet Riot", 1, 1, 3), (rankings[0].team, rankings[0].rank, rankings[0].gamesPlayed, rankings[0].points));
        Assert.Equal(("Ratt", 1, 1, 3), (rankings[1].team, rankings[1].rank, rankings[1].gamesPlayed, rankings[1].points));
        Assert.Equal(("Twisted Sister", 3, 1, 0), (rankings[2].team, rankings[2].rank, rankings[2].gamesPlayed, rankings[2].points));
        Assert.Equal(("W.A.S.P.", 3, 1, 0), (rankings[3].team, rankings[3].rank, rankings[3].gamesPlayed, rankings[3].points));
    }

    [Fact]
    public void TestRankFileInput() {
        var tempFilePath = Path.GetTempFileName();

        try {
            File.WriteAllLines(tempFilePath, new[]  {
                "Mötley Crüe 3, Poison 1",
                "Whitesnake 2, Def Leppard 2",
                "Bon Jovi 0, Guns N' Roses 1"
            });

            var Ranks = new TeamRanks();
            foreach (var line in File.ReadLines(tempFilePath)) {
                Ranks.SetMatchResult(IO.ProcessScores(line));
            }

            var rankings = Ranks.GetRankings();

            Assert.Equal(6, rankings.Count);
            Assert.Equal(("Guns N' Roses", 1, 1, 3), (rankings[0].team, rankings[0].rank, rankings[0].gamesPlayed, rankings[0].points));
            Assert.Equal(("Mötley Crüe", 1, 1, 3), (rankings[1].team, rankings[1].rank, rankings[1].gamesPlayed, rankings[1].points));
            Assert.Equal(("Def Leppard", 3, 1, 1), (rankings[2].team, rankings[2].rank, rankings[2].gamesPlayed, rankings[2].points));
            Assert.Equal(("Whitesnake", 3, 1, 1), (rankings[3].team, rankings[3].rank, rankings[3].gamesPlayed, rankings[3].points));
            Assert.Equal(("Bon Jovi", 5, 1, 0), (rankings[4].team, rankings[4].rank, rankings[4].gamesPlayed, rankings[4].points));
            Assert.Equal(("Poison", 5, 1, 0), (rankings[5].team, rankings[5].rank, rankings[5].gamesPlayed, rankings[5].points));
        }
        finally {
            File.Delete(tempFilePath);
        }
    }
}


// Program.cs
using Microsoft.Extensions.Configuration;

namespace TeamRanks;
public class Program {
    private static IConfiguration Settings = ConfManager.AppSetting;
    public static void Main(string[] args) {
        while(true)  {
            try  {
                var options = Settings.GetSection("MenuItems").Get<Dictionary<string, string>>().Values.ToList();

                string option = Menu.DisplayMenu(options);

                if(option==Settings["MenuItems:1CLI"])
                    ProcessCLIInput();
                else if(option==Settings["MenuItems:2FILE"])
                    ProcessFileInput();
                else if(option==Settings["MenuItems:3EXIT"]) {
                    Console.WriteLine(Settings["Messages:BYE"]);
                    return;
                }

                Console.WriteLine(Settings["Messages:RETURN_TO_MAIN"]);
                Console.ReadKey(true);
            }
            catch(Exception e) {
                Console.Error.WriteLine($"An error occurred: {e.Message}");
                return;
            }
        }
    }

    private static void ProcessCLIInput() {
        var Lines = IO.GetData("cli");
        ProcessData(Lines);
    }

    private static void ProcessFileInput() {
        var Lines = IO.GetData("file");
        ProcessData(Lines);
    }

    private static void ProcessData(IEnumerable<string> Lines) {
        var TeamsRanks = new TeamRanks();

        foreach(var line in Lines)  {
            try {
                var (team1, team2, score1, score2) = IO.ProcessScores(line);
                TeamsRanks.SetMatchResult(team1, team2, score1, score2);
            }
            catch(FormatException e) {
                Console.Error.WriteLine($"Error while processing line: {e.Message}");
            }
        }

        SetOutput(TeamsRanks.GetRankings());
    }

    private static void SetOutput(List<(int rank, string team, int gamesPlayed, int points)> rankings) {
        Console.WriteLine("\nRank | Team                           | GP |  P");
        Console.WriteLine("---------------------------------------------");
        foreach (var (rank, team, gamesPlayed, points) in rankings) {
            Console.WriteLine($"{rank,4} | {team,-30} | {gamesPlayed,2} | {points,2}");
        }
    }
}
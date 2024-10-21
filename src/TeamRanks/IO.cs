using Microsoft.Extensions.Configuration;

namespace TeamRanks;
public static class IO {
    private static IConfiguration Settings = ConfManager.AppSetting;
    public static IEnumerable<string> GetData(string method) {
        switch (method) {
            case "cli":
                return GetCLIData();
            case "file":
                return GetFileData();
            default:
                throw new ArgumentException("Invalid input method");
        }
    }

    private static IEnumerable<string> GetCLIData() {
        var results = new List<string>();

        while(true) {
            Console.Write(Settings["Messages:ENTER_RESULT"]);
            string? input = Console.ReadLine();

            if(input.ToLower() == "exit") {
                break;
            }

            results.Add(input);
        }
        return results;
    }

    private static IEnumerable<string> GetFileData() {
        Console.Write(Settings["Messages:ENTER_PATH"]);
        string? filePath = Console.ReadLine();

        return File.ReadAllLines(filePath);
    }

    public static (string team1, string team2, int score1, int score2) ProcessScores(string dataLine) {
        string[] parts = dataLine.Split(',');

        if(parts.Length !=2) {
            throw new FormatException($"Invalid format for input line: {dataLine}");
        }

        var (team1, score1) = ProcessSegment(parts[0].Trim());
        var (team2, score2) = ProcessSegment(parts[1].Trim());

        return (team1, team2, int.Parse(score1), int.Parse(score2));
    }

    private static (string name, string score) ProcessSegment(string segment) {
        int lastSpaceIndex = segment.LastIndexOf(' ');

        if (lastSpaceIndex == -1) {
            throw new FormatException($"Invalid format for input line segment: {segment}");
        }
        else {
            string name = segment.Substring(0, lastSpaceIndex);
            string score = segment.Substring(lastSpaceIndex + 1);
            
            return (name, score);
        }
    }
}

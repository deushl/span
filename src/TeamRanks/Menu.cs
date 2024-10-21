using Microsoft.Extensions.Configuration;

namespace TeamRanks;
public static class Menu {
    private static IConfiguration Settings = ConfManager.AppSetting;
    public static string DisplayMenu(List<string> options) {
        int selectedOption = 0;

        while(true)  {
            Console.Clear();
            Console.WriteLine(Settings["Messages:TITLE"]);
            Console.WriteLine("==========");
            Console.WriteLine(Settings["Messages:SELECT_OPTION"]);
            Console.WriteLine();

            for(int i=0; i<options.Count; i++) {
                if (i==selectedOption) {
                    Console.Write(" > ");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else {
                    Console.Write("  ");
                }

                Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            var key = Console.ReadKey(true).Key;

            switch (key)  {
                case ConsoleKey.UpArrow:
                    selectedOption = (selectedOption - 1 + options.Count) % options.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selectedOption = (selectedOption + 1) % options.Count;
                    break;
                case ConsoleKey.Enter:
                    return options[selectedOption];
            }
        }
    }
}

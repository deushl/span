using Microsoft.Extensions.Configuration;

namespace TeamRanks;

public static class ConfManager {
    public static IConfiguration AppSetting {
        get;
    }
    static ConfManager() {
        AppSetting = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
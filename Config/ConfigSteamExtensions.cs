using System.Text;
using Steamworks;

namespace SteamTest;

internal static class ConfigSteamExtensions
{
    public static Dictionary<string, float> BackupStats(this Config config)
    {
        var backup = new Dictionary<string, float>();

        foreach (var stat in config.Stats)
        {
            if (SteamUserStats.GetStat(stat.Key, out float floatData))
            {
                backup.Add(stat.Key, floatData);
                continue;
            }
            if (SteamUserStats.GetStat(stat.Key, out float intData))
            {
                backup.Add(stat.Key, intData);
                continue;
            }
        }

        return backup;
    }

    public static void UnlockAchievements(this Config config)
    {
        foreach (var achievement in config.Achievements)
        {
            if (SteamUserStats.SetAchievement(achievement))
            {
                Console.WriteLine($"Unlocked achievement '{achievement}'");
            }
            else
            {
                Console.WriteLine($"Failed to unlock achievement '{achievement}'!");
            }
        }
    }

    public static void SetStats(this Config config, IReadOnlyDictionary<string, float> backups)
    {
        foreach (var stat in config.Stats)
        {
            var sb = new StringBuilder("Setting stat ")
                .Append(stat.Key)
                .Append(": ");

            if (backups.TryGetValue(stat.Key, out var backupValue))
            {
                sb.Append(backupValue).Append(" > ");
            }

            var success =
                SteamUserStats.SetStat(stat.Key, (int)stat.Value)
                || SteamUserStats.SetStat(stat.Key, stat.Value);

            sb.Append(success ? stat.Value : "Failed!");
            Console.WriteLine(sb);
        }
    }
}

using System.Text.Json;
using Steamworks;

namespace SteamTest;

internal class Program
{
    private static void Main(string[] args)
    {
        var instance = new Program();
        instance.Run();

        Console.WriteLine("Press Enter to exit...");
        Console.Read();
    }

    private void Run()
    {
        var config = LoadConfig("config.json");
        if (config == null)
        {
            Console.WriteLine("Invalid config");
            return;
        }

        if (!InitializeSteam(config.AppId))
        {
            return;
        }

        var backups = config.BackupStats();
        if (!BackupStats("backup.json", backups))
            return;

        Console.WriteLine("\nSetting stats...");
        config.SetStats(backups);

        Console.WriteLine("\nUnlocking achievements...");
        config.UnlockAchievements();

        Console.WriteLine();

        ShutdownSteam();
    }

    private Config? LoadConfig(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"Config file '{Path.GetFullPath(path)}' does not exists!");
            return null;
        }
        return Config.Load(File.ReadAllText(path));
    }

    private static bool InitializeSteam(ulong steamId)
    {
        Environment.SetEnvironmentVariable("SteamAppId", steamId.ToString());
        Environment.SetEnvironmentVariable("SteamGameId", steamId.ToString());

        if (!SteamAPI.Init())
        {
            Console.WriteLine("Failed to initialize Steam API!");
            return false;
        };

        return true;
    }

    private static void ShutdownSteam()
    {
        SteamAPI.Shutdown();
    }

    private bool BackupStats(string file, IReadOnlyDictionary<string, float> backups)
    {
        if (File.Exists(file))
        {
            Console.WriteLine($"Backup file '{Path.GetFullPath(file)}' already exists!");
            return false;
        }

        File.WriteAllText(file, JsonSerializer.Serialize(backups));
        return true;
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamTest;

internal class Config
{
    [JsonInclude, JsonPropertyName("achievements")]
    private List<string> achievements;

    [JsonInclude, JsonPropertyName("stats")]
    private Dictionary<string, float> stats;

    public Config()
    {
        AppId = 480;
        achievements = ["NEW_ACHIEVEMENT_0_4"];
        stats = new()
        {
            { "FeetTraveled", new() }
        };
    }

    [JsonPropertyName("app_id")]
    public ulong AppId { get; set; }

    [JsonIgnore] public IReadOnlyList<string> Achievements => achievements;
    [JsonIgnore] public IReadOnlyDictionary<string, float> Stats => stats;

    internal static Config? Load(string json = "")
    {
        return JsonSerializer.Deserialize<Config>(json);
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}

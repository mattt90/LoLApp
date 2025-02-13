namespace LolApp.Lcu.Models;

public class Summoner {
    public ulong SummonerId { get; set; }
    public ulong AccountId { get; set; }
    public string Puuid { get; set; } = string.Empty;
    public string InternalName { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string TagLine { get; set; } = string.Empty;
    public uint SummonerLevel { get; set; }

    public override string ToString()
    {
        var text = $"Id: {SummonerId}";
        text += $"\nAccount Id: {AccountId}";
        text += $"\nPuuid: {Puuid}";
        text += $"\nInternal Name: {InternalName}";
        text += $"\nRiot Id: {GameName}#{TagLine}";
        text += $"\nLevel: {SummonerLevel}";
        return text;
    }
}
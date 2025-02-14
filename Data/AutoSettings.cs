namespace LolApp.Data;

public class AutoSettings
{
    public int Id { get; set; }
    public bool AutoAcceptQueue { get; set; }
    public bool AutoRequeue { get; set; }
    public bool AutoSelectChampion { get; set; }
    public bool AutoRandomChampionSkin { get; set; }
    public bool AutoRerollAram { get; set; }
    // TODO: AutoBan??
}
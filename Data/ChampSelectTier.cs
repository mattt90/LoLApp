namespace LolApp.Data;

public class ChampionSelectTier
{
    public int Id { get; set; }
    public int ChampionId { get; set; }
    public ChampLane Lane { get; set; }
    public Tier StartingTier { get; set; }
    public Tier CurrentTier { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Streak { get; set; }
    public bool IsWinStreak { get; set; }
}

public enum ChampLane
{
    Top,
    Jungle,
    Mid,
    Carry,
    Support
}

public enum Tier
{
    None,
    S,
    A,
    B,
    C,
    D,
    F
}
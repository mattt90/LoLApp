namespace LolApp.Lcu;

using LolApp.Lcu.Models;

public interface ILcuClient
{
    Task<Summoner?> GetCurrentSummonerAsync();
}
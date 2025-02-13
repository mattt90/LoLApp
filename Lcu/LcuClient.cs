namespace LolApp.Lcu;

using System.Text.Json;
using BlossomiShymae.GrrrLCU;
using Kunc.RiotGames.Lol.LeagueClientUpdate;
using LolApp.Lcu.Models;
using Websocket.Client;

public class LcuClient : ILcuClient
{
    private LcuHttpClient _lcuHttpClient { get; }

    public LcuClient()
    {
        _lcuHttpClient = Connector.GetLcuHttpClientInstance();
    }

    public async Task<Summoner?> GetCurrentSummonerAsync()
    {
        var res = await _lcuHttpClient.GetAsync("/lol-summoner/v1/current-summoner")
                ?? throw new Exception("Failed to get summoner");

        if (res.IsSuccessStatusCode)
        {
            return await res.Content.ReadFromJsonAsync<Summoner>()
                ?? throw new Exception("Failed to get summoner");
        }

        return null;
    }

    public async Task AcceptMatchmakingAsync()
    {
        await _lcuHttpClient.PostAsync("/lol-matchmaking/v1/ready-check/accept", null);
    }
}
namespace LolApp.Lcu;

using System.Text.Json;
using BlossomiShymae.GrrrLCU;
using Kunc.RiotGames.Lol.LeagueClientUpdate;
using LolApp.Lcu.Models;
using Websocket.Client;

public class LcuClient : ILcuClient
{
    private LcuHttpClient _lcuHttpClient { get; }
    private LcuWebsocketClient? _lcuWebSocketClient { get; set; }
    private bool _isConnected { get; set; }

    public LcuClient()
    {
        _lcuHttpClient = Connector.GetLcuHttpClientInstance();
        InitializeWebsocketAsync().Wait();
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

    private async Task InitializeWebsocketAsync()
    {
        _lcuWebSocketClient = Connector.CreateLcuWebsocketClient();

        _lcuWebSocketClient.EventReceived.Subscribe(Client_EventReceived);
        _lcuWebSocketClient.DisconnectionHappened.Subscribe(Client_DisconnectionHappened);
        _lcuWebSocketClient.ReconnectionHappened.Subscribe(Client_ReconnectionHappened);

        await _lcuWebSocketClient.Start();
        _isConnected = true;
        
        var message = new EventMessage(EventRequestType.Subscribe, EventKinds.OnJsonApiEvent);
        _lcuWebSocketClient.Send(message);
    }

    private void Client_ReconnectionHappened(ReconnectionInfo info)
    {
        _isConnected = true;//??
        // Do nothing
    }

    private void Client_DisconnectionHappened(DisconnectionInfo info)
    {
       _lcuWebSocketClient?.Dispose();
       _lcuWebSocketClient = null;
       Console.WriteLine("Disconecting from LCUx process...");
       _isConnected = false;
    }

    private async void Client_EventReceived(EventMessage message)
    {
        if (message.Data?.Uri?.Equals("/lol-matchmaking/v1/ready-check") == true)
        {
            if (message.Data.Data["state"].ToString() == "InProgress" &&
                message.Data.Data["playerResponse"].ToString() == "None")
            {
                // Accept queue
                await AcceptMatchmakingAsync();
            }
        }
        //[8,"OnJsonApiEvent",{"data":"ReadyCheck","eventType":"Update","uri":"/lol-gameflow/v1/gameflow-phase"}]
        //[8,"OnJsonApiEvent",{"data":{"declinerIds":[],"dodgeWarning":"None","playerResponse":"None","state":"InProgress","suppressUx":false,"timer":1.0},"eventType":"Update","uri":"/lol-matchmaking/v1/ready-check"}]
        Console.WriteLine(JsonSerializer.Serialize(message));
    }
}
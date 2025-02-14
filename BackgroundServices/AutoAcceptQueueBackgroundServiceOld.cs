
using System.Text.Json;
using BlossomiShymae.GrrrLCU;
using LolApp.Data;
using LolApp.Lcu;
using Microsoft.EntityFrameworkCore;
using Websocket.Client;

namespace LolApp.BackgroundServices;

public class AutoAcceptQueueBackgroundServiceOld : LcuEventListenerBaseBackgroundService
{
    private LcuWebsocketClient? _lcuWebSocketClient { get; set; }
    private ILcuClient _lcuClient { get; set; }
    private LeagueContext _context { get; }
    private bool _connected = false;
    private bool _disposed = false;
    public AutoAcceptQueueBackgroundServiceOld(
        LeagueContext leagueContext,
        ILcuClient lcuClient) : base()
    {
        _context = leagueContext;
        _lcuClient = lcuClient;
    }
    

    public override async void Client_EventReceived(EventMessage message)
    {
        if (message.Data?.Uri?.Equals("/lol-gameflow/v1/gameflow-phase") == true)
        {
            /*
            if (message.Data.Data["state"].ToString() == "InProgress" &&
                message.Data.Data["playerResponse"].ToString() == "None")
                */
            if (message.Data?.Data?.ToString() == "ReadyCheck")
            {
                // Accept queue
                var autoSettings = await _context.AutoSettings.FirstOrDefaultAsync() ?? new();
                if (autoSettings.AutoAcceptQueue)
                {
                    await _lcuClient.AcceptMatchmakingAsync();
                    Console.WriteLine("Accepted");
                }
            }
        }

        Console.WriteLine(JsonSerializer.Serialize(message));
    }
}
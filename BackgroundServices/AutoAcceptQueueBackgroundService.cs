
using System.Text.Json;
using BlossomiShymae.GrrrLCU;
using LolApp.Data;
using LolApp.Lcu;
using Microsoft.EntityFrameworkCore;

namespace LolApp.BackgroundServices;

public class AutoAcceptQueueBackgroundService : IHostedService, IDisposable
{
    private ILcuClient _lcuClient { get; set; }
    private LeagueContext _context { get; }
    private ILcuEventListener _lcuEventListener { get; }
    public AutoAcceptQueueBackgroundService(
        LeagueContext leagueContext,
        ILcuClient lcuClient,
        ILcuEventListener lcuEventListener)
    {
        _context = leagueContext;
        _lcuClient = lcuClient;
        _lcuEventListener = lcuEventListener;
    }

    public void Dispose()
    {
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _lcuEventListener.AddEventListener(Client_EventReceived_AcceptQueuePop);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        await Task.CompletedTask;
    }

    private async void Client_EventReceived_AcceptQueuePop(EventMessage message)
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
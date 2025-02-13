
using System.Text.Json;
using BlossomiShymae.GrrrLCU;
using LolApp.Lcu;
using Websocket.Client;

namespace LolApp.BackgroundServices;

public class AutoAcceptQueueBackgroundService : IHostedService, IDisposable
{
    private LcuWebsocketClient? _lcuWebSocketClient { get; set; }
    private IConfiguration _configuration { get; }
    private ILcuClient _lcuClient { get; set; }
    public AutoAcceptQueueBackgroundService(
        IConfiguration configuration,
        ILcuClient lcuClient)
    {
        _configuration = configuration;
        _lcuClient = lcuClient;
    }

    public void Dispose()
    {
       _lcuWebSocketClient?.Dispose();
       _lcuWebSocketClient = null;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitializeWebsocketAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        Dispose();
    }
    
    private async Task InitializeWebsocketAsync()
    {
        _lcuWebSocketClient = Connector.CreateLcuWebsocketClient();

        _lcuWebSocketClient.EventReceived.Subscribe(Client_EventReceived);
        _lcuWebSocketClient.DisconnectionHappened.Subscribe(Client_DisconnectionHappened);
        _lcuWebSocketClient.ReconnectionHappened.Subscribe(Client_ReconnectionHappened);

        await _lcuWebSocketClient.Start();
        
        var message = new EventMessage(EventRequestType.Subscribe, EventKinds.OnJsonApiEvent);
        _lcuWebSocketClient.Send(message);
    }

    
    private void Client_ReconnectionHappened(ReconnectionInfo info)
    {
        //_isConnected = true;//??
        // Do nothing
    }

    private void Client_DisconnectionHappened(DisconnectionInfo info)
    {
       _lcuWebSocketClient?.Dispose();
       _lcuWebSocketClient = null;
       Console.WriteLine("Disconecting from LCUx process...");
       //_isConnected = false;
    }

    private async void Client_EventReceived(EventMessage message)
    {
        if (!_configuration.GetValue<bool>("AutoAcceptQueue"))
        {
            return;
        }

        if (message.Data?.Uri?.Equals("/lol-gameflow/v1/gameflow-phase") == true)
        {
            /*
            if (message.Data.Data["state"].ToString() == "InProgress" &&
                message.Data.Data["playerResponse"].ToString() == "None")
                */
            if (message.Data.Data.ToString() == "ReadyCheck")
            {
                // Accept queue
                await _lcuClient.AcceptMatchmakingAsync();
                Console.WriteLine("Accepted");
            }
        }
        //[8,"OnJsonApiEvent",{"data":"ReadyCheck","eventType":"Update","uri":"/lol-gameflow/v1/gameflow-phase"}]
        //[8,"OnJsonApiEvent",{"data":{"declinerIds":[],"dodgeWarning":"None","playerResponse":"None","state":"InProgress","suppressUx":false,"timer":1.0},"eventType":"Update","uri":"/lol-matchmaking/v1/ready-check"}]
        Console.WriteLine(JsonSerializer.Serialize(message));
    }
}
namespace LolApp.BackgroundServices;

using BlossomiShymae.GrrrLCU;
using Websocket.Client;

public abstract class LcuEventListenerBaseBackgroundService : IHostedService, IDisposable
{
    private LcuWebsocketClient? _lcuWebSocketClient { get; set; }
    private bool _connected = false;
    private bool _disposed = false;
    public LcuEventListenerBaseBackgroundService()
    {
    }

    public void Dispose()
    {
       _lcuWebSocketClient?.Dispose();
       _lcuWebSocketClient = null;
       _disposed = true;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        RunConnectionThread();
        //await InitializeWebsocketAsync();
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        await Task.CompletedTask;
    }

    public void RunConnectionThread()
    {
        var thread = new Thread(async () => await InitializeWebsocketAsync());
        thread.Start();
        thread.Join();
    }
    
    private async Task InitializeWebsocketAsync()
    {
        while(!_connected)
        {
            if (_disposed)
            {
                return;
            }

            if (!ProcessFinder.IsPortOpen())
            {
                // pause for 1 second
                await Task.Delay(1000);
                continue;
            }

            _lcuWebSocketClient = Connector.CreateLcuWebsocketClient();

            _lcuWebSocketClient.EventReceived.Subscribe(Client_EventReceived);
            _lcuWebSocketClient.DisconnectionHappened.Subscribe(Client_DisconnectionHappened);
            _lcuWebSocketClient.ReconnectionHappened.Subscribe(Client_ReconnectionHappened);

            await _lcuWebSocketClient.Start();
            
            var message = new EventMessage(EventRequestType.Subscribe, EventKinds.OnJsonApiEvent);
            _lcuWebSocketClient.Send(message);
            _connected = true;
        }
    }

    private void Client_ReconnectionHappened(ReconnectionInfo info)
    {
        //_isConnected = true;//??
        // Do nothing
        
       Console.WriteLine("Reconnecting from LCUx process...");
    }

    private void Client_DisconnectionHappened(DisconnectionInfo info)
    {
        _lcuWebSocketClient?.Dispose();
        //_lcuWebSocketClient = null;
        Console.WriteLine("Disconecting from LCUx process...");
        _connected = false;

        RunConnectionThread();
        //await InitializeWebsocketAsync();
    }

    public abstract void Client_EventReceived(EventMessage message);
}
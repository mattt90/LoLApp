using BlossomiShymae.GrrrLCU;
using Websocket.Client;

namespace LolApp.Lcu;

public class LcuEventListener : ILcuEventListener
{
    public LcuEventListener(
        ILogger<LcuEventListener> logger)
    {
        _logger = logger;
    }

    ~LcuEventListener()
    {
        if (_lcuWebSocketClient != null)
        {
            _lcuWebSocketClient.Dispose();
            _lcuWebSocketClient = null;
        }
        _disposed = true;
        _connected = false;
    }
    public void AddEventListener(Action<EventMessage> eventListener)
    {
        _listeners.Add(eventListener);
        if (_lcuWebSocketClient != null && _connected)
        {
            _lcuWebSocketClient.EventReceived.Subscribe(eventListener);
        }
    }

    private void Client_ReconnectionHappened(ReconnectionInfo info)
    {
        _logger.LogInformation("Reconnecting from LCUx process...");
    }

    private void Client_DisconnectionHappened(DisconnectionInfo info)
    {
        _lcuWebSocketClient?.Dispose();
        //_lcuWebSocketClient = null;
        _logger.LogInformation("Disconecting from LCUx process...");
        _connected = false;

        RunConnectionThread();
        //await InitializeWebsocketAsync();
    }

    private void RunConnectionThread()
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

            foreach (var listener in _listeners)
            {
                _lcuWebSocketClient.EventReceived.Subscribe(listener);
            }
            _lcuWebSocketClient.DisconnectionHappened.Subscribe(Client_DisconnectionHappened);
            _lcuWebSocketClient.ReconnectionHappened.Subscribe(Client_ReconnectionHappened);

            await _lcuWebSocketClient.Start();
            
            var message = new EventMessage(EventRequestType.Subscribe, EventKinds.OnJsonApiEvent);
            _lcuWebSocketClient.Send(message);
            _connected = true;
        }
    }

    private bool _connected = false;
    private bool _disposed = false;

    private List<Action<EventMessage>> _listeners = new List<Action<EventMessage>>();
    private LcuWebsocketClient? _lcuWebSocketClient { get; set;}
    private ILogger<LcuEventListener> _logger { get; }
}
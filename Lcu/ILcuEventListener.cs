using BlossomiShymae.GrrrLCU;

namespace LolApp.Lcu;

public interface ILcuEventListener
{
    void AddEventListener(Action<EventMessage> eventListener);
}
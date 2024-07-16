/// <summary>
/// This class represents the Leitsteuerungssystem. It implements the IControlSystem interface. As the implementation is not part of the instructions, all methods contain no logic and simply serve as stub methods.
/// </summary>
public class ControlSystem : IControlSystem
{
    public bool GetEmptyPlaceSensor() { return false; }
    public void SetEmptyPlaceSensor(bool value) { }
    public bool GetFullPlaceSensor() { return false; }
    public void SetFullPlaceSensor(bool value) { }
    public int StartTimer(TimeSpan timeSpan, Action action) { return -1; }
    public void KillTimer(int timerId) { }
}
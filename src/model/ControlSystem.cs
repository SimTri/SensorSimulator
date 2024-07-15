/// <summary>
/// This class reprents the Leitsteuerungssystem. It implements the API provided in the instructions. As the implementation is not part of the instructions, all methods contain no logic and simply serve as stup methods.
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
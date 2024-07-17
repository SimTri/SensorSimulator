/// <summary>
/// Provides the interface for the API of the Leitsteuerungssystem according to
/// the instructions.
/// </summary>
public interface IControlSystem
{
    public void SetEmptyPlaceSensor(bool value);
    public bool GetEmptyPlaceSensor();
    public void SetFullPlaceSensor(bool value);
    public bool GetFullPlaceSensor();
    public int StartTimer(TimeSpan timeSpan, Action action);
    public void KillTimer(int timerId);
}
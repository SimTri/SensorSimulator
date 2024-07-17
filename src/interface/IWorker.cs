/// <summary>
/// Provides the interface for a Worker such that the Leitsteuerungssystem may
/// interact with it using the "Observer Pattern".
/// </summary>
public interface IWorker
{
    public void OnEmptyPlaceSensorChanged();
    public void OnFullPlaceSensorChanged();
}
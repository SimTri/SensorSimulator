/// <summary>
/// This class defines a primitive/simple Logger to log the most important events to the console.
/// </summary>
public static class SimulatorLogger
{
    /// <summary>
    /// Logs the message with a timestamp of format HH:mm:ss:fff to the console.
    /// </summary>
    /// <param name="message">Message to be logged.</param>
    public static void Log(String message)
    {
        String timeStamp = DateTime.Now.ToString("HH:mm:ss:fff");
        Console.WriteLine(timeStamp + " " + message);
    }
}
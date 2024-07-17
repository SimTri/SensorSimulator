/// <summary>
/// This class represents a FillingMachine. The FillingMachine has an id for
/// identification, a fillingMachineState which describes the current state of
/// the FillingMachine and a timeRangeFillingTime which determines a time range
/// out of which a filling time is randomly selected. Note: the id is only used
/// for identification in logging and is not used for comparison of objects.
/// </summary>
public class FillingMachine
{
    private readonly int id;
    private FillingMachineState fillingMachineState;
    private readonly Tuple<TimeSpan, TimeSpan> timeRangeFillingTime;

    public FillingMachineState State
    {
        get { return fillingMachineState; }
        // no setter as State should never be directly modifiable
    }

    /// <summary>
    /// Creates a object of type FillingMachine.
    /// </summary>
    /// <param name="id">Simple id for identification in logging</param>
    /// <param name="timeRangeFillingTime">A Tuple of TimeSpans used to compute
    /// the fillingTime, where the first element defines the lower and the
    /// second element the upper bound</param>
    /// <param name="machineState">The initial state of the FillingMachine,
    /// defaults to READY_TO_FILL if not set</param>
    public FillingMachine(int id, Tuple<TimeSpan, TimeSpan> timeRangeFillingTime, FillingMachineState machineState = FillingMachineState.READY_TO_FILL)
    {
        this.id = id;
        this.timeRangeFillingTime = timeRangeFillingTime;
        this.fillingMachineState = machineState;
        if (machineState == FillingMachineState.FILLING)
        {
            Task.Run(() => this.SimulateFillingTime());
        }
    }

    /// <summary>
    /// If the FillingMachine is in State READY_TO_FILL, this method changes its
    /// State to FILLING, starting a callback function which sets the State to
    /// FILLING_COMPLETE after a randomly selected time defined in
    /// timeRangeFillingTime.
    /// </summary>
    /// <exception cref="Exception">Is thrown in case the FillingMachine is not
    /// in State READY_TO_FILL</exception>
    public void LoadMachine()
    {
        if (fillingMachineState != FillingMachineState.READY_TO_FILL)
        {
            throw new Exception("FillingMachine can only be loaded if State is READY_TO_FILL!");
        }
        fillingMachineState = FillingMachineState.FILLING;
        SimulatorLogger.Log("FillingMachine " + id + " changed State from READY_TO_FILL to FILLING.");
        Task.Run(() => this.SimulateFillingTime());

    }

    /// <summary>
    /// If the FillingMachine is in State FILLING_COMPLETE, this method changes
    /// its State to READY_TO_FILL.
    /// </summary>
    /// <exception cref="Exception">Is thrown in case the FillingMachine is not
    /// in State FILLING_COMPLETE</exception>
    public void UnloadMachine()
    {
        if (fillingMachineState != FillingMachineState.FILLING_COMPLETE)
        {
            throw new Exception("FillingMachine can only be unloaded if State is FILLING_COMPLETE!");
        }
        fillingMachineState = FillingMachineState.READY_TO_FILL;
        SimulatorLogger.Log("FillingMachine " + id + " changed State from FILLING_COMPLETE to READY_TO_FILL.");
    }

    /// <summary>
    /// Callback function which waits for a randomly selected TimeSpan in the
    /// interval defined by timeRangeFillingTime. Upon completion changes the
    /// State of the FillingMachine to FILLING_COMPLETE.
    /// </summary>
    private void SimulateFillingTime()
    {
        int minValueMilliSeconds = Convert.ToInt32(this.timeRangeFillingTime.Item1.TotalMilliseconds);
        int maxValueMilliSeconds = Convert.ToInt32(this.timeRangeFillingTime.Item2.TotalMilliseconds);
        Thread.Sleep(new Random().Next(minValueMilliSeconds, maxValueMilliSeconds));
        fillingMachineState = FillingMachineState.FILLING_COMPLETE;
        SimulatorLogger.Log("FillingMachine " + id + " changed State from FILLING to FILLING_COMPLETE.");
    }
}
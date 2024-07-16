/// <summary>
/// This class represents a Worker, implementing the IWorker interface. It handles a collection (3 according to the instructions) of FillingMachines. The Worker has a ControlSystem, providing API calls, an enumerable collection of FIllingMachines and a timeRangeWorkerLatency, which defines a time range out of which a latency of the workers actions is randomly selected. The primary logic is contained in methods OnEmptyPlaceSensorChanged() and OnFullPlaceSensorChanged() which cause the worker to perform actions (loading/unloading) on the FillingMachines.
/// </summary>
public class Worker : IWorker
{
    private readonly IControlSystem controlSystem;
    private readonly IEnumerable<FillingMachine> fillingMachines;
    private readonly Tuple<TimeSpan, TimeSpan> timeRangeWorkerLatency;

    /// <summary>
    /// Constructs a object of type Worker.
    /// </summary>
    /// <param name="controlSystem">An object implementing the IControlSystem interface.</param>
    /// <param name="fillingMachines">An enumerable collection of type FillingMachines the worker operates.</param>
    /// <param name="timeRangeWorkerLatency">A Tuple of TimeSpans used to compute the worker latency, where the first element defines the lower and the second element the upper bound.</param>
    public Worker(IControlSystem controlSystem, IEnumerable<FillingMachine> fillingMachines, Tuple<TimeSpan, TimeSpan> timeRangeWorkerLatency)
    {
        this.controlSystem = controlSystem;
        this.fillingMachines = fillingMachines;
        this.timeRangeWorkerLatency = timeRangeWorkerLatency;
    }

    /// <summary>
    /// This method is invoked by the ControlSystem (Leitsteuerungssystem) on a state change of the EmptyPlaceSensor. If the getEmptyPlaceSensor() returns true, a Machine in state "READY_TO_FILL" is selected and "loaded" with the empty container. This frees the EmptyPlace effectively setting its state to false.
    /// </summary>
    public void OnEmptyPlaceSensorChanged()
    {
        // implementation of "OnEmptyPlaceSensorChanged" may also invoke this method when EmptyPlaceSensor is false, we therefore check the value to be true
        if (controlSystem.GetEmptyPlaceSensor())
        {
            SimulatorLogger.Log("EmptySensor switched to true.");
            // wait till a FillingMachine is in State READY_TO_FILL
            // Note: the following code performs busy waiting which is a anti pattern. However, for the sake of simplicity, this implementation was chosen. A potential fix involves adding a Thread.Sleep(x) to reduce the polling rate.
            while (true)
            {
                foreach (FillingMachine machine in fillingMachines)
                {
                    if (machine.State == FillingMachineState.READY_TO_FILL)
                    {
                        this.SimulateWorkerLatency();
                        controlSystem.SetEmptyPlaceSensor(false);
                        SimulatorLogger.Log("EmptySensor switched to false.");
                        machine.LoadMachine();
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is invoked by the ControlSystem (Leitsteuerungssystem) on a state change of the FullPlaceSensor. If the getFullPlaceSensor() returns false, a Machine in state "FILLING_COMPLETE" is selected and "unloaded". This occupies the FullPlace effectively setting its state to true.
    /// </summary>
    public void OnFullPlaceSensorChanged()
    {
        // same explanation as for the method "OnEmptyPlaceSensorChanged()"
        if (!controlSystem.GetFullPlaceSensor())
        {
            SimulatorLogger.Log("FullSensor switched to false.");
            // wait till a FillingMachine is in State FILLING_COMPLETE
            // See same Note as in OnEmptySensorChanged()
            while (true)
            {
                foreach (FillingMachine machine in fillingMachines)
                {
                    if (machine.State == FillingMachineState.FILLING_COMPLETE)
                    {
                        this.SimulateWorkerLatency();
                        machine.UnloadMachine();
                        controlSystem.SetFullPlaceSensor(true);
                        SimulatorLogger.Log("FullSensor switched to true.");
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method aims to simulate the latency induced by the actions of worker operating the FillingMachines 
    /// </summary>
    private void SimulateWorkerLatency()
    {
        int minValueMilliSeconds = Convert.ToInt32(this.timeRangeWorkerLatency.Item1.TotalMilliseconds);
        int maxValueMilliSeconds = Convert.ToInt32(this.timeRangeWorkerLatency.Item2.TotalMilliseconds);
        Thread.Sleep(new Random().Next(minValueMilliSeconds, maxValueMilliSeconds));
    }
}

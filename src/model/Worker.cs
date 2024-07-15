/// <summary>
/// This class represents a Worker which handles a set (3 according to the instructions) of FillingMachines which turn empty containers in full containers. The Worker has a ControlSystem, providing API calls, a set of FIllingMachines and a timeRangeWorkerLatency, which defines a time range out of which a latency of the workers actions is randomly selected. The primary logic is contained in methods OnEmptyPlaceSensorChanged() and OnFullPlaceSensorChanged() which cause the worker to perform actions (loading/unloading) on the FillingMachines.
/// </summary>
public class Worker : IWorker
{
    private readonly ControlSystem controlSystem;
    private readonly HashSet<FillingMachine> fillingMachines;
    private readonly Tuple<TimeSpan, TimeSpan> timeRangeWorkerLatency;

    /// <summary>
    /// Constructs a object of type Worker.
    /// </summary>
    /// <param name="controlSystem">The ControlSystem to interact with its API.</param>
    /// <param name="fillingMachines">A set of type FillingMachines the worker operates.</param>
    /// <param name="timeRangeWorkerLatency">A Tuple of TimeSpans used to compute the worker latency, where the first element defines the lower and the second element the upper bound.</param>
    public Worker(ControlSystem controlSystem, HashSet<FillingMachine> fillingMachines, Tuple<TimeSpan, TimeSpan> timeRangeWorkerLatency)
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
        // implementation of "onEmptyPlaceSensorChanged" may also invoke this method when EmptyPlaceSensor is false, we therefore check the value to be true
        if (controlSystem.GetEmptyPlaceSensor())
        {
            SimulatorLogger.Log("EmptySensor switch to true.");
            foreach (FillingMachine machine in fillingMachines)
            {
                if (machine.State == FillingMachineState.READY_TO_FILL)
                {
                    this.SimulateWorkerLatency();
                    controlSystem.SetEmptyPlaceSensor(false);
                    SimulatorLogger.Log("EmptySensor switched to false.");
                    machine.loadMachine();
                }
            }
        }
    }

    /// <summary>
    /// This method is invoked by the ControlSystem (Leitsteuerungssystem) on a state change of the FullPlaceSensor. If the getFullPlaceSensor() returns false, a Machine in state "FILLING_COMPLETE" is selected and "unloaded". This occupies the FullPlace effectively setting its state to true.
    /// </summary>
    public void OnFullPlaceSensorChanged()
    {
        // same explanation as for the method "onEmptyPlaceSensorChanged()"
        if (!controlSystem.GetFullPlaceSensor())
        {
            SimulatorLogger.Log("FullSensor switch to false.");
            foreach (FillingMachine machine in fillingMachines)
            {
                if (machine.State == FillingMachineState.FILLING_COMPLETE)
                {
                    this.SimulateWorkerLatency();
                    machine.unloadMachine();
                    controlSystem.SetFullPlaceSensor(true);
                    SimulatorLogger.Log("FullSensor switched to true.");
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
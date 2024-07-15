SimulatorLogger.Log("Creating simulation objects...");

// creating all dependencies for a Worker
ControlSystem controlSystem = new ControlSystem();

Tuple<TimeSpan, TimeSpan> timeRangeFillingTime = Tuple.Create(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));

FillingMachine machine1 = new FillingMachine(1, timeRangeFillingTime, FillingMachineState.FILLING);
FillingMachine machine2 = new FillingMachine(2, timeRangeFillingTime, FillingMachineState.FILLING_COMPLETE);
FillingMachine machine3 = new FillingMachine(3, timeRangeFillingTime);

HashSet<FillingMachine> fillingMachines = new HashSet<FillingMachine> { machine1, machine2, machine3 };

Tuple<TimeSpan, TimeSpan> timeRangeWorkerLatency = Tuple.Create(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

// creating a Worker
Worker worker = new Worker(controlSystem, fillingMachines, timeRangeWorkerLatency);

SimulatorLogger.Log("Simulation objects creation complete!");
SimulatorLogger.Log("Starting simulation, press 'c' to terminate the simulation...");

// simple termination logic to prevent to program from termination
while (true)
{
    String terminationString = Console.ReadLine() ?? ""; // ReadLine may return null which is replaced by empty string
    if (terminationString.ToLower().Equals("c"))
    {
        break;
    }
}
SimulatorLogger.Log("Simulation terminated!");

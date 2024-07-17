/// <summary>
/// This enum represents the states a FillingMachine may have at any point in
/// time.
/// </summary>
public enum FillingMachineState
{
    // No empty container is in the machine, it is therefore ready to be filled
    READY_TO_FILL,
    // The machine is currently filling the empty containers and cannot be used 
    FILLING,
    // The empty containers have been successfully filled and need to be removed
    // from the machine
    FILLING_COMPLETE
}
/// <summary>
/// This enum reprensents the states a FillingMachine may have at any point in time.
/// </summary>
public enum FillingMachineState
{
    READY_TO_FILL, // No empty container is in the machine, it is therefore ready to be filled
    FILLING, // The machine is currently filling the empty containers and cannot be used 
    FILLING_COMPLETE // The empry containers have been successfully filled and need to be remove from the machine
}
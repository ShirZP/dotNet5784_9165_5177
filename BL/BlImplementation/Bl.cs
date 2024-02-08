using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public 

    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();
}

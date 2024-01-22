using DalApi;
using System.Diagnostics;
namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    static IDal Instance { get; } = new DalXml();
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();

    private DalXml() { }
}

namespace DalApi;

public interface IDal
{
    ITask Task { get; }
    IEngineer Engineer { get; }
    IDependency Dependency { get; }

    DateTime? ProjectStartDate { get; set; }
    DateTime? ProjectEndDate { get; set; }

}


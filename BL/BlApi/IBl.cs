namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public void initializationDB();
    public void resetDB();

    public BO.ProjectStatus CalculateProjectStatus();
    public BO.ProjectStatus GetProjectStatus();
    public void SetProjectStartDate(DateTime startDate);    
    public DateTime? GetProjectStartDate();
    public void SetProjectEndDate(DateTime endDate);
    public DateTime? GetProjectEndDate();
    public void changeStatusToBuildingSchedule();
    public void changeStatusToExecution();
    public void initializeProjectStatus();
}

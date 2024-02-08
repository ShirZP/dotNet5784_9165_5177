using BlApi;
using BO;

namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public BO.ProjectStatus CalculateProjectStatus()
    {
        //If there is no start date for the project yet - the planning stage.
        if (_dal.getProjectStartDate() == null)
        {
            return BO.ProjectStatus.Planning;
        }

        //If there is a start date for the project but not all the tasks have a planned start date yet - the building schedule stage.
        IEnumerable<DO.Task> tasksWithoutScheduledDate = _dal.Task.ReadAll(item => item.ScheduledDate == null);

        if (tasksWithoutScheduledDate.Any())
        {
            return BO.ProjectStatus.BuildingSchedule;
        }
        else
        {
            //If there is a start date for the project and all the tasks also have a planned start date – the execution stage.
            return BO.ProjectStatus.Execution;
        }
    }

    /// <summary>
    /// The function returns thw project status from the data layer.
    /// </summary>
    public BO.ProjectStatus GetProjectStatus()
    {
        int numStatus = (int)_dal.getProjectStatus();

        return (BO.ProjectStatus)numStatus;
    }

    public DateTime? GetProjectStartDate()
    {
        return _dal.getProjectStartDate();  
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        _dal.setProjectStartDate(startDate);    
    }

    public DateTime? GetProjectEndDate()
    {
        return _dal.getProjectEndDate();
    }

    public void SetProjectEndDate(DateTime endDate)
    {
        _dal.setProjectEndDate(endDate);
    }

    public void changeStatusToBuildingSchedule()
    {
        _dal.changeStatusToBuildingSchedule();
    }

    public void changeStatusToExecution()
    {
        _dal.changeStatusToExecution();
    }

    public void initializeProjectStatus()
    {
        _dal.initializeProjectStatus();
    }
}

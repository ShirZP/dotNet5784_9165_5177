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
}

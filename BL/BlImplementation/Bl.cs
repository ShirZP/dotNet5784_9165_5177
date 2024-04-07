using BlApi;
using BO;

namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IUser User => new UserImplementation();
    public ITask Task => new TaskImplementation(this);

    public IEngineer Engineer => new EngineerImplementation(this);

    public BO.ProjectStatus CalculateProjectStatus()
    {
        //If there is no start date for the project yet - the planning stage.
        if (_dal.GetProjectStartDate() == null)
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
        int numStatus = (int)_dal.GetProjectStatus();

        return (BO.ProjectStatus)numStatus;
    }

    public DateTime? GetProjectStartDate()
    {
        return _dal.GetProjectStartDate();  
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        if (GetProjectStartDate() == null)
        {
            _dal.SetProjectStartDate(startDate);
        }
        else
            throw new Exception("There is already a start date for the project");
    }

    public DateTime? GetProjectEndDate()
    {
        return _dal.GetProjectEndDate();
    }

    public void SetProjectEndDate()
    {
        if (GetProjectEndDate() == null)
        {
            DateTime? endDate = default(DateTime);

            foreach (BO.Task task in Task.ReadAllFullTasksDetails())
            {
                if (task.ForecastDate > endDate)
                    endDate = task.ForecastDate;
            }

            _dal.SetProjectEndDate(endDate.Value);
        }
        else
            throw new Exception("The project already has an end date");
    }
    public void ChangeStatusToPlanning()
    {
        _dal.ChangeStatusToPlanning();
    }

    public void ChangeStatusToBuildingSchedule()
    {
        _dal.ChangeStatusToBuildingSchedule();
    }

    public void ChangeStatusToExecution()
    {
        _dal.ChangeStatusToExecution();
    }

    public void InitializeProjectStatus()
    {
        _dal.InitializeProjectStatus();
    }

    public void InitializationDB()
    {
        DalTest.Initialization.Do();
    }

    public void ResetDB()
    {
        DalTest.Initialization.DoReset();
    }


    #region Clock

    //TODO:
    //private static DateTime s_Clock = DateTime.Now;
    //public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public DateTime GetClock()
    {
        return _dal.GetClock();
    }

    public DateTime MoveClockYearForward()
    {
        _dal.SetClock(GetClock().AddYears(1));
        return GetClock();
    }

    public DateTime MoveClockDayForward()
    {
        _dal.SetClock(GetClock().AddDays(1));
        return GetClock();
    }

    public DateTime MoveClockHourForward()
    {
        _dal.SetClock(GetClock().AddHours(1));
        return GetClock();
    }

    public DateTime InitializeClock()
    {
        _dal.SetClock(DateTime.Now);
        return GetClock();
    }
    #endregion
}

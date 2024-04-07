namespace Dal;
using DalApi;
using DO;
using System;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IUser User => new UserImplementation();

    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public DO.ProjectStatus ProjectStatus { get; set; }
    public DateTime Clock { get; set; }

    private DalList() { }

    public DateTime? GetProjectStartDate()
    {
        return DataSource.Config.projectStartDate;
    }

    public DateTime? GetProjectEndDate()
    {
        return DataSource.Config.projectEndDate;
    }

    public ProjectStatus GetProjectStatus()
    {
        return DataSource.Config.projectStatus;
    }

    public void SetProjectStartDate(DateTime startDate)
    {
        DataSource.Config.projectStartDate = startDate;
    }

    public void SetProjectEndDate(DateTime endDate)
    {

        DataSource.Config.projectStartDate = endDate;
    }

    public void InitializeProjectStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.Planning;
        DataSource.Config.projectStartDate = null;
        DataSource.Config.projectEndDate = null;
    }

    /// <summary>
    /// The function change the status to status Planning
    /// </summary>
    public void ChangeStatusToPlanning()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.Planning;
    }

    /// <summary>
    /// The function change the status to status BuildingSchedule
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status Execution to status BuildingSchedule</exception>
    public void ChangeStatusToBuildingSchedule()
    {
        if (DataSource.Config.projectStatus == DO.ProjectStatus.Execution)
        {
            throw new DalChangProjectStatusException("can't change status from Execution to BuildingSchedule");
        }

        DataSource.Config.projectStatus = DO.ProjectStatus.BuildingSchedule;
    }

    /// <summary>
    /// The function change the status to status Execution
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status planning to status Execution</exception>
    public void ChangeStatusToExecution()
    {
        if (DataSource.Config.projectStatus == DO.ProjectStatus.Planning)
        {
            throw new DalChangProjectStatusException("can't change status from planning to Execution");
        }

        DataSource.Config.projectStatus = DO.ProjectStatus.Execution;
    }

    public void SetClock(DateTime clock)
    {
        DataSource.Config.clock = clock;
    }

    public DateTime GetClock()
    {
       return DataSource.Config.clock;
    }

    public void InitializeClock()
    {
        DataSource.Config.clock = DateTime.Now;
    }
}
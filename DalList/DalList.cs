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

    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public DO.ProjectStatus ProjectStatus { get; set; }

    private DalList() { }

    public DateTime? getProjectStartDate()
    {
        return DataSource.Config.projectStartDate;
    }

    public DateTime? getProjectEndDate()
    {
        return DataSource.Config.projectEndDate;
    }

    public ProjectStatus getProjectStatus()
    {
        return DataSource.Config.projectStatus;
    }

    public void setProjectStartDate(DateTime startDate)
    {
        DataSource.Config.projectStartDate = startDate;
    }

    public void setProjectEndDate(DateTime endDate)
    {

        DataSource.Config.projectStartDate = endDate;
    }

    public void initializeProjectStatus()
    {
        DataSource.Config.projectStatus = DO.ProjectStatus.planning;
    }

    /// <summary>
    /// The function change the status to status BuildingSchedule
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status Execution to status BuildingSchedule</exception>
    public void changeStatusToBuildingSchedule()
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
    public void changeStatusToExecution()
    {
        if (DataSource.Config.projectStatus == DO.ProjectStatus.planning)
        {
            throw new DalChangProjectStatusException("can't change status from planning to Execution");
        }

        DataSource.Config.projectStatus = DO.ProjectStatus.Execution;
    }
}
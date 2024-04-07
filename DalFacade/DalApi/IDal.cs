using System;

namespace DalApi;

public interface IDal
{
    ITask Task { get; }
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
    IUser User { get; }

    DateTime? ProjectStartDate { get; set; }
    DateTime? ProjectEndDate { get; set; }
    DO.ProjectStatus ProjectStatus { get; set; }
    DateTime Clock { get; set; }

    public DateTime? GetProjectStartDate();
    public DateTime? GetProjectEndDate();
    public DO.ProjectStatus GetProjectStatus();

    public void SetProjectStartDate(DateTime projectStartDate);
    public void SetProjectEndDate(DateTime projectEndDate);
    public void InitializeProjectStatus();
    public void ChangeStatusToPlanning();
    public void ChangeStatusToBuildingSchedule();
    public void ChangeStatusToExecution();

    #region  Clock
    public void SetClock(DateTime clock);
    public DateTime GetClock();
    public void InitializeClock();
    #endregion

}


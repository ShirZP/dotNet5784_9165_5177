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

    public DateTime? getProjectStartDate();
    public DateTime? getProjectEndDate();
    public DO.ProjectStatus getProjectStatus();

    public void setProjectStartDate(DateTime projectStartDate);
    public void setProjectEndDate(DateTime projectEndDate);
    public void initializeProjectStatus();
    public void changeStatusToPlanning();
    public void changeStatusToBuildingSchedule();
    public void changeStatusToExecution();

}


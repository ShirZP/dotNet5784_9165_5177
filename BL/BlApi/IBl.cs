﻿using System.Runtime.InteropServices;

namespace BlApi;

public interface IBl
{
    public IUser User { get; }
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    public void initializationDB();
    public void resetDB();

    #region Project methods
    public BO.ProjectStatus CalculateProjectStatus();
    public BO.ProjectStatus GetProjectStatus();
    public void SetProjectStartDate(DateTime startDate);    
    public DateTime? GetProjectStartDate();
    public void SetProjectEndDate();
    public DateTime? GetProjectEndDate();
    public void changeStatusToPlanning();
    public void changeStatusToBuildingSchedule();
    public void changeStatusToExecution();
    public void initializeProjectStatus();
    #endregion

    #region Clock
    public DateTime Clock { get; }
    public DateTime GetClock();
    public void SetClock(DateTime clock);  //update the Dal Clock
    public DateTime MoveClockYearForward();
    public DateTime MoveClockDayForward();
    public DateTime MoveClockHourForward();
    public DateTime initializeClock();
    #endregion
}

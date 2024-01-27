namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(BO.Task task)
    {
        try //TODO: בדיקות תקינות וגם זרעקת חריגות מתאימות
        {

        int assignedEngineerID = task.AssignedEngineer.ID;
        bool isTaskMilestone = task.Milestone != null;
        DO.Task doTask = new DO.Task(task.ID, task.NickName, task.Description, task.ScheduledDate, task.StartDate, task.RequiredEffortTime, task.CompleteDate, task.Deliverables, task.Remarks, assignedEngineerID, task.Complexity, task.DeadlineDate, isTaskMilestone);

        }
        catch(DO.DalAlreadyExistsException dalex)
        {

        }
        catch( ) 
        {

        }
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void ScheduledDateUpdate(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task UpdatedTask)
    {
        throw new NotImplementedException();
    }
}

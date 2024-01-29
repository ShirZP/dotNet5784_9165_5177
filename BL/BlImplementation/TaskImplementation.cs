namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    /// The function recieves an object of type BO.Task.
    /// Checks the correctness of fields and adds the task to the data layer as DO.Task
    /// and adds to the data layer all the dependencies of the task
    /// </summary>
    /// <param name="task">An object of type BO.Task</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    public int Create(BO.Task task)
    {
        //validation of the task's fields
        if (task.ID <= 0)//TODO: למהההההההההההההההההה
            throw new BlPositiveIntException("The task's ID number must be positive!");
        if (task.NickName == null || task.NickName == "")
            throw new BlEmptyStringException("The task's nick name cannot be empty!");

        //creat dal task
        int assignedEngineerID = task.AssignedEngineer!.ID;
        bool isTaskMilestone = task.Milestone != null;
        DO.Task doTask = new DO.Task(task.ID, task.NickName, task.Description, task.ScheduledDate, task.StartDate, task.RequiredEffortTime, task.CompleteDate, task.Deliverables, task.Remarks, assignedEngineerID, task.Complexity, task.DeadlineDate, isTaskMilestone);

        //Creating a list of DO.Dependency objects
        IEnumerable<DO.Dependency>? dalDependenciesList = from taskInList in task.Dependencies
                                                          where taskInList != null
                                                          select new DO.Dependency(0, task.ID, taskInList.ID);
        
        //Adding the task's dependencies to the data layer
        (from dependency in dalDependenciesList
         where dependency != null
         select _dal.Dependency.Create(dependency)).ToList();

        //Add the dal task to the data layer
        int idTask = _dal.Task.Create(doTask);
        return idTask;

    }

    /// <summary>
    /// The function gets a task ID and deletes it if it exists and no other tasks depend on it.
    /// </summary>
    /// <param name="id">id of the task to delete</param>
    /// <exception cref="BlThereIsADependencyOnTheTaskException">there is other tasks that depend on the current task</exception>
    /// <exception cref="BlDoesNotExistException">if the task is not exists</exception>
    public void Delete(int id)
    {
        //Finding all dependencies on which the current task has a dependency
        IEnumerable<DO.Dependency?>? dependencies = _dal.Dependency.ReadAll(item => item.DependensOnTask == id);

        //TODO: אי אפשר למחוק משימות לאחר יצירת לו"ז הפרויקט.

        if (dependencies != null)
            throw new BlThereIsADependencyOnTheTaskException($"There are tasks that depend on the task - {id}");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException dalex)
        {
            throw new BlDoesNotExistException($"An object of type Task with ID={id} does not exist", dalex);
        }
        
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

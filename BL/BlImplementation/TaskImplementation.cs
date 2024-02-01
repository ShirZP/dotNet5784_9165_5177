﻿namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections;
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
    public int Create(BO.Task task)
    {
        //validation of the task's fields
        checkTaskFields(task);

        //create dal task
        int assignedEngineerID = task.AssignedEngineer!.ID;
        //TODO: bool isTaskMilestone = task.Milestone != null;
        DO.Task doTask = new DO.Task(task.ID, task.NickName, task.Description, task.ScheduledDate, task.StartDate, task.RequiredEffortTime, task.CompleteDate, task.Deliverables, task.Remarks, assignedEngineerID, task.Complexity, task.DeadlineDate);

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

    /// <summary>
    ///  The function receives the ID of a task and returns it as an object of type BO.
    /// </summary>
    /// <param name="id">the ID of a task</param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException">if the task is not exists</exception>
    public BO.Task Read(int id)
    {
        DO.Task? dalTask = _dal.Task.Read(item => item.ID == id);

        if(dalTask == null)
        {
            throw new BlDoesNotExistException($"An object of type Task with ID={id} does not exist");
        }

        return new BO.Task(dalTask.ID,
                           dalTask.NickName,
                           dalTask.Description,
                           calculateStatus(dalTask),
                           calculateDependencies(dalTask),
                           dalTask.CreateAtDate,
                           dalTask.ScheduledDate,
                           dalTask.StartDate,
                           calculateForecastDate(dalTask),
                           dalTask.DeadlineDate,
                           dalTask.CompleteDate,
                           dalTask.RequiredEffortTime,
                           dalTask.FinalProduct,
                           dalTask.Remarks,
                           getAssignedEngineer(dalTask),
                           dalTask.Complexity);//TODO: maybe remove dalTask.DeadlineDate 
    }

    /// <summary>
    /// The function recieved a filter and returns all tasks according to the filter
    /// </summary>
    /// <param name="filter">filter to read</param>
    /// <returns>all tasks according to the filter</returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return (from DO.Task dalTask in _dal.Task.ReadAll()
                    let blTask = Read(dalTask.ID)
                    where filter(blTask)
                    let taskInList = new TaskInList(blTask.ID, blTask.Description, blTask.NickName, blTask.Status)
                    select taskInList);
        }
        return (from DO.Task dalTask in _dal.Task.ReadAll()
                let blTask = Read(dalTask.ID)
                let taskInList = new TaskInList(blTask.ID, blTask.Description, blTask.NickName, blTask.Status)
                select taskInList);
    }

    public void Update(BO.Task updatedTask)
    {
        //validation of the task's fields
        checkTaskFields(updatedTask);




        try
        {
            //Convert to DO task object
            int? assignEngineerID = (updatedTask.AssignedEngineer != null) ? updatedTask.AssignedEngineer.ID : null;
            DO.Task dalTask = new DO.Task(updatedTask.ID,
                                          updatedTask.NickName,
                                          updatedTask.Description,
                                          updatedTask.ScheduledDate,
                                          updatedTask.StartDate,
                                          updatedTask.RequiredEffortTime,
                                          updatedTask.CompleteDate,
                                          updatedTask.Deliverables,
                                          updatedTask.Remarks,
                                          assignEngineerID,
                                          updatedTask.Complexity);

            _dal.Task.Update(dalTask);

        }
        catch (DO.DalDoesNotExistException dalEx)
        {
            throw new BO.BlDoesNotExistException($"An object of type Task with ID={updatedTask.ID} does not exist", dalEx);
        }
    }

    public void ScheduledDateUpdate(int id)
    {
        throw new NotImplementedException();
    }

   

    /// <summary>
    /// The function calculates the status of the DO task according to it's dates.
    /// </summary>
    /// <param name="dalTask">DO task object</param>
    /// <returns></returns>
    private BO.Status calculateStatus(DO.Task dalTask)
    {
        if(dalTask.CompleteDate != null)
            return BO.Status.Complete;

        if (dalTask.StartDate == null)
            return BO.Status.New;

        return Status.Active;
    }

    /// <summary>
    /// The function calculates the Dependencies field of the DO task.
    /// </summary>
    /// <param name="dalTask">DO task object</param>
    /// <returns></returns>
    private List<TaskInList>? calculateDependencies(DO.Task dalTask)
    {
        return (from DO.Dependency dalDependency in _dal.Dependency.ReadAll(item => item.DependentTask == dalTask.ID)  //all the dependencies that the current task is the dependent task
                let dependensOnTask = _dal.Task.Read(item => item.ID == dalDependency.DependensOnTask)  //Retrieving the task that the current task dependens on from the data layer
                let taskInList = new TaskInList(dependensOnTask.ID, dependensOnTask.Description, dependensOnTask.NickName, calculateStatus(dependensOnTask))  //Convert the dependensOnTask to TaskInList
                select taskInList).ToList();
    }

    /// <summary>
    /// The function calculates the ForecastDate field of the DO task.
    /// </summary>
    /// <param name="dalTask">DO task object</param>
    /// <returns></returns>
    private DateTime? calculateForecastDate(DO.Task dalTask)
    {
        //The maximum of the scheduled start date and the actual start date + the required effort time of the task
        DateTime? maxDate = (dalTask.StartDate >= dalTask.ScheduledDate) ? dalTask.StartDate : dalTask.ScheduledDate;

        if (maxDate.HasValue && dalTask.RequiredEffortTime.HasValue)
        {
            return maxDate.Value + dalTask.RequiredEffortTime.Value;
        }

        return null;
    }

    /// <summary>
    ///  The function returns the assigned engineer of the DO task
    /// </summary>
    /// <param name="dalTask">DO task object</param>
    /// <returns></returns>
    private EngineerInTask? getAssignedEngineer(DO.Task dalTask)
    {
        if (dalTask.EngineerId != null)
        {
            DO.Engineer dalEngineer = _dal.Engineer.Read(item => item.ID == dalTask.EngineerId)!;
            return new EngineerInTask(dalEngineer.ID, dalEngineer.FullName);
        }
        return null;
    }

    /// <summary>
    /// The function recieves a BO task object and check the validations of it's fields.
    /// </summary>
    /// <param name="task">BO task object</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    private void checkTaskFields(BO.Task task)
    {
        if (task.ID <= 0)//TODO: למהההההההההההההההההה
            throw new BlPositiveIntException("The task's ID number must be positive!");

        if (task.NickName == null || task.NickName == "")
            throw new BlEmptyStringException("The task's nick name cannot be empty!");
    }
}

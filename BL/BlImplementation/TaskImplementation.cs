namespace BlImplementation;
using BlApi;
using System;
using BO;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

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
        DO.Task doTask = new DO.Task(task.ID,
                                     task.NickName,
                                     task.Description, 
                                     task.ScheduledDate, 
                                     task.StartDate, 
                                     task.RequiredEffortTime, 
                                     task.CompleteDate, 
                                     task.Deliverables, 
                                     task.Remarks, 
                                     assignedEngineerID, 
                                     task.Complexity, 
                                     task.DeadlineDate);

        //Creating a list of DO.Dependency objects
        IEnumerable<DO.Dependency>? dalDependenciesList = task.Dependencies.Select(taskInList => new DO.Dependency(0, task.ID, taskInList.ID)).Where(taskInList =>  taskInList != null);
        
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
                    select new TaskInList(blTask.ID, blTask.Description, blTask.NickName, blTask.Status));
        }
        return (from DO.Task dalTask in _dal.Task.ReadAll()
                let blTask = Read(dalTask.ID)
                select new TaskInList(blTask.ID, blTask.Description, blTask.NickName, blTask.Status));
    }

    /// <summary>
    /// The function receives BO updated task object and update the task in the data layer
    /// </summary>
    /// <param name="updatedTask">BO updated task object</param>
    /// <exception cref="BO.BlDoesNotExistException">if the task is not exists</exception>
    public void Update(BO.Task updatedTask)
    {
        //validation of the task's fields
        checkTaskFields(updatedTask);

        //Adding new dependencies in the data layer for each new task that added to the dependencies list in the updated task

        //Grouping the list of tasks according to whether there is a dependency on the data layer
        IEnumerable<IGrouping<bool, BO.TaskInList>> dalDependencies;
        dalDependencies = (from taskInList in updatedTask.Dependencies
                           let dalDependency = _dal.Dependency.Read(item => item.DependentTask == updatedTask.ID && item.DependensOnTask == taskInList.ID) //Retrieving the dependency on which the updatedTask depends on the task
                           group taskInList by (dalDependency == null) into dalDep
                           select dalDep);

        //For the tasks (in updatedTask's dependencies list) that do not have dependencies in the data layer - we will create dependencies
        var d = (from task in dalDependencies
                 where task.Key == true
                 from t in task
                 let newDependency = new DO.Dependency(0, updatedTask.ID, t.ID)
                 select _dal.Dependency.Create(newDependency)).ToList();

        //Deleting all dependencies in the data layer that no longer appear in the updatedTask's dependencies list
        BO.Task blOldTask = Read(updatedTask.ID);

        if (updatedTask.Dependencies != null)
        {
            //if find dependency between task to updatedTask 
            IEnumerable<IGrouping<bool, BO.TaskInList>> missingTasks;
            missingTasks = (from task in blOldTask.Dependencies
                            let t = updatedTask.Dependencies.FirstOrDefault(item => item.ID == task.ID)
                            group task by (t == null) into dalDep
                            select dalDep);

            foreach(IGrouping<bool, BO.TaskInList> task in missingTasks)
            {
                switch (task.Key)
                {
                    case true:
                        foreach (BO.TaskInList t in task)
                        {
                            DO.Dependency oldDependency = _dal.Dependency.Read(item => item.DependentTask == updatedTask.ID && item.DependensOnTask == t.ID)!;
                            _dal.Dependency.Delete(oldDependency.ID);
                        }
                        break;
                }
            }

            //TODO: remove the foreach
        }
        else if(updatedTask.Dependencies == null && blOldTask.Dependencies != null)
        {
            foreach (BO.TaskInList task in blOldTask.Dependencies)
            {
                DO.Dependency oldDependency = _dal.Dependency.Read(item => item.DependentTask == updatedTask.ID && item.DependensOnTask == task.ID)!;
                _dal.Dependency.Delete(oldDependency.ID);
            }
        }

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

    public void ScheduledDateUpdate(int id, DateTime? newScheduledDate)
    {
        if (newScheduledDate.HasValue)
        {
            BO.Task updateTask = Read(id);
            BO.TaskInList? nullScheduledDateInTask = (from taskInList in updateTask.Dependencies
                                                      where Read(taskInList.ID).ScheduledDate == null
                                                      select taskInList).FirstOrDefault();
            if (nullScheduledDateInTask != null)
            {
                throw new BlNullScheduledDateInDependensOnTaskException($"There is no scheduled start date on at least one of the tasks that the task - {id} - depends on");
            }

            BO.TaskInList? earlyForecastDateInTask = (from taskInList in updateTask.Dependencies
                                                      where Read(taskInList.ID).ForecastDate > newScheduledDate
                                                      select taskInList).FirstOrDefault();

            if (earlyForecastDateInTask != null)
            {
                throw new BlDependentsTasksException($"The scheduled start date of the depended task - {id} - is earlier than the forecast date of a previous task");
            }
        }
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
    private List<TaskInList> calculateDependencies(DO.Task dalTask)
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
        if (task.ID <= 0)
            throw new BlIntException("The task's ID number must be positive!");

        if (task.NickName == null || task.NickName == "")
            throw new BlEmptyStringException("The task's nick name cannot be empty!");

        if (task.Description == null || task.Description == "")
            throw new BlEmptyStringException("The task's description cannot be empty!");

        checkStatusField(task);

        ScheduledDateUpdate(task.ID, task.ScheduledDate);

        if (task.RequiredEffortTime != null && task.RequiredEffortTime.Value.TotalMilliseconds <= 0)
            throw new BlIntException($"Required effort time can't be 0 or below");

        checkAssignedEngineerField(task);


    }

    /// <summary>
    /// If the status is Active or Complete- the function checks whether there are previous tasks that have not been completed
    /// </summary>
    /// <param name="task">BO task object</param>
    /// <exception cref="BO.BlDependentsTasksException">is a previous task for the task that has not been completed</exception>
    private void checkStatusField(BO.Task task)
    {
        switch (task.Status)
        {
            case Status.Active:
            case Status.Complete:
                //Checking whether the previous tasks have been completed
                TaskInList? notCompleteTask = (from taskInList in task.Dependencies
                                               where taskInList.Status != BO.Status.Complete
                                               select taskInList).FirstOrDefault();

                if (notCompleteTask != null)
                    throw new BO.BlDependentsTasksException($"There is a previous task for the task - {task.ID} - that has not been completed");
                break;
        }
    }

    /// <summary>
    /// The function checks that the responsible engineer fields are correct
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlIntException"></exception>
    /// <exception cref="BO.BlEmptyStringException"></exception>
    private void checkAssignedEngineerField(BO.Task task)
    {
        if (task.AssignedEngineer != null)
        {
            int assignedEngineerID = task.AssignedEngineer.ID;
            try
            {
                _dal.Engineer.Read(item => item.ID == assignedEngineerID);
            }
            catch(DO.DalDoesNotExistException dalEx)
            {
                throw new BlDoesNotExistException($"An engineer with ID={assignedEngineerID} does not exist", dalEx);
            }

            if (assignedEngineerID < 100000000 || assignedEngineerID > 999999999)
            {
                throw new BO.BlIntException("The engineer's ID number must be 9 digits!");
            }

            if (task.AssignedEngineer.Name == null && task.AssignedEngineer.Name == "")
            {
                throw new BO.BlEmptyStringException("The engineer's full name can't be empty!");
            }
        }
    }

    /// <summary>
    /// The function returns a collection of TaskInList objects sorted by ID
    /// </summary>
    /// <returns>A collection of TaskInList objects sorted by ID</returns>
    public IEnumerable<BO.TaskInList> SortByID()
    {
        return from taskInList in ReadAll()
               orderby taskInList.ID
               select taskInList;
    }
}

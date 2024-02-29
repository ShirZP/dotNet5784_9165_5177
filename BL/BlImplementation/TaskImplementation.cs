namespace BlImplementation;
using BlApi;
using System;
using BO;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    //private BlApi.IBl _bl = BlApi.Factory.Get();  //TODO: ask????
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;
    

    /// <summary>
    /// The function receives an object of type BO.Task.
    /// Checks the correctness of fields and adds the task to the data layer as DO.Task
    /// and adds to the data layer all the dependencies of the task
    /// </summary>
    /// <param name="task">An object of type BO.Task</param>
    public int Create(BO.Task task)
    {
        BO.ProjectStatus projectStatus = _bl.GetProjectStatus();
        if(projectStatus == BO.ProjectStatus.Execution)
        {
            throw new BO.BlProjectStatusException("New tasks cannot be added during the execution stage!");
        }

        //validation of the task's fields
        checkTaskFields(task, projectStatus);

        //create dal task
        DO.Task doTask = new DO.Task(task.ID,
                                     task.NickName,
                                     task.Description,
                                     task.ScheduledDate,
                                     task.StartDate,
                                     task.RequiredEffortTime,
                                     task.CompleteDate,
                                     task.Deliverables,
                                     task.Remarks,
                                     null,
                                     (DO.EngineerExperience)task.Complexity);

        //Creating a list of DO.Dependency objects
        IEnumerable<DO.Dependency>? dalDependenciesList = task.Dependencies.Select(taskInList => new DO.Dependency(0, task.ID, taskInList.ID));
        
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
        if (_bl.GetProjectStatus() != BO.ProjectStatus.Execution)
        {
            //Finding all dependencies on which they depends on current task
            IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll(item => item.DependensOnTask == id);

            if (dependencies.Any())
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

        DO.EngineerExperience? dalExperience = dalTask.Complexity;
        BO.EngineerExperience blExperience;

        if (dalExperience == null)
            blExperience = BO.EngineerExperience.Beginner;
        else
            blExperience = (BO.EngineerExperience)dalExperience;


        return new BO.Task(dalTask.ID,
                           dalTask.NickName,
                           dalTask.Description,
                           calculateStatus(dalTask),
                           calculateDependencies(dalTask),
                           dalTask.ScheduledDate,
                           dalTask.StartDate,
                           calculateForecastDate(dalTask),
                           dalTask.CompleteDate,
                           dalTask.RequiredEffortTime,
                           dalTask.FinalProduct,
                           dalTask.Remarks,
                           getAssignedEngineer(dalTask),
                           blExperience);
    }

    /// <summary>
    /// The function received a filter and returns all tasks as taskInList according to the filter
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
    /// The function received a filter and returns all tasks as Task according to the filter
    /// </summary>
    /// <param name="filter">filter to read</param>
    /// <returns>all tasks according to the filter</returns>
    public IEnumerable<BO.Task> ReadAllFullTasksDetails(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return (from DO.Task dalTask in _dal.Task.ReadAll()
                    let blTask = Read(dalTask.ID)
                    where filter(blTask)
                    select blTask);
        }
        return (from DO.Task dalTask in _dal.Task.ReadAll()
                let blTask = Read(dalTask.ID)
                select blTask);
    }

    /// <summary>
    /// The function receives BO updated task object and update the task in the data layer
    /// </summary>
    /// <param name="updatedTask">BO updated task object</param>
    /// <exception cref="BO.BlDoesNotExistException">if the task is not exists</exception>
    public void Update(BO.Task updatedTask)
    {
        BO.ProjectStatus projectStatus = _bl.GetProjectStatus();

        //validation of the task's fields
        checkTaskFields(updatedTask, projectStatus);

        //Adding new dependencies in the data layer for each new task that added to the dependencies list in the updated task

        //Grouping the list of tasks according to whether there is a dependency on the data layer
        IEnumerable<IGrouping<bool, BO.TaskInList>> dalDependencies;
        dalDependencies = (from taskInList in updatedTask.Dependencies
                           let dalDependency = _dal.Dependency.Read(item => item.DependentTask == updatedTask.ID && item.DependensOnTask == taskInList.ID) //Retrieving the dependency on which the updatedTask depends on the task
                           group taskInList by (dalDependency == null) into dalDep
                           select dalDep);

        if (projectStatus == BO.ProjectStatus.Execution)
        {
            BO.TaskInList? noDependencytask = (from taskInList in dalDependencies
                                               where taskInList.Key == true
                                               from task in taskInList
                                               select task).FirstOrDefault();

            if (noDependencytask != null)
                throw new BO.BlProjectStatusException("New dependencies cannot be added during the execution stage!");
        }
    

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

        }
        else if(updatedTask.Dependencies == null && blOldTask.Dependencies != null)
        {
            foreach (BO.TaskInList task in blOldTask.Dependencies)
            {
                DO.Dependency oldDependency = _dal.Dependency.Read(item => item.DependentTask == updatedTask.ID && item.DependensOnTask == task.ID)!;
                _dal.Dependency.Delete(oldDependency.ID);
            }
        }

        if(updatedTask.Status == BO.Status.Complete && updatedTask.CompleteDate == null)
        {
            updatedTask.CompleteDate = _bl.Clock;
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
                                          (DO.EngineerExperience)updatedTask.Complexity);

            _dal.Task.Update(dalTask);

        }
        catch (DO.DalDoesNotExistException dalEx)
        {
            throw new BO.BlDoesNotExistException($"An object of type Task with ID={updatedTask.ID} does not exist", dalEx);
        }
    }

    private void checkScheduledDateField(int id, DateTime? newScheduledDate, BO.ProjectStatus projectStatus)
    {
        if (newScheduledDate.HasValue)
        {
            if (projectStatus == BO.ProjectStatus.Planning)
            {
                throw new BO.BlProjectStatusException("A scheduled task date cannot be edited when the project status is in planning!");
            }

            BO.Task updateTask = Read(id);

            if (projectStatus == BO.ProjectStatus.Execution && updateTask.ScheduledDate != newScheduledDate)
            {
                throw new BO.BlProjectStatusException("A scheduled task date cannot be edited when the project status is in Execution!");
            }

            //If the task has no dependencies - checking whether the new scheduled date is after the project start date
            if (!updateTask.Dependencies.Any())
                if (newScheduledDate < _bl.GetProjectStartDate())
                    throw new BO.BlscheduledDateException("A task's scheduled start date cannot be before the project's start date");

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

    public void ScheduledDateUpdate(int id, DateTime? newScheduledDate, BO.ProjectStatus projectStatus)
    {
        checkScheduledDateField(id, newScheduledDate, projectStatus);

        BO.Task oldTask = Read(id);

        BO.Task updatedTask = oldTask;
        updatedTask.ScheduledDate = newScheduledDate;

        Update(updatedTask);
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
    /// The function receives a BO task object and check the validations of it's fields.
    /// </summary>
    /// <param name="task">BO task object</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    private void checkTaskFields(BO.Task task, BO.ProjectStatus projectStatus)
    {
        if (task.ID < 0)
            throw new BlIntException("The task's ID number must be positive!");

        if (task.NickName == null || task.NickName == "")
            throw new BlStringException("The task's nick name cannot be empty!");

        if (task.Description == null || task.Description == "")
            throw new BlStringException("The task's description cannot be empty!");

        checkStatusField(task, projectStatus);

        checkScheduledDateField(task.ID, task.ScheduledDate, projectStatus);

        checkRequiredEffortTimeField(task, projectStatus);

        checkAssignedEngineerField(task, projectStatus);
    }

    private void checkRequiredEffortTimeField(BO.Task task, BO.ProjectStatus projectStatus)
    {
        if (task.RequiredEffortTime != null)
        {
            if (projectStatus == BO.ProjectStatus.Execution)
            {
                DO.Task? dalTask = _dal.Task.Read(item => item.ID == task.ID);

                if (task.RequiredEffortTime != dalTask!.RequiredEffortTime)
                    throw new BO.BlProjectStatusException("A required effort time cannot be edited when the project status is in Execution!");
            }

            if (task.RequiredEffortTime.Value.TotalMilliseconds <= 0)
                throw new BlIntException($"Required effort time can't be 0 or below");
        }

    }

    /// <summary>
    /// If the status is Active or Complete- the function checks whether there are previous tasks that have not been completed
    /// </summary>
    /// <param name="task">BO task object</param>
    /// <exception cref="BO.BlDependentsTasksException">is a previous task for the task that has not been completed</exception>
    private void checkStatusField(BO.Task task, BO.ProjectStatus projectStatus)
    {
        if(projectStatus == BO.ProjectStatus.Planning && task.Status != Status.New)
        {
            throw new BO.BlProjectStatusException("A task status field cannot be edited when the project status is in planning!");
        }

        BO.Task originTask = Read(task.ID);

        switch (task.Status)
        {
            case Status.Active:
            case Status.Complete:

                if (originTask.Status > task.Status)
                    throw new BO.BlStatusException($"A task status cannot be changed from \"{originTask.Status}\" to \"{task.Status}\"!");


                if (projectStatus == BO.ProjectStatus.Execution && task.AssignedEngineer == null)
                {
                    throw new BO.BlEngineerNotAssignedToTaskException("A task cannot be in active or complete status if no engineer is assigned to the task");
                }

                BO.Engineer assignedEngineer = _bl.Engineer.Read(task.AssignedEngineer!.ID);
                if(assignedEngineer.EngineerCurrentTask == null || assignedEngineer.EngineerCurrentTask.ID != task.ID)
                {
                    throw new BO.BlEngineerNotAssignedToTaskException("A task status cannot be changed to active or completed when it is not the engineer's current task");
                }

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
    private void checkAssignedEngineerField(BO.Task task, BO.ProjectStatus projectStatus)
    {
        if (task.AssignedEngineer != null)
        {
            if (projectStatus == BO.ProjectStatus.Planning)
            {
                throw new BO.BlProjectStatusException("A assigned engineer field cannot be edited when the project status is in planning!");
            }

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
                throw new BO.BlStringException("The engineer's full name can't be empty!");
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

    public void autoScheduledDate(BO.Task task)
    {
        if (task.ScheduledDate != null) return;

        DateTime? scheduledDateAuto;
        DateTime? pStartDate = _bl.GetProjectStartDate();
        DateTime projectStartDate;

        if (pStartDate.HasValue)
        {
            projectStartDate = pStartDate.Value;

            //If there are no dependencies then the Scheduled date is equal to the project start date plus a day
            if (!task.Dependencies.Any())
            {
                scheduledDateAuto = projectStartDate.AddDays(1);
            }
            else
            {

                //All Dependencies that do not have a Scheduled Date
                IEnumerable<BO.Task>? nullScheduledDateDependencies = (from Dependency in task.Dependencies
                                                                       let t = Read(Dependency.ID)
                                                                       where t.ScheduledDate == null
                                                                       select t).ToList();

                foreach (BO.Task t in nullScheduledDateDependencies)
                {
                    autoScheduledDate(t);
                }

                //from here all the dependencies have Forecast Date

                //Finding the latest Forecast Date among the Dependencies
                DateTime? maxDate = Read(task.Dependencies.First().ID).ForecastDate;
                DateTime? tempDate;
                foreach (BO.TaskInList t in task.Dependencies)
                {
                    tempDate = Read(t.ID).ForecastDate;
                    if (tempDate > maxDate) maxDate = tempDate;
                }

                scheduledDateAuto = maxDate?.AddDays(1);
            }

            DO.Task updateTask = new DO.Task(task.ID,
                                             task.NickName,
                                             task.Description,
                                             scheduledDateAuto,
                                             task.StartDate,
                                             task.RequiredEffortTime,
                                             task.CompleteDate,
                                             task.Deliverables,
                                             task.Remarks,
                                             task.AssignedEngineer is not null ? task.AssignedEngineer.ID : 0,
                                             (DO.EngineerExperience)task.Complexity);
            try
            {
                _dal.Task.Update(updateTask);
            }
            catch(DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"An object of type Task with ID = {updateTask.ID} does not exist", ex);
            }
        }

    }

    /// <summary>
    /// The function accepts a task and returns a list of possible dependent tasks to the user so that no chain dependencies are created.
    /// </summary>
    /// <returns>potential dependencies</returns>
    public List<TaskInList> PotentialDependencies(int currentTaskID)
    {
        IEnumerable<TaskInList> allTasks = ReadAll(item => item.ID != currentTaskID);

        List<TaskInList> potentialDependencies = (from task in allTasks
                                 where ChainDependencyTest(task, currentTaskID)
                                 select task).ToList();

        return potentialDependencies;
    }

    /// <summary>
    /// A recursive function that receives a task and the ID of the task to check. Returns true if the task to be tested does not appear as one of the dependencies of the current task.
    /// </summary>
    private bool ChainDependencyTest(BO.TaskInList task, int requestedTaskID)
    {
        foreach (BO.TaskInList dependency in Read(task.ID).Dependencies)
        {
            if (dependency.ID == requestedTaskID)
                return false;
            else
                return ChainDependencyTest(dependency, requestedTaskID);

        }
        return true;
    }
}

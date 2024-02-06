using DalApi;

namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();

    /// <summary>
    ///The function recieves an object of type BO.Engineer, Checks the correctness of fields and adds the engineer to the data layer as DO.Engineer.
    /// </summary>
    /// <param name="engineer">An object of type BO.Engineer</param>
    /// <exception cref="BlAlreadyExistsException">If the engineer already exists throw an exception from the data layer.</exception>
    public int Create(BO.Engineer engineer)
    {
        try
        {
            //validation of the engineer's fields
            checkEngineerFields(engineer);

            //create dal engineer
            DO.Engineer doEngineer = new DO.Engineer(engineer.ID, engineer.FullName, engineer.Email, engineer.Level, engineer.Cost);

            //add the dal engineer to data layer
            int idEngineer = _dal.Engineer.Create(doEngineer);

            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException dalEx)
        {
            throw new BO.BlAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist", dalEx);
        }
    }

    /// <summary>
    /// The function receives the ID of the engineer and deletes him if he exists and does not have a current task or tasks he has performed in the past
    /// </summary>
    /// <param name="id">id of engineer to delete</param>
    /// <exception cref="BlCompleteOrActiveTasksException">if the engineer have a current task or tasks he has performed in the past</exception>
    /// <exception cref="BlDoesNotExistException">if the engineer is not exists</exception>
    public void Delete(int id)
    {
        //Retrieving all the tasks that the delete engineer is registered on
        IEnumerable<DO.Task>? tasks = _dal.Task.ReadAll(item => item.EngineerId == id);

        //Checks whether there is a task from the engineer's task list that has been performed or is being performed
        DO.Task? startedTask = tasks.Select(task => task).Where(task => task.StartDate != null).FirstOrDefault();

        if (startedTask != null)
            throw new BO.BlCompleteOrActiveTasksException($"It is not possible to delete the engineer - {id} because he has already finished a task or is actively working on a task");


        //Deleting the engineer from all the tasks he is registered on
        if (tasks != null)
        {
            IEnumerable<DO.Task?> updatedTasks = (from task in tasks
                                                  let updateTask = task with { EngineerId = null }
                                                  select updateTask).ToList();
            //TODO: how to do without foreach 
            foreach (DO.Task? task in updatedTasks)
            {
                if (task != null)
                    _dal.Task.Update(task);
            }
        }

        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException dalex)
        {
            throw new BO.BlDoesNotExistException($"An object of type Engineer with ID={id} does not exist", dalex);
        }
    }

    /// <summary>
    /// The function receives the ID of an engineer and returns it as an object of type BO.
    /// </summary>
    /// <param name="id">the ID of an engineer</param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException">if the engineer is not exists</exception>
    public BO.Engineer Read(int id)
    {
        //Retrieving the engineer from the data layer
        DO.Engineer? dalEngineer = _dal.Engineer.Read(item => item.ID == id);

        if (dalEngineer == null)
            throw new BO.BlDoesNotExistException($"An object of type Engineer with ID={id} does not exist");

        //Retrieving the task the engineer is working on
        DO.Task? engineerTask = _dal.Task.ReadAll(item => item.EngineerId == id).Select(task => task).Where(task => task.StartDate != null && task.CompleteDate == null).FirstOrDefault();

        BO.TaskInEngineer? taskInEngineer = null;

        if (engineerTask != null)
            taskInEngineer = new BO.TaskInEngineer(engineerTask.ID, engineerTask.NickName);

        //Creating a BO engineer and returning it
        return new BO.Engineer(dalEngineer.ID, dalEngineer.FullName!, dalEngineer.Email!, dalEngineer.Level, dalEngineer.Cost, taskInEngineer);
    }

    /// <summary>
    /// The function recieved a filter and returns all engineers according to the filter
    /// </summary>
    /// <param name="filter">filter to read</param>
    /// <returns>all engineers according to the filter</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return (from DO.Engineer dalEngineer in _dal.Engineer.ReadAll()
                    let blEngineer = Read(dalEngineer.ID)
                    where filter(blEngineer)
                    select blEngineer);
        }

        return _dal.Engineer.ReadAll().Select(dalEngineer => Read(dalEngineer.ID));
    }

    /// <summary>
    /// The function receives BO updated engineer object and update the engineer in the data layer. 
    /// </summary>
    /// <param name="updatedEngineer">BO updated engineer object</param>
    /// <exception cref="BO.BlEngineerNotAssignedToTaskException">if the engineer is not assign to the task</exception>
    /// <exception cref="BO.BlDoesNotExistException">if the engineer is not exists</exception>
    public void Update(BO.Engineer updatedEngineer)
    {
        try
        {
            //validation of the engineer's fields
            checkEngineerFields(updatedEngineer);

            DO.Engineer doEngineer = new DO.Engineer(updatedEngineer.ID, updatedEngineer.FullName, updatedEngineer.Email, updatedEngineer.Level, updatedEngineer.Cost);
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalDoesNotExistException dalEx)
        {
            throw new BO.BlDoesNotExistException($"An object of type Engineer with ID={updatedEngineer.ID} does not exist", dalEx);
        }
    }

    /// <summary>
    /// The function recieves a BO engineer object and check the validations of it's fields.
    /// </summary>
    /// <param name="engineer">BO engineer object</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    private void checkEngineerFields(BO.Engineer engineer)
    {
        if (engineer.ID < 100000000 || engineer.ID > 999999999)
        {
            throw new BO.BlIntException("The engineer's ID number must be 9 digits!");
        }

        if (engineer.FullName == null && engineer.FullName == "")
        {
            throw new BO.BlEmptyStringException("The engineer's full name can't be empty!");
        }

        if (engineer.Cost <= 0)
        {
            throw new BO.BlIntException("The engineer's salary must be positive!");
        }

        //check email
        new EmailAddressAttribute().IsValid(engineer.Email);

        if (engineer.Level == null)
        {
            throw new BO.BlEmptyEnumException("The engineer's level can't be empty!");
        }

        checkEngineerCurrentTask(engineer);
    }

    private void checkEngineerCurrentTask(BO.Engineer updatedEngineer)
    {
        //checking the updating of the field engineerCurrentTask
        if (updatedEngineer.EngineerCurrentTask != null)
        {
            int idTaskInEngineer = updatedEngineer.EngineerCurrentTask.ID;  //The current task ID of the engineer
            DO.Task currentTaskEngineer = _dal.Task.Read(item => item.ID == idTaskInEngineer)!;

            //if the engineer doesn't assigned to the current task engineer
            if (currentTaskEngineer.EngineerId != null && currentTaskEngineer.EngineerId != updatedEngineer.ID)
            {
                throw new BO.BlEngineerNotAssignedToTaskException($"Engineer - {updatedEngineer.ID} id not assigned to the task - {idTaskInEngineer}");
            }

            //if the start date is empty so the engineer start to work on the task now
            if (currentTaskEngineer.StartDate == null)
            {
                BO.Task blCurrentTaskEngineer = _bl.Task.Read(idTaskInEngineer);

                //Checking the status of the previous tasks
                TaskInList? completeTask = (from task in blCurrentTaskEngineer.Dependencies
                                            where task.Status != BO.Status.Complete
                                            select task).FirstOrDefault();

                if (completeTask != null)
                    throw new BO.BlDependentsTasksException($"There is a previous task for the task - {idTaskInEngineer} - that has not been completed");

                //Checking the engineer level compared to the task complexity
                if (updatedEngineer.Level > blCurrentTaskEngineer.Complexity)
                    throw new BlInappropriateLevelException($"The level of the engineer - {updatedEngineer.ID}, is not high enough for the level of the assigned task");

                DO.Task updatedTask = currentTaskEngineer with { StartDate = DateTime.Now, EngineerId = updatedEngineer.ID };
                _dal.Task.Update(updatedTask);
            }
        }

    }
}

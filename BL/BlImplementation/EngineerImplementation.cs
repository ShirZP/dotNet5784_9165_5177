namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    /// <summary>
    ///The function recieves an object of type BO.Engineer, Checks the correctness of fields and adds the engineer to the data layer as DO.Engineer.
    /// </summary>
    /// <param name="engineer">An object of type BO.Engineer</param>
    /// <exception cref="BlPositiveIntException">If the number is negative or equal to zero, throw an exception.</exception>
    /// <exception cref="BlEmptyStringException">If the string is empty throw an exception.</exception>
    /// <exception cref="BlAlreadyExistsException">If the engineer already exists throw an exception from the data layer.</exception>
    public int Create(BO.Engineer engineer)
    {     
        //TaskInEngineer //TODO: השדה הזה לא מוגדר בבנאי מתי צריך להוסיף אותו?
        try
        {
            //validation of the engineer's fields

            if(engineer.ID <= 0)
            {
                throw new BlPositiveIntException("The engineer's ID number must be positive!");
            }

            if(engineer.FullName == null)
            {
                throw new BlEmptyStringException("The engineer's full name can't be empty!");
            }

            if (engineer.Cost <= 0)
            {
                throw new BlPositiveIntException("The engineer's salary must be positive!");
            }

            if(engineer.Email == null) 
            {
                throw new BlEmptyStringException("The engineer's email can't be empty!");
            }

            //create dal engineer
            DO.Engineer doEngineer = new DO.Engineer(engineer.ID, engineer.FullName, engineer.Email, engineer.Level, engineer.Cost);

            //add the dal engineer to data layer
            int idEngineer = _dal.Engineer.Create(doEngineer);

            return idEngineer;
        }

        catch (DO.DalAlreadyExistsException dalEx)
        {
            throw new BlAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist", dalEx);
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
        IEnumerable<DO.Task?>? tasks = _dal.Task.ReadAll(item => item.EngineerId == id);

        //Checks whether there is a task from the engineer's task list that has been performed or is being performed
        DO.Task? startedTask = (from task in tasks
                                  where task.StartDate != null //לפי ההיגיון שלנו צריך (where task.complete == null)
                                  select task).FirstOrDefault();
        if (startedTask != null)
            throw new BlCompleteOrActiveTasksException($"It is not possible to delete the engineer - {id} because he has already finished a task or is actively working on a task");


        //Deleting the engineer from all the tasks he is registered on
        if (tasks != null)
        {
            IEnumerable<DO.Task?> updatedTasks  = (from task in tasks
                                                   let updateTask = task with { EngineerId = null }
                                                   select updateTask).ToList();
            //TODO: no foreach!?!? :(
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
            throw new BlDoesNotExistException($"An object of type Engineer with ID={id} does not exist", dalex);
        }
    }

    /// <summary>
    /// The function receives the ID of an engineer and returns it as an object of type BO.
    /// </summary>
    /// <param name="id">the ID of an engineer</param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException">if the engineer is not exists</exception>
    public BO.Engineer Read(int id)
    {
        //Retrieving the engineer from the data layer
        DO.Engineer? dalEngineer = _dal.Engineer.Read(item => item.ID == id);

        if (dalEngineer == null)
            throw new BlDoesNotExistException($"An object of type Engineer with ID={id} does not exist");

        //Retrieving the task the engineer is working on
        DO.Task? engineerTask = (from task in _dal.Task.ReadAll(item => item.EngineerId == id)
                                        where task.StartDate != null && task.CompleteDate == null
                                        select task).FirstOrDefault();

        TaskInEngineer? taskInEngineer = null;

        if (engineerTask != null)
            taskInEngineer = new TaskInEngineer(engineerTask.ID, engineerTask.NickName);

        //Creating a BO engineer and returning it
        return new BO.Engineer(dalEngineer.ID, dalEngineer.FullName!, dalEngineer.Email!, dalEngineer.Level, dalEngineer.Cost, taskInEngineer);
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        //TODO: סינון לפי מהנדס לא מוקצה

        return (from engineer in _dal.Engineer.ReadAll(filter)
                let blEngineer = Read(engineer.ID)
                select blEngineer);
    }

    public void Update(BO.Engineer UpdatedEngineer)
    {
        throw new NotImplementedException();
    }
}

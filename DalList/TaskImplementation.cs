namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;


public class TaskImplementation : ITask
{
    /// <summary>
    /// The function adds the new task to the list of tasks with a unique ID for it.
    /// </summary>
    /// <param name="item">New task to add to the tasksList</param>
    /// <returns>The new task uniqe id</returns>
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;
        Task taskWithNewID = item with { ID =  newId };

        DataSource.Tasks.Add(taskWithNewID);

        return newId;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The function returns a reference to the task with the requested ID
    /// </summary>
    /// <param name="id">the task id that we want to find</param>
    /// <returns>reference to the task with the requested ID</returns>
    public Task? Read(int id)
    {
        Task? task = DataSource.Tasks.Find(t => t.ID == id);

        return task; 
    }

    /// <summary>
    /// The function returns a copy of the task list
    /// </summary>
    /// <returns>A copy of the task list</returns>
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    /// <summary>
    /// The function updates a task from the task list
    /// </summary>
    /// <param name="task">The updated task</param>
    /// <exception cref="Exception">If the task you want to update does not exist in the list</exception>
    public void Update(Task task)
    {
        int taskID = task.ID;
        Task? oldTask = DataSource.Tasks.Find(t => t.ID == taskID);  //Search for the task to update in the list
        if (oldTask != null)
        { 
            DataSource.Tasks.Remove(oldTask);
            Task updateTask = task with { ID = taskID };
            DataSource.Tasks.Add(updateTask);
        }
        else
        {
            throw new Exception("An object of type Task with this ID does not exist");
        }    
    }
}

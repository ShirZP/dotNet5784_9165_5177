using DalApi;
using DO;
using System.Data.Common;
using System.Threading.Tasks;


namespace Dal;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// The function adds the new task to the xml of tasks with a unique ID for it.
    /// </summary>
    /// <param name="task">New task to add to the tasks xml</param>
    /// <returns>The new task uniqe id</returns>
    public int Create(DO.Task task)
    {
        //Deserialize
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        
        int newId = Config.NextTaskId;
        DO.Task taskWithNewID = task with { ID = newId };
        tasksList.Add(taskWithNewID);

        //Serialize
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasksList, s_tasks_xml);

        return newId;
    }

    /// <summary>
    /// The function deletes the recieved task from the tasks xml
    /// </summary>
    /// <param name="id">The ID of the task to delete</param>
    /// <exception cref="DalDoesNotExistException">If the task does not exist in the tasks xml</exception>
    public void Delete(int id)
    {
        //Deserialize
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        DO.Task? task = tasksList.Find(t => t.ID == id);
        if (task != null)
        {
            tasksList.Remove(task);
            //Serialize
            XMLTools.SaveListToXMLSerializer<DO.Task>(tasksList, s_tasks_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Task with ID={id} does not exist");
        }
    }

    /// <summary>
    /// The function returns a reference to the task with the requested filter
    /// </summary>
    /// <param name="filter">Delegate Func that recieves Task and returns bool</param>
    /// <returns>reference to the task with the requested filter</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        //Deserialize
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        return tasksList.FirstOrDefault(filter);
    }

    /// <summary>
    /// The function returns a copy of tasks list from the xml with filter
    /// </summary>
    /// <returns>A copy of the tasks list with filter</returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task?, bool>? filter = null) 
    {
        //Deserialize
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (filter == null)
            return tasksList.Select(item => item);
        else
            return tasksList.Where(filter);
    }

    /// <summary>
    /// The function updates a task from the tasks xml
    /// </summary>
    /// <param name="task">The updated task</param>
    /// <exception cref="DalDoesNotExistException">If the task you want to update does not exist in the xml</exception>
    public void Update(DO.Task? task)
    {
        //Deserialize
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);

        if (task != null)
        {
            int taskID = task.ID;
            DO.Task? oldTask = tasksList.Find(t => t.ID == taskID);  //Search for the task to update in the list
            if (oldTask != null)
            {
                tasksList.Remove(oldTask);
                DO.Task updateTask = task with { ID = taskID };
                tasksList.Add(updateTask);

                //Serialize
                XMLTools.SaveListToXMLSerializer<DO.Task>(tasksList, s_tasks_xml);
            }
            else
            {
                throw new DalDoesNotExistException($"An object of type Task with ID={task.ID} does not exist");
            }
        }
    }
}

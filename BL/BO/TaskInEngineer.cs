namespace BO;

public class TaskInEngineer
{
    /// <summary>
    /// ID of the task 
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Nickname of the task
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// TaskInEngineer Ctor with parameters.
    /// </summary>
    public TaskInEngineer(int id, string nickName)
    {
        ID = id;
        NickName = nickName;
    }
}

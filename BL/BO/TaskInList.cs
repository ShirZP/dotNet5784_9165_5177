namespace BO;

public class TaskInList
{
    /// <summary>
    /// Personal unique ID for the task (automatic number)
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// A description of what needs to be done in the task in order for it to be completed
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Nickname of the task
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// The status of the task
    /// </summary>
    public BO.Status Status { get; set; }

    /// <summary>
    /// TaskInList Ctor with parameters.
    /// </summary>
    public TaskInList(int id, string description, string nickName, Status status)
    {
        ID = id;
        Description = description;
        NickName = nickName;
        Status = status;
    }   
}

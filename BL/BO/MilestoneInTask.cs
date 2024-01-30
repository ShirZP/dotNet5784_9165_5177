namespace BO;

public class MilestoneInTask
{
    /// <summary>
    /// Personal unique ID for the milestone (automatic number)
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Nickname of the milestone
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// MilestoneInTask Ctor with parameters.
    /// </summary>
    public MilestoneInTask(int iD, string nickName)
    {
        ID = iD;
        NickName = nickName;
    }   
}

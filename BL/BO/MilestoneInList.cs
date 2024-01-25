namespace BO;

public class MilestoneInList
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
    /// A description of the tasks included in this milestone
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The status of the milestone
    /// </summary>
    public BO.Status Status { get; set; }

    /// <summary>
    /// The percentage of completed assignments (of the assignments on which the milestone depends)
    /// </summary>
    public double? CompletionPercentage { get; set; }
}

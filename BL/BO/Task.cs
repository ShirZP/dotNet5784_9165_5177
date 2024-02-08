namespace BO;

public class Task
{
    /// <summary>
    /// Personal unique ID for the task (automatic number)
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Nickname of the task
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// A description of what needs to be done in the task in order for it to be completed
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The status of the task
    /// </summary>
    public BO.Status Status { get; set; }

    /// <summary>
    /// A list of tasks that the current task depends on
    /// </summary>
    public List<TaskInList> Dependencies { get; set; }
   
    /// <summary>
    /// Task creation date - calculated property
    /// </summary>
    public DateTime CreateAtDate { get; init; }

    /// <summary>
    /// Desired date for the start of the task
    /// </summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>
    /// Actual task start date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Estimated date for task completion - calculated property
    /// </summary>
    public DateTime? ForecastDate { get; set; } 

    /// <summary>
    /// The latest possible date to Complete the task
    /// </summary>
    public DateTime? DeadlineDate { get; set; }

    /// <summary>
    /// Actual task completion date
    /// </summary>
    public DateTime? CompleteDate { get; set; }

    /// <summary>
    /// The time required to work on the task
    /// </summary>
    public TimeSpan? RequiredEffortTime { get; set; }

    /// <summary>
    /// The deliverables of the task
    /// </summary>
    public string? Deliverables { get; set; }

    /// <summary>
    /// Option to add remarks on the task
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Engineer responsible for carrying out the task
    /// </summary>
    public EngineerInTask? AssignedEngineer { get; set; }

    /// <summary>
    /// The difficulty level defines the minimum engineer level that can work on it
    /// </summary>
    public DO.EngineerExperience? Complexity { get; set; }

    /// <summary>
    /// Task Ctor with parameters.
    /// </summary>
    public Task(int id, string nickName, string description, BO.Status status, List<TaskInList> dependencies, DateTime createAtDate, DateTime? scheduledDate, DateTime? startDate, DateTime? forecastDate, DateTime? deadlineDate, DateTime? completeDate, TimeSpan? requiredEffortTime, string? deliverables, string? remarks, EngineerInTask? assignedEngineer, DO.EngineerExperience? complexity)
    {//MilestoneInTask? milestone


        ID = id;
        NickName = nickName;    
        Description = description;
        Status = status;    
        Dependencies = dependencies;    
        //Milestone = milestone;
        CreateAtDate = createAtDate;
        ScheduledDate = scheduledDate;  
        StartDate = startDate;  
        ForecastDate = forecastDate;    
        DeadlineDate = deadlineDate;    
        RequiredEffortTime = requiredEffortTime;    
        Deliverables = deliverables;    
        Remarks = remarks;  
        AssignedEngineer = assignedEngineer;    
        Complexity = complexity;    
    }

    public override string ToString() => Tools.ToStringProperty(this);
}

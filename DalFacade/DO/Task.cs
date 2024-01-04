namespace DO;
/// <summary>
/// A task entity represents a task with all its data.
/// </summary>
/// <param name="ID">Personal unique ID for the task (automatic number).</param>
/// <param name="NickName">Nickname of the task.</param>
/// <param name="Description">A description of what needs to be done in the task in order for it to be completed.</param>
/// <param name="ScheduledDate">Desired date for the start of the task.</param>
/// <param name="StartDate">Actual task start date.</param>
/// <param name="RequiredEffortTime">The time required to work on the task.</param>
/// <param name="CompleteDate">Actual task completion date.</param>
/// <param name="FinalProduct">The results or items provided at the end of the task.</param>
/// <param name="Remarks">Option to add remarks on the task.</param>
/// <param name="EngineerId">ID number of the engineer responsible for carrying out the task.</param>
/// <param name="Complexity">The difficulty level defines the minimum engineer level that can work on it.</param>
/// <param name="DeadlineDate">The latest possible date on which the task is finished will not cause the project to fail, so that the entire sequence of tasks that depend on it will be completed before the deadline of the entire project.</param>
/// <param name="IsMileStone">Does the task depend on other tasks before and after it.</param>
public record Task
(
    int ID,
    string NickName,
    string Description,  
    DateTime? ScheduledDate,  
    DateTime? StartDate,       
    TimeSpan? RequiredEffortTime,   
    DateTime? CompleteDate,
    string? FinalProduct,
    string? Remarks,
    int? EngineerId,
    DO.EngineerExperience? Complexity,
    DateTime? DeadlineDate = null,
    bool? IsMileStone = false
)
{
    public Task() : this(0, "", "", null, null, null, null, "", "", 0, null) { }
    //TODO: Ctor parameters
    public DateTime CreateAtDate => DateTime.Now;
}

namespace DO;
/// <summary>
/// Indicates the dependency between tasks, which task depends on another.
/// </summary>
/// <param name="ID">Personal unique ID for the task (automatic number).</param>
/// <param name="DependentTask">Which task depends on the current task.</param>
/// <param name="DependensOnTask">Which task the current task depends on.</param>
public record Dependency
(
    int ID,
    int DependentTask,
    int DependensOnTask
)
{
    public Dependency() : this(0,0,0) { } //empty ctor for stage 3
    // TODO: parameter ctor

}

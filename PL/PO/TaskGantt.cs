namespace PL.PO;

public class TaskGantt
{
    public int TaskID { get; init; }
    public string TaskName { get; init; }
    public GanttTaskStatus TaskStatus { get; init; }
    public List<string> DependenciesName { get; init; }
    public int Duration { get; set; }    //width of rectangle task
    public double TimeFromStart { get; set; }  //width of empty rectangle from start to task
    public double TimeToEnd { get; set; }    //width of empty rectangle from task to end
}


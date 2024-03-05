namespace PL.PO;

public class TaskGantt
{
    public int taskID { get; init; }
    public string taskName { get; init; }
    public int duration { get; set; }    //width of rectangle task
    public int timeFromStart { get; set; }  //width of empty rectangle from start to task
    public int timeToEnd { get; set; }    //width of empty rectangle from task to end
}


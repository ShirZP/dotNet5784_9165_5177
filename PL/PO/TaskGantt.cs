namespace PL.PO;

public class TaskGantt
{
    public int taskID { get; init; }
    public string taskName { get; init; }
    public BO.Status taskStatus { get; init; }
    public int duration { get; set; }    //width of rectangle task
    public double timeFromStart { get; set; }  //width of empty rectangle from start to task
    public double timeToEnd { get; set; }    //width of empty rectangle from task to end
}


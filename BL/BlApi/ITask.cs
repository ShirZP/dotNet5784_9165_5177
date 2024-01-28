namespace BlApi;

public interface ITask
{
    // TODO: לשאול את שירה ושיר
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null); //Reads all tasks by filter 

    public BO.Task Read(int id); //Reads task by id
    
    public int Create(BO.Task task); //Add new task

    public void Update(BO.Task UpdatedTask); //Updates task

    public void Delete(int id);  //Deletes a task by its id

    public void ScheduledDateUpdate(int id);  //Update a task's scheduled start date
}


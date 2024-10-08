﻿namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null); //Reads all tasks by filter 

    public IEnumerable<BO.Task> ReadAllFullTasksDetails(Func<BO.Task, bool>? filter = null); //Reads all tasks by filter 

    public BO.Task Read(int id); //Reads task by id
    
    public int Create(BO.Task task); //Add new task

    public void Update(BO.Task updatedTask); //Updates task

    public void Delete(int id);  //Deletes a task by its id

    public void autoScheduledDate();

    public IEnumerable<BO.Task> SortByID();

    public List<BO.TaskInList> PotentialDependencies(int currentTaskID);
}


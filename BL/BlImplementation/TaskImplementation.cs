namespace BlImplementation;
using BlApi;
using BO;///**************************************************
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(Task task)
    {
       
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskInList> ReadAll(Func<Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void ScheduledDateUpdate(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Task UpdatedTask)
    {
        throw new NotImplementedException();
    }
}

namespace BlImplementation;
using BlApi;
using BO;   //TODO: delete
using System;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = Factory.Get;

    public void Create(Engineer engineer)
    {       
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer UpdatedEngineer)
    {
        throw new NotImplementedException();
    }
}
